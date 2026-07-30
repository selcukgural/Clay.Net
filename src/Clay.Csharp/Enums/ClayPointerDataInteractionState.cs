namespace Clay.Csharp.Enums;

/// <summary>
/// Represents the current state of interaction with clay this frame.
/// </summary>
public enum ClayPointerDataInteractionState : byte
{
    ClayPointerDataPressedThisFrame = 0,
    ClayPointerDataPressed = 1,
    ClayPointerDataReleasedThisFrame = 2,
    ClayPointerDataReleased = 3,
}