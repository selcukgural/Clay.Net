using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Represents a bounding box with position and dimensions.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayBoundingBox
{
    public float x;
    public float y;
    public float width;
    public float height;
}