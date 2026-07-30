using System.Runtime.InteropServices;
using Clay.Csharp.Structs;
using Xunit;

namespace Clay.Csharp.Tests;

/// <summary>
/// Marshal.SizeOf&lt;T&gt;() for every struct that mirrors a native Clay_* type, against sizes hand-derived
/// from native/clay_native/third_party/clay/clay.h and independently cross-checked against the real,
/// compiler-computed sizes returned by ClayNative_GetAbiSizes() (see NativeAbiSizeTests). This is the
/// tier of protection that runs on every OS unconditionally, with no native library required - it would
/// have caught this session's original ClayHelpers.GetElementId-adjacent bugs (wrong field presence/size)
/// and did in fact catch a real one during construction: ClayRectangleRenderData carried a stray
/// boundingBox field that doesn't exist in clay.h, inflating it from 32 to 48 bytes.
/// </summary>
public class AbiSizeTests
{
    [Theory]
    [InlineData(typeof(ClayArena), 24)]
    [InlineData(typeof(ClayAspectRatioElementConfig), 4)]
    [InlineData(typeof(ClayBorderElementConfig), 28)]
    [InlineData(typeof(ClayBorderRenderData), 44)]
    [InlineData(typeof(ClayBorderWidth), 10)]
    [InlineData(typeof(ClayBoundingBox), 16)]
    [InlineData(typeof(ClayChildAlignment), 2)]
    [InlineData(typeof(ClayClipElementConfig), 12)]
    [InlineData(typeof(ClayClipRenderData), 2)]
    [InlineData(typeof(ClayColor), 16)]
    [InlineData(typeof(ClayCornerRadius), 16)]
    [InlineData(typeof(ClayCustomElementConfig), 8)]
    [InlineData(typeof(ClayCustomRenderData), 40)]
    [InlineData(typeof(ClayDimensions), 8)]
    [InlineData(typeof(ClayElementData), 20)]
    [InlineData(typeof(ClayElementDeclaration), 248)]
    [InlineData(typeof(ClayElementId), 32)]
    [InlineData(typeof(ClayElementIdArray), 16)]
    [InlineData(typeof(ClayErrorData), 32)]
    [InlineData(typeof(ClayErrorHandler), 16)]
    [InlineData(typeof(ClayFloatingAttachPoints), 2)]
    [InlineData(typeof(ClayFloatingElementConfig), 28)]
    [InlineData(typeof(ClayImageElementConfig), 8)]
    [InlineData(typeof(ClayImageRenderData), 40)]
    [InlineData(typeof(ClayLayoutConfig), 40)]
    [InlineData(typeof(ClayOverlayColorRenderData), 16)]
    [InlineData(typeof(ClayPadding), 8)]
    [InlineData(typeof(ClayPointerData), 12)]
    [InlineData(typeof(ClayRectangleRenderData), 32)]
    [InlineData(typeof(ClayRenderCommand), 80)]
    [InlineData(typeof(ClayRenderCommandArray), 16)]
    [InlineData(typeof(ClayRenderData), 48)]
    [InlineData(typeof(ClayScrollContainerData), 40)]
    [InlineData(typeof(ClaySizing), 24)]
    [InlineData(typeof(ClaySizingAxis), 12)]
    [InlineData(typeof(ClaySizingMinMax), 8)]
    [InlineData(typeof(ClayString), 16)]
    [InlineData(typeof(ClayStringSlice), 24)]
    [InlineData(typeof(ClayTextElementConfig), 40)]
    [InlineData(typeof(ClayTextRenderData), 48)]
    [InlineData(typeof(ClayTransitionCallbackArguments), 176)]
    [InlineData(typeof(ClayTransitionData), 76)]
    [InlineData(typeof(ClayTransitionElementConfig), 56)]
    [InlineData(typeof(ClayTransitionElementConfigEnter), 16)]
    [InlineData(typeof(ClayTransitionElementConfigExit), 16)]
    [InlineData(typeof(ClayVector2), 8)]
    public void StructSize_MatchesClayH(Type type, int expectedSize)
    {
        Assert.Equal(expectedSize, Marshal.SizeOf(type));
    }
}
