using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Layout
{
    public sealed class WrapStory : StatefulElement
    {
        public override State CreateState() => new WrapStoryState();

        private sealed class WrapStoryState : State<WrapStory>
        {
            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);
                
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        StoryHelpers.Section(
                            label: "Basic",
                            content: BuildBasic(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Game Example",
                            content: BuildGame(theme),
                            theme
                        )
                    }
                };
            }

            private static Element BuildBasic(Theme theme)
            {
                return new Wrap
                {
                    Children = new Element[]
                    {
                        Tag(
                            label: "Alpha",
                            color: theme.PrimaryColor,
                            theme
                        ),
                        Tag(
                            label: "Beta",
                            color: theme.SecondaryColor,
                            theme
                        ),
                        Tag(
                            label: "Gamma",
                            color: theme.ErrorColor,
                            theme
                        ),
                        Tag(
                            label: "Delta",
                            color: theme.PrimaryColor.WithAlpha(0.5f),
                            theme
                        )
                    }
                };
            }

            private static Element BuildGame(Theme theme)
            {
                var classes = new (string name, FrameColor color)[]
                {
                    ("Warrior", FrameColor.FromHex("#FF6B6B")),
                    ("Mage", FrameColor.FromHex("#7B61FF")),
                    ("Rogue", FrameColor.FromHex("#94A3B8")),
                    ("Healer", FrameColor.FromHex("#00D4AA")),
                    ("Ranger", FrameColor.FromHex("#FFB347")),
                    ("Paladin", FrameColor.FromHex("#FFD700")),
                    ("Berserker", FrameColor.FromHex("#FF4444"))
                };

                var tags = new List<Element>();
                foreach (var (name, color) in classes)
                {
                    tags.Add(Tag(name, color, theme));
                }

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new Text(
                            "Active class filters",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Wrap
                        {
                            Children = tags.ToArray()
                        }
                    }
                };
            }

            private static Element Tag(string label, FrameColor color, Theme theme)
            {
                return new Margin(new EdgeInsets(0f, 6f, 6f, 0f),
                    child: new Container(
                        decoration: new BoxDecoration(
                            Color: color.WithAlpha(0.15f),
                            Border: new Border(
                                Color: color.WithAlpha(0.4f),
                                Width: 1f
                            ),
                            BorderRadius: BorderRadius.All(theme.BorderRadius)
                        ),
                        padding: EdgeInsets.Symmetric(horizontal: 10f, vertical: 4f)
                    )
                    {
                        Child = new Text(
                            label,
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: color
                            )
                        )
                    }
                );
            }
        }
    }
}