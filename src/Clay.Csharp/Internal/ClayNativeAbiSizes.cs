using System.Runtime.InteropServices;

namespace Clay.Csharp.Internal;

/// <summary>
/// Mirrors native/clay_native's ClayNative_AbiSizes: real, compiler-computed sizeof(...) for every
/// Clay_* struct the C# bindings mirror. Used by tests to cross-check Marshal.SizeOf&lt;T&gt;() against
/// ground truth instead of relying purely on hand-derived constants. Field order is a fixed contract
/// with clay_native.h - append-only, never reorder.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct ClayNativeAbiSizes
{
    public uint SizeofClayArena;
    public uint SizeofClayAspectRatioElementConfig;
    public uint SizeofClayBorderElementConfig;
    public uint SizeofClayBorderRenderData;
    public uint SizeofClayBorderWidth;
    public uint SizeofClayBoundingBox;
    public uint SizeofClayChildAlignment;
    public uint SizeofClayClipElementConfig;
    public uint SizeofClayClipRenderData;
    public uint SizeofClayColor;
    public uint SizeofClayCornerRadius;
    public uint SizeofClayCustomElementConfig;
    public uint SizeofClayCustomRenderData;
    public uint SizeofClayDimensions;
    public uint SizeofClayElementData;
    public uint SizeofClayElementDeclaration;
    public uint SizeofClayElementId;
    public uint SizeofClayElementIdArray;
    public uint SizeofClayErrorData;
    public uint SizeofClayErrorHandler;
    public uint SizeofClayFloatingAttachPoints;
    public uint SizeofClayFloatingElementConfig;
    public uint SizeofClayImageElementConfig;
    public uint SizeofClayImageRenderData;
    public uint SizeofClayLayoutConfig;
    public uint SizeofClayOverlayColorRenderData;
    public uint SizeofClayPadding;
    public uint SizeofClayPointerData;
    public uint SizeofClayRectangleRenderData;
    public uint SizeofClayRenderCommand;
    public uint SizeofClayRenderCommandArray;
    public uint SizeofClayRenderData;
    public uint SizeofClayScrollContainerData;
    public uint SizeofClaySizing;
    public uint SizeofClaySizingAxis;
    public uint SizeofClaySizingMinMax;
    public uint SizeofClayString;
    public uint SizeofClayStringSlice;
    public uint SizeofClayTextElementConfig;
    public uint SizeofClayTextRenderData;
    public uint SizeofClayTransitionCallbackArguments;
    public uint SizeofClayTransitionData;
    public uint SizeofClayTransitionElementConfig;
    public uint SizeofClayTransitionElementConfigEnter;
    public uint SizeofClayTransitionElementConfigExit;
    public uint SizeofClayVector2;
}
