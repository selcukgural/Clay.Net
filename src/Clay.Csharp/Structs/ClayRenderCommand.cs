using System.Runtime.InteropServices;
using Clay.Csharp.Enums;

namespace Clay.Csharp.Structs;

/// <summary>
/// Represents a single render command to be executed by the renderer.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayRenderCommand
{
    /// <summary>A rectangular box that fully encloses this UI element, with the position relative to the root of the layout.</summary>
    public ClayBoundingBox boundingBox;

    /// <summary>A struct union containing data specific to this command's commandType.</summary>
    public ClayRenderData renderData;

    /// <summary>A pointer transparently passed through from the original element declaration.</summary>
    public IntPtr userData;

    /// <summary>The id of this element, transparently passed through from the original element declaration.</summary>
    public uint id;

    /// <summary>The z order required for drawing this command correctly.</summary>
    public short zIndex;

    /// <summary>Specifies how to handle rendering of this command.</summary>
    public ClayRenderCommandType commandType;
}
