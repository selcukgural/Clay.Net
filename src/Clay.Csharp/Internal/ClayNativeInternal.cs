using System.Runtime.InteropServices;
using Clay.Csharp.Structs;

namespace Clay.Csharp.Internal;

internal static partial class ClayNativeInternal
{
    private const string ClayDll = "clay_native";

    [LibraryImport(ClayDll, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial IntPtr ClayNative_GetVersion();

    [LibraryImport(ClayDll)]
    internal static partial int ClayNative_GetVersionMajor();

    [LibraryImport(ClayDll)]
    internal static partial int ClayNative_GetVersionMinor();

    [LibraryImport(ClayDll)]
    internal static partial int ClayNative_GetVersionPatch();

    // ========================================
    // MEMORY MANAGEMENT
    // ========================================

    /// <summary>
    /// Returns the size, in bytes, of the minimum amount of memory Clay requires to operate at its current settings.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial uint Clay_MinMemorySize();

    /// <summary>
    /// Creates an arena for clay to use for its internal allocations, given a certain capacity in bytes and a pointer to an allocation of at least that size.
    /// Intended to be used with Clay_MinMemorySize in the following way:
    /// uint32_t minMemoryRequired = Clay_MinMemorySize();
    /// Clay_Arena clayMemory = Clay_CreateArenaWithCapacityAndMemory(minMemoryRequired, malloc(minMemoryRequired));
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial ClayArena Clay_CreateArenaWithCapacityAndMemory(UIntPtr capacity, IntPtr memory);

    // ========================================
    // POINTER STATE
    // ========================================

    /// <summary>
    /// Sets the state of the "pointer" (i.e. the mouse or touch) in Clay's internal data. Used for detecting and responding to mouse events in the debug view,
    /// as well as for Clay_Hovered() and scroll element handling.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void Clay_SetPointerState(ClayVector2 position, [MarshalAs(UnmanagedType.I1)] bool pointerDown);

    /// <summary>
    /// Returns the state of the "pointer" (i.e. the mouse or touch) which was set via Clay_SetPointerState().
    /// </summary>
    [DllImport(ClayDll, CallingConvention = CallingConvention.Cdecl)]
    internal static extern ClayPointerData Clay_GetPointerState();

    // ========================================
    // INITIALIZATION
    // ========================================

    /// <summary>
    /// Initialize Clay's internal arena and setup required data before layout can begin. Only needs to be called once.
    /// - arena can be created using Clay_CreateArenaWithCapacityAndMemory()
    /// - layoutDimensions are the initial bounding dimensions of the layout (i.e. the screen width and height for a full screen layout)
    /// - errorHandler is used by Clay to inform you if something has gone wrong in configuration or layout.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial IntPtr Clay_Initialize(ClayArena arena, ClayDimensions layoutDimensions, ClayErrorHandler errorHandler);

    /// <summary>
    /// Returns the Context that clay is currently using. Used when using multiple instances of clay simultaneously.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial IntPtr Clay_GetCurrentContext();

    /// <summary>
    /// Sets the context that clay will use to compute the layout.
    /// Used to restore a context saved from Clay_GetCurrentContext when using multiple instances of clay simultaneously.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void Clay_SetCurrentContext(IntPtr context);

    // ========================================
    // SCROLL CONTAINERS
    // ========================================

    /// <summary>
    /// Updates the state of Clay's internal scroll data, updating scroll content positions if scrollDelta is non zero, and progressing momentum scrolling.
    /// - enableDragScrolling when set to true will enable mobile device like "touch drag" scroll of scroll containers, including momentum scrolling after the touch has ended.
    /// - scrollDelta is the amount to scroll this frame on each axis in pixels.
    /// - deltaTime is the time in seconds since the last "frame" (scroll update)
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void Clay_UpdateScrollContainers([MarshalAs(UnmanagedType.I1)] bool enableDragScrolling, ClayVector2 scrollDelta,
                                                           float deltaTime);

    /// <summary>
    /// Returns the internally stored scroll offset for the currently open element.
    /// Generally intended for use with clip elements to create scrolling containers.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial ClayVector2 Clay_GetScrollOffset();

    /// <summary>
    /// Returns data representing the state of the scrolling element with the provided ID.
    /// The returned Clay_ScrollContainerData contains a `found` bool that will be true if a scroll element was found with the provided ID.
    /// </summary>
    [DllImport(ClayDll, CallingConvention = CallingConvention.Cdecl)]
    internal static extern ClayScrollContainerData Clay_GetScrollContainerData(ClayElementId id);

    // ========================================
    // LAYOUT DIMENSIONS
    // ========================================

    /// <summary>
    /// Updates the layout dimensions in response to the window or outer container being resized.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void Clay_SetLayoutDimensions(ClayDimensions dimensions);

    /// <summary>
    /// Returns the current dimensions set by Clay_SetLayoutDimensions.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial ClayDimensions Clay_GetLayoutDimensions();

    // ========================================
    // LAYOUT COMPUTATION
    // ========================================

    /// <summary>
    /// Called before starting any layout declarations.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void Clay_BeginLayout();

    /// <summary>
    /// Called when all layout declarations are finished.
    /// Computes the layout and generates and returns the array of render commands to draw.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial ClayRenderCommandArray Clay_EndLayout(float deltaTime);

    // ========================================
    // ELEMENT ID AND DATA
    // ========================================

    /// <summary>
    /// Gets the ID of the currently open element, useful for retrieving IDs generated by auto ID macros
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial uint Clay_GetOpenElementId();

    /// <summary>
    /// Calculates a hash ID from the given idString.
    /// Generally only used for dynamic strings when string literal IDs can't be used.
    /// </summary>
    [DllImport(ClayDll, CallingConvention = CallingConvention.Cdecl)]
    internal static extern ClayElementId Clay_GetElementId(ClayString idString);

    /// <summary>
    /// Calculates a hash ID from the given idString and index.
    /// - index is used to avoid constructing dynamic ID strings in loops.
    /// Generally only used for dynamic strings when indexed string literal IDs can't be used.
    /// </summary>
    [DllImport(ClayDll, CallingConvention = CallingConvention.Cdecl)]
    internal static extern ClayElementId Clay_GetElementIdWithIndex(ClayString idString, uint index);

    /// <summary>
    /// Returns layout data such as the final calculated bounding box for an element with a given ID.
    /// The returned Clay_ElementData contains a `found` bool that will be true if an element with the provided ID was found.
    /// This ID can be calculated either with CLAY_ID() for string literal IDs, or Clay_GetElementId for dynamic strings.
    /// </summary>
    [DllImport(ClayDll, CallingConvention = CallingConvention.Cdecl)]
    internal static extern ClayElementData Clay_GetElementData(ClayElementId id);

    // ========================================
    // POINTER INTERACTIONS
    // ========================================

    /// <summary>
    /// Returns true if the pointer position provided by Clay_SetPointerState is within the current element's bounding box.
    /// Works during element declaration.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Clay_Hovered();

    /// <summary>
    /// Bind a callback that will be called when the pointer position provided by Clay_SetPointerState is within the current element's bounding box.
    /// - onHoverFunction is a function pointer to a user defined function.
    /// - userData is a pointer that will be transparently passed through when the onHoverFunction is called.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void Clay_OnHover(ClayNative.OnHoverFunction onHoverFunction, IntPtr userData);

    /// <summary>
    /// An imperative function that returns true if the pointer position provided by Clay_SetPointerState is within the element with the provided ID's bounding box.
    /// This ID can be calculated either with CLAY_ID() for string literal IDs, or Clay_GetElementId for dynamic strings.
    /// </summary>
    [DllImport(ClayDll, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static extern bool Clay_PointerOver(ClayElementId elementId);

    /// <summary>
    /// Returns the array of element IDs that the pointer is currently over.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial ClayElementIdArray Clay_GetPointerOverIds();

    // ========================================
    // TEXT MEASUREMENT
    // ========================================

    /// <summary>
    /// Binds a callback function that Clay will call to determine the dimensions of a given string slice.
    /// - measureTextFunction is a user provided function that adheres to the interface Clay_Dimensions (Clay_StringSlice text, Clay_TextElementConfig *config, void *userData);
    /// - userData is a pointer that will be transparently passed through when the measureTextFunction is called.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void Clay_SetMeasureTextFunction(ClayNative.MeasureTextFunction measureTextFunction, IntPtr userData);

    /// <summary>
    /// Experimental - Used in cases where Clay needs to integrate with a system that manages its own scrolling containers externally.
    /// Please reach out if you plan to use this function, as it may be subject to change.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void Clay_SetQueryScrollOffsetFunction(ClayNative.QueryScrollOffsetFunction queryScrollOffsetFunction, IntPtr userData);

    /// <summary>
    /// A bounds-checked "get" function for the Clay_RenderCommandArray returned from Clay_EndLayout().
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial IntPtr Clay_RenderCommandArray_Get(ref ClayRenderCommandArray array, int index);

    // ========================================
    // DEBUG TOOLS
    // ========================================

    /// <summary>
    /// Enables and disables Clay's internal debug tools.
    /// This state is retained and does not need to be set each frame.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void Clay_SetDebugModeEnabled([MarshalAs(UnmanagedType.I1)] bool enabled);

    /// <summary>
    /// Returns true if Clay's internal debug tools are currently enabled.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Clay_IsDebugModeEnabled();

    // ========================================
    // CULLING
    // ========================================

    /// <summary>
    /// Enables and disables visibility culling. By default, Clay will not generate render commands for elements whose bounding box is entirely outside the screen.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void Clay_SetCullingEnabled([MarshalAs(UnmanagedType.I1)] bool enabled);

    // ========================================
    // CONFIGURATION
    // ========================================

    /// <summary>
    /// Returns the maximum number of UI elements supported by Clay's current configuration.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial int Clay_GetMaxElementCount();

    /// <summary>
    /// Modifies the maximum number of UI elements supported by Clay's current configuration.
    /// This may require reallocating additional memory, and re-calling Clay_Initialize();
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void Clay_SetMaxElementCount(int maxElementCount);

    /// <summary>
    /// Returns the maximum number of measured "words" (whitespace seperated runs of characters) that Clay can store in its internal text measurement cache.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial int Clay_GetMaxMeasureTextCacheWordCount();

    /// <summary>
    /// Modifies the maximum number of measured "words" (whitespace seperated runs of characters) that Clay can store in its internal text measurement cache.
    /// This may require reallocating additional memory, and re-calling Clay_Initialize();
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void Clay_SetMaxMeasureTextCacheWordCount(int maxMeasureTextCacheWordCount);

    /// <summary>
    /// Resets Clay's internal text measurement cache. Useful if font mappings have changed or fonts have been reloaded.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void Clay_ResetMeasureTextCache();

    // ========================================
    // TRANSITIONS
    // ========================================

    /// <summary>
    /// A built-in transition function that uses the "Ease Out" curve
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Clay_EaseOut(ClayTransitionCallbackArguments arguments);

    // ========================================
    // INTERNAL API (Required by macros in C, but useful for C# interop)
    // ========================================

    /// <summary>
    /// Internal: Open an element. Used by Clay's declarative macros.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void Clay__OpenElement();

    /// <summary>
    /// Internal: Open an element with a specific ID. Used by Clay's declarative macros.
    /// </summary>
    [DllImport(ClayDll, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void Clay__OpenElementWithId(ClayElementId elementId);

    /// <summary>
    /// Internal: Configure the currently open element. Used by Clay's declarative macros.
    /// </summary>
    [DllImport(ClayDll, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void Clay__ConfigureOpenElement(ClayElementDeclaration config);

    /// <summary>
    /// Internal: Configure the currently open element with a pointer. Used by Clay's declarative macros.
    /// </summary>
    [DllImport(ClayDll, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void Clay__ConfigureOpenElementPtr(ref ClayElementDeclaration config);

    /// <summary>
    /// Internal: Close the currently open element. Used by Clay's declarative macros.
    /// </summary>
    [LibraryImport(ClayDll)]
    [UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvCdecl)])]
    internal static partial void Clay__CloseElement();

    /// <summary>
    /// Internal: Calculate a hash from a string.
    /// </summary>
    [DllImport(ClayDll, CallingConvention = CallingConvention.Cdecl)]
    internal static extern ClayElementId Clay__HashString(ClayString key, uint seed);

    /// <summary>
    /// Internal: Calculate a hash from a string with offset.
    /// </summary>
    [DllImport(ClayDll, CallingConvention = CallingConvention.Cdecl)]
    internal static extern ClayElementId Clay__HashStringWithOffset(ClayString key, uint offset, uint seed);

    /// <summary>
    /// Internal: Open a text element.
    /// </summary>
    [DllImport(ClayDll, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void Clay__OpenTextElement(ClayString text, ClayTextElementConfig textConfig);
}