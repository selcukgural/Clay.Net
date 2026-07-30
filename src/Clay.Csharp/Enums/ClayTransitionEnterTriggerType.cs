namespace Clay.Csharp.Enums;

/// <summary>
/// Controls when a transition's "enter" state is triggered.
/// </summary>
public enum ClayTransitionEnterTriggerType : byte
{
    ClayTransitionEnterSkipOnFirstParentFrame = 0,
    ClayTransitionEnterTriggerOnFirstParentFrame = 1,
}
