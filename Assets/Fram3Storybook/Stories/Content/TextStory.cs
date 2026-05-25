using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;
using UnityEngine.TextCore.Text;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class TextStory : StatefulElement
    {
        public override State CreateState() => new TextStoryState();

        private sealed class TextStoryState : State<TextStory>
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
                            label: "Font Families",
                            content: BuildFontFamilies(theme),
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
                return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                {
                    Children = new Element[]
                    {
                        new Text(
                            "Default text",
                            style: new TextStyle(Color: theme.PrimaryTextColor)
                        ),
                        new Text(
                            "Large bold text",
                            style: new TextStyle(
                                FontSize: 24f,
                                Bold: true,
                                Color: theme.PrimaryTextColor
                            )
                        ),
                        new Text(
                            "Accent italic text",
                            style: new TextStyle(
                                FontSize: 16f,
                                Color: theme.PrimaryColor,
                                Italic: true
                            )
                        ),
                        new Text(
                            "Small with letter spacing",
                            style: new TextStyle(
                                FontSize: 11f,
                                LetterSpacing: 2f,
                                Color: theme.SecondaryTextColor
                            )
                        )
                    }
                };
            }

            private static Element BuildFontFamilies(Theme theme)
            {
                FontAsset? primaryFont = theme.FontFamily;
                FontAsset? displayFont = StorybookApp.DisplayFont;

                return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                {
                    Children = new Element[]
                    {
                        // --- Global font via theme ---
                        // Set Theme.FontFamily on the root ThemeProvider (or any ancestor ThemeProvider)
                        // and every Text in that subtree inherits it automatically.
                        new Text(
                            "Global: set Theme.FontFamily on a ThemeProvider",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor,
                                Bold: true,
                                LetterSpacing: 1f
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new ThemeProvider(theme with { FontFamily = primaryFont })
                        {
                            Child = new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                            {
                                Children = new Element[]
                                {
                                    new Text("Heading — inherits global font",
                                        style: new TextStyle(FontSize: 18f, Bold: true, Color: theme.PrimaryTextColor)),
                                    new Text("Body — inherits global font",
                                        style: new TextStyle(Color: theme.PrimaryTextColor)),
                                    new Text("Caption — inherits global font",
                                        style: new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.SecondaryTextColor))
                                }
                            }
                        },

                        SizedBox.FromSize(height: theme.Spacing * 2f),

                        // --- Spot override via TextStyle.FontAsset ---
                        // Any single Text can break from the theme font by passing FontAsset directly.
                        new Text(
                            "Spot: pass FontAsset to TextStyle to override one element",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor,
                                Bold: true,
                                LetterSpacing: 1f
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Text("Inherits theme font",
                            style: new TextStyle(Color: theme.PrimaryTextColor)),
                        new Text("Spot override — display font",
                            style: new TextStyle(Color: theme.PrimaryColor, FontAsset: displayFont)),
                        new Text("Back to theme font",
                            style: new TextStyle(Color: theme.PrimaryTextColor))
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
                            "KILL FEED",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        KillFeedEntry(
                            "Drex Ironfoot", "eliminated", "a Goblin Scout",
                            actorColor: FrameColor.FromHex("#FF6B6B"),
                            theme
                        ),
                        KillFeedEntry(
                            "Lyria Moonwhisper", "cast", "Arcane Surge",
                            actorColor: FrameColor.FromHex("#7B61FF"),
                            theme
                        ),
                        KillFeedEntry(
                            "Seraphine Vale", "healed", "Aric for 142 HP",
                            actorColor: FrameColor.FromHex("#00D4AA"),
                            theme
                        ),
                        KillFeedEntry(
                            "Vex Nightfall", "looted", "Shadowblade",
                            actorColor: FrameColor.FromHex("#FFD700"),
                            theme
                        )
                    }
                };
            }

            private static Element KillFeedEntry(
                string actor,
                string verb,
                string target,
                FrameColor actorColor,
                Theme theme
            )
            {
                return new Padding(EdgeInsets.Symmetric(vertical: 2f, horizontal: 0f))
                {
                    Child = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = new Element[]
                        {
                            new Text(
                                actor,
                                style: new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Bold: true,
                                    Color: actorColor
                                )
                            ),
                            new Text(
                                $" {verb} ",
                                style: new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Color: theme.SecondaryTextColor
                                )
                            ),
                            new Text(
                                target,
                                style: new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Color: theme.PrimaryTextColor
                                )
                            )
                        }
                    }
                };
            }
        }
    }
}