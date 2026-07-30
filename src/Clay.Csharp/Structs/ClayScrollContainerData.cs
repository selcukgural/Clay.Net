using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Data representing the current internal state of a scrolling element.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayScrollContainerData
{
    /// <summary>Pointer to the real internal scroll position (Clay_Vector2*). Mutating it may cause a change in final layout.</summary>
    public IntPtr scrollPosition;

    /// <summary>The bounding box of the scroll element.</summary>
    public ClayDimensions scrollContainerDimensions;

    /// <summary>The outer dimensions of the inner scroll container content, including the padding of the parent scroll container.</summary>
    public ClayDimensions contentDimensions;

    /// <summary>The config that was originally passed to the clip element.</summary>
    public ClayClipElementConfig config;

    /// <summary>Indicates whether an actual scroll container matched the provided ID or if the default struct was returned.</summary>
    [MarshalAs(UnmanagedType.I1)]
    public bool found;
}
