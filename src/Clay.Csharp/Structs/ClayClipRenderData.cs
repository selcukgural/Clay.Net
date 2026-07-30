using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Render command data when commandType == CLAY_RENDER_COMMAND_TYPE_SCISSOR_START || commandType == CLAY_RENDER_COMMAND_TYPE_SCISSOR_END
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayClipRenderData
{
    [MarshalAs(UnmanagedType.I1)]
    public bool horizontal;

    [MarshalAs(UnmanagedType.I1)]
    public bool vertical;
}
