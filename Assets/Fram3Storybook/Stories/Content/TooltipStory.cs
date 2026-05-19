using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class TooltipStory : StatefulElement
    {
        public override State CreateState() => new TooltipStoryState();

        private sealed class TooltipStoryState : State<TooltipStory>
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
                            "Hover over the box below:",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Tooltip("This is the tooltip message!")
                        {
                            Child = new Container(
                                decoration: new BoxDecoration(
                                    Color: theme.PrimaryColor.WithAlpha(0.15f),
                                    Border: new Border(theme.PrimaryColor, 1f),
                                    BorderRadius: BorderRadius.All(theme.BorderRadius)
                                ),
                                padding: EdgeInsets.All(12f)
                            )
                            {
                                Child = new Text(
                                    "Hover me",
                                    style: new TextStyle(
                                        Color: theme.PrimaryTextColor
                                    )
                                )
                            }
                        }
                    }
                };
            }

            private static Element BuildGame(Theme theme)
            {
                return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                {
                    Children = new Element[]
                    {
                        new Text(
                            "Hover over an ability icon:",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = new Element[]
                            {
                                AbilityIcon(
                                    key: "C",
                                    name: "Cleave",
                                    description: "Deals 150% weapon damage to all enemies in front.",
                                    color: FrameColor.FromHex("#FF6B6B"),
                                    theme
                                ),
                                SizedBox.FromSize(width: theme.Spacing),
                                AbilityIcon(
                                    key: "B",
                                    name: "Battlecry",
                                    description: "Increases party ATK by 20% for 10 seconds.",
                                    color: FrameColor.FromHex("#FFD700"),
                                    theme
                                ),
                                SizedBox.FromSize(width: theme.Spacing),
                                AbilityIcon(
                                    key: "S",
                                    name: "Shield Bash",
                                    description: "Stuns target for 2 seconds. 12s cooldown.",
                                    color: FrameColor.FromHex("#7B61FF"),
                                    theme
                                )
                            }
                        }
                    }
                };
            }

            private static Element AbilityIcon(
                string key,
                string name,
                string description,
                FrameColor color,
                Theme theme
            )
            {
                return new Tooltip($"{name}: {description}")
                {
                    Child = new Container(
                        width: 52f, height: 52f,
                        decoration: new BoxDecoration(
                            Color: color.WithAlpha(0.2f),
                            Border: new Border(color, 2f),
                            BorderRadius: BorderRadius.All(8f)
                        )
                    )
                    {
                        Child = new Center
                        {
                            Child = new Text(
                                key,
                                style: new TextStyle(
                                    Bold: true,
                                    FontSize: theme.FontSizeLarge,
                                    Color: color
                                )
                            )
                        }
                    }
                };
            }
        }
    }
}