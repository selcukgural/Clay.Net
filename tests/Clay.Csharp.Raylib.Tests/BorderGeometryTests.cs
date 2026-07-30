using System.Numerics;
using Clay.Csharp.Raylib;
using Clay.Csharp.Structs;
using Xunit;

namespace Clay.Csharp.Raylib.Tests;

/// <summary>
/// Pure geometry tests for the corner-radius-aware border math added this session (fixing a bug where
/// borders were always drawn as square-cornered bars regardless of the element's rounding). No raylib
/// draw calls are involved - ComputeBorderBar/ComputeBorderCorner only compute positions/sizes/angles.
/// </summary>
public class BorderGeometryTests
{
    private static readonly ClayBoundingBox Box = new() { x = 10, y = 20, width = 100, height = 80 };
    private static readonly ClayBorderWidth UniformWidth4 = new() { left = 4, right = 4, top = 4, bottom = 4 };
    private static readonly ClayCornerRadius UniformRadius8 = new() { topLeft = 8, topRight = 8, bottomLeft = 8, bottomRight = 8 };

    [Fact]
    public void ComputeBorderBar_Left_ShortenedByAdjacentCornerRadii()
    {
        (Vector2 position, Vector2 size) = ClayRaylibRenderer.ComputeBorderBar(BorderSide.Left, Box, UniformWidth4, UniformRadius8);
        Assert.Equal(new Vector2(10, 28), position);
        Assert.Equal(new Vector2(4, 64), size);
    }

    [Fact]
    public void ComputeBorderBar_Right_ShortenedByAdjacentCornerRadii()
    {
        (Vector2 position, Vector2 size) = ClayRaylibRenderer.ComputeBorderBar(BorderSide.Right, Box, UniformWidth4, UniformRadius8);
        Assert.Equal(new Vector2(106, 28), position);
        Assert.Equal(new Vector2(4, 64), size);
    }

    [Fact]
    public void ComputeBorderBar_Top_ShortenedByAdjacentCornerRadii()
    {
        (Vector2 position, Vector2 size) = ClayRaylibRenderer.ComputeBorderBar(BorderSide.Top, Box, UniformWidth4, UniformRadius8);
        Assert.Equal(new Vector2(18, 20), position);
        Assert.Equal(new Vector2(84, 4), size);
    }

    [Fact]
    public void ComputeBorderBar_Bottom_ShortenedByAdjacentCornerRadii()
    {
        (Vector2 position, Vector2 size) = ClayRaylibRenderer.ComputeBorderBar(BorderSide.Bottom, Box, UniformWidth4, UniformRadius8);
        Assert.Equal(new Vector2(18, 96), position);
        Assert.Equal(new Vector2(84, 4), size);
    }

    [Fact]
    public void ComputeBorderBar_NoRounding_SpansFullEdge()
    {
        ClayCornerRadius none = default;
        (Vector2 position, Vector2 size) = ClayRaylibRenderer.ComputeBorderBar(BorderSide.Left, Box, UniformWidth4, none);
        Assert.Equal(new Vector2(10, 20), position);
        Assert.Equal(new Vector2(4, 80), size); // full box height, not shortened - this is the bug fixed this session
    }

    [Theory]
    [InlineData(BorderCorner.TopLeft, 18, 28, 180f, 270f)]
    [InlineData(BorderCorner.TopRight, 102, 28, 270f, 360f)]
    [InlineData(BorderCorner.BottomLeft, 18, 92, 90f, 180f)]
    [InlineData(BorderCorner.BottomRight, 102, 92, 0.1f, 90f)]
    public void ComputeBorderCorner_CenterAndAngles_MatchExpectedQuadrant(BorderCorner corner, float expectedCenterX, float expectedCenterY, float expectedStart, float expectedEnd)
    {
        (Vector2 center, float inner, float outer, float start, float end) = ClayRaylibRenderer.ComputeBorderCorner(corner, Box, UniformRadius8, UniformWidth4);

        Assert.Equal(new Vector2(expectedCenterX, expectedCenterY), center);
        Assert.Equal(4, inner); // outer radius (8) - border width (4)
        Assert.Equal(8, outer);
        Assert.Equal(expectedStart, start);
        Assert.Equal(expectedEnd, end);
    }
}
