using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Represents a 2D vector with x and y coordinates.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayVector2
{
    public float x;
    public float y;
}