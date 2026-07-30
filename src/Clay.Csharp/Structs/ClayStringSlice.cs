using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Clay_StringSlice is used to represent non owning string slices, and includes
/// a baseChars field which points to the string this slice is derived from.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct ClayStringSlice
{
    public int length;
    public IntPtr chars;

    /// <summary>
    /// The source string / char* that this slice was derived from
    /// </summary>
    public IntPtr baseChars;
}