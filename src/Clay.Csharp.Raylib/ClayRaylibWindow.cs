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
    private ClayArena _arena;
    private bool _disposed;

    /// <summary>
    /// Fonts available to declared UI, indexed by Clay_TextElementConfig.fontId. Must always contain at
    /// least one font (index 0 is the fallback for unmatched fontIds). Defaults to a single slot holding
    /// raylib's built-in font. If you replace this with fonts you've loaded yourself (Raylib.LoadFont),
    /// you own them - Dispose() only unloads what this window created (the default font), not anything
    /// you add here.
    /// </summary>
    public Font[] Fonts { get; set; }

    /// <summary>Color used to clear the window at the start of every RunFrame(). Defaults to black.</summary>
    public Color ClearColor { get; set; } = Color.Black;

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
            ClayRaylibRenderer.Render(commands, Fonts);
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
        ClayHelpers.FreeArena(ref _arena);
        global::Raylib_cs.Raylib.CloseWindow();
    }

    private static void DefaultErrorHandler(ClayErrorData error) =>
        Console.Error.WriteLine($"[Clay error] {error.errorType}: {ClayHelpers.ClayStringToManaged(error.errorText)}");
}
