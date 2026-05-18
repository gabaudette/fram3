using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Layout
{
    public sealed class CenterStory : StatefulElement
    {
        public override State CreateState() => new CenterStoryState();

        private sealed class CenterStoryState : State<CenterStory>
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
                        BorderRadius: BorderRadius.All(theme.BorderRadius)
                    ),
                    width: 300f,
                    height: 120f
                )
                {
                    Child = new Center
                    {
                        Child = new Text(
                            "Centered content",
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
                        Border: new Border(theme.SecondaryTextColor.WithAlpha(0.2f), 1f)
                    ),
                    width: 300f,
                    height: 120f
                )
                {
                    Child = new Center
                    {
                        Child = new Column(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = new Element[]
                            {
                                new Text(
                                    "ROUND OVER",
                                    style: new TextStyle(
                                        FontSize: theme.FontSizeLarge,
                                        Bold: true,
                                        Color: theme.ErrorColor
                                    )
                                ),
                                new Text(
                                    "Respawning in 5s",
                                    style: new TextStyle(
                                        FontSize: theme.FontSizeSmall,
                                        Color: theme.SecondaryTextColor
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