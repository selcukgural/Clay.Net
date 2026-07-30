using System.Runtime.InteropServices;
using Clay.Csharp.Enums;

namespace Clay.Csharp.Structs;

/// <summary>
/// Controls various settings that affect the size and position of an element, as well as the sizes and positions
/// of any child elements.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayLayoutConfig
{
    /// <summary>Controls the sizing of this element inside it's parent container, including FIT, GROW, PERCENT and FIXED sizing.</summary>
    public ClaySizing sizing;

    /// <summary>Controls "padding" in pixels, which is a gap between the bounding box of this element and where its children will be placed.</summary>
    public ClayPadding padding;

    /// <summary>Controls the gap in pixels between child elements along the layout axis (horizontal gap for LEFT_TO_RIGHT, vertical gap for TOP_TO_BOTTOM).</summary>
    public ushort childGap;

    /// <summary>Controls how child elements are aligned on each axis.</summary>
    public ClayChildAlignment childAlignment;

    /// <summary>Controls the direction in which child elements will be automatically laid out.</summary>
    public ClayLayoutDirection layoutDirection;
}