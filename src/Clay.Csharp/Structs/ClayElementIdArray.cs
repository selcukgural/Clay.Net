using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// A sized array of Clay_ElementId.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayElementIdArray
{
    public int capacity;
    public int length;
    public IntPtr internalArray; // Clay_ElementId*
}