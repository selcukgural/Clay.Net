using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// A sized array of render commands.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayRenderCommandArray
{
    public int capacity;
    public int length;
    public IntPtr internalArray; // Clay_RenderCommand*
}