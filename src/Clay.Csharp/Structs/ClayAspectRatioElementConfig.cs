using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Controls various settings related to aspect ratio scaling element.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayAspectRatioElementConfig
{
    /// <summary>A float representing the target "Aspect ratio" for an element, which is its final width divided by its final height.</summary>
    public float aspectRatio;
}