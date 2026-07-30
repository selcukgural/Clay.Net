using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Render command data when commandType == CLAY_RENDER_COMMAND_TYPE_CUSTOM
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayCustomRenderData
{
    public ClayColor backgroundColor;
    public ClayCornerRadius cornerRadius;

    /// <summary>A pointer transparently passed through from the original element declaration.</summary>
    public IntPtr customData;
}
