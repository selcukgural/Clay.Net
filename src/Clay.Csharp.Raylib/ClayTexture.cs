using Raylib_cs;

namespace Clay.Csharp.Raylib;

/// <summary>
/// A raylib texture paired with a stable unmanaged pointer to it, suitable for
/// <c>ClayImageElementConfig.imageData</c> - Clay's image render commands carry that pointer through
/// unchanged, and <see cref="ClayRaylibRenderer"/> reads it back as a <see cref="Texture2D"/> when drawing.
/// Created by <see cref="ClayRaylibWindow.LoadTexture"/>, which also owns the unmanaged memory and unloads
/// the underlying GPU texture on <see cref="ClayRaylibWindow.Dispose"/>.
/// </summary>
public readonly struct ClayTexture
{
    /// <summary>The loaded raylib texture, for direct use outside of Clay if needed.</summary>
    public Texture2D Texture { get; }

    /// <summary>Pass this into <c>ClayElementDeclaration.image.imageData</c> when declaring an element.</summary>
    public IntPtr ImageData { get; }

    internal ClayTexture(Texture2D texture, IntPtr imageData)
    {
        Texture = texture;
        ImageData = imageData;
    }
}
