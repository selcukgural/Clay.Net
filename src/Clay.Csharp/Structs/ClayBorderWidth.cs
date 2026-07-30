using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Controls the widths of individual element borders.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayBorderWidth
{
    public ushort left;
    public ushort right;
    public ushort top;
    public ushort bottom;
    public ushort betweenChildren;
}