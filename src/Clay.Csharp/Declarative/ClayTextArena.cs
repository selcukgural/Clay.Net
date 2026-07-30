using System.Runtime.InteropServices;
using System.Text;
using Clay.Csharp.Structs;

namespace Clay.Csharp.Declarative;

/// <summary>
/// Clay_String / Clay_StringSlice never copy the underlying char* - the caller must keep it alive for as
/// long as Clay may reference it (through the end of the frame's render commands). This arena copies each
/// managed string into unmanaged (UTF8) memory, and is reset once per frame in Clay.BeginLayout(), which
/// assumes the previous frame's render commands have already been consumed by the renderer before the next
/// Clay.BeginLayout() call - the same assumption the upstream C examples make.
/// </summary>
internal static class ClayTextArena
{
    private static readonly List<IntPtr> Allocations = new();

    public static void Reset()
    {
        foreach (IntPtr ptr in Allocations)
        {
            Marshal.FreeHGlobal(ptr);
        }

        Allocations.Clear();
    }

    public static ClayString Intern(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new ClayString { isStaticallyAllocated = false, length = 0, chars = IntPtr.Zero };
        }

        byte[] bytes = Encoding.UTF8.GetBytes(text);
        IntPtr ptr = Marshal.AllocHGlobal(bytes.Length);
        Marshal.Copy(bytes, 0, ptr, bytes.Length);
        Allocations.Add(ptr);

        return new ClayString { isStaticallyAllocated = false, length = bytes.Length, chars = ptr };
    }
}
