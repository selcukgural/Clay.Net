//
// Created by Selçuk Güral on 27.07.2026.
//

#pragma once

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

#ifdef __cplusplus
}
#endif