using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Layout
{
    public sealed class StackStory : StatefulElement
    {
        public override State CreateState() => new StackStoryState();

        private sealed class StackStoryState : State<StackStory>
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
                return new Container(
                    width: 140f,
                    height: 140f
                )
                {
                    Child = new Stack
                    {
                        Children = new Element[]
                        {
                            new Container(
                                decoration: new BoxDecoration(
                                    Color: theme.PrimaryColor.WithAlpha(0.4f)
                                ),
                                width: 120f,
                                height: 120f
                            ),
                            new Container(
                                decoration: new BoxDecoration(
                                    Color: theme.SecondaryColor.WithAlpha(0.5f)
                                ),
                                width: 80f,
                                height: 80f
                            ),
                            new Container(
                                decoration: new BoxDecoration(
                                    Color: FrameColor.Black.WithAlpha(0.6f),
                                    BorderRadius: BorderRadius.All(2f)
                                ),
                                padding: EdgeInsets.Symmetric(vertical: 2f, horizontal: 6f)
                            )
                            {
                                Child = new Text(
                                    "Stacked",
                                    style: new TextStyle(
                                        Color: FrameColor.White,
                                        FontSize: 10f
                                    )
                                )
                            }
                        }
                    }
                };
            }

            private static Element BuildGame(Theme theme)
            {
                var mapBg = FrameColor.FromHex("#0A1628");
                var pingColor = FrameColor.FromHex("#FF6B6B");
                var playerColor = FrameColor.FromHex("#00D4AA");

                return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                {
                    Children = new Element[]
                    {
                        new Text(
                            "Minimap overlay",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.PrimaryTextColor
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Container(
                            width: 160f,
                            height: 160f,
                            decoration: new BoxDecoration(
                                Color: mapBg,
                                BorderRadius: BorderRadius.All(8f),
                                Border: new Border(
                                    Color: theme.SecondaryTextColor.WithAlpha(0.3f),
                                    Width: 1f
                                )
                            )
                        )
                        {
                            Child = new Stack
                            {
                                Children = new Element[]
                                {
                                    new Container(
                                        decoration: new BoxDecoration(
                                            Color: FrameColor.FromHex("#0D2040").WithAlpha(0.8f)
                                        ),
                                        width: 160f,
                                        height: 160f
                                    ),
                                    new Margin(
                                        margin: new EdgeInsets(30f, 0f, 0f, 100f),
                                        child: new Container(
                                            width: 10f,
                                            height: 10f,
                                            decoration: new BoxDecoration(
                                                Color: pingColor,
                                                BorderRadius: BorderRadius.All(5f)
                                            )
                                        )
                                    ),
                                    new Margin(
                                        margin: new EdgeInsets(80f, 0f, 0f, 60f),
                                        child: new Container(
                                            width: 8f, height: 8f,
                                            decoration: new BoxDecoration(
                                                Color: playerColor,
                                                BorderRadius: BorderRadius.All(4f)
                                            )
                                        )
                                    ),
                                    new Margin(
                                        margin: new EdgeInsets(75f, 0f, 0f, 55f),
                                        child: new Container(
                                            padding: EdgeInsets.Symmetric(horizontal: 4f, vertical: 2f),
                                            decoration: new BoxDecoration(
                                                Color: FrameColor.Black.WithAlpha(0.7f),
                                                BorderRadius: BorderRadius.All(3f)
                                            )
                                        )
                                        {
                                            Child = new Text(
                                                "YOU",
                                                style: new TextStyle(
                                                    FontSize: 9f,
                                                    Bold: true,
                                                    Color: playerColor
                                                )
                                            )
                                        }
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