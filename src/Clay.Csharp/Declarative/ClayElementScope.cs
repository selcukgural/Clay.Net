namespace Clay.Csharp.Declarative;

/// <summary>
/// Disposable scope returned by Layout.Element(...), mirroring the C library's CLAY(id, ...) { ... } macro,
/// which is implemented in C as a for-loop that opens the element before the block and closes it after.
/// Usage: using (Layout.Element("Container", declaration)) { ...children... }
/// </summary>
public readonly struct ClayElementScope : IDisposable
{
    public void Dispose() => Layout.Clay__CloseElement();
}
