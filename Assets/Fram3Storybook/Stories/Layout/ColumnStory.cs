using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Layout
{
    public sealed class ColumnStory : StatefulElement
    {
        public override State CreateState() => new ColumnStoryState();

        private sealed class ColumnStoryState : State<ColumnStory>
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
                            content: StoryHelpers.HalfWidth(BuildBasic(theme)),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Game Example",
                            content: StoryHelpers.HalfWidth(BuildGame(theme)),
                            theme
                        )
                    }
                };
            }

            private static Element BuildBasic(Theme theme)
            {
                return new Column(mainAxisAlignment: MainAxisAlignment.Start,
                    crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new Container(
                            decoration: new BoxDecoration(Color: theme.PrimaryColor.WithAlpha(0.15f)),
                            padding: EdgeInsets.All(8f)
                        )
                        {
                            Child = new Text(
                                "First item",
                                style: new TextStyle(
                                    Color: theme.PrimaryTextColor
                                )
                            )
                        },
                        SizedBox.FromSize(height: 4f),
                        new Container(
                            decoration: new BoxDecoration(Color: theme.SecondaryColor.WithAlpha(0.15f)),
                            padding: EdgeInsets.All(8f)
                        )
                        {
                            Child = new Text(
                                "Second item",
                                style: new TextStyle(
                                    Color: theme.PrimaryTextColor
                                )
                            )
                        },
                        SizedBox.FromSize(height: 4f),
                        new Container(
                            decoration: new BoxDecoration(Color: theme.ErrorColor.WithAlpha(0.15f)),
                            padding: EdgeInsets.All(8f)
                        )
                        {
                            Child = new Text(
                                "Third item",
                                style: new TextStyle(
                                    Color: theme.PrimaryTextColor
                                )
                            )
                        }
                    }
                };
            }

            private static Element BuildGame(Theme theme)
            {
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new Text(
                            "PLAYER STATS",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        StatRow("Health", 87, 100, FrameColor.FromHex("#FF6B6B"), theme),
                        SizedBox.FromSize(height: 6f),
                        StatRow("Mana", 42, 80, FrameColor.FromHex("#7B61FF"), theme),
                        SizedBox.FromSize(height: 6f),
                        StatRow("Stamina", 65, 100, FrameColor.FromHex("#00D4AA"), theme)
                    }
                };
            }

            private static Element StatRow(
                string label,
                int value,
                int max,
                FrameColor color,
                Theme theme
            )
            {
                return new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                {
                    Children = new Element[]
                    {
                        new Container(width: 64f)
                        {
                            Child = new Text(
                                label,
                                style: new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Color: theme.SecondaryTextColor
                                )
                            )
                        },
                        new Expanded
                        {
                            Child = new Container(
                                height: 8f,
                                decoration: new BoxDecoration(
                                    Color: color.WithAlpha(0.2f),
                                    BorderRadius: BorderRadius.All(4f)
                                )
                            )
                            {
                                Child = new Container(
                                    height: 8f,
                                    width: 160f * value / max,
                                    decoration: new BoxDecoration(
                                        Color: color,
                                        BorderRadius: BorderRadius.All(4f)
                                    )
                                )
                            }
                        },
                        SizedBox.FromSize(width: 8f),
                        new Text(
                            $"{value}/{max}",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor
                            )
                        )
                    }
                };
            }
        }
    }
}