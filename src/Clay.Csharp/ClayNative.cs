using System.Runtime.InteropServices;
using Clay.Csharp.Enums;
using Clay.Csharp.Internal;
using Clay.Csharp.Structs;

namespace Clay.Csharp;

public static class ClayNative
{
    #region DELEGATE TYPES FOR CALLBACKS
    // All of these are marshaled to raw native function pointers (via Marshal.GetFunctionPointerForDelegate,
    // either directly or through the LibraryImport source generator), so they're pinned to the C ABI's
    // default calling convention explicitly - Clay is a plain C library, its exports are never __stdcall.

    /// <summary>
    /// Delegate for text measurement function.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate ClayDimensions MeasureTextFunction(ClayStringSlice text, IntPtr config, IntPtr userData);

    /// <summary>
    /// Delegate for scroll offset query function.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate ClayVector2 QueryScrollOffsetFunction(uint elementId, IntPtr userData);

    /// <summary>
    /// Delegate for pointer hover callback.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnHoverFunction(ClayElementId elementId, ClayPointerData pointerData, IntPtr userData);

    /// <summary>
    /// Delegate for error handler callback.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ErrorHandlerFunction(ClayErrorData errorData);

    /// <summary>
    /// Delegate for a transition's main callback (Clay_TransitionElementConfig.handler). Return true to keep
    /// receiving updates next frame, false once the transition has reached its target and should stop.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool TransitionFunction(ClayTransitionCallbackArguments arguments);

    /// <summary>
    /// Delegate matching Clay_TransitionElementConfig's nested .enter.setInitialState and .exit.setFinalState
    /// function pointers - given a reference state and the set of properties being animated, returns the
    /// Clay_TransitionData to animate from (enter) or to (exit).
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate ClayTransitionData TransitionStateSetter(ClayTransitionData state, ClayTransitionProperty properties);

    #endregion

    #region VERSION

    public static string GetVersion() => Marshal.PtrToStringUTF8(ClayNativeInternal.ClayNative_GetVersion())!;
    public static int GetMajorVersion() => ClayNativeInternal.ClayNative_GetVersionMajor();
    public static int GetMinorVersion() => ClayNativeInternal.ClayNative_GetVersionMinor();
    public static int GetPatchVersion() => ClayNativeInternal.ClayNative_GetVersionPatch();

    #endregion

    #region MEMORY MANAGEMENT

    public static uint Clay_MinMemorySize() => ClayNativeInternal.Clay_MinMemorySize();

    public static ClayArena Clay_CreateArenaWithCapacityAndMemory(UIntPtr capacity, IntPtr memory) =>
        ClayNativeInternal.Clay_CreateArenaWithCapacityAndMemory(capacity, memory);

    #endregion

    #region POINTER STATE

    public static void Clay_SetPointerState(ClayVector2 position, bool pointerDown) =>
        ClayNativeInternal.Clay_SetPointerState(position, pointerDown);

    public static ClayPointerData Clay_GetPointerState() => ClayNativeInternal.Clay_GetPointerState();

    #endregion

    #region INITIALIZATION

    public static IntPtr Clay_Initialize(ClayArena arena, ClayDimensions layoutDimensions, ClayErrorHandler errorHandler) =>
        ClayNativeInternal.Clay_Initialize(arena, layoutDimensions, errorHandler);

    public static IntPtr Clay_GetCurrentContext() => ClayNativeInternal.Clay_GetCurrentContext();

    public static void Clay_SetCurrentContext(IntPtr context) => ClayNativeInternal.Clay_SetCurrentContext(context);

    #endregion

    #region SCROLL CONTAINERS

    public static void
        Clay_UpdateScrollContainers(bool enableDragScrolling, ClayVector2 scrollDelta, float deltaTime) =>
        ClayNativeInternal.Clay_UpdateScrollContainers(enableDragScrolling, scrollDelta, deltaTime);

    public static ClayVector2 Clay_GetScrollOffset() => ClayNativeInternal.Clay_GetScrollOffset();

    public static ClayScrollContainerData Clay_GetScrollContainerData(ClayElementId id) => ClayNativeInternal.Clay_GetScrollContainerData(id);

    #endregion

    #region LAYOUT DIMENSIONS

    public static void Clay_SetLayoutDimensions(ClayDimensions dimensions) => ClayNativeInternal.Clay_SetLayoutDimensions(dimensions);

    public static ClayDimensions Clay_GetLayoutDimensions() => ClayNativeInternal.Clay_GetLayoutDimensions();

    #endregion

    #region LAYOUT COMPUTATION

    /// <summary>Called before starting any layout declarations.</summary>
    public static void Clay_BeginLayout() => ClayNativeInternal.Clay_BeginLayout();

    /// <summary>Called when all layout declarations are finished. Computes the layout and returns render commands.</summary>
    public static ClayRenderCommandArray Clay_EndLayout(float deltaTime) => ClayNativeInternal.Clay_EndLayout(deltaTime);

    #endregion

    #region ELEMENT ID AND DATA

    /// <summary>Gets the ID of the currently open element.</summary>
    public static uint Clay_GetOpenElementId() => ClayNativeInternal.Clay_GetOpenElementId();

    /// <summary>Calculates a hash ID from the given idString.</summary>
    public static ClayElementId Clay_GetElementId(ClayString idString) => ClayNativeInternal.Clay_GetElementId(idString);

    /// <summary>Calculates a hash ID from the given idString and index.</summary>
    public static ClayElementId Clay_GetElementIdWithIndex(ClayString idString, uint index) =>
        ClayNativeInternal.Clay_GetElementIdWithIndex(idString, index);

    /// <summary>Returns layout data such as the final calculated bounding box for an element with a given ID.</summary>
    public static ClayElementData Clay_GetElementData(ClayElementId id) => ClayNativeInternal.Clay_GetElementData(id);

    #endregion

    #region POINTER INTERACTIONS

    /// <summary>Returns true if the pointer is within the current element's bounding box. Works during element declaration.</summary>
    public static bool Clay_Hovered() => ClayNativeInternal.Clay_Hovered();

    /// <summary>Bind a callback that will be called when the pointer is within the current element's bounding box.</summary>
    public static void Clay_OnHover(OnHoverFunction onHoverFunction, IntPtr userData) =>
        ClayNativeInternal.Clay_OnHover(onHoverFunction, userData);

    /// <summary>Returns true if the pointer is within the element with the provided ID's bounding box.</summary>
    public static bool Clay_PointerOver(ClayElementId elementId) => ClayNativeInternal.Clay_PointerOver(elementId);

    /// <summary>Returns the array of element IDs that the pointer is currently over.</summary>
    public static ClayElementIdArray Clay_GetPointerOverIds() => ClayNativeInternal.Clay_GetPointerOverIds();

    #endregion

    #region TEXT MEASUREMENT

    /// <summary>Binds a callback function that Clay will call to determine the dimensions of a given string slice.</summary>
    public static void Clay_SetMeasureTextFunction(MeasureTextFunction measureTextFunction, IntPtr userData) =>
        ClayNativeInternal.Clay_SetMeasureTextFunction(measureTextFunction, userData);

    /// <summary>Experimental - used when Clay needs to integrate with externally managed scrolling containers.</summary>
    public static void Clay_SetQueryScrollOffsetFunction(QueryScrollOffsetFunction queryScrollOffsetFunction, IntPtr userData) =>
        ClayNativeInternal.Clay_SetQueryScrollOffsetFunction(queryScrollOffsetFunction, userData);

    /// <summary>A bounds-checked "get" function for the Clay_RenderCommandArray returned from Clay_EndLayout().</summary>
    public static ClayRenderCommand Clay_RenderCommandArray_Get(ref ClayRenderCommandArray array, int index)
    {
        IntPtr commandPtr = ClayNativeInternal.Clay_RenderCommandArray_Get(ref array, index);
        return Marshal.PtrToStructure<ClayRenderCommand>(commandPtr);
    }

    #endregion

    #region DEBUG TOOLS

    /// <summary>Enables and disables Clay's internal debug tools. This state is retained and does not need to be set each frame.</summary>
    public static void Clay_SetDebugModeEnabled(bool enabled) => ClayNativeInternal.Clay_SetDebugModeEnabled(enabled);

    /// <summary>Returns true if Clay's internal debug tools are currently enabled.</summary>
    public static bool Clay_IsDebugModeEnabled() => ClayNativeInternal.Clay_IsDebugModeEnabled();

    #endregion

    #region CULLING

    /// <summary>Enables and disables visibility culling.</summary>
    public static void Clay_SetCullingEnabled(bool enabled) => ClayNativeInternal.Clay_SetCullingEnabled(enabled);

    #endregion

    #region CONFIGURATION

    /// <summary>Returns the maximum number of UI elements supported by Clay's current configuration.</summary>
    public static int Clay_GetMaxElementCount() => ClayNativeInternal.Clay_GetMaxElementCount();

    /// <summary>Modifies the maximum number of UI elements supported. May require reallocating memory and re-calling Clay_Initialize().</summary>
    public static void Clay_SetMaxElementCount(int maxElementCount) => ClayNativeInternal.Clay_SetMaxElementCount(maxElementCount);

    /// <summary>Returns the maximum number of measured "words" Clay can store in its internal text measurement cache.</summary>
    public static int Clay_GetMaxMeasureTextCacheWordCount() => ClayNativeInternal.Clay_GetMaxMeasureTextCacheWordCount();

    /// <summary>Modifies the maximum number of measured "words" Clay can store in its text measurement cache.</summary>
    public static void Clay_SetMaxMeasureTextCacheWordCount(int maxMeasureTextCacheWordCount) =>
        ClayNativeInternal.Clay_SetMaxMeasureTextCacheWordCount(maxMeasureTextCacheWordCount);

    /// <summary>Resets Clay's internal text measurement cache. Useful if font mappings have changed or fonts have been reloaded.</summary>
    public static void Clay_ResetMeasureTextCache() => ClayNativeInternal.Clay_ResetMeasureTextCache();

    #endregion

    #region TRANSITIONS

    /// <summary>A built-in transition function that uses the "Ease Out" curve.</summary>
    public static bool Clay_EaseOut(ClayTransitionCallbackArguments arguments) => ClayNativeInternal.Clay_EaseOut(arguments);

    #endregion

    #region DECLARATIVE API (internal - used by ClayElement / ClayScope, see Element.cs)

    internal static void Clay__OpenElement() => ClayNativeInternal.Clay__OpenElement();
    internal static void Clay__OpenElementWithId(ClayElementId elementId) => ClayNativeInternal.Clay__OpenElementWithId(elementId);
    internal static void Clay__ConfigureOpenElement(ClayElementDeclaration config) => ClayNativeInternal.Clay__ConfigureOpenElement(config);
    internal static void Clay__ConfigureOpenElementPtr(ref ClayElementDeclaration config) => ClayNativeInternal.Clay__ConfigureOpenElementPtr(ref config);
    internal static void Clay__CloseElement() => ClayNativeInternal.Clay__CloseElement();
    internal static ClayElementId Clay__HashString(ClayString key, uint seed) => ClayNativeInternal.Clay__HashString(key, seed);
    internal static ClayElementId Clay__HashStringWithOffset(ClayString key, uint offset, uint seed) => ClayNativeInternal.Clay__HashStringWithOffset(key, offset, seed);
    internal static void Clay__OpenTextElement(ClayString text, ClayTextElementConfig textConfig) => ClayNativeInternal.Clay__OpenTextElement(text, textConfig);

    #endregion
}
