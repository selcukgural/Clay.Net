//
// Created by Selçuk Güral on 27.07.2026.
//

#include "clay_native.h"


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