using Clay.Csharp.Raylib;
using Clay.Csharp.Structs;
using Xunit;

namespace Clay.Csharp.Raylib.Tests;

/// <summary>
/// Pure tests for UnionBoundingBox, the helper that approximates an overlaid element's true bounds from
/// everything actually drawn inside it - see its doc comment in ClayRaylibRenderer for why this is needed
/// at all (clay.h doesn't populate boundingBox on OVERLAY_COLOR_START/END commands).
/// </summary>
public class OverlayColorTests
{
    [Fact]
    public void UnionBoundingBox_NoAccumulatedBox_ReturnsTheGivenBox()
    {
        ClayBoundingBox box = new() { x = 10, y = 20, width = 30, height = 40 };
        ClayBoundingBox? result = ClayRaylibRenderer.UnionBoundingBox(null, box);
        Assert.Equal(box.x, result!.Value.x);
        Assert.Equal(box.y, result.Value.y);
        Assert.Equal(box.width, result.Value.width);
        Assert.Equal(box.height, result.Value.height);
    }

    [Fact]
    public void UnionBoundingBox_ZeroSizedBox_LeavesAccumulatedBoxUnchanged()
    {
        // This is what makes OVERLAY_COLOR_START/END's own (always zero-sized) boundingBox a no-op input.
        ClayBoundingBox accumulated = new() { x = 10, y = 20, width = 30, height = 40 };
        ClayBoundingBox zero = new() { x = 999, y = 999, width = 0, height = 0 };

        ClayBoundingBox? result = ClayRaylibRenderer.UnionBoundingBox(accumulated, zero);

        Assert.Equal(accumulated.x, result!.Value.x);
        Assert.Equal(accumulated.y, result.Value.y);
        Assert.Equal(accumulated.width, result.Value.width);
        Assert.Equal(accumulated.height, result.Value.height);
    }

    [Fact]
    public void UnionBoundingBox_DisjointBoxes_ReturnsSmallestEnclosingBox()
    {
        ClayBoundingBox a = new() { x = 0, y = 0, width = 10, height = 10 };
        ClayBoundingBox b = new() { x = 50, y = 50, width = 10, height = 10 };

        ClayBoundingBox? result = ClayRaylibRenderer.UnionBoundingBox(a, b);

        Assert.Equal(0, result!.Value.x);
        Assert.Equal(0, result.Value.y);
        Assert.Equal(60, result.Value.width);
        Assert.Equal(60, result.Value.height);
    }

    [Fact]
    public void UnionBoundingBox_BoxFullyInsideAccumulated_LeavesAccumulatedBoxUnchanged()
    {
        ClayBoundingBox outer = new() { x = 0, y = 0, width = 100, height = 100 };
        ClayBoundingBox inner = new() { x = 10, y = 10, width = 5, height = 5 };

        ClayBoundingBox? result = ClayRaylibRenderer.UnionBoundingBox(outer, inner);

        Assert.Equal(outer.x, result!.Value.x);
        Assert.Equal(outer.y, result.Value.y);
        Assert.Equal(outer.width, result.Value.width);
        Assert.Equal(outer.height, result.Value.height);
    }
}
