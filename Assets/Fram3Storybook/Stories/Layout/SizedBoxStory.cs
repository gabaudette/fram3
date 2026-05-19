using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Layout
{
    public sealed class SizedBoxStory : StatefulElement
    {
        public override State CreateState() => new SizedBoxStoryState();

        private sealed class SizedBoxStoryState : State<SizedBoxStory>
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
                        new Text(
                            "Fixed gap (width: 48px) between two boxes:",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: 6f),
                        new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = new Element[]
                            {
                                new Container(
                                    width: 60f, height: 32f,
                                    decoration: new BoxDecoration(
                                        Color: theme.PrimaryColor.WithAlpha(0.3f),
                                        BorderRadius: BorderRadius.All(4f)
                                    )
                                )
                                {
                                    Child = new Center
                                    {
                                        Child = new Text(
                                            "A",
                                            style: new TextStyle(
                                                Color: theme.PrimaryTextColor
                                            )
                                        )
                                    }
                                },
                                SizedBox.FromSize(width: 48f),
                                new Container(
                                    width: 60f,
                                    height: 32f,
                                    decoration: new BoxDecoration(
                                        Color: theme.SecondaryColor.WithAlpha(0.3f),
                                        BorderRadius: BorderRadius.All(4f)
                                    )
                                )
                                {
                                    Child = new Center
                                    {
                                        Child = new Text(
                                            "B",
                                            style: new TextStyle(
                                                Color: theme.PrimaryTextColor
                                            )
                                        )
                                    }
                                }
                            }
                        },
                        SizedBox.FromSize(height: theme.Spacing * 2f),
                        new Text(
                            "Square spacer (32x32) between two boxes:",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: 6f),
                        new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = new Element[]
                            {
                                new Container(
                                    width: 60f,
                                    height: 32f,
                                    decoration: new BoxDecoration(
                                        Color: theme.PrimaryColor.WithAlpha(0.3f),
                                        BorderRadius: BorderRadius.All(4f)
                                    )
                                )
                                {
                                    Child = new Center
                                    {
                                        Child = new Text(
                                            "C",
                                            style: new TextStyle(
                                                Color: theme.PrimaryTextColor
                                            )
                                        )
                                    }
                                },
                                SizedBox.Square(32f),
                                new Container(
                                    width: 60f,
                                    height: 32f,
                                    decoration: new BoxDecoration(
                                        Color: theme.SecondaryColor.WithAlpha(0.3f),
                                        BorderRadius: BorderRadius.All(4f)
                                    )
                                )
                                {
                                    Child = new Center
                                    {
                                        Child = new Text(
                                            "D",
                                            style: new TextStyle(
                                                Color: theme.PrimaryTextColor
                                            )
                                        )
                                    }
                                }
                            }
                        },
                        SizedBox.FromSize(height: theme.Spacing * 2f),
                        new Text(
                            "SizedBox.Expand() pushes content to opposite ends:",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: 6f),
                        new Container(
                            height: 40f,
                            decoration: new BoxDecoration(
                                Color: theme.SurfaceColor,
                                BorderRadius: BorderRadius.All(4f)
                            )
                        )
                        {
                            Child = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                            {
                                Children = new Element[]
                                {
                                    new Text(
                                        "Left",
                                        style: new TextStyle(
                                            Color: theme.PrimaryTextColor
                                        )
                                    ),
                                    SizedBox.Expand(),
                                    new Text(
                                        "Right",
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
                            "XP bar with fixed icon + expanded fill",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = new Element[]
                            {
                                new Text(
                                    "XP",
                                    style: new TextStyle(
                                        FontSize: theme.FontSizeSmall,
                                        Bold: true,
                                        Color: FrameColor.FromHex("#FFD700")
                                    )
                                ),
                                SizedBox.FromSize(width: theme.Spacing),
                                new Expanded
                                {
                                    Child = new Container(
                                        height: 10f,
                                        decoration: new BoxDecoration(
                                            Color: FrameColor.FromHex("#FFD700").WithAlpha(0.15f),
                                            BorderRadius: BorderRadius.All(5f)
                                        )
                                    )
                                    {
                                        Child = new Container(
                                            height: 10f,
                                            width: 120f,
                                            decoration: new BoxDecoration(
                                                Color: FrameColor.FromHex("#FFD700"),
                                                BorderRadius: BorderRadius.All(5f)
                                            )
                                        )
                                    }
                                },
                                SizedBox.FromSize(width: theme.Spacing),
                                new Text(
                                    "Lv 12",
                                    style: new TextStyle(
                                        FontSize: theme.FontSizeSmall,
                                        Bold: true,
                                        Color: theme.PrimaryTextColor
                                    )
                                )
                            }
                        }
                    }
                };
            }
        }
    }
}