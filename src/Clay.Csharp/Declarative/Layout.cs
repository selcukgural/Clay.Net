using Clay.Csharp.Structs;

namespace Clay.Csharp.Declarative;

/// <summary>
/// Idiomatic C# surface for declaring Clay layouts, standing in for the C library's macro-based
/// declarative syntax (CLAY, CLAY_TEXT, CLAY_ID, ...) which has no direct C# equivalent.
/// Typical usage (pair with `using static Clay.Csharp.Declarative.Layout;` for macro-like terseness):
/// <code>
/// BeginLayout();
/// using (Element("Container", new ClayElementDeclaration { layout = ..., backgroundColor = ... }))
/// {
///     Text("Hello, Clay!", new ClayTextElementConfig { fontSize = 16, textColor = ... });
/// }
/// ClayRenderCommandArray commands = EndLayout(deltaTime: 0.016f);
/// </code>
/// </summary>
public static class Layout
{
    #region FRAME LIFECYCLE

    /// <summary>Resets the per-frame text/id string arena and calls Clay_BeginLayout().</summary>
    public static void BeginLayout()
    {
        ClayTextArena.Reset();
        ClayNative.Clay_BeginLayout();
    }

    /// <summary>Computes the layout and returns the array of render commands to draw.</summary>
    public static ClayRenderCommandArray EndLayout(float deltaTime) => ClayNative.Clay_EndLayout(deltaTime);

    #endregion

    #region ELEMENTS

    /// <summary>Opens an element with an explicit string id. Equivalent to CLAY(CLAY_ID(id), declaration) { ... }.</summary>
    public static ClayElementScope Element(string id, in ClayElementDeclaration declaration) => Element(Id(id), declaration);

    /// <summary>Opens an element with an explicit, already-hashed id. Equivalent to CLAY(id, declaration) { ... }.</summary>
    public static ClayElementScope Element(ClayElementId id, in ClayElementDeclaration declaration)
    {
        Clay__OpenElementWithId(id);
        ClayElementDeclaration decl = declaration;
        Clay__ConfigureOpenElementPtr(ref decl);
        return default;
    }

    /// <summary>Opens an element without an explicit id, letting Clay assign one automatically. Equivalent to CLAY_AUTO_ID(declaration) { ... }.</summary>
    public static ClayElementScope Element(in ClayElementDeclaration declaration)
    {
        Clay__OpenElement();
        ClayElementDeclaration decl = declaration;
        Clay__ConfigureOpenElementPtr(ref decl);
        return default;
    }

    /// <summary>Declares a text element. Equivalent to CLAY_TEXT(text, config).</summary>
    public static void Text(string text, in ClayTextElementConfig config) =>
        Clay__OpenTextElement(ClayTextArena.Intern(text), config);

    #endregion

    #region IDS

    /// <summary>Calculates a hash ID from a string literal. Equivalent to CLAY_ID(label).</summary>
    public static ClayElementId Id(string label) => Clay__HashString(ClayTextArena.Intern(label), 0);

    /// <summary>Calculates a hash ID from a string and index, to avoid constructing dynamic ID strings in loops. Equivalent to CLAY_IDI(label, index).</summary>
    public static ClayElementId Id(string label, uint index) => Clay__HashStringWithOffset(ClayTextArena.Intern(label), index, 0);

    /// <summary>Calculates a hash ID scoped to the currently open parent element. Equivalent to CLAY_ID_LOCAL(label).</summary>
    public static ClayElementId IdLocal(string label) => Clay__HashString(ClayTextArena.Intern(label), ClayNative.Clay_GetOpenElementId());

    /// <summary>Calculates a hash ID scoped to the currently open parent element, with an index. Equivalent to CLAY_IDI_LOCAL(label, index).</summary>
    public static ClayElementId IdLocal(string label, uint index) =>
        Clay__HashStringWithOffset(ClayTextArena.Intern(label), index, ClayNative.Clay_GetOpenElementId());

    #endregion

    #region INTERNAL - forwards to ClayNative's declarative-support functions

    internal static void Clay__OpenElement() => ClayNative.Clay__OpenElement();
    internal static void Clay__OpenElementWithId(ClayElementId id) => ClayNative.Clay__OpenElementWithId(id);
    internal static void Clay__ConfigureOpenElementPtr(ref ClayElementDeclaration config) => ClayNative.Clay__ConfigureOpenElementPtr(ref config);
    internal static void Clay__CloseElement() => ClayNative.Clay__CloseElement();
    internal static ClayElementId Clay__HashString(ClayString key, uint seed) => ClayNative.Clay__HashString(key, seed);
    internal static ClayElementId Clay__HashStringWithOffset(ClayString key, uint offset, uint seed) => ClayNative.Clay__HashStringWithOffset(key, offset, seed);
    internal static void Clay__OpenTextElement(ClayString text, ClayTextElementConfig config) => ClayNative.Clay__OpenTextElement(text, config);

    #endregion
}
