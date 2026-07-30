using Clay.Csharp.Enums;
using Xunit;

namespace Clay.Csharp.Tests;

/// <summary>
/// Every enum's underlying type (byte for CLAY_PACKED_ENUM, the default int/uint for the two enums that
/// are deliberately NOT packed - Clay_TransitionState, Clay_TransitionProperty) and every member's exact
/// numeric value, against native/clay_native/third_party/clay/clay.h.
/// </summary>
public class EnumValueTests
{
    private static void AssertEnum<T>(Type expectedUnderlyingType, params (T value, long expected)[] members)
        where T : struct, Enum
    {
        Assert.Equal(expectedUnderlyingType, Enum.GetUnderlyingType(typeof(T)));
        foreach ((T value, long expected) in members)
        {
            Assert.Equal(expected, Convert.ToInt64(value));
        }
    }

    [Fact]
    public void ClayErrorType_Values() => AssertEnum<ClayErrorType>(typeof(byte),
        (ClayErrorType.ClayErrorTypeTextMeasurementFunctionNotProvided, 0),
        (ClayErrorType.ClayErrorTypeArenaCapacityExceeded, 1),
        (ClayErrorType.ClayErrorTypeElementsCapacityExceeded, 2),
        (ClayErrorType.ClayErrorTypeTextMeasurementCapacityExceeded, 3),
        (ClayErrorType.ClayErrorTypeDuplicateId, 4),
        (ClayErrorType.ClayErrorTypeFloatingContainerParentNotFound, 5),
        (ClayErrorType.ClayErrorTypePercentageOver1, 6),
        (ClayErrorType.ClayErrorTypeInternalError, 7),
        (ClayErrorType.ClayErrorTypeUnbalancedOpenClose, 8),
        (ClayErrorType.ClayErrorTypeHashMapCapacityExceeded, 9));

    [Fact]
    public void ClayExitTransitionSiblingOrdering_Values() => AssertEnum<ClayExitTransitionSiblingOrdering>(typeof(byte),
        (ClayExitTransitionSiblingOrdering.ClayExitTransitionOrderingUnderneathSiblings, 0),
        (ClayExitTransitionSiblingOrdering.ClayExitTransitionOrderingNaturalOrder, 1),
        (ClayExitTransitionSiblingOrdering.ClayExitTransitionOrderingAboveSiblings, 2));

    [Fact]
    public void ClayFloatingAttachPointType_Values() => AssertEnum<ClayFloatingAttachPointType>(typeof(byte),
        (ClayFloatingAttachPointType.ClayAttachPointLeftTop, 0),
        (ClayFloatingAttachPointType.ClayAttachPointLeftCenter, 1),
        (ClayFloatingAttachPointType.ClayAttachPointLeftBottom, 2),
        (ClayFloatingAttachPointType.ClayAttachPointCenterTop, 3),
        (ClayFloatingAttachPointType.ClayAttachPointCenterCenter, 4),
        (ClayFloatingAttachPointType.ClayAttachPointCenterBottom, 5),
        (ClayFloatingAttachPointType.ClayAttachPointRightTop, 6),
        (ClayFloatingAttachPointType.ClayAttachPointRightCenter, 7),
        (ClayFloatingAttachPointType.ClayAttachPointRightBottom, 8));

    [Fact]
    public void ClayFloatingAttachToElement_Values() => AssertEnum<ClayFloatingAttachToElement>(typeof(byte),
        (ClayFloatingAttachToElement.ClayAttachToNone, 0),
        (ClayFloatingAttachToElement.ClayAttachToParent, 1),
        (ClayFloatingAttachToElement.ClayAttachToElementWithId, 2),
        (ClayFloatingAttachToElement.ClayAttachToRoot, 3));

    [Fact]
    public void ClayFloatingClipToElement_Values() => AssertEnum<ClayFloatingClipToElement>(typeof(byte),
        (ClayFloatingClipToElement.ClayClipToNone, 0),
        (ClayFloatingClipToElement.ClayClipToAttachedParent, 1));

    [Fact]
    public void ClayLayoutAlignmentX_Values() => AssertEnum<ClayLayoutAlignmentX>(typeof(byte),
        (ClayLayoutAlignmentX.ClayAlignXLeft, 0),
        (ClayLayoutAlignmentX.ClayAlignXRight, 1),
        (ClayLayoutAlignmentX.ClayAlignXCenter, 2));

    [Fact]
    public void ClayLayoutAlignmentY_Values() => AssertEnum<ClayLayoutAlignmentY>(typeof(byte),
        (ClayLayoutAlignmentY.ClayAlignYTop, 0),
        (ClayLayoutAlignmentY.ClayAlignYBottom, 1),
        (ClayLayoutAlignmentY.ClayAlignYCenter, 2));

    [Fact]
    public void ClayLayoutDirection_Values() => AssertEnum<ClayLayoutDirection>(typeof(byte),
        (ClayLayoutDirection.ClayLeftToRight, 0),
        (ClayLayoutDirection.ClayTopToBottom, 1));

    [Fact]
    public void ClayPointerCaptureMode_Values() => AssertEnum<ClayPointerCaptureMode>(typeof(byte),
        (ClayPointerCaptureMode.ClayPointerCaptureModeCapture, 0),
        (ClayPointerCaptureMode.ClayPointerCaptureModePassthrough, 1));

    [Fact]
    public void ClayPointerDataInteractionState_Values() => AssertEnum<ClayPointerDataInteractionState>(typeof(byte),
        (ClayPointerDataInteractionState.ClayPointerDataPressedThisFrame, 0),
        (ClayPointerDataInteractionState.ClayPointerDataPressed, 1),
        (ClayPointerDataInteractionState.ClayPointerDataReleasedThisFrame, 2),
        (ClayPointerDataInteractionState.ClayPointerDataReleased, 3));

    [Fact]
    public void ClayRenderCommandType_Values() => AssertEnum<ClayRenderCommandType>(typeof(byte),
        (ClayRenderCommandType.ClayRenderCommandTypeNone, 0),
        (ClayRenderCommandType.ClayRenderCommandTypeRectangle, 1),
        (ClayRenderCommandType.ClayRenderCommandTypeBorder, 2),
        (ClayRenderCommandType.ClayRenderCommandTypeText, 3),
        (ClayRenderCommandType.ClayRenderCommandTypeImage, 4),
        (ClayRenderCommandType.ClayRenderCommandTypeScissorStart, 5),
        (ClayRenderCommandType.ClayRenderCommandTypeScissorEnd, 6),
        (ClayRenderCommandType.ClayRenderCommandTypeOverlayColorStart, 7),
        (ClayRenderCommandType.ClayRenderCommandTypeOverlayColorEnd, 8),
        (ClayRenderCommandType.ClayRenderCommandTypeCustom, 9));

    [Fact]
    public void ClaySizingType_Values() => AssertEnum<ClaySizingType>(typeof(byte),
        (ClaySizingType.ClaySizingTypeFit, 0),
        (ClaySizingType.ClaySizingTypeGrow, 1),
        (ClaySizingType.ClaySizingTypePercent, 2),
        (ClaySizingType.ClaySizingTypeFixed, 3));

    [Fact]
    public void ClayTextAlignment_Values() => AssertEnum<ClayTextAlignment>(typeof(byte),
        (ClayTextAlignment.ClayTextAlignLeft, 0),
        (ClayTextAlignment.ClayTextAlignCenter, 1),
        (ClayTextAlignment.ClayTextAlignRight, 2));

    [Fact]
    public void ClayTextElementConfigWrapMode_Values() => AssertEnum<ClayTextElementConfigWrapMode>(typeof(byte),
        (ClayTextElementConfigWrapMode.ClayTextWrapWords, 0),
        (ClayTextElementConfigWrapMode.ClayTextWrapNewlines, 1),
        (ClayTextElementConfigWrapMode.ClayTextWrapNone, 2));

    [Fact]
    public void ClayTransitionEnterTriggerType_Values() => AssertEnum<ClayTransitionEnterTriggerType>(typeof(byte),
        (ClayTransitionEnterTriggerType.ClayTransitionEnterSkipOnFirstParentFrame, 0),
        (ClayTransitionEnterTriggerType.ClayTransitionEnterTriggerOnFirstParentFrame, 1));

    [Fact]
    public void ClayTransitionExitTriggerType_Values() => AssertEnum<ClayTransitionExitTriggerType>(typeof(byte),
        (ClayTransitionExitTriggerType.ClayTransitionExitSkipWhenParentExits, 0),
        (ClayTransitionExitTriggerType.ClayTransitionExitTriggerWhenParentExits, 1));

    [Fact]
    public void ClayTransitionInteractionHandlingType_Values() => AssertEnum<ClayTransitionInteractionHandlingType>(typeof(byte),
        (ClayTransitionInteractionHandlingType.ClayTransitionDisableInteractionsWhileTransitioningPosition, 0),
        (ClayTransitionInteractionHandlingType.ClayTransitionAllowInteractionsWhileTransitioningPosition, 1));

    [Fact]
    public void ClayTransitionProperty_IsUintBacked_NotPacked() =>
        Assert.Equal(typeof(uint), Enum.GetUnderlyingType(typeof(ClayTransitionProperty)));

    [Fact]
    public void ClayTransitionProperty_Values() => AssertEnum<ClayTransitionProperty>(typeof(uint),
        (ClayTransitionProperty.ClayTransitionPropertyNone, 0),
        (ClayTransitionProperty.ClayTransitionPropertyX, 1),
        (ClayTransitionProperty.ClayTransitionPropertyY, 2),
        (ClayTransitionProperty.ClayTransitionPropertyPosition, 3),
        (ClayTransitionProperty.ClayTransitionPropertyWidth, 4),
        (ClayTransitionProperty.ClayTransitionPropertyHeight, 8),
        (ClayTransitionProperty.ClayTransitionPropertyDimensions, 12),
        (ClayTransitionProperty.ClayTransitionPropertyBoundingBox, 15),
        (ClayTransitionProperty.ClayTransitionPropertyBackgroundColor, 16),
        (ClayTransitionProperty.ClayTransitionPropertyOverlayColor, 32),
        (ClayTransitionProperty.ClayTransitionPropertyCornerRadius, 64),
        (ClayTransitionProperty.ClayTransitionPropertyBorderColor, 128),
        (ClayTransitionProperty.ClayTransitionPropertyBorderWidth, 256),
        (ClayTransitionProperty.ClayTransitionPropertyBorder, 384));

    [Fact]
    public void ClayTransitionState_IsIntBacked_NotPacked() =>
        Assert.Equal(typeof(int), Enum.GetUnderlyingType(typeof(ClayTransitionState)));

    [Fact]
    public void ClayTransitionState_Values() => AssertEnum<ClayTransitionState>(typeof(int),
        (ClayTransitionState.ClayTransitionStateIdle, 0),
        (ClayTransitionState.ClayTransitionStateEntering, 1),
        (ClayTransitionState.ClayTransitionStateTransitioning, 2),
        (ClayTransitionState.ClayTransitionStateExiting, 3));
}
