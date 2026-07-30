using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Bounding box and other data for a specific UI element.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayElementData
{
    public ClayBoundingBox boundingBox;
    [MarshalAs(UnmanagedType.I1)]
    public bool found;
}