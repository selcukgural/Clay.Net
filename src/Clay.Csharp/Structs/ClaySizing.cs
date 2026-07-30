using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Controls the sizing of this element along one axis inside its parent container.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClaySizing
{
    /// <summary>Controls the width sizing of the element, along the x axis.</summary>
    public ClaySizingAxis width;

    /// <summary>Controls the height sizing of the element, along the y axis.</summary>
    public ClaySizingAxis height;
}