namespace Clay.Csharp.Enums;

/// <summary>
/// Controls how wrapped lines of text are horizontally aligned within the outer text bounding box.
/// </summary>
public enum ClayTextAlignment : byte
{
    /// <summary>(default) Horizontally aligns wrapped lines of text to the left hand side of their bounding box.</summary>
    ClayTextAlignLeft = 0,

    ClayTextAlignCenter = 1,
    ClayTextAlignRight = 2,
}