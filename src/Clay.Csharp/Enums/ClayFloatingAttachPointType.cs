namespace Clay.Csharp.Enums;

/// <summary>
/// Controls where a floating element is offset relative to its parent element.
/// Note: see https://github.com/user-attachments/assets/b8c6dfaa-c1b1-41a4-be55-013473e4a6ce for a visual explanation.
/// </summary>
public enum ClayFloatingAttachPointType : byte
{
    ClayAttachPointLeftTop = 0,
    ClayAttachPointLeftCenter = 1,
    ClayAttachPointLeftBottom = 2,
    ClayAttachPointCenterTop = 3,
    ClayAttachPointCenterCenter = 4,
    ClayAttachPointCenterBottom = 5,
    ClayAttachPointRightTop = 6,
    ClayAttachPointRightCenter = 7,
    ClayAttachPointRightBottom = 8,
}