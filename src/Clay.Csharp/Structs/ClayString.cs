using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Clay_String is not guaranteed to be null terminated. It may be if created from a literal C string,
/// but it is also used to represent slices.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct ClayString
{
    /// <summary>
    /// Set this boolean to true if the char* data underlying this string will live for the entire lifetime of the program.
    /// This will automatically be set for strings created with CLAY_STRING, as the macro requires a string literal.
    /// </summary>
    [MarshalAs(UnmanagedType.I1)]
    public bool isStaticallyAllocated;

    public int length;

    /// <summary>
    /// The underlying character memory. Note: this will not be copied and will not extend the lifetime of the underlying memory.
    /// </summary>
    public IntPtr chars;
}