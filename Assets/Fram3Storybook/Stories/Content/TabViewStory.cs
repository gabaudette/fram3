using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class TabViewStory : StatefulElement
    {
        public override State CreateState() => new TabViewStoryState();

        private sealed class TabViewStoryState : State<TabViewStory>
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
                return new TabView(
                    tabs: new Tab[]
                    {
                        new Tab(
                            label: "Alpha",
                            content: new Padding(EdgeInsets.All(16f))
                            {
                                Child = new Text(
                                    "Content for tab Alpha.",
                                    style: new TextStyle(Color: theme.PrimaryTextColor)
                                )
                            }
                        ),
                        new Tab(
                            label: "Beta",
                            content: new Padding(EdgeInsets.All(16f))
                            {
                                Child = new Text(
                                    "Content for tab Beta.",
                                    style: new TextStyle(Color: theme.PrimaryTextColor)
                                )
                            }
                        ),
                        new Tab(
                            label: "Gamma",
                            content: new Padding(EdgeInsets.All(16f))
                            {
                                Child = new Text(
                                    "Content for tab Gamma.",
                                    style: new TextStyle(Color: theme.PrimaryTextColor)
                                )
                            }
                        )
                    },
                    initialIndex: 0
                );
            }

            private static Element BuildGame(Theme theme)
            {
                return new TabView(
                    tabs: new Tab[]
                    {
                        new Tab(
                            label: "Character",
                            content: new Padding(EdgeInsets.All(theme.Spacing))
                            {
                                Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                                {
                                    Children = new Element[]
                                    {
                                        new Text("Aric Stormblade",
                                            style: new TextStyle(
                                                Bold: true, FontSize: theme.FontSizeLarge,
                                                Color: theme.PrimaryTextColor
                                            )
                                        ),
                                        new Text(
                                            "Warrior - Level 42",
                                            style: new TextStyle(
                                                Color: theme.SecondaryTextColor,
                                                FontSize: theme.FontSizeSmall
                                            )
                                        )
                                    }
                                }
                            }
                        ),
                        new Tab(
                            label: "Inventory",
                            content: new Padding(EdgeInsets.All(theme.Spacing))
                            {
                                Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                                {
                                    Children = new Element[]
                                    {
                                        new Text(
                                            "Shadowblade x1",
                                            style: new TextStyle(Color: theme.PrimaryTextColor)
                                        ),
                                        new Text(
                                            "Health Potion x5",
                                            style: new TextStyle(Color: theme.PrimaryTextColor)
                                        ),
                                        new Text(
                                            "Gold x1,240",
                                            style: new TextStyle(
                                                Color: FrameColor.FromHex("#FFD700"),
                                                Bold: true
                                            )
                                        )
                                    }
                                }
                            }
                        ),
                        new Tab(
                            label: "Skills",
                            content: new Padding(EdgeInsets.All(theme.Spacing))
                            {
                                Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                                {
                                    Children = new Element[]
                                    {
                                        new Text(
                                            "Cleave - Lv 3",
                                            style: new TextStyle(Color: theme.PrimaryTextColor)
                                        ),
                                        new Text(
                                            "Battlecry - Lv 2",
                                            style: new TextStyle(Color: theme.PrimaryTextColor)
                                        ),
                                        new Text(
                                            "Shield Bash - Lv 5",
                                            style: new TextStyle(Color: theme.PrimaryTextColor)
                                        )
                                    }
                                }
                            }
                        )
                    },
                    initialIndex: 0
                );
            }
        }
    }
}