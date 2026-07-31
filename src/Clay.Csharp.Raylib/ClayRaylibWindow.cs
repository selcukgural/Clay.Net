using System.Numerics;
using System.Runtime.InteropServices;
using Clay.Csharp;
using Clay.Csharp.Declarative;
using Clay.Csharp.Internal;
using Clay.Csharp.Structs;
using Raylib_cs;

namespace Clay.Csharp.Raylib;

/// <summary>
/// Batteries-included Clay + raylib window: owns the raylib window, Clay's memory arena, text
/// measurement wiring and the per-frame glue (pointer state, layout dimensions, scroll containers,
/// begin/end layout, drawing) so a consumer only has to describe *what* to lay out, not how to wire it up.
///
/// <code>
/// using ClayRaylibWindow window = ClayRaylibWindow.Create(800, 600, "My App");
/// while (!window.ShouldClose)
/// {
///     window.RunFrame(() =>
///     {
///         using (Layout.Element("Root", new ClayElementDeclaration { ... }))
///         {
///             Layout.Text("Hello!", new ClayTextElementConfig { ... });
///         }
///     });
/// }
/// </code>
///
/// For lower-level control (e.g. custom frame pacing, multiple Clay contexts, or a non-default render
/// pipeline), use <see cref="ClayRaylibRenderer"/> directly instead.
/// </summary>
public sealed class ClayRaylibWindow : IDisposable
{
    private readonly ClayNative.ErrorHandlerFunction _errorHandler;
    private readonly ClayNative.MeasureTextFunction _measureText;
    private readonly List<Font> _ownedFonts = new();
    private readonly List<ClayTexture> _ownedTextures = new();
    private ClayArena _arena;
    private bool _disposed;

    /// <summary>
    /// Fonts available to declared UI, indexed by Clay_TextElementConfig.fontId. Must always contain at
    /// least one font (index 0 is the fallback for unmatched fontIds). Defaults to a single slot holding
    /// raylib's built-in font - which is a tiny 10px bitmap font not meant to be scaled up, and looks
    /// blocky/blurry at any real UI text size. Call <see cref="LoadFont"/> to replace it with a proper
    /// TTF/OTF font (see samples/Clay.Samples.Raylib for a bundled example). If you assign fonts you've
    /// loaded yourself (Raylib.LoadFontEx) directly into this array instead, you own them - Dispose()
    /// only unloads fonts loaded through <see cref="LoadFont"/>.
    /// </summary>
    public Font[] Fonts { get; set; }

    /// <summary>Color used to clear the window at the start of every RunFrame(). Defaults to black.</summary>
    public Color ClearColor { get; set; } = Color.Black;

    /// <summary>
    /// Called for each CUSTOM render command during <see cref="RunFrame"/> - see
    /// <see cref="ClayRaylibRenderer.Render"/>'s <c>onCustom</c> parameter. Null (the default) means
    /// custom commands are silently skipped.
    /// </summary>
    public Action<ClayCustomRenderData, ClayBoundingBox>? OnCustomRender { get; set; }

    /// <summary>True once the user has requested the window be closed (matches Raylib.WindowShouldClose()).</summary>
    public bool ShouldClose => global::Raylib_cs.Raylib.WindowShouldClose();

    private ClayRaylibWindow(ClayArena arena, ClayNative.ErrorHandlerFunction errorHandler, Font[] fonts)
    {
        _arena = arena;
        _errorHandler = errorHandler;
        Fonts = fonts;
        _measureText = ClayRaylibRenderer.CreateMeasureTextFunction(() => Fonts);

        ClayErrorHandler clayErrorHandler = new()
        {
            function = Marshal.GetFunctionPointerForDelegate(_errorHandler),
            userData = IntPtr.Zero,
        };

        ClayDimensions initialDimensions = ClayHelpers.CreateDimensions(global::Raylib_cs.Raylib.GetScreenWidth(), global::Raylib_cs.Raylib.GetScreenHeight());
        ClayNative.Clay_Initialize(_arena, initialDimensions, clayErrorHandler);
        ClayNative.Clay_SetMeasureTextFunction(_measureText, IntPtr.Zero);
    }

    /// <summary>
    /// Opens a raylib window and an associated Clay context sized to fit it.
    /// </summary>
    /// <param name="onError">Called when Clay reports an internal error (e.g. arena too small). Defaults to logging to stderr.</param>
    public static ClayRaylibWindow Create(int width, int height, string title, int targetFps = 60, ConfigFlags flags = default, Action<ClayErrorData>? onError = null)
    {
        global::Raylib_cs.Raylib.SetConfigFlags(flags);
        global::Raylib_cs.Raylib.InitWindow(width, height, title);
        global::Raylib_cs.Raylib.SetTargetFPS(targetFps);

        ClayNative.ErrorHandlerFunction errorHandler = onError is null
            ? DefaultErrorHandler
            : error => onError(error);

        ClayArena arena = ClayHelpers.CreateArena(ClayNative.Clay_MinMemorySize());
        Font[] fonts = [global::Raylib_cs.Raylib.GetFontDefault()];

        return new ClayRaylibWindow(arena, errorHandler, fonts);
    }

    /// <summary>
    /// Loads a TTF/OTF font from disk and installs it into <see cref="Fonts"/> at the given fontId
    /// (growing the array if needed), with bilinear texture filtering enabled so it scales cleanly at
    /// whatever size Clay's layout ends up drawing it at. <paramref name="baseSize"/> is the resolution
    /// the glyphs are rasterized at - higher looks crisper when text is displayed larger, at the cost of
    /// a bigger glyph atlas texture; 48 is a reasonable default for typical UI text sizes. The font is
    /// unloaded automatically on <see cref="Dispose"/>.
    /// </summary>
    public void LoadFont(string filePath, int fontId = 0, int baseSize = 48)
    {
        Font font = global::Raylib_cs.Raylib.LoadFontEx(filePath, baseSize, null, 0);
        global::Raylib_cs.Raylib.SetTextureFilter(font.Texture, TextureFilter.Bilinear);
        _ownedFonts.Add(font);

        Font[] fonts = new Font[Math.Max(Fonts.Length, fontId + 1)];
        Array.Copy(Fonts, fonts, Fonts.Length);
        fonts[fontId] = font;
        Fonts = fonts;
    }

    /// <summary>
    /// Loads a texture from disk and returns a <see cref="ClayTexture"/> whose <c>ImageData</c> pointer
    /// can be passed straight into <c>ClayElementDeclaration.image.imageData</c> to draw it via an IMAGE
    /// render command. The underlying GPU texture (and the small unmanaged copy backing that pointer) are
    /// unloaded automatically on <see cref="Dispose"/>.
    /// </summary>
    public ClayTexture LoadTexture(string filePath)
    {
        Texture2D texture = global::Raylib_cs.Raylib.LoadTexture(filePath);

        // ClayImageElementConfig.imageData needs a stable address a renderer can dereference later - a
        // managed Texture2D value doesn't have one, so a copy is pinned in unmanaged memory instead.
        IntPtr imageData = Marshal.AllocHGlobal(Marshal.SizeOf<Texture2D>());
        Marshal.StructureToPtr(texture, imageData, false);

        ClayTexture clayTexture = new(texture, imageData);
        _ownedTextures.Add(clayTexture);
        return clayTexture;
    }

    /// <summary>
    /// Runs one full frame: reads raylib input state into Clay, calls buildUI() between
    /// Layout.BeginLayout()/EndLayout(), then draws the resulting render commands. Returns the render
    /// commands in case the caller wants to inspect them (tests, debugging, etc) - note that the
    /// returned array's internal pointer lives inside this window's Clay arena, which gets reused on the
    /// next RunFrame() call and freed on Dispose(), so don't hold onto it past either of those.
    /// </summary>
    public ClayRenderCommandArray RunFrame(Action buildUI)
    {
        float deltaTime = global::Raylib_cs.Raylib.GetFrameTime();
        Vector2 mouse = global::Raylib_cs.Raylib.GetMousePosition();

        ClayNative.Clay_SetPointerState(ClayHelpers.CreateVector2(mouse.X, mouse.Y), global::Raylib_cs.Raylib.IsMouseButtonDown(MouseButton.Left));
        ClayNative.Clay_SetLayoutDimensions(ClayHelpers.CreateDimensions(global::Raylib_cs.Raylib.GetScreenWidth(), global::Raylib_cs.Raylib.GetScreenHeight()));
        ClayNative.Clay_UpdateScrollContainers(false, ClayHelpers.CreateVector2(0, 0), deltaTime);

        // Each phase is wrapped so a throwing buildUI/Render doesn't leave Clay's open-element stack or
        // raylib's draw batch in a broken state for the *next* frame - the exception itself still
        // propagates to the caller once EndLayout/EndDrawing have run.
        Layout.BeginLayout();
        ClayRenderCommandArray commands;
        try
        {
            buildUI();
        }
        finally
        {
            commands = Layout.EndLayout(deltaTime);
        }

        global::Raylib_cs.Raylib.BeginDrawing();
        try
        {
            global::Raylib_cs.Raylib.ClearBackground(ClearColor);
            ClayRaylibRenderer.Render(commands, Fonts, OnCustomRender);
        }
        finally
        {
            global::Raylib_cs.Raylib.EndDrawing();
        }

        return commands;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        // Fonts/textures must be unloaded while the GL context is still alive, so before CloseWindow().
        foreach (Font font in _ownedFonts)
        {
            global::Raylib_cs.Raylib.UnloadFont(font);
        }

        foreach (ClayTexture texture in _ownedTextures)
        {
            global::Raylib_cs.Raylib.UnloadTexture(texture.Texture);
            Marshal.FreeHGlobal(texture.ImageData);
        }

        ClayHelpers.FreeArena(ref _arena);
        global::Raylib_cs.Raylib.CloseWindow();
    }

    private static void DefaultErrorHandler(ClayErrorData error) =>
        Console.Error.WriteLine($"[Clay error] {error.errorType}: {ClayHelpers.ClayStringToManaged(error.errorText)}");
}
