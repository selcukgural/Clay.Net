using Clay.Csharp.Raylib;
using Clay.Csharp.Structs;
using Raylib_cs;
using Xunit;

namespace Clay.Csharp.Raylib.Tests;

public class ColorConversionTests
{
    [Fact]
    public void ToRaylibColor_ConvertsChannelsDirectly()
    {
        ClayColor input = new() { r = 80, g = 120, b = 200, a = 255 };
        Color result = ClayRaylibRenderer.ToRaylibColor(input);

        Assert.Equal((byte)80, result.R);
        Assert.Equal((byte)120, result.G);
        Assert.Equal((byte)200, result.B);
        Assert.Equal((byte)255, result.A);
    }

    [Theory]
    [InlineData(-10f, 0)]
    [InlineData(0f, 0)]
    [InlineData(255f, 255)]
    [InlineData(300f, 255)]
    public void ToRaylibColor_ClampsOutOfRangeChannels(float channel, byte expected)
    {
        ClayColor input = new() { r = channel, g = channel, b = channel, a = channel };
        Color result = ClayRaylibRenderer.ToRaylibColor(input);

        Assert.Equal(expected, result.R);
        Assert.Equal(expected, result.G);
        Assert.Equal(expected, result.B);
        Assert.Equal(expected, result.A);
    }
}
