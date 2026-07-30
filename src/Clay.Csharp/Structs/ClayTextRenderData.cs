using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Render command data when commandType == CLAY_RENDER_COMMAND_TYPE_TEXT
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayTextRenderData
{
    /// <summary>A string slice containing the text to be rendered. Not guaranteed to be null terminated.</summary>
    public ClayStringSlice stringContents;

    public ClayColor textColor;
    public ushort fontId;
    public ushort fontSize;

    /// <summary>Specifies the extra whitespace gap in pixels between each character.</summary>
    public ushort letterSpacing;

    /// <summary>The height of the bounding box for this line of text.</summary>
    public ushort lineHeight;
}
