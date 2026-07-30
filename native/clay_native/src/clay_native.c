//
// Created by Selçuk Güral on 27.07.2026.
//

#include "clay_native.h"
#include "clay.h" // declarations only - CLAY_IMPLEMENTATION is not defined here, clay.c owns that


const char* ClayNative_GetVersion(void)
{
    return VERSION;
}

int ClayNative_GetVersionMajor(void)
{
    return CLAY_NATIVE_VERSION_MAJOR;
}
int ClayNative_GetVersionMinor(void)
{
    return CLAY_NATIVE_VERSION_MINOR;
}
int ClayNative_GetVersionPatch(void)
{
    return CLAY_NATIVE_VERSION_PATCH;
}

ClayNative_AbiSizes ClayNative_GetAbiSizes(void)
{
    ClayNative_AbiSizes sizes;
    sizes.sizeofClayArena = (uint32_t)sizeof(Clay_Arena);
    sizes.sizeofClayAspectRatioElementConfig = (uint32_t)sizeof(Clay_AspectRatioElementConfig);
    sizes.sizeofClayBorderElementConfig = (uint32_t)sizeof(Clay_BorderElementConfig);
    sizes.sizeofClayBorderRenderData = (uint32_t)sizeof(Clay_BorderRenderData);
    sizes.sizeofClayBorderWidth = (uint32_t)sizeof(Clay_BorderWidth);
    sizes.sizeofClayBoundingBox = (uint32_t)sizeof(Clay_BoundingBox);
    sizes.sizeofClayChildAlignment = (uint32_t)sizeof(Clay_ChildAlignment);
    sizes.sizeofClayClipElementConfig = (uint32_t)sizeof(Clay_ClipElementConfig);
    sizes.sizeofClayClipRenderData = (uint32_t)sizeof(Clay_ClipRenderData);
    sizes.sizeofClayColor = (uint32_t)sizeof(Clay_Color);
    sizes.sizeofClayCornerRadius = (uint32_t)sizeof(Clay_CornerRadius);
    sizes.sizeofClayCustomElementConfig = (uint32_t)sizeof(Clay_CustomElementConfig);
    sizes.sizeofClayCustomRenderData = (uint32_t)sizeof(Clay_CustomRenderData);
    sizes.sizeofClayDimensions = (uint32_t)sizeof(Clay_Dimensions);
    sizes.sizeofClayElementData = (uint32_t)sizeof(Clay_ElementData);
    sizes.sizeofClayElementDeclaration = (uint32_t)sizeof(Clay_ElementDeclaration);
    sizes.sizeofClayElementId = (uint32_t)sizeof(Clay_ElementId);
    sizes.sizeofClayElementIdArray = (uint32_t)sizeof(Clay_ElementIdArray);
    sizes.sizeofClayErrorData = (uint32_t)sizeof(Clay_ErrorData);
    sizes.sizeofClayErrorHandler = (uint32_t)sizeof(Clay_ErrorHandler);
    sizes.sizeofClayFloatingAttachPoints = (uint32_t)sizeof(Clay_FloatingAttachPoints);
    sizes.sizeofClayFloatingElementConfig = (uint32_t)sizeof(Clay_FloatingElementConfig);
    sizes.sizeofClayImageElementConfig = (uint32_t)sizeof(Clay_ImageElementConfig);
    sizes.sizeofClayImageRenderData = (uint32_t)sizeof(Clay_ImageRenderData);
    sizes.sizeofClayLayoutConfig = (uint32_t)sizeof(Clay_LayoutConfig);
    sizes.sizeofClayOverlayColorRenderData = (uint32_t)sizeof(Clay_OverlayColorRenderData);
    sizes.sizeofClayPadding = (uint32_t)sizeof(Clay_Padding);
    sizes.sizeofClayPointerData = (uint32_t)sizeof(Clay_PointerData);
    sizes.sizeofClayRectangleRenderData = (uint32_t)sizeof(Clay_RectangleRenderData);
    sizes.sizeofClayRenderCommand = (uint32_t)sizeof(Clay_RenderCommand);
    sizes.sizeofClayRenderCommandArray = (uint32_t)sizeof(Clay_RenderCommandArray);
    sizes.sizeofClayRenderData = (uint32_t)sizeof(Clay_RenderData);
    sizes.sizeofClayScrollContainerData = (uint32_t)sizeof(Clay_ScrollContainerData);
    sizes.sizeofClaySizing = (uint32_t)sizeof(Clay_Sizing);
    sizes.sizeofClaySizingAxis = (uint32_t)sizeof(Clay_SizingAxis);
    sizes.sizeofClaySizingMinMax = (uint32_t)sizeof(Clay_SizingMinMax);
    sizes.sizeofClayString = (uint32_t)sizeof(Clay_String);
    sizes.sizeofClayStringSlice = (uint32_t)sizeof(Clay_StringSlice);
    sizes.sizeofClayTextElementConfig = (uint32_t)sizeof(Clay_TextElementConfig);
    sizes.sizeofClayTextRenderData = (uint32_t)sizeof(Clay_TextRenderData);
    sizes.sizeofClayTransitionCallbackArguments = (uint32_t)sizeof(Clay_TransitionCallbackArguments);
    sizes.sizeofClayTransitionData = (uint32_t)sizeof(Clay_TransitionData);
    sizes.sizeofClayTransitionElementConfig = (uint32_t)sizeof(Clay_TransitionElementConfig);
    sizes.sizeofClayTransitionElementConfigEnter = (uint32_t)sizeof(((Clay_TransitionElementConfig*)0)->enter);
    sizes.sizeofClayTransitionElementConfigExit = (uint32_t)sizeof(((Clay_TransitionElementConfig*)0)->exit);
    sizes.sizeofClayVector2 = (uint32_t)sizeof(Clay_Vector2);
    return sizes;
}