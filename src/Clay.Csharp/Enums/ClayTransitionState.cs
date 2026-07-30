namespace Clay.Csharp.Enums;

/// <summary>
/// Represents the state of a transition.
/// Note: unlike most Clay enums, this one is NOT packed to a single byte in the native library
/// (it uses a plain C `enum`, which defaults to the compiler's native `int` size).
/// </summary>
public enum ClayTransitionState
{
    ClayTransitionStateIdle = 0,
    ClayTransitionStateEntering = 1,
    ClayTransitionStateTransitioning = 2,
    ClayTransitionStateExiting = 3,
}
