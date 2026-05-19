using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Input
{
    public sealed class ToggleStory : StatefulElement
    {
        public override State CreateState() => new ToggleStoryState();

        private sealed class ToggleStoryState : State<ToggleStory>
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
                            content: BuildBasic(),
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

            private static Element BuildBasic()
            {
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new FrameToggle(
                            value: false,
                            label: "Toggle off"
                        ),
                        SizedBox.FromSize(height: 4f),
                        new FrameToggle(
                            value: true,
                            label: "Toggle on"
                        ),
                        SizedBox.FromSize(height: 4f),
                        new FrameToggle(
                            value: true,
                            label: "Sound effects"
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
                            "HUD VISIBILITY",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new FrameToggle(
                            value: true,
                            label: "Show minimap"
                        ),
                        SizedBox.FromSize(height: 4f),
                        new FrameToggle(
                            value: true,
                            label: "Show HP bar"
                        ),
                        SizedBox.FromSize(height: 4f),
                        new FrameToggle(
                            value: false,
                            label: "Show FPS counter"
                        ),
                        SizedBox.FromSize(height: 4f),
                        new FrameToggle(
                            value: true,
                            label: "Show ability cooldowns"
                        )
                    }
                };
            }
        }
    }
}