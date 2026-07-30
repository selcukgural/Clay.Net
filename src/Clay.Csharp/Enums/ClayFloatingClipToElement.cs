namespace Clay.Csharp.Enums;

/// <summary>
/// Controls whether or not a floating element is clipped to the same clipping rectangle as the element it's attached to.
/// </summary>
public enum ClayFloatingClipToElement : byte
{
    /// <summary>(default) The floating element does not inherit clipping.</summary>
    ClayClipToNone = 0,

    /// <summary>The floating element is clipped to the same clipping rectangle as the element it's attached to.</summary>
    ClayClipToAttachedParent = 1,
}
