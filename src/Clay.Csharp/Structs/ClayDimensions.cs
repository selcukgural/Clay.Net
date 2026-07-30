using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Represents a 2D dimension with width and height.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayDimensions
{
    public float width;
    public float height;
}