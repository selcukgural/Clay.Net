using Clay.Csharp;
using Clay.Csharp.Declarative;
using Clay.Csharp.Enums;
using Clay.Csharp.Internal;
using Clay.Csharp.Raylib;
using Clay.Csharp.Structs;

Console.WriteLine($"Clay native version: {ClayNative.GetVersion()}");

using ClayRaylibWindow window = ClayRaylibWindow.Create(800, 600, "Clay.Net Sandbox");

while (!window.ShouldClose)
{
    window.RunFrame(() =>
    {
        using (Layout.Element("Root", new ClayElementDeclaration
               {
                   layout = new ClayLayoutConfig
                   {
                       sizing = new ClaySizing
                       {
                           width = ClaySizingAxis.Grow(),
                           height = ClaySizingAxis.Grow(),
                       },
                       padding = ClayHelpers.CreatePaddingUniform(24),
                       childGap = 16,
                       layoutDirection = ClayLayoutDirection.ClayTopToBottom,
                   },
                   backgroundColor = ClayHelpers.CreateColor(30, 30, 35),
               }))
        {
            Layout.Text("Hello, Clay.Net!", new ClayTextElementConfig
            {
                fontSize = 28,
                textColor = ClayHelpers.CreateColor(255, 255, 255),
            });

            using (Layout.Element("Box", new ClayElementDeclaration
                   {
                       layout = new ClayLayoutConfig
                       {
                           sizing = new ClaySizing
                           {
                               width = ClaySizingAxis.Grow(),
                               height = ClaySizingAxis.Fixed(80),
                           },
                           childAlignment = new ClayChildAlignment
                           {
                               x = ClayLayoutAlignmentX.ClayAlignXCenter,
                               y = ClayLayoutAlignmentY.ClayAlignYCenter,
                           },
                       },
                       backgroundColor = ClayNative.Clay_Hovered()
                           ? ClayHelpers.CreateColor(120, 160, 240)
                           : ClayHelpers.CreateColor(80, 120, 200),
                       cornerRadius = ClayHelpers.CreateCornerRadius(12),
                   }))
            {
                Layout.Text(ClayNative.Clay_Hovered() ? "Hovering!" : "Hover me", new ClayTextElementConfig
                {
                    fontSize = 20,
                    textColor = ClayHelpers.CreateColor(255, 255, 255),
                });
            }
        }
    });
}
