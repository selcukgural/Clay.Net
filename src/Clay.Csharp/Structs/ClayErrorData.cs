using System.Runtime.InteropServices;
using Clay.Csharp.Enums;

namespace Clay.Csharp.Structs;

/// <summary>
/// Data to identify the error that clay has encountered.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayErrorData
{
    public ClayErrorType errorType;
    public ClayString errorText;
    public IntPtr userData; // void*
}