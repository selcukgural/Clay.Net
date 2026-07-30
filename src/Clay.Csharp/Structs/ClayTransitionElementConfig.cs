using System.Runtime.InteropServices;
using Clay.Csharp.Enums;

namespace Clay.Csharp.Structs;

/// <summary>
/// Mirrors the anonymous "enter" struct nested inside Clay_TransitionElementConfig.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayTransitionElementConfigEnter
{
    /// <summary>Clay_TransitionData (*setInitialState)(Clay_TransitionData targetState, Clay_TransitionProperty properties)</summary>
    public IntPtr setInitialState;
    public ClayTransitionEnterTriggerType trigger;
}

/// <summary>
/// Mirrors the anonymous "exit" struct nested inside Clay_TransitionElementConfig.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayTransitionElementConfigExit
{
    /// <summary>Clay_TransitionData (*setFinalState)(Clay_TransitionData initialState, Clay_TransitionProperty properties)</summary>
    public IntPtr setFinalState;
    public ClayTransitionExitTriggerType trigger;
    public ClayExitTransitionSiblingOrdering siblingOrdering;
}

/// <summary>
/// Controls settings related to animated transitions of an element's properties.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayTransitionElementConfig
{
    /// <summary>bool (*handler)(Clay_TransitionCallbackArguments arguments)</summary>
    public IntPtr handler;

    public float duration;
    public ClayTransitionProperty properties;
    public ClayTransitionInteractionHandlingType interactionHandling;
    public ClayTransitionElementConfigEnter enter;
    public ClayTransitionElementConfigExit exit;
}
