using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Render command data when commandType == CLAY_RENDER_COMMAND_TYPE_BORDER
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayBorderRenderData
{
    public ClayColor color;
    public ClayCornerRadius cornerRadius;
    public ClayBorderWidth width;
}
