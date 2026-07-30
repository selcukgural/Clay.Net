using System.Numerics;
using System.Runtime.InteropServices;
using Clay.Csharp;
using Clay.Csharp.Enums;
using Clay.Csharp.Structs;
using Raylib_cs;

namespace Clay.Csharp.Raylib;

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
    public static void Render(ClayRenderCommandArray commands, Font[] fonts)
    {
        for (int i = 0; i < commands.length; i++)
        {
            ClayRenderCommand command = ClayNative.Clay_RenderCommandArray_Get(ref commands, i);
            ClayBoundingBox box = command.boundingBox;
            Rectangle rect = new(box.x, box.y, box.width, box.height);

            switch (command.commandType)
            {
                case ClayRenderCommandType.ClayRenderCommandTypeRectangle:
                {
                    ClayRectangleRenderData data = command.renderData.rectangle;
                    Color color = ToRaylibColor(data.backgroundColor);
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
                    if (data.width.left > 0)
                    {
                        global::Raylib_cs.Raylib.DrawRectangle((int)box.x, (int)box.y, data.width.left, (int)box.height, color);
                    }

                    if (data.width.right > 0)
                    {
                        global::Raylib_cs.Raylib.DrawRectangle((int)(box.x + box.width - data.width.right), (int)box.y, data.width.right, (int)box.height, color);
                    }

                    if (data.width.top > 0)
                    {
                        global::Raylib_cs.Raylib.DrawRectangle((int)box.x, (int)box.y, (int)box.width, data.width.top, color);
                    }

                    if (data.width.bottom > 0)
                    {
                        global::Raylib_cs.Raylib.DrawRectangle((int)box.x, (int)(box.y + box.height - data.width.bottom), (int)box.width, data.width.bottom, color);
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
                case ClayRenderCommandType.ClayRenderCommandTypeCustom:
                case ClayRenderCommandType.ClayRenderCommandTypeOverlayColorStart:
                case ClayRenderCommandType.ClayRenderCommandTypeOverlayColorEnd:
                case ClayRenderCommandType.ClayRenderCommandTypeNone:
                    // Not wired up yet - image/custom rendering need an app-defined asset lookup, and
                    // color-overlay needs a shader pass. Left as a hook point for consumers to extend.
                    break;
            }
        }
    }

    private static Font ResolveFont(Font[] fonts, int fontId) =>
        fontId >= 0 && fontId < fonts.Length ? fonts[fontId] : fonts[0];

    private static Color ToRaylibColor(ClayColor color) => new(
        (byte)Math.Clamp(color.r, 0, 255),
        (byte)Math.Clamp(color.g, 0, 255),
        (byte)Math.Clamp(color.b, 0, 255),
        (byte)Math.Clamp(color.a, 0, 255));
}
