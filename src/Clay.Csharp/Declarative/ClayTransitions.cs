using System.Runtime.InteropServices;
using Clay.Csharp.Enums;
using Clay.Csharp.Structs;

namespace Clay.Csharp.Declarative;

/// <summary>
/// Builds Clay_TransitionElementConfig values from managed delegates.
///
/// Clay stores these as raw C function pointers inside layout elements and may invoke them on later
/// frames (while the transition is in progress, or on element exit) - there is no "unregister" call, so a
/// delegate handed to Clay must not be garbage collected for as long as it might still be called. This
/// class permanently roots every delegate it marshals, which is the standard, simple tradeoff for wrapping
/// long-lived native callbacks: it leaks a delegate reference per distinct transition config for the
/// process lifetime, which in practice is bounded by the (small, static) set of transition configs an app
/// declares, not by element/frame count.
/// </summary>
public static class ClayTransitions
{
    private static readonly List<Delegate> KeepAlive = new();

    /// <summary>
    /// Builds a Clay_TransitionElementConfig. Pass null for setInitialState/setFinalState to use Clay's
    /// default behavior of animating from/to the element's current on-screen state.
    /// </summary>
    public static ClayTransitionElementConfig Create(
        ClayNative.TransitionFunction handler,
        float duration,
        ClayTransitionProperty properties,
        ClayTransitionInteractionHandlingType interactionHandling = ClayTransitionInteractionHandlingType.ClayTransitionDisableInteractionsWhileTransitioningPosition,
        ClayTransitionEnterTriggerType enterTrigger = ClayTransitionEnterTriggerType.ClayTransitionEnterTriggerOnFirstParentFrame,
        ClayNative.TransitionStateSetter? setInitialState = null,
        ClayTransitionExitTriggerType exitTrigger = ClayTransitionExitTriggerType.ClayTransitionExitTriggerWhenParentExits,
        ClayExitTransitionSiblingOrdering siblingOrdering = ClayExitTransitionSiblingOrdering.ClayExitTransitionOrderingNaturalOrder,
        ClayNative.TransitionStateSetter? setFinalState = null)
    {
        return new ClayTransitionElementConfig
        {
            handler = Pin(handler),
            duration = duration,
            properties = properties,
            interactionHandling = interactionHandling,
            enter = new ClayTransitionElementConfigEnter
            {
                setInitialState = Pin(setInitialState),
                trigger = enterTrigger,
            },
            exit = new ClayTransitionElementConfigExit
            {
                setFinalState = Pin(setFinalState),
                trigger = exitTrigger,
                siblingOrdering = siblingOrdering,
            },
        };
    }

    /// <summary>
    /// Uses Clay's built-in "Ease Out" curve (Clay_EaseOut) as the transition handler.
    /// </summary>
    public static ClayNative.TransitionFunction EaseOut { get; } = ClayNative.Clay_EaseOut;

    private static IntPtr Pin(Delegate? callback)
    {
        if (callback is null)
        {
            return IntPtr.Zero;
        }

        KeepAlive.Add(callback);
        return Marshal.GetFunctionPointerForDelegate(callback);
    }
}
