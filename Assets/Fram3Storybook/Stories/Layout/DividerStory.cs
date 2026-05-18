using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Layout
{
    public sealed class DividerStory : StatefulElement
    {
        public override State CreateState() => new DividerStoryState();

        private sealed class DividerStoryState : State<DividerStory>
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
                            "Above divider",
                            style: new TextStyle(
                                Color: theme.PrimaryTextColor
                            )
                        ),
                        new Divider(color: theme.SecondaryTextColor.WithAlpha(0.3f)),
                        new Text(
                            "Below divider",
                            style: new TextStyle(
                                Color: theme.PrimaryTextColor
                            )
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
                            "INVENTORY",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )
                        ),
                        SizedBox.FromSize(height: 4f),
                        new Divider(color: theme.SecondaryColor.WithAlpha(0.4f)),
                        SizedBox.FromSize(height: 4f),
                        new Text(
                            "Shadowblade x1",
                            style: new TextStyle(
                                Color: theme.PrimaryTextColor
                            )
                        ),
                        new Divider(
                            color: theme.SecondaryTextColor.WithAlpha(0.15f),
                            indent: 8f
                        ),
                        new Text(
                            "Health Potion x5",
                            style: new TextStyle(
                                Color: theme.PrimaryTextColor
                            )
                        ),
                        new Divider(
                            color: theme.SecondaryTextColor.WithAlpha(0.15f),
                            indent: 8f
                        ),
                        new Text(
                            "Emberfall Gauntlets x1",
                            style: new TextStyle(
                                Color: theme.PrimaryTextColor
                            )
                        )
                    }
                };
            }
        }
    }
}