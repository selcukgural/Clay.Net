using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Controls whether an element should clip its contents, as well as providing child x,y offset configuration for scrolling.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayClipElementConfig
{
    /// <summary>Clip overflowing elements on the X axis.</summary>
    [MarshalAs(UnmanagedType.I1)]
    public bool horizontal;

    /// <summary>Clip overflowing elements on the Y axis.</summary>
    [MarshalAs(UnmanagedType.I1)]
    public bool vertical;

    /// <summary>Offsets the x,y positions of all child elements. Used primarily for scrolling containers.</summary>
    public ClayVector2 childOffset;
}
