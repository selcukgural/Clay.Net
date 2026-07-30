cmake_minimum_required(VERSION 3.20)
project(clay_native C)

set(CMAKE_C_STANDARD 17)
set(CMAKE_C_STANDARD_REQUIRED ON)
set(CMAKE_C_EXTENSIONS OFF)

add_library(clay_native
        SHARED
        src/clay.c
        include/clay_native.h
        src/clay_native.c
)

target_include_directories(clay_native
        PRIVATE
        third_party/clay

        PUBLIC
        include
)

# clay.h's public functions (and our own clay_native.c wrapper functions) are only annotated with
# __declspec(dllexport) when the CLAY_DLL macro is defined - which we deliberately don't define, since
# that annotation also switches the calling convention to __stdcall, while every C# P/Invoke declaration
# in Clay.Csharp assumes the default Cdecl convention (matching Linux/macOS, where shared libraries
# export all public symbols by default with no annotation needed).
# WINDOWS_EXPORT_ALL_SYMBOLS reproduces that same "export everything, default calling convention" behavior
# on Windows, without touching upstream clay.h or changing the calling convention. It's a no-op on
# non-Windows platforms, so this is safe to set unconditionally.
set_target_properties(clay_native PROPERTIES WINDOWS_EXPORT_ALL_SYMBOLS ON)
