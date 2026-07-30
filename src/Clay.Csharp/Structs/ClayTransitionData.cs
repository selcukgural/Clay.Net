using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Represents a snapshot of the animatable properties of an element, used by transition callbacks.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayTransitionData
{
    public ClayBoundingBox boundingBox;
    public ClayColor backgroundColor;
    public ClayColor overlayColor;
    public ClayColor borderColor;
    public ClayBorderWidth borderWidth;
}
