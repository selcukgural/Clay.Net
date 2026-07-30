namespace Clay.Csharp.Enums;

/// <summary>
/// Represents the type of error clay encountered while computing layout.
/// </summary>
public enum ClayErrorType : byte
{
    /// <summary>A text measurement function wasn't provided using Clay_SetMeasureTextFunction(), or the provided function was null.</summary>
    ClayErrorTypeTextMeasurementFunctionNotProvided = 0,

    /// <summary>Clay attempted to allocate its internal data structures but ran out of space.</summary>
    ClayErrorTypeArenaCapacityExceeded = 1,

    /// <summary>Clay ran out of capacity in its internal array for storing elements. Increase with Clay_SetMaxElementCount().</summary>
    ClayErrorTypeElementsCapacityExceeded = 2,

    /// <summary>Clay ran out of capacity in its internal text measurement cache. Increase with Clay_SetMaxMeasureTextCacheWordCount().</summary>
    ClayErrorTypeTextMeasurementCapacityExceeded = 3,

    /// <summary>Two elements were declared with exactly the same ID within one layout.</summary>
    ClayErrorTypeDuplicateId = 4,

    /// <summary>A floating element was declared using CLAY_ATTACH_TO_ELEMENT_ID and either an invalid .parentId was provided or no element with the provided .parentId was found.</summary>
    ClayErrorTypeFloatingContainerParentNotFound = 5,

    /// <summary>An element was declared using CLAY_SIZING_PERCENT but the percentage value was over 1.</summary>
    ClayErrorTypePercentageOver1 = 6,

    /// <summary>Clay encountered an internal error.</summary>
    ClayErrorTypeInternalError = 7,

    /// <summary>Clay__OpenElement was called more times than Clay__CloseElement.</summary>
    ClayErrorTypeUnbalancedOpenClose = 8,

    /// <summary>Clay ran out of capacity in its internal hash map for storing element IDs -> elements. Increase with Clay_SetMaxElementCount().</summary>
    ClayErrorTypeHashMapCapacityExceeded = 9,
}
