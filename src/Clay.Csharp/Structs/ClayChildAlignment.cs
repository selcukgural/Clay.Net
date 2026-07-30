using System.Runtime.InteropServices;
using Clay.Csharp.Enums;

namespace Clay.Csharp.Structs;

/// <summary>
/// Controls how child elements are aligned on each axis.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayChildAlignment
{
    /// <summary>Controls alignment of children along the x axis.</summary>
    public ClayLayoutAlignmentX x;

    /// <summary>Controls alignment of children along the y axis.</summary>
    public ClayLayoutAlignmentY y;
}