namespace Clay.Csharp.Enums;

/// <summary>
/// Controls when a transition's "exit" state is triggered.
/// </summary>
public enum ClayTransitionExitTriggerType : byte
{
    ClayTransitionExitSkipWhenParentExits = 0,
    ClayTransitionExitTriggerWhenParentExits = 1,
}
