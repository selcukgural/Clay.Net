# Clay.Net

A .NET port of [Clay](https://github.com/nicbarker/clay), Nic Barker's high-performance, single-header
C UI layout library. Clay.Net wraps the real, unmodified `clay.h` via P/Invoke and adds an idiomatic C#
API on top of it, so you get Clay's flexbox-like layout engine with C# ergonomics instead of C macros.

Clay itself only computes layout and emits an abstract list of render commands ("draw this rectangle
here", "draw this text there") - it does not draw anything to the screen. Clay.Net follows the same
design: the core library is renderer-agnostic, and a small renderer package (currently for
[raylib](https://www.raylib.com/)) turns those render commands into pixels.

## Installation

[![NuGet](https://img.shields.io/nuget/v/Clay.Csharp.svg?label=Clay.Csharp)](https://www.nuget.org/packages/Clay.Csharp)
[![NuGet](https://img.shields.io/nuget/v/Clay.Csharp.Raylib.svg?label=Clay.Csharp.Raylib)](https://www.nuget.org/packages/Clay.Csharp.Raylib)

```sh
dotnet add package Clay.Csharp
dotnet add package Clay.Csharp.Raylib   # optional: raylib renderer + window wrapper
```

`Clay.Csharp` ships the prebuilt native `clay_native` binary for every supported platform inside the
package itself (`runtimes/<RID>/native/`, the standard NuGet convention for native assets) - the right
one is picked automatically at load time, no manual build step required.

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
tests/Clay.Csharp.Tests/     Struct/enum ABI tests, ClayHelpers/declarative-API unit tests, and native
                              integration tests (real layout computation, hover, transitions).
tests/Clay.Csharp.Raylib.Tests/ Pure-logic tests for the renderer (color conversion, font fallback,
                              border geometry) - no raylib window/GL context required.
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

## Testing

```sh
dotnet test Clay.Net.sln
```

Tests are split into two tiers (see `tests/Clay.Csharp.Tests`):
- **Struct/enum ABI + helper/declarative-API tests** run on every OS unconditionally, no native library
  required - this is what would have (and, during development, did) caught byte-layout bugs like a
  struct carrying a field that doesn't exist in `clay.h`.
- **Native integration tests** (real layout computation, hover, transitions, error reporting) are tagged
  `[Trait("RequiresNative", "true")]` and need the real `clay_native` binary for your platform - run
  everything with a plain `dotnet test`, or exclude them with `dotnet test --filter "RequiresNative!=true"`
  on a platform without a prebuilt binary yet (see [Platform support](#platform-support)).

CI (`.github/workflows/ci.yml`) runs the full suite on macOS, and the native-independent tier on Linux
and Windows.

## A minimal example

This is a 1:1 C# port of the ["Quick Start"](https://github.com/nicbarker/clay#quick-start) example from
upstream Clay's own README - a fixed-width sidebar with a profile picture and five repeated items, next
to a flexible-width main content area. The original spells out `Clay_MinMemorySize` /
`Clay_CreateArenaWithCapacityAndMemory` / `Clay_Initialize`, a measure-text function, and a manual
render-command switch/loop by hand; `ClayRaylibWindow` exists specifically to hide that boilerplate, so
only the interesting part - the actual layout - is shown here (see `samples/Clay.Samples.Raylib` for the
runnable version, or `ClayRaylibWindow`'s own source for the low-level equivalent of what upstream's
`main()` does by hand).

```csharp
ClayColor colorLight = ClayHelpers.CreateColor(224, 215, 210);
ClayColor colorRed = ClayHelpers.CreateColor(168, 66, 28);
ClayColor colorOrange = ClayHelpers.CreateColor(225, 138, 50);

// Layout config is just a struct that can be declared once, statically, and reused.
ClayElementDeclaration sidebarItemConfig = new()
{
    layout = new ClayLayoutConfig
    {
        sizing = new ClaySizing { width = ClaySizingAxis.Grow(), height = ClaySizingAxis.Fixed(50) },
    },
    backgroundColor = colorOrange,
};

// Re-usable components are just normal C# methods.
void SidebarItemComponent(int index)
{
    using (Layout.Element(Layout.Id("SidebarItem", (uint)index), sidebarItemConfig))
    {
        // children go here...
    }
}

using ClayRaylibWindow window = ClayRaylibWindow.Create(800, 600, "Clay.Net Quick Start");

// raylib's built-in default font is a tiny 10px bitmap font - fine for debug overlays, not for real UI
// text at 20-28px like this example uses. Load a proper font instead (see LoadFont's doc comment).
window.LoadFont(Path.Combine(AppContext.BaseDirectory, "Assets", "Fonts", "Inter-Regular.ttf"));

while (!window.ShouldClose)
{
    window.RunFrame(() =>
    {
        // An example of laying out a UI with a fixed width sidebar and flexible width main content.
        using (Layout.Element("OuterContainer", new ClayElementDeclaration
        {
            layout = new ClayLayoutConfig
            {
                sizing = new ClaySizing { width = ClaySizingAxis.Grow(), height = ClaySizingAxis.Grow() },
                padding = ClayHelpers.CreatePaddingUniform(16),
                childGap = 16,
            },
            backgroundColor = ClayHelpers.CreateColor(250, 250, 255),
        }))
        {
            using (Layout.Element("SideBar", new ClayElementDeclaration
            {
                layout = new ClayLayoutConfig
                {
                    layoutDirection = ClayLayoutDirection.ClayTopToBottom,
                    sizing = new ClaySizing { width = ClaySizingAxis.Fixed(300), height = ClaySizingAxis.Grow() },
                    padding = ClayHelpers.CreatePaddingUniform(16),
                    childGap = 16,
                },
                backgroundColor = colorLight,
            }))
            {
                using (Layout.Element("ProfilePictureOuter", new ClayElementDeclaration
                {
                    layout = new ClayLayoutConfig
                    {
                        sizing = new ClaySizing { width = ClaySizingAxis.Grow() },
                        padding = ClayHelpers.CreatePaddingUniform(16),
                        childGap = 16,
                        childAlignment = new ClayChildAlignment { y = ClayLayoutAlignmentY.ClayAlignYCenter },
                    },
                    backgroundColor = colorRed,
                }))
                {
                    using (Layout.Element("ProfilePicture", new ClayElementDeclaration
                    {
                        layout = new ClayLayoutConfig
                        {
                            sizing = new ClaySizing { width = ClaySizingAxis.Fixed(60), height = ClaySizingAxis.Fixed(60) },
                        },
                        // Upstream sets `.image = { .imageData = &profilePicture }` here - image render
                        // commands aren't wired up in ClayRaylibRenderer yet (see Status below), so this
                        // stays a plain colored placeholder for now.
                        backgroundColor = ClayHelpers.CreateColor(180, 180, 180),
                        cornerRadius = ClayHelpers.CreateCornerRadius(30),
                    }))
                    {
                    }

                    Layout.Text("Clay.Net - UI Library", new ClayTextElementConfig
                    {
                        fontSize = 24,
                        textColor = ClayHelpers.CreateColor(255, 255, 255),
                    });
                }

                // Standard C# code like loops etc work inside components.
                for (int i = 0; i < 5; i++)
                {
                    SidebarItemComponent(i);
                }

                using (Layout.Element("MainContent", new ClayElementDeclaration
                {
                    layout = new ClayLayoutConfig
                    {
                        sizing = new ClaySizing { width = ClaySizingAxis.Grow(), height = ClaySizingAxis.Grow() },
                    },
                    backgroundColor = colorLight,
                }))
                {
                }
            }
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

**Fonts:** `ClayRaylibWindow` defaults to raylib's built-in font, which is a tiny 10px bitmap font not
meant to be scaled up - it looks blocky/blurry at any real UI text size (the example above calls
`window.LoadFont(path)` to replace it with a proper TTF, which is what actually produces the crisp text
in `samples/Clay.Samples.Raylib`, bundling [Inter](https://github.com/rsms/inter) under OFL-1.1).

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
at the default 4-byte size), and every struct's `Marshal.SizeOf<T>()` is cross-checked in tests against
the real, compiler-computed native size (`ClayNative_GetAbiSizes` in `native/clay_native`) rather than
trusting hand-derived constants alone - see [Testing](#testing).

## Status

- Struct/enum ABI: complete, audited against upstream `clay.h`, covered by tests (both hand-derived and
  native-verified - see [Testing](#testing)).
- P/Invoke surface (`Clay.Csharp.Internal.ClayNativeInternal`) and public facade (`ClayNative`): complete.
- Declarative element API, per-frame text/id string arena, transition callback marshaling,
  `Clay_OnHover`: implemented and verified against the real native library.
- Raylib renderer: rectangles, text, borders (including corner-radius-aware borders), scissor/clip -
  working. Images, custom render commands, and color-overlay transitions are not wired up yet (see
  `ClayRaylibRenderer.Render`).
- Only one renderer (raylib) exists so far; SDL2/SDL3/etc. would follow the same pattern as
  `src/Clay.Csharp.Raylib` as separate, optional packages.
- The repo's committed `native/clay_native/prebuilt/` only bundles macOS arm64 (see
  [Platform support](#platform-support)) - that's what `dotnet build`/`dotnet run` from source use. The
  **published NuGet packages** are cross-platform: the [release workflow](#releasing-a-new-version)
  builds `clay_native` fresh for linux-x64, win-x64 and osx-arm64 and bundles all three into the package.

## Releasing a new version

Maintainer-only. Tag `main` with a semver-ish version and create a GitHub Release from it:

```sh
git tag v0.2.0
git push origin v0.2.0
gh release create v0.2.0 --generate-notes
```

Publishing the release triggers `.github/workflows/release.yml`, which re-runs the full test suite,
builds `clay_native` from source for linux-x64/win-x64/osx-arm64, packs `Clay.Csharp` and
`Clay.Csharp.Raylib` at that version, pushes both to NuGet.org, and attaches the `.nupkg`/`.snupkg` files
to the GitHub Release. Requires a `NUGET_API_KEY` repository secret (a nuget.org API key with push rights
to both package IDs) to already exist - one-time setup: `gh secret set NUGET_API_KEY`.

The workflow can also be run manually from the Actions tab (`workflow_dispatch`) as a dry run - it builds,
tests and packs exactly the same way, but only pushes to NuGet.org if you explicitly tick `publish`.

## Credits & license

Clay.Net bundles the original, unmodified `clay.h` from
[nicbarker/clay](https://github.com/nicbarker/clay) by Nic Barker. Licensed under the zlib/libpng
license - see [LICENSE](LICENSE).
