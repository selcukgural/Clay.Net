namespace Clay.Csharp.Enums;

/// <summary>
/// Used by renderers to determine specific handling for each render command.
/// </summary>
public enum ClayRenderCommandType : byte
{
    /// <summary>This command type should be skipped.</summary>
    ClayRenderCommandTypeNone = 0,
    ClayRenderCommandTypeRectangle = 1,
    ClayRenderCommandTypeBorder = 2,
    ClayRenderCommandTypeText = 3,
    ClayRenderCommandTypeImage = 4,
    ClayRenderCommandTypeScissorStart = 5,
    ClayRenderCommandTypeScissorEnd = 6,
    ClayRenderCommandTypeOverlayColorStart = 7,
    ClayRenderCommandTypeOverlayColorEnd = 8,
    ClayRenderCommandTypeCustom = 9,
}
