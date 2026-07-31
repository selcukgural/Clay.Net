using System.Numerics;
using System.Runtime.InteropServices;
using Clay.Csharp;
using Clay.Csharp.Enums;
using Clay.Csharp.Structs;
using Raylib_cs;

namespace Clay.Csharp.Raylib;

/// <summary>Identifies one of the four straight border segments drawn by ClayRaylibRenderer's border case.</summary>
public enum BorderSide
{
    Left,
    Right,
    Top,
    Bottom,
}

/// <summary>Identifies one of the four rounded border corners drawn by ClayRaylibRenderer's border case.</summary>
public enum BorderCorner
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight,
}

/// <summary>
/// Translates Clay's abstract render commands into raylib draw calls, and provides a matching text
/// measurement function - the C# equivalent of the upstream clay_renderer_raylib.c reference renderer.
/// This class is stateless/static: it only touches whatever Font[]/Clay_RenderCommandArray you pass it,
/// so it composes with any window/loop setup you already have. For a batteries-included window + frame
/// loop, see <see cref="ClayRaylibWindow"/> instead.
/// </summary>
public static class ClayRaylibRenderer
{
    /// <summary>
    /// Builds a Clay measure-text callback bound to the given fonts, indexed by Clay_TextElementConfig.fontId.
    /// Falls back to fonts[0] if fontId is out of range or the referenced font failed to load.
    /// </summary>
    public static ClayNative.MeasureTextFunction CreateMeasureTextFunction(Func<Font[]> fonts) =>
        (text, configPtr, _) =>
        {
            ClayTextElementConfig config = Marshal.PtrToStructure<ClayTextElementConfig>(configPtr);
            Font font = ResolveFont(fonts(), config.fontId);
            string str = text.length == 0 ? string.Empty : Marshal.PtrToStringUTF8(text.chars, text.length)!;
            Vector2 size = global::Raylib_cs.Raylib.MeasureTextEx(font, str, config.fontSize, config.letterSpacing);
            return new ClayDimensions { width = size.X, height = size.Y };
        };

    /// <summary>
    /// Draws a full frame's worth of Clay render commands using raylib, indexing text/image commands'
    /// fontId against the provided fonts array. Call between Raylib.BeginDrawing() / EndDrawing().
    /// </summary>
    /// <param name="onCustom">
    /// Called for each CUSTOM render command (see <see cref="ClayCustomElementConfig"/>), since Clay.Net
    /// has no generic way to know what a custom element should look like - it's app-defined (upstream's
    /// own reference renderer, for example, uses this hook to draw a 3D model). Custom commands are
    /// silently skipped if this is null.
    /// </param>
    public static void Render(ClayRenderCommandArray commands, Font[] fonts, Action<ClayCustomRenderData, ClayBoundingBox>? onCustom = null)
    {
        // Tracks color + accumulated bounding box for every currently-open OVERLAY_COLOR_START..END
        // bracket (a List, not a Stack<T>, since every active level needs its accumulator updated in
        // place as commands stream by - see the ComputeOverlayColor* helpers below for why a box has to
        // be accumulated at all rather than just read off the command).
        List<(ClayColor Color, ClayBoundingBox? Bounds)> overlayStack = new();

        for (int i = 0; i < commands.length; i++)
        {
            ClayRenderCommand command = ClayNative.Clay_RenderCommandArray_Get(ref commands, i);
            ClayBoundingBox box = command.boundingBox;
            Rectangle rect = new(box.x, box.y, box.width, box.height);

            // OVERLAY_COLOR_START/END commands don't carry a usable boundingBox of their own (clay.h
            // leaves it zero-initialized on both), so every other command's box is folded into every
            // currently-open overlay level as it streams by - see UnionBoundingBox's doc comment.
            if (overlayStack.Count > 0 && command.commandType is not (ClayRenderCommandType.ClayRenderCommandTypeOverlayColorStart or ClayRenderCommandType.ClayRenderCommandTypeOverlayColorEnd))
            {
                for (int level = 0; level < overlayStack.Count; level++)
                {
                    overlayStack[level] = (overlayStack[level].Color, UnionBoundingBox(overlayStack[level].Bounds, box));
                }
            }

            switch (command.commandType)
            {
                case ClayRenderCommandType.ClayRenderCommandTypeRectangle:
                {
                    ClayRectangleRenderData data = command.renderData.rectangle;
                    Color color = ToRaylibColor(data.backgroundColor);
                    // Only topLeft gates/drives rounding - asymmetric per-corner radii aren't supported
                    // here, matching the upstream C reference renderer's own simplification.
                    if (data.cornerRadius.topLeft > 0)
                    {
                        float shortSide = MathF.Min(box.width, box.height);
                        float roundness = shortSide > 0 ? data.cornerRadius.topLeft * 2 / shortSide : 0;
                        global::Raylib_cs.Raylib.DrawRectangleRounded(rect, roundness, 8, color);
                    }
                    else
                    {
                        global::Raylib_cs.Raylib.DrawRectangleRec(rect, color);
                    }

                    break;
                }
                case ClayRenderCommandType.ClayRenderCommandTypeText:
                {
                    ClayTextRenderData data = command.renderData.text;
                    Font font = ResolveFont(fonts, data.fontId);
                    string text = data.stringContents.length == 0
                        ? string.Empty
                        : Marshal.PtrToStringUTF8(data.stringContents.chars, data.stringContents.length)!;
                    global::Raylib_cs.Raylib.DrawTextEx(font, text, new Vector2(box.x, box.y), data.fontSize, data.letterSpacing, ToRaylibColor(data.textColor));
                    break;
                }
                case ClayRenderCommandType.ClayRenderCommandTypeBorder:
                {
                    ClayBorderRenderData data = command.renderData.border;
                    Color color = ToRaylibColor(data.color);

                    // Each edge bar is shortened by its two adjacent corner radii, and the corners
                    // themselves are filled separately by DrawRing below - otherwise a rounded element's
                    // border would overshoot square-cornered past the rounding. Mirrors the upstream C
                    // reference renderer's border-drawing approach. Geometry is computed by the pure
                    // ComputeBorderBar/ComputeBorderCorner helpers below so it can be unit tested without
                    // a raylib graphics context.
                    if (data.width.left > 0)
                    {
                        (Vector2 position, Vector2 size) = ComputeBorderBar(BorderSide.Left, box, data.width, data.cornerRadius);
                        global::Raylib_cs.Raylib.DrawRectangleV(position, size, color);
                    }

                    if (data.width.right > 0)
                    {
                        (Vector2 position, Vector2 size) = ComputeBorderBar(BorderSide.Right, box, data.width, data.cornerRadius);
                        global::Raylib_cs.Raylib.DrawRectangleV(position, size, color);
                    }

                    if (data.width.top > 0)
                    {
                        (Vector2 position, Vector2 size) = ComputeBorderBar(BorderSide.Top, box, data.width, data.cornerRadius);
                        global::Raylib_cs.Raylib.DrawRectangleV(position, size, color);
                    }

                    if (data.width.bottom > 0)
                    {
                        (Vector2 position, Vector2 size) = ComputeBorderBar(BorderSide.Bottom, box, data.width, data.cornerRadius);
                        global::Raylib_cs.Raylib.DrawRectangleV(position, size, color);
                    }

                    if (data.cornerRadius.topLeft > 0)
                    {
                        (Vector2 center, float inner, float outer, float start, float end) = ComputeBorderCorner(BorderCorner.TopLeft, box, data.cornerRadius, data.width);
                        global::Raylib_cs.Raylib.DrawRing(center, inner, outer, start, end, 10, color);
                    }

                    if (data.cornerRadius.topRight > 0)
                    {
                        (Vector2 center, float inner, float outer, float start, float end) = ComputeBorderCorner(BorderCorner.TopRight, box, data.cornerRadius, data.width);
                        global::Raylib_cs.Raylib.DrawRing(center, inner, outer, start, end, 10, color);
                    }

                    if (data.cornerRadius.bottomLeft > 0)
                    {
                        (Vector2 center, float inner, float outer, float start, float end) = ComputeBorderCorner(BorderCorner.BottomLeft, box, data.cornerRadius, data.width);
                        global::Raylib_cs.Raylib.DrawRing(center, inner, outer, start, end, 10, color);
                    }

                    if (data.cornerRadius.bottomRight > 0)
                    {
                        (Vector2 center, float inner, float outer, float start, float end) = ComputeBorderCorner(BorderCorner.BottomRight, box, data.cornerRadius, data.width);
                        global::Raylib_cs.Raylib.DrawRing(center, inner, outer, start, end, 10, color);
                    }

                    break;
                }
                case ClayRenderCommandType.ClayRenderCommandTypeScissorStart:
                {
                    global::Raylib_cs.Raylib.BeginScissorMode((int)box.x, (int)box.y, (int)box.width, (int)box.height);
                    break;
                }
                case ClayRenderCommandType.ClayRenderCommandTypeScissorEnd:
                {
                    global::Raylib_cs.Raylib.EndScissorMode();
                    break;
                }
                case ClayRenderCommandType.ClayRenderCommandTypeImage:
                {
                    ClayImageRenderData data = command.renderData.image;
                    if (data.imageData != IntPtr.Zero)
                    {
                        Texture2D texture = Marshal.PtrToStructure<Texture2D>(data.imageData);
                        global::Raylib_cs.Raylib.DrawTexturePro(
                            texture,
                            new Rectangle(0, 0, texture.Width, texture.Height),
                            rect,
                            Vector2.Zero,
                            0,
                            ResolveImageTint(data.backgroundColor));
                    }

                    break;
                }
                case ClayRenderCommandType.ClayRenderCommandTypeCustom:
                {
                    ClayCustomRenderData data = command.renderData.custom;
                    if (ShouldInvokeCustom(data))
                    {
                        onCustom?.Invoke(data, box);
                    }

                    break;
                }
                case ClayRenderCommandType.ClayRenderCommandTypeOverlayColorStart:
                {
                    overlayStack.Add((command.renderData.overlayColor.color, null));
                    break;
                }
                case ClayRenderCommandType.ClayRenderCommandTypeOverlayColorEnd:
                {
                    if (overlayStack.Count > 0)
                    {
                        int top = overlayStack.Count - 1;
                        (ClayColor color, ClayBoundingBox? bounds) = overlayStack[top];
                        overlayStack.RemoveAt(top);

                        if (bounds is { } b)
                        {
                            global::Raylib_cs.Raylib.DrawRectangleRec(new Rectangle(b.x, b.y, b.width, b.height), ToRaylibColor(color));

                            // Fold this now-closed level's box into its parent (if any), so a parent
                            // overlay also covers whatever its overlaid child just covered.
                            if (overlayStack.Count > 0)
                            {
                                int parent = overlayStack.Count - 1;
                                overlayStack[parent] = (overlayStack[parent].Color, UnionBoundingBox(overlayStack[parent].Bounds, b));
                            }
                        }
                    }

                    break;
                }
                case ClayRenderCommandType.ClayRenderCommandTypeNone:
                    break;
            }
        }
    }

    /// <summary>
    /// Computes the position/size of one straight border edge bar, shortened by its two adjacent corner
    /// radii so it meets the rounded corner arcs drawn by <see cref="ComputeBorderCorner"/> instead of
    /// overshooting past them. Pure geometry - does not touch raylib.
    /// </summary>
    internal static (Vector2 Position, Vector2 Size) ComputeBorderBar(BorderSide side, ClayBoundingBox box, ClayBorderWidth width, ClayCornerRadius radius) =>
        side switch
        {
            BorderSide.Left => (
                new Vector2(box.x, box.y + radius.topLeft),
                new Vector2(width.left, box.height - radius.topLeft - radius.bottomLeft)),
            BorderSide.Right => (
                new Vector2(box.x + box.width - width.right, box.y + radius.topRight),
                new Vector2(width.right, box.height - radius.topRight - radius.bottomRight)),
            BorderSide.Top => (
                new Vector2(box.x + radius.topLeft, box.y),
                new Vector2(box.width - radius.topLeft - radius.topRight, width.top)),
            BorderSide.Bottom => (
                new Vector2(box.x + radius.bottomLeft, box.y + box.height - width.bottom),
                new Vector2(box.width - radius.bottomLeft - radius.bottomRight, width.bottom)),
            _ => throw new ArgumentOutOfRangeException(nameof(side), side, null),
        };

    /// <summary>
    /// Computes the ring (center, inner/outer radius, start/end angle in degrees) that fills one rounded
    /// border corner. Pure geometry - does not touch raylib.
    /// </summary>
    internal static (Vector2 Center, float InnerRadius, float OuterRadius, float StartAngle, float EndAngle) ComputeBorderCorner(
        BorderCorner corner, ClayBoundingBox box, ClayCornerRadius radius, ClayBorderWidth width) =>
        corner switch
        {
            BorderCorner.TopLeft => (
                new Vector2(box.x + radius.topLeft, box.y + radius.topLeft),
                radius.topLeft - width.top, radius.topLeft, 180f, 270f),
            BorderCorner.TopRight => (
                new Vector2(box.x + box.width - radius.topRight, box.y + radius.topRight),
                radius.topRight - width.top, radius.topRight, 270f, 360f),
            BorderCorner.BottomLeft => (
                new Vector2(box.x + radius.bottomLeft, box.y + box.height - radius.bottomLeft),
                radius.bottomLeft - width.bottom, radius.bottomLeft, 90f, 180f),
            BorderCorner.BottomRight => (
                new Vector2(box.x + box.width - radius.bottomRight, box.y + box.height - radius.bottomRight),
                radius.bottomRight - width.bottom, radius.bottomRight, 0.1f, 90f),
            _ => throw new ArgumentOutOfRangeException(nameof(corner), corner, null),
        };

    internal static Font ResolveFont(Font[] fonts, int fontId)
    {
        if (fonts.Length == 0)
        {
            throw new InvalidOperationException(
                $"{nameof(ClayRaylibWindow)}.{nameof(ClayRaylibWindow.Fonts)} is empty - it must contain at least one font " +
                "(index 0 is used as the fallback for any Clay_TextElementConfig.fontId that doesn't have a matching entry).");
        }

        return fontId >= 0 && fontId < fonts.Length ? fonts[fontId] : fonts[0];
    }

    internal static Color ToRaylibColor(ClayColor color) => new(
        (byte)Math.Clamp(color.r, 0, 255),
        (byte)Math.Clamp(color.g, 0, 255),
        (byte)Math.Clamp(color.b, 0, 255),
        (byte)Math.Clamp(color.a, 0, 255));

    /// <summary>
    /// An all-zero backgroundColor on an image render command means "untinted" per clay.h's own doc
    /// comment (0,0,0,0 is indistinguishable from "not set"), so it maps to opaque white (draw the
    /// texture's own colors unchanged) rather than fully transparent/invisible.
    /// </summary>
    internal static Color ResolveImageTint(ClayColor backgroundColor) =>
        backgroundColor is { r: 0, g: 0, b: 0, a: 0 } ? Color.White : ToRaylibColor(backgroundColor);

    /// <summary>Mirrors upstream's own null-check before dereferencing a CUSTOM command's opaque data pointer.</summary>
    internal static bool ShouldInvokeCustom(ClayCustomRenderData data) => data.customData != IntPtr.Zero;

    /// <summary>
    /// Returns the smallest box containing both <paramref name="accumulated"/> (if any) and
    /// <paramref name="box"/>, ignoring <paramref name="box"/> entirely if it's zero-sized (which is what
    /// every OVERLAY_COLOR_START/END command's own boundingBox looks like, since clay.h doesn't populate
    /// it for those two command types - see ClayRaylibRenderer's Render loop for how this is used to
    /// approximate an overlaid element's true bounds from everything actually drawn inside it instead).
    /// Pure geometry - does not touch raylib.
    /// </summary>
    internal static ClayBoundingBox? UnionBoundingBox(ClayBoundingBox? accumulated, ClayBoundingBox box)
    {
        if (box.width <= 0 || box.height <= 0)
        {
            return accumulated;
        }

        if (accumulated is not { } a)
        {
            return box;
        }

        float minX = MathF.Min(a.x, box.x);
        float minY = MathF.Min(a.y, box.y);
        float maxX = MathF.Max(a.x + a.width, box.x + box.width);
        float maxY = MathF.Max(a.y + a.height, box.y + box.height);
        return new ClayBoundingBox { x = minX, y = minY, width = maxX - minX, height = maxY - minY };
    }
}
