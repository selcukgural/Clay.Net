//
// Created by Selçuk Güral on 27.07.2026.
//

#pragma once

#include <stdint.h>

#ifdef __cplusplus
extern "C" {
#endif

#define STRINGIFY(x) #x
#define TOSTRING(x) STRINGIFY(x)

#define CLAY_NATIVE_VERSION_MAJOR 0
#define CLAY_NATIVE_VERSION_MINOR 1
#define CLAY_NATIVE_VERSION_PATCH 0

#define VERSION \
    TOSTRING(CLAY_NATIVE_VERSION_MAJOR) "." \
    TOSTRING(CLAY_NATIVE_VERSION_MINOR) "." \
    TOSTRING(CLAY_NATIVE_VERSION_PATCH)

const char* ClayNative_GetVersion(void);
int ClayNative_GetVersionMajor(void);
int ClayNative_GetVersionMinor(void);
int ClayNative_GetVersionPatch(void);

// Real, compiler-computed sizeof(...) for every Clay_* struct the C# bindings mirror, so tests can
// cross-check Marshal.SizeOf<T>() against ground truth instead of relying purely on hand-derived
// constants. Field order here is a fixed contract with the matching C# ClayNativeAbiSizes struct in
// Clay.Csharp.Tests - if you add a struct to one side, add it to the other, in the same position
// (append-only; never reorder existing fields, or every existing C# offset assumption breaks).
typedef struct ClayNative_AbiSizes
{
    uint32_t sizeofClayArena;
    uint32_t sizeofClayAspectRatioElementConfig;
    uint32_t sizeofClayBorderElementConfig;
    uint32_t sizeofClayBorderRenderData;
    uint32_t sizeofClayBorderWidth;
    uint32_t sizeofClayBoundingBox;
    uint32_t sizeofClayChildAlignment;
    uint32_t sizeofClayClipElementConfig;
    uint32_t sizeofClayClipRenderData;
    uint32_t sizeofClayColor;
    uint32_t sizeofClayCornerRadius;
    uint32_t sizeofClayCustomElementConfig;
    uint32_t sizeofClayCustomRenderData;
    uint32_t sizeofClayDimensions;
    uint32_t sizeofClayElementData;
    uint32_t sizeofClayElementDeclaration;
    uint32_t sizeofClayElementId;
    uint32_t sizeofClayElementIdArray;
    uint32_t sizeofClayErrorData;
    uint32_t sizeofClayErrorHandler;
    uint32_t sizeofClayFloatingAttachPoints;
    uint32_t sizeofClayFloatingElementConfig;
    uint32_t sizeofClayImageElementConfig;
    uint32_t sizeofClayImageRenderData;
    uint32_t sizeofClayLayoutConfig;
    uint32_t sizeofClayOverlayColorRenderData;
    uint32_t sizeofClayPadding;
    uint32_t sizeofClayPointerData;
    uint32_t sizeofClayRectangleRenderData;
    uint32_t sizeofClayRenderCommand;
    uint32_t sizeofClayRenderCommandArray;
    uint32_t sizeofClayRenderData;
    uint32_t sizeofClayScrollContainerData;
    uint32_t sizeofClaySizing;
    uint32_t sizeofClaySizingAxis;
    uint32_t sizeofClaySizingMinMax;
    uint32_t sizeofClayString;
    uint32_t sizeofClayStringSlice;
    uint32_t sizeofClayTextElementConfig;
    uint32_t sizeofClayTextRenderData;
    uint32_t sizeofClayTransitionCallbackArguments;
    uint32_t sizeofClayTransitionData;
    uint32_t sizeofClayTransitionElementConfig;
    uint32_t sizeofClayTransitionElementConfigEnter;
    uint32_t sizeofClayTransitionElementConfigExit;
    uint32_t sizeofClayVector2;
} ClayNative_AbiSizes;

ClayNative_AbiSizes ClayNative_GetAbiSizes(void);

#ifdef __cplusplus
}
#endif