using System.Runtime.InteropServices;
using Clay.Csharp.Enums;

namespace Clay.Csharp.Structs;

/// <summary>
/// Controls various functionality related to text elements.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayTextElementConfig
{
    /// <summary>A pointer that will be transparently passed through to the resulting render command.</summary>
    public IntPtr userData;

    /// <summary>The RGBA color of the font to render, conventionally specified as 0-255.</summary>
    public ClayColor textColor;

    /// <summary>An integer transparently passed to Clay_MeasureText to identify the font to use.</summary>
    public ushort fontId;

    /// <summary>Controls the size of the font. Handled by the function provided to Clay_MeasureText.</summary>
    public ushort fontSize;

    /// <summary>Controls extra horizontal spacing between characters.</summary>
    public ushort letterSpacing;

    /// <summary>Controls additional vertical space between wrapped lines of text.</summary>
    public ushort lineHeight;

    /// <summary>Controls how text "wraps" when there is insufficient horizontal space.</summary>
    public ClayTextElementConfigWrapMode wrapMode;

    /// <summary>Controls how wrapped lines of text are horizontally aligned within the outer text bounding box.</summary>
    public ClayTextAlignment textAlignment;
}
