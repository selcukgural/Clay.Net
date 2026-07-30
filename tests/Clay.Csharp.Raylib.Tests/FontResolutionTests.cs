using Clay.Csharp.Raylib;
using Raylib_cs;
using Xunit;

namespace Clay.Csharp.Raylib.Tests;

public class FontResolutionTests
{
    [Fact]
    public void ResolveFont_ValidIndex_ReturnsThatFont()
    {
        Font first = default;
        Font second = new() { BaseSize = 42 };
        Font[] fonts = [first, second];

        Font resolved = ClayRaylibRenderer.ResolveFont(fonts, 1);

        Assert.Equal(42, resolved.BaseSize);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(5)]
    public void ResolveFont_OutOfRangeIndex_FallsBackToFontZero(int fontId)
    {
        Font zero = new() { BaseSize = 7 };
        Font[] fonts = [zero, new Font { BaseSize = 99 }];

        Font resolved = ClayRaylibRenderer.ResolveFont(fonts, fontId);

        Assert.Equal(7, resolved.BaseSize);
    }

    [Fact]
    public void ResolveFont_EmptyFontsArray_ThrowsInvalidOperationException()
    {
        Font[] empty = [];
        Assert.Throws<InvalidOperationException>(() => ClayRaylibRenderer.ResolveFont(empty, 0));
    }
}
