#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories
{
    internal static class StoryHelpers
    {
        internal static Element HalfWidth(Element content)
        {
            return new Row(crossAxisAlignment: CrossAxisAlignment.Start)
            {
                Children = new Element[]
                {
                    new Expanded { Child = content },
                    SizedBox.Expand()
                }
            };
        }

        internal static Element Section(string label, Element content, Theme theme)
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text(
                        label.ToUpperInvariant(),
                        style: new TextStyle(
                            FontSize: theme.FontSizeSmall,
                            Bold: true,
                            Color: theme.SecondaryColor,
                            LetterSpacing: 1.5f
                        )
                    ),
                    SizedBox.FromSize(height: theme.Spacing),
                    new Container(
                        decoration: new BoxDecoration(
                            Color: theme.SurfaceColor,
                            BorderRadius: BorderRadius.All(theme.BorderRadius),
                            Border: new Border(theme.SecondaryTextColor.WithAlpha(0.15f), 1f)
                        ),
                        padding: EdgeInsets.All(theme.Spacing * 2f)
                    )
                    {
                        Child = content
                    }
                }
            };
        }
    }
}