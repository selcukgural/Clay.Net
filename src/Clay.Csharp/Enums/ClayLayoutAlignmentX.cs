namespace Clay.Csharp.Enums;

/// <summary>
/// Controls the alignment along the x axis (horizontal) of child elements.
/// </summary>
public enum ClayLayoutAlignmentX : byte
{
    /// <summary>(Default) Aligns child elements to the left hand side of this element, offset by padding.width.left</summary>
    ClayAlignXLeft = 0,

    /// <summary>Aligns child elements to the right hand side of this element, offset by padding.width.right</summary>
    ClayAlignXRight = 1,

    /// <summary>Aligns child elements horizontally to the center of this element</summary>
    ClayAlignXCenter = 2,
}