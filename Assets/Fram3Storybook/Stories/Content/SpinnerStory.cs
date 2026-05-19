using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class SpinnerStory : StatefulElement
    {
        public override State CreateState() => new SpinnerStoryState();

        private sealed class SpinnerStoryState : State<SpinnerStory>
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
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new Text(
                            "Default spinner (32px):",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: 4f),
                        new Spinner(),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Text(
                            "Large spinner (64px, primary color):",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: 4f),
                        new Spinner(
                            size: 64f,
                            strokeWidth: 6f,
                            color: theme.PrimaryColor,
                            speed: 2f
                        )
                    }
                };
            }

            private static Element BuildGame(Theme theme)
            {
                return new Container(
                    decoration: new BoxDecoration(
                        Color: theme.SurfaceColor,
                        BorderRadius: BorderRadius.All(theme.BorderRadius)
                    ),
                    padding: EdgeInsets.All(theme.Spacing * 3f)
                )
                {
                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = new Element[]
                        {
                            new Spinner(
                                size: 48f,
                                strokeWidth: 4f,
                                color: theme.SecondaryColor
                            ),
                            SizedBox.FromSize(height: theme.Spacing * 2f),
                            new Text(
                                "Connecting to server...",
                                style: new TextStyle(
                                    FontSize: theme.FontSize,
                                    Bold: true,
                                    Color: theme.PrimaryTextColor
                                )
                            ),
                            new Text(
                                "Please wait",
                                style: new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Color: theme.SecondaryTextColor
                                )
                            )
                        }
                    }
                };
            }
        }
    }
}