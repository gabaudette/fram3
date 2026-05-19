using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Layout
{
    public sealed class PaddingStory : StatefulElement
    {
        public override State CreateState() => new PaddingStoryState();

        private sealed class PaddingStoryState : State<PaddingStory>
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
                    decoration: new BoxDecoration(
                        Color: theme.PrimaryColor.WithAlpha(0.08f),
                        Border: new Border(
                            Color: theme.PrimaryColor.WithAlpha(0.3f),
                            Width: 1f
                        ),
                        BorderRadius: BorderRadius.All(theme.BorderRadius)
                    )
                )
                {
                    Child = new Padding(EdgeInsets.All(24f))
                    {
                        Child = new Text(
                            "This text has 24px padding on all sides.",
                            style: new TextStyle(
                                Color: theme.PrimaryTextColor
                            )
                        )
                    }
                };
            }

            private static Element BuildGame(Theme theme)
            {
                return new Container(
                    decoration: new BoxDecoration(
                        Color: theme.SurfaceColor,
                        BorderRadius: BorderRadius.All(theme.BorderRadius),
                        Border: new Border(
                            Color: theme.SecondaryTextColor.WithAlpha(0.2f),
                            Width: 1f
                        )
                    )
                )
                {
                    Child = new Padding(EdgeInsets.All(theme.Spacing * 2f))
                    {
                        Child = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = new Element[]
                            {
                                new Container(
                                    width: 40f,
                                    height: 40f,
                                    decoration: new BoxDecoration(
                                        Color: FrameColor.FromHex("#FFD700").WithAlpha(0.2f),
                                        BorderRadius: BorderRadius.All(6f),
                                        Border: new Border(
                                            Color: FrameColor.FromHex("#FFD700"),
                                            Width: 1f
                                        )
                                    )
                                )
                                {
                                    Child = new Center
                                    {
                                        Child = new Text(
                                            "G",
                                            style: new TextStyle(
                                                Bold: true,
                                                FontSize: theme.FontSize,
                                                Color: FrameColor.FromHex("#FFD700")
                                            )
                                        )
                                    }
                                },
                                SizedBox.FromSize(width: theme.Spacing),
                                new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                                {
                                    Children = new Element[]
                                    {
                                        new Text(
                                            "Gold Chest",
                                            style: new TextStyle(
                                                Bold: true,
                                                Color: theme.PrimaryTextColor
                                            )
                                        ),
                                        new Text(
                                            "Padded card layout",
                                            style: new TextStyle(
                                                Color: theme.SecondaryTextColor,
                                                FontSize: theme.FontSizeSmall
                                            )
                                        )
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }
    }
}