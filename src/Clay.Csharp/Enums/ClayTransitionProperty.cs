namespace Clay.Csharp.Enums;

/// <summary>
/// Represents transition properties that can be animated. Bitflags - can be combined.
/// Note: unlike most Clay enums, this one is NOT packed to a single byte in the native library
/// (it uses a plain C `enum`, which defaults to the compiler's native `int` size).
/// </summary>
[Flags]
public enum ClayTransitionProperty : uint
{
    ClayTransitionPropertyNone = 0,
    ClayTransitionPropertyX = 1,
    ClayTransitionPropertyY = 2,
    ClayTransitionPropertyPosition = ClayTransitionPropertyX | ClayTransitionPropertyY,
    ClayTransitionPropertyWidth = 4,
    ClayTransitionPropertyHeight = 8,
    ClayTransitionPropertyDimensions = ClayTransitionPropertyWidth | ClayTransitionPropertyHeight,
    ClayTransitionPropertyBoundingBox = ClayTransitionPropertyPosition | ClayTransitionPropertyDimensions,
    ClayTransitionPropertyBackgroundColor = 16,
    ClayTransitionPropertyOverlayColor = 32,
    ClayTransitionPropertyCornerRadius = 64,
    ClayTransitionPropertyBorderColor = 128,
    ClayTransitionPropertyBorderWidth = 256,
    ClayTransitionPropertyBorder = ClayTransitionPropertyBorderColor | ClayTransitionPropertyBorderWidth,
}
