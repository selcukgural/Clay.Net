namespace Clay.Csharp.Enums;

/// <summary>
/// Controls the direction in which child elements will be automatically laid out.
/// </summary>
public enum ClayLayoutDirection : byte
{
    /// <summary>(Default) Lays out child elements from left to right with increasing x.</summary>
    ClayLeftToRight = 0,

    /// <summary>Lays out child elements from top to bottom with increasing y.</summary>
    ClayTopToBottom = 1,
}