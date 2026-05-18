using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Layout
{
    public sealed class ScrollViewStory : StatefulElement
    {
        public override State CreateState() => new ScrollViewStoryState();

        private sealed class ScrollViewStoryState : State<ScrollViewStory>
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
                var items = new Element[12];
                for (var i = 0; i < 12; i++)
                {
                    var label = $"Scrollable item {i + 1}";
                    items[i] = new Padding(
                        EdgeInsets.Symmetric(vertical: 4f, horizontal: 0f)
                    )
                    {
                        Child = new Text(
                            label,
                            style: new TextStyle(Color: theme.PrimaryTextColor)
                        )
                    };
                }

                return new Container(height: 160f)
                {
                    Child = new ScrollView
                    {
                        Child = new Column { Children = items }
                    }
                };
            }

            private static Element BuildGame(Theme theme)
            {
                var logs = new (string text, FrameColor color)[]
                {
                    ("Aric Stormblade slew a Goblin.", FrameColor.FromHex("#00D4AA")),
                    ("Lyria Moonwhisper cast Fireball.", FrameColor.FromHex("#FF8C00")),
                    ("Drex Ironfoot picked up Gold x50.", FrameColor.FromHex("#FFD700")),
                    ("Vex Nightfall used Smoke Bomb.", theme.SecondaryTextColor),
                    ("Seraphine Vale healed Aric for 80 HP.", FrameColor.FromHex("#00D4AA")),
                    ("Korroth was defeated by a Troll.", FrameColor.FromHex("#FF6B6B")),
                    ("Nira Emberveil found a Rare Helm.", FrameColor.FromHex("#B04AFF")),
                    ("Theron Ashwood gained 200 XP.", FrameColor.FromHex("#FFD700"))
                };

                var entries = new List<Element>();
                foreach (var (text, color) in logs)
                {
                    entries.Add(
                        new Padding(
                            EdgeInsets.Symmetric(vertical: 2f, horizontal: 0f)
                        )
                        {
                            Child = new Text(
                                text,
                                style: new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Color: color
                                )
                            )
                        }
                    );
                }

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new Text(
                            "COMBAT LOG",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Container(height: 140f)
                        {
                            Child = new ScrollView
                            {
                                Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                                {
                                    Children = entries.ToArray()
                                }
                            }
                        }
                    }
                };
            }
        }
    }
}