using Clay.Csharp.Declarative;
using Clay.Csharp.Enums;
using Clay.Csharp.Structs;
using Xunit;

namespace Clay.Csharp.Tests;

/// <summary>
/// Confirms Clay's error-reporting path itself round-trips correctly: declaring two elements with the
/// same explicit ID should make Clay report CLAY_ERROR_TYPE_DUPLICATE_ID through the managed error
/// handler delegate. This is a purely logical check (a hash map collision) rather than a memory-sizing
/// one - deliberately undersizing the arena to provoke CLAY_ERROR_TYPE_ARENA_CAPACITY_EXCEEDED was tried
/// first and found to crash the process rather than report gracefully once the shortfall is large enough
/// to corrupt Clay's internal allocations, so that path isn't exercised here.
/// </summary>
[Collection("ClayNative")]
[Trait("RequiresNative", "true")]
public class ErrorHandlerTests
{
    [Fact]
    public void DuplicateElementId_ReportsDuplicateIdError()
    {
        using ClayTestContext context = new(300, 200);

        Layout.BeginLayout();
        using (Layout.Element("Root", new ClayElementDeclaration
               {
                   layout = new ClayLayoutConfig
                   {
                       sizing = new ClaySizing { width = ClaySizingAxis.Fixed(300), height = ClaySizingAxis.Fixed(200) },
                   },
               }))
        {
            using (Layout.Element("Duplicate", new ClayElementDeclaration
                   {
                       layout = new ClayLayoutConfig
                       {
                           sizing = new ClaySizing { width = ClaySizingAxis.Fixed(10), height = ClaySizingAxis.Fixed(10) },
                       },
                   }))
            {
            }

            using (Layout.Element("Duplicate", new ClayElementDeclaration
                   {
                       layout = new ClayLayoutConfig
                       {
                           sizing = new ClaySizing { width = ClaySizingAxis.Fixed(10), height = ClaySizingAxis.Fixed(10) },
                       },
                   }))
            {
            }
        }

        Layout.EndLayout(deltaTime: 0.016f);

        Assert.Contains(context.Errors, e => e.errorType == ClayErrorType.ClayErrorTypeDuplicateId);
    }
}
