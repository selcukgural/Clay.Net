using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Controls various settings related to image elements.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayImageElementConfig
{
    /// <summary>A transparent pointer used to pass image data through to the renderer.</summary>
    public IntPtr imageData;
}