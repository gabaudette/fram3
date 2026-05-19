using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Input
{
    public sealed class SliderStory : StatefulElement
    {
        public override State CreateState() => new SliderStoryState();

        private sealed class SliderStoryState : State<SliderStory>
        {
            private float _crosshairSize = 8f;
            private float _fov = 80f;

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
                            "Default slider (0-1):",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: 4f),
                        new FrameSlider(value: 0.5f),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Text(
                            "Slider with label and range:",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: 4f),
                        new FrameSlider(
                            value: 60f,
                            min: 0f,
                            max: 100f,
                            label: "Volume"
                        )
                    }
                };
            }

            private Element BuildGame(Theme theme)
            {
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new Text(
                            "CROSSHAIR & VIEW",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new FrameSlider(
                            value: _crosshairSize,
                            min: 4f,
                            max: 20f,
                            label: $"Crosshair Size  {_crosshairSize:F0}px",
                            onChanged: v => SetState(() => _crosshairSize = v)
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new FrameSlider(
                            value: _fov,
                            min: 60f,
                            max: 120f,
                            label: $"Field of View  {_fov:F0} deg",
                            onChanged: v => SetState(() => _fov = v)
                        )
                    }
                };
            }
        }
    }
}