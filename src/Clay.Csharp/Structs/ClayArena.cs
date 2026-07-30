using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Clay_Arena is a memory arena structure that is used by clay to manage its internal allocations.
/// Rather than creating it by hand, it's easier to use Clay_CreateArenaWithCapacityAndMemory()
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayArena
{
    public UIntPtr nextAllocation;
    public UIntPtr capacity;
    public IntPtr memory;
}