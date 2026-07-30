using System.Runtime.InteropServices;
using Clay.Csharp.Structs;
using Xunit;

namespace Clay.Csharp.Tests;

/// <summary>
/// Field-level Marshal.OffsetOf&lt;T&gt;() checks for the structs most likely to hide a silent bug that a
/// total-size check alone wouldn't catch (a same-size field swap, a union member landing at the wrong
/// offset, incorrect padding around a bool/enum/pointer) - the highest-risk structs: unions, ones with
/// nested anonymous-struct equivalents, ones mixing pointers/bools/packed enums, and ones this session
/// specifically fixed. Simple structs (ClayVector2, ClayColor, ClayPadding, etc.) are covered by
/// AbiSizeTests only - reordering 2-4 same-typed sequential fields can't produce a silent bug the way
/// these can.
/// </summary>
public class AbiFieldOffsetTests
{
    [Fact]
    public void ClayString_FieldOffsets()
    {
        Assert.Equal(0, Marshal.OffsetOf<ClayString>(nameof(ClayString.isStaticallyAllocated)).ToInt32());
        Assert.Equal(4, Marshal.OffsetOf<ClayString>(nameof(ClayString.length)).ToInt32());
        Assert.Equal(8, Marshal.OffsetOf<ClayString>(nameof(ClayString.chars)).ToInt32());
    }

    [Fact]
    public void ClayElementId_FieldOffsets()
    {
        Assert.Equal(0, Marshal.OffsetOf<ClayElementId>(nameof(ClayElementId.id)).ToInt32());
        Assert.Equal(4, Marshal.OffsetOf<ClayElementId>(nameof(ClayElementId.offset)).ToInt32());
        Assert.Equal(8, Marshal.OffsetOf<ClayElementId>(nameof(ClayElementId.baseId)).ToInt32());
        Assert.Equal(16, Marshal.OffsetOf<ClayElementId>(nameof(ClayElementId.stringId)).ToInt32());
    }

    [Fact]
    public void ClaySizingAxis_UnionMembersShareOffsetZero_TypeFollowsAt8()
    {
        Assert.Equal(0, Marshal.OffsetOf<ClaySizingAxis>(nameof(ClaySizingAxis.minMax)).ToInt32());
        Assert.Equal(0, Marshal.OffsetOf<ClaySizingAxis>(nameof(ClaySizingAxis.percent)).ToInt32());
        Assert.Equal(8, Marshal.OffsetOf<ClaySizingAxis>(nameof(ClaySizingAxis.type)).ToInt32());
    }

    [Fact]
    public void ClayRenderData_AllUnionMembersShareOffsetZero()
    {
        Assert.Equal(0, Marshal.OffsetOf<ClayRenderData>(nameof(ClayRenderData.rectangle)).ToInt32());
        Assert.Equal(0, Marshal.OffsetOf<ClayRenderData>(nameof(ClayRenderData.text)).ToInt32());
        Assert.Equal(0, Marshal.OffsetOf<ClayRenderData>(nameof(ClayRenderData.image)).ToInt32());
        Assert.Equal(0, Marshal.OffsetOf<ClayRenderData>(nameof(ClayRenderData.custom)).ToInt32());
        Assert.Equal(0, Marshal.OffsetOf<ClayRenderData>(nameof(ClayRenderData.border)).ToInt32());
        Assert.Equal(0, Marshal.OffsetOf<ClayRenderData>(nameof(ClayRenderData.clip)).ToInt32());
        Assert.Equal(0, Marshal.OffsetOf<ClayRenderData>(nameof(ClayRenderData.overlayColor)).ToInt32());
    }

    [Fact]
    public void ClayFloatingElementConfig_FieldOffsets()
    {
        Assert.Equal(0, Marshal.OffsetOf<ClayFloatingElementConfig>(nameof(ClayFloatingElementConfig.offset)).ToInt32());
        Assert.Equal(8, Marshal.OffsetOf<ClayFloatingElementConfig>(nameof(ClayFloatingElementConfig.expand)).ToInt32());
        Assert.Equal(16, Marshal.OffsetOf<ClayFloatingElementConfig>(nameof(ClayFloatingElementConfig.parentId)).ToInt32());
        Assert.Equal(20, Marshal.OffsetOf<ClayFloatingElementConfig>(nameof(ClayFloatingElementConfig.zIndex)).ToInt32());
        Assert.Equal(22, Marshal.OffsetOf<ClayFloatingElementConfig>(nameof(ClayFloatingElementConfig.attachPoints)).ToInt32());
        Assert.Equal(24, Marshal.OffsetOf<ClayFloatingElementConfig>(nameof(ClayFloatingElementConfig.pointerCaptureMode)).ToInt32());
        Assert.Equal(25, Marshal.OffsetOf<ClayFloatingElementConfig>(nameof(ClayFloatingElementConfig.attachTo)).ToInt32());
        Assert.Equal(26, Marshal.OffsetOf<ClayFloatingElementConfig>(nameof(ClayFloatingElementConfig.clipTo)).ToInt32());
    }

    [Fact]
    public void ClayTextElementConfig_FieldOffsets()
    {
        Assert.Equal(0, Marshal.OffsetOf<ClayTextElementConfig>(nameof(ClayTextElementConfig.userData)).ToInt32());
        Assert.Equal(8, Marshal.OffsetOf<ClayTextElementConfig>(nameof(ClayTextElementConfig.textColor)).ToInt32());
        Assert.Equal(24, Marshal.OffsetOf<ClayTextElementConfig>(nameof(ClayTextElementConfig.fontId)).ToInt32());
        Assert.Equal(26, Marshal.OffsetOf<ClayTextElementConfig>(nameof(ClayTextElementConfig.fontSize)).ToInt32());
        Assert.Equal(28, Marshal.OffsetOf<ClayTextElementConfig>(nameof(ClayTextElementConfig.letterSpacing)).ToInt32());
        Assert.Equal(30, Marshal.OffsetOf<ClayTextElementConfig>(nameof(ClayTextElementConfig.lineHeight)).ToInt32());
        Assert.Equal(32, Marshal.OffsetOf<ClayTextElementConfig>(nameof(ClayTextElementConfig.wrapMode)).ToInt32());
        Assert.Equal(33, Marshal.OffsetOf<ClayTextElementConfig>(nameof(ClayTextElementConfig.textAlignment)).ToInt32());
    }

    [Fact]
    public void ClayTransitionElementConfigEnter_FieldOffsets()
    {
        Assert.Equal(0, Marshal.OffsetOf<ClayTransitionElementConfigEnter>(nameof(ClayTransitionElementConfigEnter.setInitialState)).ToInt32());
        Assert.Equal(8, Marshal.OffsetOf<ClayTransitionElementConfigEnter>(nameof(ClayTransitionElementConfigEnter.trigger)).ToInt32());
    }

    [Fact]
    public void ClayTransitionElementConfigExit_FieldOffsets()
    {
        Assert.Equal(0, Marshal.OffsetOf<ClayTransitionElementConfigExit>(nameof(ClayTransitionElementConfigExit.setFinalState)).ToInt32());
        Assert.Equal(8, Marshal.OffsetOf<ClayTransitionElementConfigExit>(nameof(ClayTransitionElementConfigExit.trigger)).ToInt32());
        Assert.Equal(9, Marshal.OffsetOf<ClayTransitionElementConfigExit>(nameof(ClayTransitionElementConfigExit.siblingOrdering)).ToInt32());
    }

    [Fact]
    public void ClayTransitionElementConfig_FieldOffsets()
    {
        Assert.Equal(0, Marshal.OffsetOf<ClayTransitionElementConfig>(nameof(ClayTransitionElementConfig.handler)).ToInt32());
        Assert.Equal(8, Marshal.OffsetOf<ClayTransitionElementConfig>(nameof(ClayTransitionElementConfig.duration)).ToInt32());
        Assert.Equal(12, Marshal.OffsetOf<ClayTransitionElementConfig>(nameof(ClayTransitionElementConfig.properties)).ToInt32());
        Assert.Equal(16, Marshal.OffsetOf<ClayTransitionElementConfig>(nameof(ClayTransitionElementConfig.interactionHandling)).ToInt32());
        Assert.Equal(24, Marshal.OffsetOf<ClayTransitionElementConfig>(nameof(ClayTransitionElementConfig.enter)).ToInt32());
        Assert.Equal(40, Marshal.OffsetOf<ClayTransitionElementConfig>(nameof(ClayTransitionElementConfig.exit)).ToInt32());
    }

    [Fact]
    public void ClayScrollContainerData_FieldOffsets()
    {
        Assert.Equal(0, Marshal.OffsetOf<ClayScrollContainerData>(nameof(ClayScrollContainerData.scrollPosition)).ToInt32());
        Assert.Equal(8, Marshal.OffsetOf<ClayScrollContainerData>(nameof(ClayScrollContainerData.scrollContainerDimensions)).ToInt32());
        Assert.Equal(16, Marshal.OffsetOf<ClayScrollContainerData>(nameof(ClayScrollContainerData.contentDimensions)).ToInt32());
        Assert.Equal(24, Marshal.OffsetOf<ClayScrollContainerData>(nameof(ClayScrollContainerData.config)).ToInt32());
        Assert.Equal(36, Marshal.OffsetOf<ClayScrollContainerData>(nameof(ClayScrollContainerData.found)).ToInt32());
    }

    [Fact]
    public void ClayElementDeclaration_FieldOffsets()
    {
        Assert.Equal(0, Marshal.OffsetOf<ClayElementDeclaration>(nameof(ClayElementDeclaration.layout)).ToInt32());
        Assert.Equal(40, Marshal.OffsetOf<ClayElementDeclaration>(nameof(ClayElementDeclaration.backgroundColor)).ToInt32());
        Assert.Equal(56, Marshal.OffsetOf<ClayElementDeclaration>(nameof(ClayElementDeclaration.overlayColor)).ToInt32());
        Assert.Equal(72, Marshal.OffsetOf<ClayElementDeclaration>(nameof(ClayElementDeclaration.cornerRadius)).ToInt32());
        Assert.Equal(88, Marshal.OffsetOf<ClayElementDeclaration>(nameof(ClayElementDeclaration.aspectRatio)).ToInt32());
        Assert.Equal(96, Marshal.OffsetOf<ClayElementDeclaration>(nameof(ClayElementDeclaration.image)).ToInt32());
        Assert.Equal(104, Marshal.OffsetOf<ClayElementDeclaration>(nameof(ClayElementDeclaration.floating)).ToInt32());
        Assert.Equal(136, Marshal.OffsetOf<ClayElementDeclaration>(nameof(ClayElementDeclaration.custom)).ToInt32());
        Assert.Equal(144, Marshal.OffsetOf<ClayElementDeclaration>(nameof(ClayElementDeclaration.clip)).ToInt32());
        Assert.Equal(156, Marshal.OffsetOf<ClayElementDeclaration>(nameof(ClayElementDeclaration.border)).ToInt32());
        Assert.Equal(184, Marshal.OffsetOf<ClayElementDeclaration>(nameof(ClayElementDeclaration.transition)).ToInt32());
        Assert.Equal(240, Marshal.OffsetOf<ClayElementDeclaration>(nameof(ClayElementDeclaration.userData)).ToInt32());
    }

    [Fact]
    public void ClayRenderCommand_FieldOffsets()
    {
        Assert.Equal(0, Marshal.OffsetOf<ClayRenderCommand>(nameof(ClayRenderCommand.boundingBox)).ToInt32());
        Assert.Equal(16, Marshal.OffsetOf<ClayRenderCommand>(nameof(ClayRenderCommand.renderData)).ToInt32());
        Assert.Equal(64, Marshal.OffsetOf<ClayRenderCommand>(nameof(ClayRenderCommand.userData)).ToInt32());
        Assert.Equal(72, Marshal.OffsetOf<ClayRenderCommand>(nameof(ClayRenderCommand.id)).ToInt32());
        Assert.Equal(76, Marshal.OffsetOf<ClayRenderCommand>(nameof(ClayRenderCommand.zIndex)).ToInt32());
        Assert.Equal(78, Marshal.OffsetOf<ClayRenderCommand>(nameof(ClayRenderCommand.commandType)).ToInt32());
    }

    [Fact]
    public void ClayTransitionData_FieldOffsets()
    {
        Assert.Equal(0, Marshal.OffsetOf<ClayTransitionData>(nameof(ClayTransitionData.boundingBox)).ToInt32());
        Assert.Equal(16, Marshal.OffsetOf<ClayTransitionData>(nameof(ClayTransitionData.backgroundColor)).ToInt32());
        Assert.Equal(32, Marshal.OffsetOf<ClayTransitionData>(nameof(ClayTransitionData.overlayColor)).ToInt32());
        Assert.Equal(48, Marshal.OffsetOf<ClayTransitionData>(nameof(ClayTransitionData.borderColor)).ToInt32());
        Assert.Equal(64, Marshal.OffsetOf<ClayTransitionData>(nameof(ClayTransitionData.borderWidth)).ToInt32());
    }

    [Fact]
    public void ClayTransitionCallbackArguments_FieldOffsets()
    {
        Assert.Equal(0, Marshal.OffsetOf<ClayTransitionCallbackArguments>(nameof(ClayTransitionCallbackArguments.transitionState)).ToInt32());
        Assert.Equal(4, Marshal.OffsetOf<ClayTransitionCallbackArguments>(nameof(ClayTransitionCallbackArguments.initial)).ToInt32());
        Assert.Equal(80, Marshal.OffsetOf<ClayTransitionCallbackArguments>(nameof(ClayTransitionCallbackArguments.current)).ToInt32());
        Assert.Equal(88, Marshal.OffsetOf<ClayTransitionCallbackArguments>(nameof(ClayTransitionCallbackArguments.target)).ToInt32());
        Assert.Equal(164, Marshal.OffsetOf<ClayTransitionCallbackArguments>(nameof(ClayTransitionCallbackArguments.elapsedTime)).ToInt32());
        Assert.Equal(168, Marshal.OffsetOf<ClayTransitionCallbackArguments>(nameof(ClayTransitionCallbackArguments.duration)).ToInt32());
        Assert.Equal(172, Marshal.OffsetOf<ClayTransitionCallbackArguments>(nameof(ClayTransitionCallbackArguments.properties)).ToInt32());
    }
}
