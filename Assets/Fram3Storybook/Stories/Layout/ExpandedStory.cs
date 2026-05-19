using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Layout
{
    public sealed class ExpandedStory : StatefulElement
    {
        public override State CreateState() => new ExpandedStoryState();

        private sealed class ExpandedStoryState : State<ExpandedStory>
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
                return new Container(height: 60f)
                {
                    Child = new Row
                    {
                        Children = new Element[]
                        {
                            new Container(
                                decoration: new BoxDecoration(
                                    Color: theme.PrimaryColor.WithAlpha(0.3f)
                                ),
                                width: 80f
                            )
                            {
                                Child = new Text(
                                    "Fixed",
                                    style: new TextStyle(
                                        Color: theme.PrimaryTextColor
                                    )
                                )
                            },
                            new Expanded
                            {
                                Child = new Container(
                                    decoration: new BoxDecoration(
                                        Color: theme.SecondaryColor.WithAlpha(0.3f)
                                    )
                                )
                                {
                                    Child = new Text(
                                        "Expanded",
                                        style: new TextStyle(
                                            Color: theme.PrimaryTextColor
                                        )
                                    )
                                }
                            }
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
                            "XP Progress",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor
                            )
                        ),
                        SizedBox.FromSize(height: 6f),
                        new Container(height: 32f)
                        {
                            Child = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                            {
                                Children = new Element[]
                                {
                                    new Text(
                                        "Lv 11",
                                        style: new TextStyle(
                                            FontSize: theme.FontSizeSmall,
                                            Color: theme.SecondaryTextColor
                                        )
                                    ),
                                    SizedBox.FromSize(width: theme.Spacing),
                                    new Expanded
                                    {
                                        Child = new Container(
                                            height: 12f,
                                            decoration: new BoxDecoration(
                                                Color: FrameColor.FromHex("#FFD700").WithAlpha(0.15f),
                                                BorderRadius: BorderRadius.All(6f)
                                            )
                                        )
                                        {
                                            Child = new Container(
                                                height: 12f,
                                                width: 100f,
                                                decoration: new BoxDecoration(
                                                    Color: FrameColor.FromHex("#FFD700"),
                                                    BorderRadius: BorderRadius.All(6f)
                                                )
                                            )
                                        }
                                    },
                                    SizedBox.FromSize(width: theme.Spacing),
                                    new Text(
                                        "Lv 12",
                                        style: new TextStyle(
                                            FontSize: theme.FontSizeSmall,
                                            Color: theme.PrimaryTextColor
                                        )
                                    )
                                }
                            }
                        }
                    }
                };
            }
        }
    }
}