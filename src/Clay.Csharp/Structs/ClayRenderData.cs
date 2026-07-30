using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// A struct union containing data specific to this command's .commandType
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 48)]
public struct ClayRenderData
{
    [FieldOffset(0)]
    public ClayTextRenderData text;

    [FieldOffset(0)]
    public ClayRectangleRenderData rectangle;

    [FieldOffset(0)]
    public ClayImageRenderData image;

    [FieldOffset(0)]
    public ClayCustomRenderData custom;

    [FieldOffset(0)]
    public ClayClipRenderData clip;

    [FieldOffset(0)]
    public ClayOverlayColorRenderData overlayColor;

    [FieldOffset(0)]
    public ClayBorderRenderData border;
}