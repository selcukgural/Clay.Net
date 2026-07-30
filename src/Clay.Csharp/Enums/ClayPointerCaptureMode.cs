namespace Clay.Csharp.Enums;

/// <summary>
/// Controls how mouse pointer events like hover and click are captured or passed through to elements underneath a floating element.
/// </summary>
public enum ClayPointerCaptureMode : byte
{
    ClayPointerCaptureModeCapture = 0,
    ClayPointerCaptureModePassthrough = 1,
}