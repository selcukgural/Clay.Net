using System.Reflection;
using System.Runtime.InteropServices;
using Clay.Csharp.Internal;
using Clay.Csharp.Structs;
using Xunit;

namespace Clay.Csharp.Tests;

/// <summary>
/// Cross-checks Marshal.SizeOf&lt;T&gt;() for every bound struct against the real, compiler-computed
/// sizeof(...) reported by the native library itself (ClayNative_GetAbiSizes) - a self-maintaining
/// ground truth independent of the hand-derived constants in AbiSizeTests/AbiFieldOffsetTests, and the
/// only check that would catch a future upstream clay.h change silently breaking the bindings. This is
/// exactly the check that caught a real bug while this test suite was being built: ClayRectangleRenderData
/// carried a stray boundingBox field not present in clay.h, inflating it from 32 to 48 bytes - a mismatch
/// this test would have failed on immediately.
///
/// Requires the native clay_native library for the current platform - not available on every CI runner
/// yet (see README's Platform support table), hence the RequiresNative trait.
/// </summary>
[Trait("RequiresNative", "true")]
public class NativeAbiSizeTests
{
    // Maps each struct's simple name to its Clay.Csharp.Structs.* CLR type, matching the field naming
    // convention "Sizeof{Name}" on the native-returned ClayNativeAbiSizes struct.
    private static readonly string[] StructNames =
    [
        "ClayArena", "ClayAspectRatioElementConfig", "ClayBorderElementConfig", "ClayBorderRenderData",
        "ClayBorderWidth", "ClayBoundingBox", "ClayChildAlignment", "ClayClipElementConfig",
        "ClayClipRenderData", "ClayColor", "ClayCornerRadius", "ClayCustomElementConfig",
        "ClayCustomRenderData", "ClayDimensions", "ClayElementData", "ClayElementDeclaration",
        "ClayElementId", "ClayElementIdArray", "ClayErrorData", "ClayErrorHandler",
        "ClayFloatingAttachPoints", "ClayFloatingElementConfig", "ClayImageElementConfig",
        "ClayImageRenderData", "ClayLayoutConfig", "ClayOverlayColorRenderData", "ClayPadding",
        "ClayPointerData", "ClayRectangleRenderData", "ClayRenderCommand", "ClayRenderCommandArray",
        "ClayRenderData", "ClayScrollContainerData", "ClaySizing", "ClaySizingAxis", "ClaySizingMinMax",
        "ClayString", "ClayStringSlice", "ClayTextElementConfig", "ClayTextRenderData",
        "ClayTransitionCallbackArguments", "ClayTransitionData", "ClayTransitionElementConfig",
        "ClayTransitionElementConfigEnter", "ClayTransitionElementConfigExit", "ClayVector2",
    ];

    public static IEnumerable<object[]> AllStructNames => StructNames.Select(name => new object[] { name });

    [Theory]
    [MemberData(nameof(AllStructNames))]
    public void CSharpStructSize_MatchesNativeSizeof(string structName)
    {
        Type? csharpType = typeof(ClayVector2).Assembly.GetType($"Clay.Csharp.Structs.{structName}");
        Assert.True(csharpType is not null, $"No Clay.Csharp.Structs.{structName} type found.");

        ClayNativeAbiSizes abiSizes = ClayNativeInternal.ClayNative_GetAbiSizes();
        FieldInfo? field = typeof(ClayNativeAbiSizes).GetField($"Sizeof{structName}");
        Assert.True(field is not null, $"ClayNativeAbiSizes has no field Sizeof{structName}.");

        uint nativeSize = (uint)field!.GetValue(abiSizes)!;
        int csharpSize = Marshal.SizeOf(csharpType!);

        Assert.Equal((int)nativeSize, csharpSize);
    }
}
