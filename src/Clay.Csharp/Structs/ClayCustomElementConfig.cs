using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Controls various settings related to custom elements. Used to create CUSTOM render commands, usually to render element types not supported by Clay.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayCustomElementConfig
{
    /// <summary>A transparent pointer through which you can pass custom data to the renderer.</summary>
    public IntPtr customData;
}
