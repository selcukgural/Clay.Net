using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Render command data when commandType == CLAY_RENDER_COMMAND_TYPE_RECTANGLE
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayRectangleRenderData
{
    /// <summary>The solid background color to fill this rectangle with. Conventionally represented as 0-255 for each channel.</summary>
    public ClayColor backgroundColor;
    public ClayCornerRadius cornerRadius;
}