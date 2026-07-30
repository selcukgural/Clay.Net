using System.Runtime.InteropServices;
using Clay.Csharp.Internal;
using Clay.Csharp.Structs;

namespace Clay.Csharp.Tests;

/// <summary>
/// Shared setup for Tier 2 (native-dependent) tests: creates a fresh Clay arena/context sized to the
/// given dimensions, wires a simple deterministic measure-text function (width = 8px * char count,
/// height = fontSize, matching the placeholder used for this session's manual verification), and
/// collects any Clay_ErrorData the native library reports instead of silently swallowing it.
/// </summary>
internal sealed class ClayTestContext : IDisposable
{
    private readonly ClayNative.ErrorHandlerFunction _errorHandler;
    private readonly ClayNative.MeasureTextFunction _measureText;
    private ClayArena _arena;

    public List<ClayErrorData> Errors { get; } = new();

    public ClayTestContext(float width = 300, float height = 200)
    {
        _errorHandler = error => Errors.Add(error);
        _measureText = (text, configPtr, _) =>
        {
            ClayTextElementConfig config = Marshal.PtrToStructure<ClayTextElementConfig>(configPtr);
            return new ClayDimensions { width = text.length * 8, height = config.fontSize > 0 ? config.fontSize : 16 };
        };

        _arena = ClayHelpers.CreateArena(ClayNative.Clay_MinMemorySize());
        ClayErrorHandler errorHandler = new()
        {
            function = Marshal.GetFunctionPointerForDelegate(_errorHandler),
            userData = IntPtr.Zero,
        };

        ClayNative.Clay_Initialize(_arena, ClayHelpers.CreateDimensions(width, height), errorHandler);
        ClayNative.Clay_SetMeasureTextFunction(_measureText, IntPtr.Zero);
    }

    public void Dispose() => ClayHelpers.FreeArena(ref _arena);
}
