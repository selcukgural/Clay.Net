using System.Reflection;
using System.Runtime.InteropServices;

namespace Clay.Csharp.Internal;

/// <summary>
/// Explicitly resolves the "clay_native" P/Invoke library relative to Clay.Csharp.dll's own location
/// (runtimes/&lt;RID&gt;/native/&lt;file&gt;, matching Clay.Csharp.csproj's Content items), instead of
/// relying on the CLR's default native-library probing.
///
/// This matters because that default probing is based on the *process's* base directory, which is fine
/// for `dotnet run` (the process base dir is the app's own output folder) but not for test runners like
/// `dotnet test` / vstest, which host the test assembly inside a shared, generic testhost process whose
/// base directory is unrelated to Clay.Csharp.Tests' own output folder - the native library silently
/// fails to load there even though it was copied to the right place at build time. Resolving relative to
/// Assembly.Location instead of the process is host-independent and works the same way under `dotnet
/// run`, `dotnet test`, and a published app.
///
/// Registered from ClayNativeInternal's static constructor (guaranteed to run once, before that class's
/// first P/Invoke call) rather than a [ModuleInitializer], to avoid CA2255 - module initializers are
/// meant for application entry points, not library code that only needs to act before its own first use.
/// </summary>
internal static class ClayNativeLibraryResolver
{
    private const string LibraryName = "clay_native";
    private static bool _registered;

    internal static void EnsureRegistered()
    {
        if (_registered)
        {
            return;
        }

        _registered = true;
        NativeLibrary.SetDllImportResolver(typeof(ClayNativeLibraryResolver).Assembly, Resolve);
    }

    private static IntPtr Resolve(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (libraryName != LibraryName)
        {
            return IntPtr.Zero; // let the CLR fall back to its default resolution for anything else
        }

        string? assemblyDirectory = Path.GetDirectoryName(assembly.Location);
        if (string.IsNullOrEmpty(assemblyDirectory))
        {
            return IntPtr.Zero;
        }

        string candidate = Path.Combine(assemblyDirectory, "runtimes", GetRuntimeIdentifier(), "native", GetPlatformFileName());
        return File.Exists(candidate) && NativeLibrary.TryLoad(candidate, out IntPtr handle) ? handle : IntPtr.Zero;
    }

    private static string GetRuntimeIdentifier()
    {
        string os =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "win" :
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "osx" : "linux";

        string arch = RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.Arm64 => "arm64",
            Architecture.X64 => "x64",
            Architecture.X86 => "x86",
            Architecture other => other.ToString().ToLowerInvariant(),
        };

        return $"{os}-{arch}";
    }

    private static string GetPlatformFileName()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "clay_native.dll";
        }

        return RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "libclay_native.dylib" : "libclay_native.so";
    }
}
