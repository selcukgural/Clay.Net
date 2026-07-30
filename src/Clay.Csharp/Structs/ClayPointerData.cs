using System.Runtime.InteropServices;
using Clay.Csharp.Enums;

namespace Clay.Csharp.Structs;

/// <summary>
/// Information on the current state of pointer interactions this frame.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayPointerData
{
    /// <summary>The position of the mouse / touch / pointer relative to the root of the layout.</summary>
    public ClayVector2 position;

    /// <summary>Represents the current state of interaction with clay this frame.</summary>
    public ClayPointerDataInteractionState state;
}
