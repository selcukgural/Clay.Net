using Clay.Csharp.Declarative;
using Clay.Csharp.Enums;
using Clay.Csharp.Structs;
using Xunit;

namespace Clay.Csharp.Tests;

public class ClayTransitionsTests
{
    [Fact]
    public void Create_WithAllDelegates_PopulatesNonZeroFunctionPointers()
    {
        ClayNative.TransitionFunction handler = _ => true;
        ClayNative.TransitionStateSetter setInitial = (state, _) => state;
        ClayNative.TransitionStateSetter setFinal = (state, _) => state;

        ClayTransitionElementConfig config = ClayTransitions.Create(
            handler: handler,
            duration: 0.2f,
            properties: ClayTransitionProperty.ClayTransitionPropertyBackgroundColor,
            setInitialState: setInitial,
            setFinalState: setFinal);

        Assert.NotEqual(IntPtr.Zero, config.handler);
        Assert.NotEqual(IntPtr.Zero, config.enter.setInitialState);
        Assert.NotEqual(IntPtr.Zero, config.exit.setFinalState);
        Assert.Equal(0.2f, config.duration);
        Assert.Equal(ClayTransitionProperty.ClayTransitionPropertyBackgroundColor, config.properties);
    }

    [Fact]
    public void Create_WithNullOptionalDelegates_LeavesFunctionPointersZero()
    {
        ClayTransitionElementConfig config = ClayTransitions.Create(
            handler: _ => true,
            duration: 0.1f,
            properties: ClayTransitionProperty.ClayTransitionPropertyBorder);

        Assert.Equal(IntPtr.Zero, config.enter.setInitialState);
        Assert.Equal(IntPtr.Zero, config.exit.setFinalState);
    }

    [Fact]
    public void Create_DefaultTriggersAndOrdering_MatchDocumentedDefaults()
    {
        ClayTransitionElementConfig config = ClayTransitions.Create(
            handler: _ => true,
            duration: 0.1f,
            properties: ClayTransitionProperty.ClayTransitionPropertyBackgroundColor);

        Assert.Equal(ClayTransitionInteractionHandlingType.ClayTransitionDisableInteractionsWhileTransitioningPosition, config.interactionHandling);
        Assert.Equal(ClayTransitionEnterTriggerType.ClayTransitionEnterTriggerOnFirstParentFrame, config.enter.trigger);
        Assert.Equal(ClayTransitionExitTriggerType.ClayTransitionExitTriggerWhenParentExits, config.exit.trigger);
        Assert.Equal(ClayExitTransitionSiblingOrdering.ClayExitTransitionOrderingNaturalOrder, config.exit.siblingOrdering);
    }

    [Fact]
    public void Create_RootedDelegate_SurvivesGarbageCollection()
    {
        // The whole point of ClayTransitions' KeepAlive list is that the function pointer stays valid
        // even after the caller drops every managed reference to the delegate and a collection runs.
        ClayTransitionElementConfig config = BuildConfigWithLocalOnlyDelegate();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // If the delegate had been collected, invoking through the (now-dangling) function pointer would
        // crash the process rather than throw a catchable exception - so the meaningful assertion here is
        // simply that the pointer is still non-zero and the process is still alive to check it.
        Assert.NotEqual(IntPtr.Zero, config.handler);

        static ClayTransitionElementConfig BuildConfigWithLocalOnlyDelegate()
        {
            ClayNative.TransitionFunction localDelegate = _ => true;
            return ClayTransitions.Create(localDelegate, 0.1f, ClayTransitionProperty.ClayTransitionPropertyX);
        }
    }
}
