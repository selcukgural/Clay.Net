namespace Clay.Csharp.Enums;

/// <summary>
/// Controls how the element takes up space inside its parent container.
/// </summary>
public enum ClaySizingType : byte
{
    /// <summary>(default) Wraps tightly to the size of the element's contents.</summary>
    ClaySizingTypeFit = 0,

    /// <summary>Expands along this axis to fill available space in the parent element, sharing it with other GROW elements.</summary>
    ClaySizingTypeGrow = 1,

    /// <summary>Expects 0-1 range. Clamps the axis size to a percent of the parent container's axis size minus padding and child gaps.</summary>
    ClaySizingTypePercent = 2,

    /// <summary>Clamps the axis size to an exact size in pixels.</summary>
    ClaySizingTypeFixed = 3,
}