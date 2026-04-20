#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories
{
    /// <summary>
    /// Shared layout helpers used across all story builders.
    /// </summary>
    internal static class StoryHelpers
    {
        /// <summary>
        /// Constrains <paramref name="content"/> to 50% of the available width by placing it in
        /// the first half of a two-Expanded row.
        /// </summary>
        internal static Element HalfWidth(Element content)
        {
            return new Row(crossAxisAlignment: CrossAxisAlignment.Start)
            {
                Children = new Element[]
                {
                    new Expanded { Child = content },
                    SizedBox.Expand(),
                }
            };
        }

        /// <summary>
        /// Wraps <paramref name="content"/> in a labeled section block with a colored header label.
        /// </summary>
        internal static Element Section(string label, Element content, Theme theme)
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text(label.ToUpperInvariant(), new TextStyle(
                        FontSize: theme.FontSizeSmall,
                        Bold: true,
                        Color: theme.SecondaryColor,
                        LetterSpacing: 1.5f
                    )),
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
                    },
                }
            };
        }
    }
}
