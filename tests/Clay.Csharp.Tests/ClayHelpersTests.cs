using System.Runtime.InteropServices;
using Clay.Csharp.Internal;
using Clay.Csharp.Structs;
using Xunit;

namespace Clay.Csharp.Tests;

public class ClayHelpersTests
{
    [Fact]
    public void GetElementId_ReadsStructsByValue_NotPointers()
    {
        // Regression test for a real bug: GetElementId used to treat the native Clay_ElementId* array as
        // if it held *pointers* to ClayElementId (via a spurious Marshal.ReadIntPtr), when the array
        // actually holds the structs contiguously by value. Build a small unmanaged array by hand and
        // confirm each element round-trips correctly.
        const int count = 3;
        int elemSize = Marshal.SizeOf<ClayElementId>();
        IntPtr buffer = Marshal.AllocHGlobal(elemSize * count);
        try
        {
            ClayElementId[] expected = new ClayElementId[count];
            for (int i = 0; i < count; i++)
            {
                ClayElementId e = new() { id = (uint)(1000 + i), offset = (uint)i, baseId = (uint)(i * 7) };
                expected[i] = e;
                Marshal.StructureToPtr(e, buffer + i * elemSize, false);
            }

            ClayElementIdArray array = new() { capacity = count, length = count, internalArray = buffer };

            for (int i = 0; i < count; i++)
            {
                ClayElementId actual = ClayHelpers.GetElementId(array, i);
                Assert.Equal(expected[i].id, actual.id);
                Assert.Equal(expected[i].offset, actual.offset);
                Assert.Equal(expected[i].baseId, actual.baseId);
            }
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    [Fact]
    public void GetElementId_OutOfRange_Throws()
    {
        ClayElementIdArray array = new() { capacity = 0, length = 0, internalArray = IntPtr.Zero };
        Assert.Throws<IndexOutOfRangeException>(() => ClayHelpers.GetElementId(array, 0));
        Assert.Throws<IndexOutOfRangeException>(() => ClayHelpers.GetElementId(array, -1));
    }

    [Theory]
    [InlineData("Hello, Clay.Net!")]
    [InlineData("Merhaba dünya! 😀")]
    [InlineData("")]
    public void CreateClayString_RoundTripsThroughClayStringToManaged(string original)
    {
        ClayString clayString = ClayHelpers.CreateClayString(original);
        try
        {
            string roundTripped = ClayHelpers.ClayStringToManaged(clayString);
            Assert.Equal(original, roundTripped);

            // Regression test: length must be the UTF-8 *byte* count, not the UTF-16 char count - they
            // differ for the emoji test case above (surrogate pair -> 4 UTF-8 bytes, 2 UTF-16 chars).
            System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
            Assert.Equal(utf8.GetByteCount(original), clayString.length);
        }
        finally
        {
            if (clayString.chars != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(clayString.chars);
            }
        }
    }

    [Fact]
    public void ClayStringToManaged_NullChars_ReturnsEmpty()
    {
        ClayString clayString = new() { isStaticallyAllocated = false, length = 0, chars = IntPtr.Zero };
        Assert.Equal(string.Empty, ClayHelpers.ClayStringToManaged(clayString));
    }

    [Fact]
    public void GetRenderCommand_OutOfRange_Throws()
    {
        ClayRenderCommandArray array = new() { capacity = 0, length = 0, internalArray = IntPtr.Zero };
        Assert.Throws<IndexOutOfRangeException>(() => ClayHelpers.GetRenderCommand(array, 0));
    }

    [Fact]
    public void CreateArena_ThenFreeArena_DoesNotThrow_AndZeroesMemoryPointer()
    {
        ClayArena arena = ClayHelpers.CreateArena(1024);
        Assert.NotEqual(IntPtr.Zero, arena.memory);
        Assert.Equal((UIntPtr)1024, arena.capacity);

        ClayHelpers.FreeArena(ref arena);
        Assert.Equal(IntPtr.Zero, arena.memory);
    }

    [Fact]
    public void FreeArena_CalledTwice_IsSafe()
    {
        ClayArena arena = ClayHelpers.CreateArena(64);
        ClayHelpers.FreeArena(ref arena);
        ClayHelpers.FreeArena(ref arena); // must not double-free
    }

    [Fact]
    public void CreateColor_DefaultsAlphaTo255()
    {
        ClayColor color = ClayHelpers.CreateColor(10, 20, 30);
        Assert.Equal(10, color.r);
        Assert.Equal(20, color.g);
        Assert.Equal(30, color.b);
        Assert.Equal(255, color.a);
    }

    [Fact]
    public void CreateCornerRadius_AppliesUniformlyToAllCorners()
    {
        ClayCornerRadius radius = ClayHelpers.CreateCornerRadius(12);
        Assert.Equal(12, radius.topLeft);
        Assert.Equal(12, radius.topRight);
        Assert.Equal(12, radius.bottomLeft);
        Assert.Equal(12, radius.bottomRight);
    }

    [Fact]
    public void CreatePaddingUniform_AppliesToAllSides()
    {
        ClayPadding padding = ClayHelpers.CreatePaddingUniform(8);
        Assert.Equal((ushort)8, padding.left);
        Assert.Equal((ushort)8, padding.right);
        Assert.Equal((ushort)8, padding.top);
        Assert.Equal((ushort)8, padding.bottom);
    }
}
