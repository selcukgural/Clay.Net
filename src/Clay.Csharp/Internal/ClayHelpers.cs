using System.Runtime.InteropServices;
using Clay.Csharp.Structs;

namespace Clay.Csharp.Internal;

/// <summary>
/// Helper class providing convenience methods for working with Clay from C#
/// </summary>
public static class ClayHelpers
{
    /// <summary>
    /// Creates a Clay_String from a .NET string (stores as static reference, use with caution for long-lived strings)
    /// </summary>
    public static ClayString CreateClayString(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new ClayString { isStaticallyAllocated = false, length = 0, chars = IntPtr.Zero };
        }
        
        IntPtr unmanagedString = Marshal.StringToHGlobalAnsi(text);
        return new ClayString
        {
            isStaticallyAllocated = false,
            length = text.Length,
            chars = unmanagedString
        };
    }

    /// <summary>
    /// Converts a Clay_String to a .NET string
    /// </summary>
    public static string ClayStringToManaged(ClayString clayString)
    {
        return clayString.chars == IntPtr.Zero || clayString.length == 0
                   ? string.Empty
                   : Marshal.PtrToStringAnsi(clayString.chars, clayString.length);
    }

    /// <summary>
    /// Gets a render command from the render command array
    /// </summary>
    public static ClayRenderCommand GetRenderCommand(ClayRenderCommandArray array, int index)
    {
        if (index < 0 || index >= array.length)
        {
            throw new IndexOutOfRangeException("Render command index out of range");
        }

        IntPtr commandPtr = ClayNativeInternal.Clay_RenderCommandArray_Get(ref array, index);
        return Marshal.PtrToStructure<ClayRenderCommand>(commandPtr);
    }

    /// <summary>
    /// Gets an element ID from the element ID array
    /// </summary>
    public static ClayElementId GetElementId(ClayElementIdArray array, int index)
    {
        if (index < 0 || index >= array.length)
        {
            throw new IndexOutOfRangeException("Element ID index out of range");
        }

        IntPtr elementPtr = Marshal.ReadIntPtr(new IntPtr(array.internalArray.ToInt64() + index * Marshal.SizeOf<ClayElementId>()));
        return Marshal.PtrToStructure<ClayElementId>(elementPtr);
    }

    /// <summary>
    /// Creates a Clay_Arena with allocated memory
    /// </summary>
    public static ClayArena CreateArena(uint sizeInBytes)
    {
        IntPtr memory = Marshal.AllocHGlobal((int)sizeInBytes);
        return ClayNativeInternal.Clay_CreateArenaWithCapacityAndMemory(new UIntPtr(sizeInBytes), memory);
    }

    /// <summary>
    /// Frees a Clay_Arena and its allocated memory
    /// </summary>
    public static void FreeArena(ref ClayArena arena)
    {
        if (arena.memory == IntPtr.Zero)
        {
            return;
        }

        Marshal.FreeHGlobal(arena.memory);
        arena.memory = IntPtr.Zero;
    }

    /// <summary>
    /// Helper to create a Clay_Color from RGBA values (0-255)
    /// </summary>
    public static ClayColor CreateColor(float red, float green, float blue, float alpha = 255.0f)
    {
        return new ClayColor { r = red, g = green, b = blue, a = alpha };
    }

    /// <summary>
    /// Helper to create a Clay_Vector2
    /// </summary>
    public static ClayVector2 CreateVector2(float x, float y)
    {
        return new ClayVector2 { x = x, y = y };
    }

    /// <summary>
    /// Helper to create a Clay_Dimensions
    /// </summary>
    public static ClayDimensions CreateDimensions(float width, float height)
    {
        return new ClayDimensions { width = width, height = height };
    }

    /// <summary>
    /// Helper to create a Clay_BoundingBox
    /// </summary>
    public static ClayBoundingBox CreateBoundingBox(float x, float y, float width, float height)
    {
        return new ClayBoundingBox { x = x, y = y, width = width, height = height };
    }

    /// <summary>
    /// Helper to create a Clay_CornerRadius with uniform radius
    /// </summary>
    public static ClayCornerRadius CreateCornerRadius(float radius)
    {
        return new ClayCornerRadius
        {
            topLeft = radius,
            topRight = radius,
            bottomLeft = radius,
            bottomRight = radius
        };
    }

    /// <summary>
    /// Helper to create a Clay_Padding
    /// </summary>
    public static ClayPadding CreatePadding(ushort left, ushort right, ushort top, ushort bottom)
    {
        return new ClayPadding { left = left, right = right, top = top, bottom = bottom };
    }

    /// <summary>
    /// Helper to create a Clay_Padding with uniform padding
    /// </summary>
    public static ClayPadding CreatePaddingUniform(ushort padding)
    {
        return new ClayPadding { left = padding, right = padding, top = padding, bottom = padding };
    }
}