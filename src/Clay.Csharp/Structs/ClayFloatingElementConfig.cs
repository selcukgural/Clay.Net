using System.Runtime.InteropServices;
using Clay.Csharp.Enums;

namespace Clay.Csharp.Structs;

/// <summary>
/// Controls various settings related to "floating" elements, which are elements that "float" above other elements, potentially overlapping their boundaries,
/// and not affecting the layout of sibling or parent elements.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayFloatingElementConfig
{
    /// <summary>Offsets this floating element by the provided x,y coordinates from its attachPoints.</summary>
    public ClayVector2 offset;

    /// <summary>Expands the boundaries of the outer floating element without affecting its children.</summary>
    public ClayDimensions expand;

    /// <summary>When used in conjunction with .attachTo = CLAY_ATTACH_TO_ELEMENT_WITH_ID, attaches this floating element to the element in the hierarchy with the provided ID.</summary>
    public uint parentId;

    /// <summary>Controls the z index of this floating element and all its children.</summary>
    public short zIndex;

    /// <summary>Controls the origin points that this floating element attaches to on itself and its parent.</summary>
    public ClayFloatingAttachPoints attachPoints;

    /// <summary>Controls how mouse pointer events like hover and click are captured or passed through to elements underneath a floating element.</summary>
    public ClayPointerCaptureMode pointerCaptureMode;

    /// <summary>Controls which element a floating element is "attached" to (i.e. relative offset from).</summary>
    public ClayFloatingAttachToElement attachTo;

    /// <summary>Controls whether or not a floating element is clipped to the same clipping rectangle as the element it's attached to.</summary>
    public ClayFloatingClipToElement clipTo;
}
