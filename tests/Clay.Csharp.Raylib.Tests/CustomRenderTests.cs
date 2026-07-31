using Clay.Csharp.Raylib;
using Clay.Csharp.Structs;
using Xunit;

namespace Clay.Csharp.Raylib.Tests;

/// <summary>Pure tests for ShouldInvokeCustom's null-pointer guard (mirrors upstream's own check).</summary>
public class CustomRenderTests
{
    [Fact]
    public void ShouldInvokeCustom_ZeroPointer_ReturnsFalse()
    {
        Assert.False(ClayRaylibRenderer.ShouldInvokeCustom(new ClayCustomRenderData { customData = IntPtr.Zero }));
    }

    [Fact]
    public void ShouldInvokeCustom_NonZeroPointer_ReturnsTrue()
    {
        Assert.True(ClayRaylibRenderer.ShouldInvokeCustom(new ClayCustomRenderData { customData = new IntPtr(1) }));
    }
}
