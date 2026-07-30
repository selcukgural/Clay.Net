using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Controls settings related to element borders.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayBorderElementConfig
{
    /// <summary>Controls the color of all borders with width > 0. Conventionally represented as 0-255, but interpretation is up to the renderer.</summary>
    public ClayColor color;

    /// <summary>Controls the widths of individual borders. At least one of these should be > 0 for a BORDER render command to be generated.</summary>
    public ClayBorderWidth width;
}