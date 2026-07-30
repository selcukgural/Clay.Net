namespace Clay.Csharp.Enums;

/// <summary>
/// Controls the ordering of exit transitions relative to siblings.
/// </summary>
public enum ClayExitTransitionSiblingOrdering : byte
{
    ClayExitTransitionOrderingUnderneathSiblings = 0,
    ClayExitTransitionOrderingNaturalOrder = 1,
    ClayExitTransitionOrderingAboveSiblings = 2,
}
