using Clay.Csharp.Declarative;
using Clay.Csharp.Enums;
using Clay.Csharp.Internal;
using Clay.Csharp.Structs;
using Xunit;

namespace Clay.Csharp.Tests;

/// <summary>
/// Formalizes the scenario used to manually verify this port end-to-end against the real native library
/// this session: a 300x200 fixed root with 16px padding and 8px child gap, containing a text element and
/// a Grow-width box, and asserts the exact resulting bounding boxes. This is the single strongest
/// regression test for the whole ABI layer - any struct layout bug tends to either crash outright or
/// produce visibly wrong numbers here, not just "close enough" ones.
/// </summary>
[Collection("ClayNative")]
public class LayoutComputationTests
{
    [Fact]
    public void RootTextAndGrowBox_ProduceExpectedBoundingBoxes()
    {
        using ClayTestContext context = new(300, 200);

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
                   backgroundColor = ClayHelpers.CreateColor(30, 30, 30),
               }))
        {
            Layout.Text("Hello, Clay.Net!", new ClayTextElementConfig { fontSize = 16 });

            using (Layout.Element("Box", new ClayElementDeclaration
                   {
                       layout = new ClayLayoutConfig
                       {
                           sizing = new ClaySizing { width = ClaySizingAxis.Grow(), height = ClaySizingAxis.Fixed(50) },
                       },
                       backgroundColor = ClayHelpers.CreateColor(80, 120, 200),
                   }))
            {
            }
        }

        ClayRenderCommandArray commands = Layout.EndLayout(deltaTime: 0.016f);

        Assert.Empty(context.Errors);
        Assert.Equal(3, commands.length);

        ClayRenderCommand root = ClayNative.Clay_RenderCommandArray_Get(ref commands, 0);
        Assert.Equal(ClayRenderCommandType.ClayRenderCommandTypeRectangle, root.commandType);
        AssertBox(root.boundingBox, 0, 0, 300, 200);

        ClayRenderCommand text = ClayNative.Clay_RenderCommandArray_Get(ref commands, 1);
        Assert.Equal(ClayRenderCommandType.ClayRenderCommandTypeText, text.commandType);
        // "Hello, Clay.Net!" is 16 chars -> 16*8=128 wide, 16 tall (matches ClayTestContext's measure fn).
        AssertBox(text.boundingBox, 16, 16, 128, 16);

        ClayRenderCommand box = ClayNative.Clay_RenderCommandArray_Get(ref commands, 2);
        Assert.Equal(ClayRenderCommandType.ClayRenderCommandTypeRectangle, box.commandType);
        // y = 16 (padding) + 16 (text height) + 8 (childGap) = 40; width = 300 - 2*16 (padding) = 268.
        AssertBox(box.boundingBox, 16, 40, 268, 50);
    }

    private static void AssertBox(ClayBoundingBox box, float x, float y, float width, float height)
    {
        Assert.Equal(x, box.x);
        Assert.Equal(y, box.y);
        Assert.Equal(width, box.width);
        Assert.Equal(height, box.height);
    }
}
