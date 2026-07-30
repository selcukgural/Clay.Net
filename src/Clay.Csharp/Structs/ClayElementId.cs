using System.Runtime.InteropServices;

namespace Clay.Csharp.Structs;

/// <summary>
/// Primarily created via hashing functions.
/// Represents a hashed string ID used for identifying and finding specific clay UI elements, required
/// by functions such as Clay_PointerOver() and Clay_GetElementData().
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayElementId
{
    /// <summary>The resulting hash generated from the other fields.</summary>
    public uint id;

    /// <summary>A numerical offset applied after computing the hash from stringId.</summary>
    public uint offset;

    /// <summary>A base hash value to start from, for example the parent element ID is used when calculating local IDs.</summary>
    public uint baseId;

    /// <summary>The string id to hash.</summary>
    public ClayString stringId;
}