using System.Runtime.InteropServices;
using Clay.Csharp.Enums;

namespace Clay.Csharp.Structs;

/// <summary>
/// Controls where a floating element is offset relative to its parent element.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayFloatingAttachPoints
{
    /// <summary>Controls the origin point on the element itself where the offset is applied from.</summary>
    public ClayFloatingAttachPointType element;

    /// <summary>Controls the origin point on the parent element that the floating element attaches to.</summary>
    public ClayFloatingAttachPointType parent;
}