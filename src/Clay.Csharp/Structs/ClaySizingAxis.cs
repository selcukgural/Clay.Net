using System.Runtime.InteropServices;
using Clay.Csharp.Enums;

namespace Clay.Csharp.Structs;

/// <summary>
/// Controls the sizing of this element along one axis inside its parent container.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct ClaySizingAxis
{
    [FieldOffset(0)]
    public ClaySizingMinMax minMax;

    [FieldOffset(0)]
    public float percent;

    /// <summary>Controls how the element takes up space inside its parent container.</summary>
    [FieldOffset(8)]
    public ClaySizingType type;

    /// <summary>Clamps the axis size to an exact size in pixels. Equivalent to CLAY_SIZING_FIXED.</summary>
    public static ClaySizingAxis Fixed(float size) =>
        new() { minMax = new ClaySizingMinMax { min = size, max = size }, type = ClaySizingType.ClaySizingTypeFixed };

    /// <summary>
    /// Expands along this axis to fill available space, sharing it with other GROW elements. Equivalent to CLAY_SIZING_GROW.
    /// A max of 0 (the default) is treated by Clay as "unbounded".
    /// </summary>
    public static ClaySizingAxis Grow(float min = 0, float max = 0) =>
        new() { minMax = new ClaySizingMinMax { min = min, max = max }, type = ClaySizingType.ClaySizingTypeGrow };

    /// <summary>
    /// Wraps tightly to the size of the element's contents. Equivalent to CLAY_SIZING_FIT.
    /// A max of 0 (the default) is treated by Clay as "unbounded".
    /// </summary>
    public static ClaySizingAxis Fit(float min = 0, float max = 0) =>
        new() { minMax = new ClaySizingMinMax { min = min, max = max }, type = ClaySizingType.ClaySizingTypeFit };

    /// <summary>Clamps the axis size to a percent (0-1) of the parent container's axis size. Equivalent to CLAY_SIZING_PERCENT.</summary>
    public static ClaySizingAxis Percent(float percent) =>
        new() { percent = percent, type = ClaySizingType.ClaySizingTypePercent };
}