using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Render command data when commandType == CLAY_RENDER_COMMAND_TYPE_OVERLAY_COLOR_START || commandType == CLAY_RENDER_COMMAND_TYPE_OVERLAY_COLOR_END
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayOverlayColorRenderData
{
    public ClayColor color;
}