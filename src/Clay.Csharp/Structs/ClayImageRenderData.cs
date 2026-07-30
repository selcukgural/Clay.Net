using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Render command data when commandType == CLAY_RENDER_COMMAND_TYPE_IMAGE
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayImageRenderData
{
    /// <summary>The tint color for this image. Default 0,0,0,0 should be interpreted as "untinted".</summary>
    public ClayColor backgroundColor;

    public ClayCornerRadius cornerRadius;

    /// <summary>A pointer transparently passed through from the original element definition, typically used to represent image data.</summary>
    public IntPtr imageData;
}
