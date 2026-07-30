namespace Clay.Csharp.Enums;

/// <summary>
/// Controls how text "wraps", that is how it is broken into multiple lines when there is insufficient horizontal space.
/// </summary>
public enum ClayTextElementConfigWrapMode : byte
{
    /// <summary>(default) breaks on whitespace characters.</summary>
    ClayTextWrapWords = 0,

    /// <summary>Don't break on space characters, only on newlines.</summary>
    ClayTextWrapNewlines = 1,

    /// <summary>Disable text wrapping entirely.</summary>
    ClayTextWrapNone = 2,
}