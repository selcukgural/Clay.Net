using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Represents the declaration of a UI element with its configuration.
/// Note: unlike the C API, the element's id is NOT part of this struct - it is passed
/// separately to Clay__OpenElementWithId (see the CLAY(id, ...) macro in clay.h).
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayElementDeclaration
{
    /// <summary>Controls various settings that affect the size and position of an element, as well as the sizes and positions of any child elements.</summary>
    public ClayLayoutConfig layout;

    /// <summary>Controls the background color of the resulting element.</summary>
    public ClayColor backgroundColor;

    /// <summary>Perform an image editing style "Color Overlay" on this element and all its children.</summary>
    public ClayColor overlayColor;

    /// <summary>Controls the "radius", or corner rounding of elements, including rectangles, borders and images.</summary>
    public ClayCornerRadius cornerRadius;

    /// <summary>Controls settings related to aspect ratio scaling.</summary>
    public ClayAspectRatioElementConfig aspectRatio;

    /// <summary>Controls settings related to image elements.</summary>
    public ClayImageElementConfig image;

    /// <summary>Controls whether and how an element "floats" above other elements.</summary>
    public ClayFloatingElementConfig floating;

    /// <summary>Used to create CUSTOM render commands, usually to render element types not supported by Clay.</summary>
    public ClayCustomElementConfig custom;

    /// <summary>Controls whether an element should clip its contents, as well as child x,y offset configuration for scrolling.</summary>
    public ClayClipElementConfig clip;

    /// <summary>Controls settings related to element borders, and will generate BORDER render commands.</summary>
    public ClayBorderElementConfig border;

    /// <summary>Controls settings related to animated transitions of this element's properties.</summary>
    public ClayTransitionElementConfig transition;

    /// <summary>A pointer that will be transparently passed through to resulting render commands.</summary>
    public IntPtr userData;
}
