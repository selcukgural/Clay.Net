using System.Runtime.InteropServices;
using System.Text;
using Clay.Csharp.Declarative;
using Clay.Csharp.Structs;
using Xunit;

namespace Clay.Csharp.Tests;

/// <summary>
/// ClayTextArena is internal - accessible here via InternalsVisibleTo (see Clay.Csharp.csproj). Tests run
/// sequentially against xUnit's default collection behavior would still race on the arena's shared static
/// state if parallelized across test classes, so this class opts out of parallelization.
/// </summary>
[Collection("ClayNative")]
public class ClayTextArenaTests
{
    [Fact]
    public void Intern_ProducesCorrectUtf8BytesAndLength()
    {
        ClayTextArena.Reset();
        try
        {
            const string text = "Merhaba dünya! 😀";
            ClayString clayString = ClayTextArena.Intern(text);

            byte[] expectedBytes = Encoding.UTF8.GetBytes(text);
            Assert.Equal(expectedBytes.Length, clayString.length);

            byte[] actualBytes = new byte[clayString.length];
            Marshal.Copy(clayString.chars, actualBytes, 0, clayString.length);
            Assert.Equal(expectedBytes, actualBytes);
        }
        finally
        {
            ClayTextArena.Reset();
        }
    }

    [Fact]
    public void Intern_EmptyOrNull_ReturnsZeroLengthNullChars()
    {
        ClayString empty = ClayTextArena.Intern("");
        Assert.Equal(0, empty.length);
        Assert.Equal(IntPtr.Zero, empty.chars);
    }

    [Fact]
    public void Reset_FreesPreviousAllocations_SubsequentInternStillWorks()
    {
        ClayTextArena.Reset();
        ClayString first = ClayTextArena.Intern("first frame's text");
        Assert.NotEqual(IntPtr.Zero, first.chars);

        // After Reset(), `first`'s underlying memory is freed - Intern() must still work correctly for
        // the next frame's strings (this doesn't (and can't, safely) assert the freed pointer is
        // unreadable - it asserts the arena's internal bookkeeping isn't corrupted by the free/reset).
        ClayTextArena.Reset();
        ClayString second = ClayTextArena.Intern("second frame's text");
        Assert.NotEqual(IntPtr.Zero, second.chars);

        byte[] actualBytes = new byte[second.length];
        Marshal.Copy(second.chars, actualBytes, 0, second.length);
        Assert.Equal(Encoding.UTF8.GetBytes("second frame's text"), actualBytes);

        ClayTextArena.Reset();
    }
}
