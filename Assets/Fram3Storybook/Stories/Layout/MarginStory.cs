using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Layout
{
    public sealed class MarginStory : StatefulElement
    {
        public override State CreateState() => new MarginStoryState();

        private sealed class MarginStoryState : State<MarginStory>
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
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new Container(
                            decoration: new BoxDecoration(
                                Color: theme.PrimaryColor.WithAlpha(0.2f)
                            ),
                            height: 40f
                        )
                        {
                            Child = new Text(
                                "Block A",
                                style: new TextStyle(
                                    Color: theme.PrimaryTextColor
                                )
                            )
                        },
                        new Margin(EdgeInsets.Symmetric(vertical: 16f, horizontal: 0f),
                            new Container(
                                decoration: new BoxDecoration(
                                    Color: theme.SecondaryColor.WithAlpha(0.2f)
                                ),
                                height: 40f
                            )
                            {
                                Child = new Text(
                                    "Block B (16px vertical margin)",
                                    style: new TextStyle(
                                        Color: theme.PrimaryTextColor
                                    )
                                )
                            }
                        ),
                        new Container(
                            decoration: new BoxDecoration(
                                Color: theme.ErrorColor.WithAlpha(0.2f)
                            ),
                            height: 40f
                        )
                        {
                            Child = new Text(
                                "Block C",
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
                            "HUD section spacing",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Container(
                            decoration: new BoxDecoration(
                                Color: theme.PrimaryColor.WithAlpha(0.1f),
                                BorderRadius: BorderRadius.All(6f)
                            ),
                            padding: EdgeInsets.All(theme.Spacing)
                        )
                        {
                            Child = new Text(
                                "Kills: 12",
                                style: new TextStyle(
                                    Color: theme.PrimaryTextColor
                                )
                            )
                        },
                        new Margin(
                            EdgeInsets.Symmetric(vertical: theme.Spacing * 1.5f, horizontal: 0f),
                            child: new Container(
                                decoration: new BoxDecoration(
                                    Color: theme.SecondaryColor.WithAlpha(0.1f),
                                    BorderRadius: BorderRadius.All(6f)
                                ),
                                padding: EdgeInsets.All(theme.Spacing)
                            )
                            {
                                Child = new Text(
                                    "Assists: 5",
                                    style: new TextStyle(
                                        Color: theme.PrimaryTextColor
                                    )
                                )
                            }
                        ),
                        new Container(
                            decoration: new BoxDecoration(
                                Color: theme.ErrorColor.WithAlpha(0.1f),
                                BorderRadius: BorderRadius.All(6f)
                            ),
                            padding: EdgeInsets.All(theme.Spacing)
                        )
                        {
                            Child = new Text(
                                "Deaths: 2",
                                style: new TextStyle(
                                    Color: theme.PrimaryTextColor
                                )
                            )
                        }
                    }
                };
            }
        }
    }
}