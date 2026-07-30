using Clay.Csharp.Declarative;
using Clay.Csharp.Enums;
using Clay.Csharp.Internal;
using Clay.Csharp.Structs;
using Xunit;

namespace Clay.Csharp.Tests;

/// <summary>
/// Formalizes two scenarios discovered/verified by hand this session:
/// 1. Clay_SetPointerState scans the *previously committed* frame's tree (not the one being declared),
///    so OnHover only fires from the second frame a pointer sits over an element onward.
/// 2. Attaching a ClayTransitions-built config to an element must round-trip through Clay's internal
///    element storage without corrupting anything - if ClayTransitionElementConfig's layout were wrong,
///    this would corrupt the whole ClayElementDeclaration it's embedded in, well beyond just breaking
///    transitions.
/// </summary>
[Collection("ClayNative")]
[Trait("RequiresNative", "true")]
public class HoverAndTransitionTests
{
    [Fact]
    public void OnHover_FiresFromSecondFrameOnward_ForPointerOverPreviousFramesBox()
    {
        using ClayTestContext context = new(300, 200);

        bool hovered = false;
        ClayNative.OnHoverFunction onHover = (_, _, _) => hovered = true;

        // "Box" ends up laid out at (16, 40, 268, 50) with these exact declarations - point the pointer
        // inside it up front.
        ClayVector2 pointerPosition = ClayHelpers.CreateVector2(100, 60);

        void DeclareFrame()
        {
            Layout.BeginLayout();
            using (Layout.Element("Root", new ClayElementDeclaration
                   {
                       layout = new ClayLayoutConfig
                       {
                           sizing = new ClaySizing { width = ClaySizingAxis.Fixed(300), height = ClaySizingAxis.Fixed(200) },
                           padding = ClayHelpers.CreatePaddingUniform(16),
                           childGap = 8,
                           layoutDirection = ClayLayoutDirection.ClayTopToBottom,
                       },
                   }))
            {
                Layout.Text("Hello, Clay.Net!", new ClayTextElementConfig { fontSize = 16 });
                using (Layout.Element("Box", new ClayElementDeclaration
                       {
                           layout = new ClayLayoutConfig
                           {
                               sizing = new ClaySizing { width = ClaySizingAxis.Grow(), height = ClaySizingAxis.Fixed(50) },
                           },
                       }))
                {
                    ClayNative.Clay_OnHover(onHover, IntPtr.Zero);
                }
            }

            Layout.EndLayout(deltaTime: 0.016f);
        }

        ClayNative.Clay_SetPointerState(pointerPosition, pointerDown: false);
        DeclareFrame();
        Assert.False(hovered); // no previously-committed tree yet on the very first frame

        ClayNative.Clay_SetPointerState(pointerPosition, pointerDown: false);
        DeclareFrame();
        Assert.True(hovered); // now scans frame 1's committed tree, which has "Box" at the pointer position

        Assert.Empty(context.Errors);
    }

    [Fact]
    public void TransitionConfig_AttachedToElement_DoesNotCorruptLayoutOrCrash()
    {
        using ClayTestContext context = new(300, 200);

        for (int frame = 0; frame < 3; frame++)
        {
            Layout.BeginLayout();
            using (Layout.Element("Root", new ClayElementDeclaration
                   {
                       layout = new ClayLayoutConfig
                       {
                           sizing = new ClaySizing { width = ClaySizingAxis.Fixed(300), height = ClaySizingAxis.Fixed(200) },
                       },
                   }))
            {
                using (Layout.Element("AnimatedBox", new ClayElementDeclaration
                       {
                           layout = new ClayLayoutConfig
                           {
                               sizing = new ClaySizing { width = ClaySizingAxis.Fixed(100), height = ClaySizingAxis.Fixed(50) },
                           },
                           backgroundColor = ClayHelpers.CreateColor(80, 120, 200),
                           transition = ClayTransitions.Create(
                               handler: args => args.transitionState != ClayTransitionState.ClayTransitionStateIdle,
                               duration: 0.2f,
                               properties: ClayTransitionProperty.ClayTransitionPropertyBackgroundColor),
                       }))
                {
                }
            }

            ClayRenderCommandArray commands = Layout.EndLayout(deltaTime: 0.016f);
            Assert.True(commands.length > 0);
        }

        Assert.Empty(context.Errors);
    }
}
