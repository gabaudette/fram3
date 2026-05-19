using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class CardStory : StatefulElement
    {
        public override State CreateState() => new CardStoryState();

        private sealed class CardStoryState : State<CardStory>
        {
            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        StoryHelpers.Section(
                            label: "Elevated",
                            content: BuildElevated(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Outlined",
                            content: BuildOutlined(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "With Header",
                            content: BuildWithHeader(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "With Header and Footer",
                            content: BuildWithHeaderAndFooter(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Game Example — Quest Card",
                            content: BuildGameExample(theme),
                            theme
                        )
                    }
                };
            }

            private static Element BuildElevated(Theme theme)
            {
                return new Card(
                    content: new Text(
                        "This is an elevated card. It uses a shadow to convey depth above the surface.",
                        style: new TextStyle(
                            FontSize: theme.FontSize,
                            Color: theme.PrimaryTextColor
                        )
                    )
                );
            }

            private static Element BuildOutlined(Theme theme)
            {
                return new Card(
                    outlined: true,
                    content: new Text(
                        "This is an outlined card. It uses a border instead of a shadow for a flat look.",
                        style: new TextStyle(
                            FontSize: theme.FontSize,
                            Color: theme.PrimaryTextColor
                        )
                    )
                );
            }

            private static Element BuildWithHeader(Theme theme)
            {
                return new Card(
                    header: new Text(
                        "Card Header",
                        style: new TextStyle(
                            FontSize: theme.FontSize,
                            Bold: true,
                            Color: theme.PrimaryTextColor
                        )
                    ),
                    content: new Text(
                        "Card body content appears here. The header above is separated by a divider.",
                        style: new TextStyle(
                            FontSize: theme.FontSize,
                            Color: theme.PrimaryTextColor
                        )
                    )
                );
            }

            private static Element BuildWithHeaderAndFooter(Theme theme)
            {
                return new Card(
                    header: new Text(
                        "Card Header",
                        style: new TextStyle(
                            FontSize: theme.FontSize,
                            Bold: true,
                            Color: theme.PrimaryTextColor
                        )
                    ),
                    content: new Text(
                        "Card body content appears here.",
                        style: new TextStyle(
                            FontSize: theme.FontSize,
                            Color: theme.PrimaryTextColor
                        )
                    ),
                    footer: new Row(
                        crossAxisAlignment: CrossAxisAlignment.Center,
                        mainAxisAlignment: MainAxisAlignment.End
                    )
                    {
                        Children = new Element[]
                        {
                            new Button(
                                "Cancel",
                                onPressed: () => { }
                            ),
                            SizedBox.FromSize(width: theme.Spacing),
                            new Button(
                                "Confirm",
                                onPressed: () => { }
                            )
                        }
                    }
                );
            }

            private static Element BuildGameExample(Theme theme)
            {
                return new Row(crossAxisAlignment: CrossAxisAlignment.Start)
                {
                    Children = new Element[]
                    {
                        new Expanded
                        {
                            Child = new Card(
                                header: new Row(
                                    crossAxisAlignment: CrossAxisAlignment.Center,
                                    mainAxisAlignment: MainAxisAlignment.SpaceBetween
                                )
                                {
                                    Children = new Element[]
                                    {
                                        new Text(
                                            "Defeat the Dragon",
                                            style: new TextStyle(
                                                FontSize: theme.FontSize,
                                                Bold: true,
                                                Color: theme.PrimaryTextColor
                                            )
                                        ),
                                        new Badge(
                                            new Container(
                                                width: theme.Spacing * 3f,
                                                height: theme.Spacing * 3f,
                                                decoration: new BoxDecoration(
                                                    Color: theme.PrimaryColor,
                                                    BorderRadius: BorderRadius.All(theme.BorderRadius)
                                                )
                                            ),
                                            count: 3
                                        )
                                    }
                                },
                                content: new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                                {
                                    Children = new Element[]
                                    {
                                        new Text(
                                            "Travel to the Ember Peaks and slay the" +
                                            " ancient dragon terrorizing the region.",
                                            style: new TextStyle(
                                                FontSize: theme.FontSize,
                                                Color: theme.PrimaryTextColor
                                            )
                                        ),
                                        SizedBox.FromSize(height: theme.Spacing),
                                        new Text(
                                            "Reward: 500 gold",
                                            style: new TextStyle(
                                                FontSize: theme.FontSizeSmall,
                                                Color: theme.SecondaryTextColor
                                            )
                                        )
                                    }
                                },
                                footer: new Row(
                                    crossAxisAlignment: CrossAxisAlignment.Center,
                                    mainAxisAlignment: MainAxisAlignment.End
                                )
                                {
                                    Children = new Element[]
                                    {
                                        new Button(
                                            "Abandon",
                                            onPressed: () => { }
                                        ),
                                        SizedBox.FromSize(width: theme.Spacing),
                                        new Button(
                                            "Track Quest",
                                            onPressed: () => { }
                                        )
                                    }
                                }
                            )
                        },
                        SizedBox.FromSize(width: theme.Spacing * 3f),
                        new Expanded
                        {
                            Child = new Card(
                                outlined: true,
                                header: new Text(
                                    "Gather Herbs",
                                    style: new TextStyle(
                                        FontSize: theme.FontSize,
                                        Bold: true,
                                        Color: theme.PrimaryTextColor
                                    )
                                ),
                                content: new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                                {
                                    Children = new Element[]
                                    {
                                        new Text(
                                            "Collect 10 moonbloom herbs from the Verdant Forest.",
                                            style: new TextStyle(
                                                FontSize: theme.FontSize,
                                                Color: theme.PrimaryTextColor
                                            )
                                        ),
                                        SizedBox.FromSize(height: theme.Spacing),
                                        new Text(
                                            "Reward: 80 gold",
                                            style: new TextStyle(
                                                FontSize: theme.FontSizeSmall,
                                                Color: theme.SecondaryTextColor
                                            )
                                        )
                                    }
                                }
                            )
                        }
                    }
                };
            }
        }
    }
}