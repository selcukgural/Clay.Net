using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// A wrapper struct around Clay's error handler function.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayErrorHandler
{
    public IntPtr function; // Function pointer
    public IntPtr userData; // void*
}