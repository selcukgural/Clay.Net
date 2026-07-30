using Xunit;

namespace Clay.Csharp.Tests;

/// <summary>
/// Clay's native context, measure-text callback, and ClayTextArena are process-global/static state - all
/// tests that touch them (via ClayTestContext or Layout.*) must run sequentially relative to each other,
/// not just within one test class but across all of them, hence one shared collection rather than one
/// per class. xUnit still runs this collection in parallel with unrelated collections/test classes.
/// </summary>
[CollectionDefinition("ClayNative", DisableParallelization = true)]
public class ClayNativeTestCollection;
