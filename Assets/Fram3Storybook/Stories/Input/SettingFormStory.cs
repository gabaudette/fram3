using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Input
{
    public sealed class SettingsFormStory : StatefulElement
    {
        public override State CreateState() => new SettingsFormStoryState();

        private sealed class SettingsFormStoryState : State<SettingsFormStory>
        {
            private float _masterVolume = 80f;
            private float _musicVolume = 60f;
            private float _sfxVolume = 70f;
            private int _qualityIndex = 2;
            private int _resolutionIndex = 1;
            private bool _fullscreen = true;
            private bool _vsync;
            private float _mouseSensitivity = 0.5f;
            private bool _controllerVibration = true;
            private bool _showFpsCounter;

            private static readonly string[] QualityOptions =
            {
                "Low",
                "Medium",
                "High",
                "Ultra"
            };

            private static readonly string[] ResolutionOptions =
            {
                "1280x720",
                "1920x1080",
                "2560x1440",
                "3840x2160"
            };

            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        BuildSection(
                            title: "Audio",
                            content: BuildAudioSection(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 2f),
                        BuildSection(
                            title: "Graphics",
                            content: BuildGraphicsSection(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 2f),
                        BuildSection(
                            title: "Gameplay",
                            content: BuildGameplaySection(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        BuildFooter(theme)
                    }
                };
            }

            private static Element BuildSection(string title, Element content, Theme theme)
            {
                return new Container(
                    decoration: new BoxDecoration(
                        Color: theme.SurfaceColor,
                        BorderRadius: BorderRadius.All(theme.BorderRadius),
                        Border: new Border(theme.SecondaryTextColor.WithAlpha(0.15f), 1f)
                    ),
                    padding: EdgeInsets.All(theme.Spacing * 2f)
                )
                {
                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text(
                                title.ToUpperInvariant(),
                                style: new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Bold: true,
                                    Color: theme.PrimaryColor,
                                    LetterSpacing: 1.5f
                                )
                            ),
                            SizedBox.FromSize(height: theme.Spacing * 1.5f),
                            new Container(
                                height: 1f,
                                decoration: new BoxDecoration(
                                    Color: theme.SecondaryTextColor.WithAlpha(0.15f)
                                )
                            ),
                            SizedBox.FromSize(height: theme.Spacing * 1.5f),
                            content
                        }
                    }
                };
            }

            private Element BuildAudioSection(Theme theme)
            {
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new FrameSlider(
                            value: _masterVolume, min: 0f, max: 100f,
                            label: $"Master Volume  {(int)_masterVolume}%",
                            onChanged: v => SetState(() => _masterVolume = v)
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new FrameSlider(
                            value: _musicVolume, min: 0f, max: 100f,
                            label: $"Music Volume  {(int)_musicVolume}%",
                            onChanged: v => SetState(() => _musicVolume = v)
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new FrameSlider(
                            value: _sfxVolume, min: 0f, max: 100f,
                            label: $"SFX Volume  {(int)_sfxVolume}%",
                            onChanged: v => SetState(() => _sfxVolume = v)
                        )
                    }
                };
            }

            private Element BuildGraphicsSection(Theme theme)
            {
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new Dropdown(
                            options: QualityOptions,
                            selectedIndex: _qualityIndex,
                            label: "Quality Preset",
                            onChanged: i => SetState(() => _qualityIndex = i)
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Dropdown(
                            options: ResolutionOptions, selectedIndex: _resolutionIndex,
                            label: "Resolution",
                            onChanged: i => SetState(() => _resolutionIndex = i)
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new FrameToggle(
                            value: _fullscreen,
                            label: "Fullscreen",
                            onChanged: v => SetState(() => _fullscreen = v)
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new FrameToggle(
                            value: _vsync,
                            label: "VSync",
                            onChanged: v => SetState(() => _vsync = v)
                        )
                    }
                };
            }

            private Element BuildGameplaySection(Theme theme)
            {
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new FrameSlider(
                            value: _mouseSensitivity,
                            min: 0.1f,
                            max: 2f,
                            label: $"Mouse Sensitivity  {_mouseSensitivity:F1}",
                            onChanged: v => SetState(() => _mouseSensitivity = v)
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new FrameToggle(
                            value: _controllerVibration,
                            label: "Controller Vibration",
                            onChanged: v => SetState(() => _controllerVibration = v)
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new FrameToggle(
                            value: _showFpsCounter,
                            label: "Show FPS Counter",
                            onChanged: v => SetState(() => _showFpsCounter = v
                            )
                        )
                    }
                };
            }

            private static Element BuildFooter(Theme theme)
            {
                return new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                {
                    Children = new Element[]
                    {
                        new Button(label: "Cancel"),
                        SizedBox.FromSize(width: theme.Spacing),
                        new Button(label: "Save Changes", onPressed: () => { })
                    }
                };
            }
        }
    }
}