using System;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class ProgressBarStory : StatefulElement
    {
        public override State CreateState() => new ProgressBarStoryState();

        private sealed class ProgressBarStoryState : State<ProgressBarStory>
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
                            "25% progress:",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: 4f),
                        ThemedBar(
                            value: 25f,
                            max: 100f,
                            fillColor: theme.PrimaryColor
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Text(
                            "75% progress:",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: 4f),
                        ThemedBar(
                            value: 75f,
                            max: 100f,
                            fillColor: theme.PrimaryColor
                        )
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
                            "HERO STATS",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        GameStatBar(
                            label: "HP",
                            value: 87,
                            max: 100,
                            color: FrameColor.FromHex("#FF6B6B"),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        GameStatBar(
                            label: "MP",
                            value: 42,
                            max: 80,
                            color: FrameColor.FromHex("#7B61FF"),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        GameStatBar(
                            label: "XP",
                            value: 1240,
                            max: 2000,
                            color: FrameColor.FromHex("#FFD700"),
                            theme
                        )
                    }
                };
            }

            private static Element GameStatBar(string label, int value, int max, FrameColor color, Theme theme)
            {
                return new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                {
                    Children = new Element[]
                    {
                        new Container(width: 28f)
                        {
                            Child = new Text(
                                label,
                                style: new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Bold: true,
                                    Color: color
                                )
                            )
                        },
                        SizedBox.FromSize(width: theme.Spacing),
                        new Expanded
                        {
                            Child = ThemedBar(value, max, color)
                        },
                        SizedBox.FromSize(width: theme.Spacing),
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

            private static Element ThemedBar(float value, float max, FrameColor fillColor)
            {
                var fill = max > 0f ? Math.Min(value / max, 1f) : 0f;
                var empty = Math.Max(1f - fill, 0f);

                return new Container(
                    height: 12f,
                    decoration: new BoxDecoration(
                        Color: fillColor.WithAlpha(0.2f),
                        BorderRadius: BorderRadius.All(6f)
                    )
                )
                {
                    Child = new Row(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Expanded(flex: fill)
                            {
                                Child = new Container(
                                    height: 12f,
                                    decoration: new BoxDecoration(
                                        Color: fillColor,
                                        BorderRadius: BorderRadius.All(6f)
                                    )
                                )
                            },
                            new Expanded(flex: empty) { Child = new Container(height: 12f) }
                        }
                    }
                };
            }
        }
    }
}