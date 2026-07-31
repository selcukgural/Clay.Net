using Clay.Csharp.Raylib;
using Clay.Csharp.Structs;
using Raylib_cs;
using Xunit;

namespace Clay.Csharp.Raylib.Tests;

/// <summary>Pure tests for ResolveImageTint's "all-zero backgroundColor means untinted" rule.</summary>
public class ImageTests
{
    [Fact]
    public void ResolveImageTint_AllZeroColor_ReturnsWhite()
    {
        Color tint = ClayRaylibRenderer.ResolveImageTint(new ClayColor { r = 0, g = 0, b = 0, a = 0 });
        Assert.Equal(Color.White, tint);
    }

    [Fact]
    public void ResolveImageTint_RealColor_IsUsedAsIs()
    {
        Color tint = ClayRaylibRenderer.ResolveImageTint(new ClayColor { r = 255, g = 0, b = 0, a = 128 });
        Assert.Equal(new Color(255, 0, 0, 128), tint);
    }

    [Fact]
    public void ResolveImageTint_ZeroAlphaButNonZeroChannel_IsNotTreatedAsUntinted()
    {
        // Only the exact all-zero case means "untinted" - anything else (even a fully transparent tint)
        // is passed through unchanged, matching clay.h's doc comment literally.
        Color tint = ClayRaylibRenderer.ResolveImageTint(new ClayColor { r = 10, g = 0, b = 0, a = 0 });
        Assert.Equal(new Color(10, 0, 0, 0), tint);
    }
}
