using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Internally clay conventionally represents colors as 0-255, but interpretation is up to the renderer.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayColor
{
    public float r;
    public float g;
    public float b;
    public float a;
}