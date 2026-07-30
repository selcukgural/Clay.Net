namespace Clay.Csharp.Enums;

/// <summary>
/// Controls the alignment along the y axis (vertical) of child elements.
/// </summary>
public enum ClayLayoutAlignmentY : byte
{
    /// <summary>(Default) Aligns child elements to the top of this element, offset by padding.width.top</summary>
    ClayAlignYTop = 0,

    /// <summary>Aligns child elements to the bottom of this element, offset by padding.width.bottom</summary>
    ClayAlignYBottom = 1,

    /// <summary>Aligns child elements vertically to the center of this element</summary>
    ClayAlignYCenter = 2,
}