namespace Clay.Csharp.Enums;

/// <summary>
/// Controls whether pointer interactions are allowed while an element is transitioning position.
/// </summary>
public enum ClayTransitionInteractionHandlingType : byte
{
    ClayTransitionDisableInteractionsWhileTransitioningPosition = 0,
    ClayTransitionAllowInteractionsWhileTransitioningPosition = 1,
}
