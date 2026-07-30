using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Controls "padding" in pixels, which is a gap between the bounding box of this element and where its children
/// will be placed.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayPadding
{
    public ushort left;
    public ushort right;
    public ushort top;
    public ushort bottom;
}