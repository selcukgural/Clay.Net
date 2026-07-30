# Clay.Net

A .NET port of [Clay](https://github.com/nicbarker/clay), Nic Barker's high-performance, single-header
C UI layout library. Clay.Net wraps the real, unmodified `clay.h` via P/Invoke and adds an idiomatic C#
API on top of it, so you get Clay's flexbox-like layout engine with C# ergonomics instead of C macros.

Clay itself only computes layout and emits an abstract list of render commands ("draw this rectangle
here", "draw this text there") - it does not draw anything to the screen. Clay.Net follows the same
design: the core library is renderer-agnostic, and a small renderer package (currently for
[raylib](https://www.raylib.com/)) turns those render commands into pixels.

## Project layout

```
Clay.Net.sln
native/clay_native/          Small CMake C project: compiles clay.h + a thin C wrapper into a
                              shared library (clay_native) that the C# bindings P/Invoke into.
src/Clay.Csharp/             Core C# bindings: structs/enums matching clay.h's ABI exactly, the
                              full P/Invoke surface, a public facade (ClayNative), and an idiomatic
                              declarative layout API (Clay.Csharp.Declarative.Layout).
src/Clay.Csharp.Raylib/      Optional renderer: translates Clay's render commands into raylib draw
                              calls, plus a batteries-included window+frame-loop wrapper.
samples/Clay.Samples.Raylib/ A small runnable example using the raylib renderer.
```

Why is the native C project separate from `src/`? It isn't C# code, and it's a build *input* to
`Clay.Csharp` rather than something you'd reference directly - keeping it at the repo root under
`native/` avoids implying it's part of the C# project tree.

## Getting started

**Prerequisites:** .NET SDK 8.0+ (pinned via `global.json`).

```sh
dotnet build Clay.Net.sln
dotnet run --project samples/Clay.Samples.Raylib
```

That's it - no manual native library build or copy step required on **macOS (Apple Silicon)**, since a
prebuilt `clay_native` binary for that platform is bundled and copied to your output directory
automatically (see [Platform support](#platform-support) below).

## A minimal example

```csharp
using ClayRaylibWindow window = ClayRaylibWindow.Create(800, 600, "My App");

while (!window.ShouldClose)
{
    window.RunFrame(() =>
    {
        using (Layout.Element("Root", new ClayElementDeclaration
        {
            layout = new ClayLayoutConfig
            {
                sizing = new ClaySizing { width = ClaySizingAxis.Grow(), height = ClaySizingAxis.Grow() },
                padding = ClayHelpers.CreatePaddingUniform(24),
                childGap = 16,
            },
            backgroundColor = ClayHelpers.CreateColor(30, 30, 35),
        }))
        {
            Layout.Text("Hello, Clay.Net!", new ClayTextElementConfig
            {
                fontSize = 28,
                textColor = ClayHelpers.CreateColor(255, 255, 255),
            });
        }
    });
}
```

`Layout.Element(...)` / the `using` block is the C# equivalent of Clay's `CLAY(id, ...) { ... }` macro -
C has no macros in C#, so opening/configuring/closing an element is instead expressed as an
`IDisposable` scope. See `src/Clay.Csharp/Declarative/Layout.cs` for the full API (`Element`, `Text`,
`Id`, `IdLocal`, ...).

If you want lower-level control (custom frame pacing, multiple Clay contexts, a different render
pipeline), you can use `ClayNative` (the 1:1 facade over Clay's C API) and `ClayRaylibRenderer`
(the stateless render-command → draw-call translator) directly instead of `ClayRaylibWindow`.

## Platform support

Clay.Net itself is fully cross-platform - the P/Invoke layer, the CMake native project, and the build
tooling all work identically on Windows, Linux and macOS. What's currently missing is *prebuilt
binaries* for platforms other than macOS arm64:

| Platform | Native `clay_native` binary |
|---|---|
| macOS (arm64) | Bundled (`native/clay_native/prebuilt/osx-arm64/native/`) |
| macOS (x64), Linux, Windows | Not bundled yet - build it yourself (below) |

Contributions adding prebuilt binaries (and CI to produce them) for other platforms are very welcome.

### Building `clay_native` from source

Requires CMake (3.20+) and a C compiler (gcc/clang/MSVC all work - the CMake project explicitly
handles MSVC's symbol-export behavior, see the comment in `CMakeLists.txt`).

```sh
cd native/clay_native
cmake -B cmake-build-debug
cmake --build cmake-build-debug
```

Then copy the resulting shared library (`libclay_native.dylib` on macOS, `libclay_native.so` on Linux,
`clay_native.dll` on Windows) into `native/clay_native/prebuilt/<your-RID>/native/`, e.g.
`prebuilt/linux-x64/native/libclay_native.so` or `prebuilt/win-x64/native/clay_native.dll`
([RID reference](https://learn.microsoft.com/dotnet/core/rid-catalog)).

No csproj changes are needed - `Clay.Csharp.csproj` picks up every `prebuilt/<RID>/native/<file>` it
finds and copies it to `runtimes/<RID>/native/` in the build output, the same convention NuGet uses for
packages that ship native binaries (e.g. `Raylib-cs`, which you can see doing exactly this in
`samples/Clay.Samples.Raylib`'s own build output). The .NET runtime picks the matching one for the
current OS/architecture automatically at load time - nothing in `Clay.Csharp` needs to know or care
which platform it's running on.

## Correctness note: why this isn't just "add DllImport and go"

`clay.h`'s structs must line up byte-for-byte with their C# equivalents for P/Invoke to work safely -
get the field order, a bool's marshaled size, or an enum's underlying type wrong, and you silently
corrupt memory rather than getting a compile error. Every struct and enum under `src/Clay.Csharp/`
has been audited field-by-field against `native/clay_native/third_party/clay/clay.h` for this reason
(notably: C's 1-byte `bool` must be marshaled as `UnmanagedType.I1`, not the default 4-byte
`UnmanagedType.Bool`; Clay's `CLAY_PACKED_ENUM`s are 1-byte and map to C# `enum : byte`, but a couple
of enums - `Clay_TransitionState`, `Clay_TransitionProperty` - are deliberately *not* packed and stay
at the default 4-byte size).

## Status

- Struct/enum ABI: complete, audited against upstream `clay.h`.
- P/Invoke surface (`Clay.Csharp.Internal.ClayNativeInternal`) and public facade (`ClayNative`): complete.
- Declarative element API, per-frame text/id string arena, transition callback marshaling,
  `Clay_OnHover`: implemented and verified against the real native library.
- Raylib renderer: rectangles, text, borders, scissor/clip - working. Images, custom render commands,
  and color-overlay transitions are not wired up yet (see `ClayRaylibRenderer.Render`).
- Only one renderer (raylib) exists so far; SDL2/SDL3/etc. would follow the same pattern as
  `src/Clay.Csharp.Raylib` as separate, optional packages.

## Credits & license

Clay.Net bundles the original, unmodified `clay.h` from
[nicbarker/clay](https://github.com/nicbarker/clay) by Nic Barker. Licensed under the zlib/libpng
license - see [LICENSE](LICENSE).
