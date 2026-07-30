using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Controls the minimum and maximum size in pixels that this element is allowed to grow or shrink to,
/// overriding sizing types such as FIT or GROW.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClaySizingMinMax
{
    /// <summary>The smallest final size of the element on this axis will be this value in pixels.</summary>
    public float min;

    /// <summary>The largest final size of the element on this axis will be this value in pixels.</summary>
    public float max;
}