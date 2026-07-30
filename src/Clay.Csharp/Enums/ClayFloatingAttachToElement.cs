namespace Clay.Csharp.Enums;

/// <summary>
/// Controls which element a floating element is "attached" to (i.e. relative offset from).
/// </summary>
public enum ClayFloatingAttachToElement : byte
{
    /// <summary>(default) Disables floating for this element.</summary>
    ClayAttachToNone = 0,

    /// <summary>Attaches this floating element to its parent, positioned based on the .attachPoints and .offset fields.</summary>
    ClayAttachToParent = 1,

    /// <summary>Attaches this floating element to an element with a specific ID, specified with the .parentId field.</summary>
    ClayAttachToElementWithId = 2,

    /// <summary>Attaches this floating element to the root of the layout, similar to "absolute positioning".</summary>
    ClayAttachToRoot = 3,
}
