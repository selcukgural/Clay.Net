using Clay.Csharp;
using Clay.Csharp.Declarative;
using Clay.Csharp.Enums;
using Clay.Csharp.Internal;
using Clay.Csharp.Raylib;
using Clay.Csharp.Structs;

// A 1:1 C# port of the "Quick Start" example from the original Clay README
// (https://github.com/nicbarker/clay#quick-start): a fixed-width sidebar with a profile picture and
// five repeated items, next to a flexible-width main content area.
//
// The original example spells out Clay_MinMemorySize/Clay_CreateArenaWithCapacityAndMemory/
// Clay_Initialize, a measure-text function, and a manual render-command switch/loop by hand - all of
// that is exactly what ClayRaylibWindow exists to hide, so only the interesting part (the actual layout)
// is reproduced here; see ClayRaylibWindow.Create/RunFrame's source if you want the low-level equivalent.

ClayColor colorLight = ClayHelpers.CreateColor(224, 215, 210);
ClayColor colorRed = ClayHelpers.CreateColor(168, 66, 28);
ClayColor colorOrange = ClayHelpers.CreateColor(225, 138, 50);

// Layout config is just a struct that can be declared once, statically, and reused.
ClayElementDeclaration sidebarItemConfig = new()
{
    layout = new ClayLayoutConfig
    {
        sizing = new ClaySizing { width = ClaySizingAxis.Grow(), height = ClaySizingAxis.Fixed(50) },
    },
    backgroundColor = colorOrange,
};

// Re-usable components are just normal C# methods.
void SidebarItemComponent(int index)
{
    using (Layout.Element(Layout.Id("SidebarItem", (uint)index), sidebarItemConfig))
    {
        // children go here...
    }
}

using ClayRaylibWindow window = ClayRaylibWindow.Create(800, 600, "Clay.Net Quick Start");

// raylib's built-in default font is a tiny 10px bitmap font - fine for debug overlays, not for real UI
// text at 20-28px like this example uses. Load a proper font instead (see LoadFont's doc comment).
window.LoadFont(Path.Combine(AppContext.BaseDirectory, "Assets", "Fonts", "Inter-Regular.ttf"));

while (!window.ShouldClose)
{
    window.RunFrame(() =>
    {
        // An example of laying out a UI with a fixed width sidebar and flexible width main content.
        using (Layout.Element("OuterContainer", new ClayElementDeclaration
               {
                   layout = new ClayLayoutConfig
                   {
                       sizing = new ClaySizing { width = ClaySizingAxis.Grow(), height = ClaySizingAxis.Grow() },
                       padding = ClayHelpers.CreatePaddingUniform(16),
                       childGap = 16,
                   },
                   backgroundColor = ClayHelpers.CreateColor(250, 250, 255),
               }))
        {
            using (Layout.Element("SideBar", new ClayElementDeclaration
                   {
                       layout = new ClayLayoutConfig
                       {
                           layoutDirection = ClayLayoutDirection.ClayTopToBottom,
                           sizing = new ClaySizing { width = ClaySizingAxis.Fixed(300), height = ClaySizingAxis.Grow() },
                           padding = ClayHelpers.CreatePaddingUniform(16),
                           childGap = 16,
                       },
                       backgroundColor = colorLight,
                   }))
            {
                using (Layout.Element("ProfilePictureOuter", new ClayElementDeclaration
                       {
                           layout = new ClayLayoutConfig
                           {
                               sizing = new ClaySizing { width = ClaySizingAxis.Grow() },
                               padding = ClayHelpers.CreatePaddingUniform(16),
                               childGap = 16,
                               childAlignment = new ClayChildAlignment { y = ClayLayoutAlignmentY.ClayAlignYCenter },
                           },
                           backgroundColor = colorRed,
                       }))
                {
                    using (Layout.Element("ProfilePicture", new ClayElementDeclaration
                           {
                               layout = new ClayLayoutConfig
                               {
                                   sizing = new ClaySizing { width = ClaySizingAxis.Fixed(60), height = ClaySizingAxis.Fixed(60) },
                               },
                               // Upstream sets `.image = { .imageData = &profilePicture }` here - Clay.Net
                               // supports image rendering now too (see ClayRaylibWindow.LoadTexture), but
                               // this sample keeps a plain colored placeholder to avoid bundling an image asset.
                               backgroundColor = ClayHelpers.CreateColor(180, 180, 180),
                               cornerRadius = ClayHelpers.CreateCornerRadius(30),
                           }))
                    {
                    }

                    Layout.Text("Clay.Net - UI Library", new ClayTextElementConfig
                    {
                        fontSize = 24,
                        textColor = ClayHelpers.CreateColor(255, 255, 255),
                    });
                }

                // Standard C# code like loops etc work inside components.
                for (int i = 0; i < 5; i++)
                {
                    SidebarItemComponent(i);
                }

                using (Layout.Element("MainContent", new ClayElementDeclaration
                       {
                           layout = new ClayLayoutConfig
                           {
                               sizing = new ClaySizing { width = ClaySizingAxis.Grow(), height = ClaySizingAxis.Grow() },
                           },
                           backgroundColor = colorLight,
                       }))
                {
                }
            }
        }
    });
}
