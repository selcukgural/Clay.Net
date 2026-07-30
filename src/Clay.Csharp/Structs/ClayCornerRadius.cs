using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Controls the "radius", or corner rounding of elements, including rectangles, borders and images.
/// The rounding is determined by drawing a circle inset into the element corner by (radius, radius) pixels.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayCornerRadius
{
    public float topLeft;
    public float topRight;
    public float bottomLeft;
    public float bottomRight;
}