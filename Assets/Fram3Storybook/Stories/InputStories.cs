#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories
{
    /// <summary>Stories for the Input chapter.</summary>
    public static class InputStories
    {
        /// <summary>Returns all input stories.</summary>
        public static IReadOnlyList<Story> All()
        {
            return new Story[]
            {
                new Story("Button", "A pressable button with a text label and an optional callback invoked on click.",
                    () => new ButtonStory()),
                new Story("TextField",
                    "A single- or multi-line text input with optional placeholder, initial value, and read-only mode.",
                    () => new TextFieldStory()),
                new Story("PasswordField",
                    "A text input that masks its characters; behaves identically to TextField but with a hidden-text display.",
                    () => new PasswordFieldStory()),
                new Story("Checkbox",
                    "A binary toggle rendered as a checkbox, with an optional text label placed beside it.",
                    () => new CheckboxStory()),
                new Story("FrameToggle",
                    "A binary toggle rendered as a switch, with an optional text label; functionally equivalent to Checkbox with a different visual style.",
                    () => new ToggleStory()),
                new Story("FrameSlider",
                    "A continuous value picker that slides between a minimum and maximum, with an optional label.",
                    () => new SliderStory()),
                new Story("MinMaxSlider",
                    "A dual-handle slider for selecting a numeric range; both the lower and upper bound are independently draggable.",
                    () => new MinMaxSliderStory()),
                new Story("RadioGroup",
                    "A list of mutually exclusive options rendered as radio buttons; at most one option can be selected at a time.",
                    () => new RadioGroupStory()),
                new Story("Dropdown",
                    "A collapsed pick-list that expands to show all available string options; supports an optional label and a pre-selected index.",
                    () => new DropdownStory()),
                new Story("IntField",
                    "A numeric input constrained to integers, with an optional label and initial value.",
                    () => new IntFieldStory()),
                new Story("FloatField",
                    "A numeric input that accepts floating-point values, with an optional label and initial value.",
                    () => new FloatFieldStory()),
                new Story("Settings Form",
                    "A complete game settings panel with Audio, Graphics, and Gameplay sections demonstrating sliders, toggles, and dropdowns working together.",
                    BuildSettingsForm),
            };
        }

        private static Element BuildSettingsForm() => new SettingsFormElement();

        // ---------------------------------------------------------------------------
        // Button
        // ---------------------------------------------------------------------------

        private sealed class ButtonStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new ButtonStoryState();

            private sealed class ButtonStoryState : Fram3.UI.Core.State<ButtonStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("Enabled button:", new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                            SizedBox.FromSize(height: 4f),
                            new Button(label: "Save", onPressed: () => { }),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Text("Disabled button (onPressed: null):", new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                            SizedBox.FromSize(height: 4f),
                            new Button(label: "Disabled"),
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("COMBAT ACTIONS", new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                            {
                                Children = new Element[]
                                {
                                    new Expanded { Child = new Button(label: "ATTACK", onPressed: () => { }) },
                                    SizedBox.FromSize(width: theme.Spacing),
                                    new Expanded { Child = new Button(label: "DEFEND", onPressed: () => { }) },
                                    SizedBox.FromSize(width: theme.Spacing),
                                    new Expanded { Child = new Button(label: "FLEE") },
                                }
                            },
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // TextField
        // ---------------------------------------------------------------------------

        private sealed class TextFieldStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new TextFieldStoryState();

            private sealed class TextFieldStoryState : Fram3.UI.Core.State<TextFieldStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("Empty field with placeholder:", new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                            SizedBox.FromSize(height: 4f),
                            new TextField(placeholder: "Enter text here..."),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Text("Pre-filled field:", new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                            SizedBox.FromSize(height: 4f),
                            new TextField(value: "Hello, Fram3!"),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Text("Read-only field:", new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                            SizedBox.FromSize(height: 4f),
                            new TextField(value: "Read-only value", readOnly: true),
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // PasswordField
        // ---------------------------------------------------------------------------

        private sealed class PasswordFieldStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new PasswordFieldStoryState();

            private sealed class PasswordFieldStoryState : Fram3.UI.Core.State<PasswordFieldStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("Password field with placeholder:", new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                            SizedBox.FromSize(height: 4f),
                            new PasswordField(placeholder: "Enter password..."),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Text("Pre-filled password field:", new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                            SizedBox.FromSize(height: 4f),
                            new PasswordField(value: "secret123"),
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // Checkbox
        // ---------------------------------------------------------------------------

        private sealed class CheckboxStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new CheckboxStoryState();

            private sealed class CheckboxStoryState : Fram3.UI.Core.State<CheckboxStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Checkbox(value: false, label: "Accept terms and conditions"),
                            new Checkbox(value: true, label: "Subscribe to newsletter"),
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // Toggle
        // ---------------------------------------------------------------------------

        private sealed class ToggleStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new ToggleStoryState();

            private sealed class ToggleStoryState : Fram3.UI.Core.State<ToggleStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new FrameToggle(value: false, label: "Toggle off"),
                            SizedBox.FromSize(height: 4f),
                            new FrameToggle(value: true, label: "Toggle on"),
                            SizedBox.FromSize(height: 4f),
                            new FrameToggle(value: true, label: "Enable dark mode"),
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("HUD VISIBILITY", new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )),
                            SizedBox.FromSize(height: theme.Spacing),
                            new FrameToggle(value: true, label: "Show minimap"),
                            SizedBox.FromSize(height: 4f),
                            new FrameToggle(value: true, label: "Show HP bar"),
                            SizedBox.FromSize(height: 4f),
                            new FrameToggle(value: false, label: "Show FPS counter"),
                            SizedBox.FromSize(height: 4f),
                            new FrameToggle(value: true, label: "Show ability cooldowns"),
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // Slider
        // ---------------------------------------------------------------------------

        private sealed class SliderStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new SliderStoryState();

            private sealed class SliderStoryState : Fram3.UI.Core.State<SliderStory>
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
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("Default slider (0-1):", new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                            SizedBox.FromSize(height: 4f),
                            new FrameSlider(value: 0.5f),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Text("Slider with label and range:", new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                            SizedBox.FromSize(height: 4f),
                            new FrameSlider(value: 60f, min: 0f, max: 100f, label: "Volume"),
                        }
                    };
                }

                private Element BuildGame(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("CROSSHAIR & VIEW", new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )),
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
                            ),
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // MinMaxSlider
        // ---------------------------------------------------------------------------

        private sealed class MinMaxSliderStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new MinMaxSliderStoryState();

            private sealed class MinMaxSliderStoryState : Fram3.UI.Core.State<MinMaxSliderStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("Default min-max slider (0-1):", new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                            SizedBox.FromSize(height: 4f),
                            new MinMaxSlider(minValue: 0.2f, maxValue: 0.8f),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Text("Min-max slider with label:", new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                            SizedBox.FromSize(height: 4f),
                            new MinMaxSlider(
                                minValue: 20f, maxValue: 80f,
                                lowLimit: 0f, highLimit: 100f,
                                label: "Level range filter"
                            ),
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // RadioGroup
        // ---------------------------------------------------------------------------

        private sealed class RadioGroupStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new RadioGroupStoryState();

            private sealed class RadioGroupStoryState : Fram3.UI.Core.State<RadioGroupStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("With pre-selected value:", new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                            SizedBox.FromSize(height: 4f),
                            new RadioGroup(
                                options: new string[] { "Easy", "Normal", "Hard", "Nightmare" },
                                selectedValue: "Normal"
                            ),
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // Dropdown
        // ---------------------------------------------------------------------------

        private sealed class DropdownStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new DropdownStoryState();

            private sealed class DropdownStoryState : Fram3.UI.Core.State<DropdownStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Dropdown(
                                options: new string[] { "Warrior", "Mage", "Rogue", "Healer", "Ranger" },
                                selectedIndex: 0,
                                label: "Class"
                            ),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Dropdown(
                                options: new string[] { "1280x720", "1920x1080", "2560x1440", "3840x2160" },
                                selectedIndex: 1,
                                label: "Resolution"
                            ),
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // IntField
        // ---------------------------------------------------------------------------

        private sealed class IntFieldStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new IntFieldStoryState();

            private sealed class IntFieldStoryState : Fram3.UI.Core.State<IntFieldStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new IntField(),
                            SizedBox.FromSize(height: theme.Spacing),
                            new IntField(value: 42, label: "Max Inventory Size"),
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // FloatField
        // ---------------------------------------------------------------------------

        private sealed class FloatFieldStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new FloatFieldStoryState();

            private sealed class FloatFieldStoryState : Fram3.UI.Core.State<FloatFieldStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new FloatField(),
                            SizedBox.FromSize(height: theme.Spacing),
                            new FloatField(value: 1.5f, label: "Move Speed Multiplier"),
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // Settings Form
        // ---------------------------------------------------------------------------

        private sealed class SettingsFormElement : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new SettingsFormState();

            private sealed class SettingsFormState : Fram3.UI.Core.State<SettingsFormElement>
            {
                private float _masterVolume = 80f;
                private float _musicVolume = 60f;
                private float _sfxVolume = 70f;
                private int _qualityIndex = 2;
                private int _resolutionIndex = 1;
                private bool _fullscreen = true;
                private bool _vsync = false;
                private float _mouseSensitivity = 0.5f;
                private bool _controllerVibration = true;
                private bool _showFpsCounter = false;

                private static readonly string[] QualityOptions = new string[] { "Low", "Medium", "High", "Ultra" };
                private static readonly string[] ResolutionOptions = new string[] { "1280x720", "1920x1080", "2560x1440", "3840x2160" };

                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);

                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            BuildSection("Audio", BuildAudioSection(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 2f),
                            BuildSection("Graphics", BuildGraphicsSection(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 2f),
                            BuildSection("Gameplay", BuildGameplaySection(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            BuildFooter(theme),
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
                                new Text(title.ToUpperInvariant(), new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Bold: true,
                                    Color: theme.PrimaryColor,
                                    LetterSpacing: 1.5f
                                )),
                                SizedBox.FromSize(height: theme.Spacing * 1.5f),
                                new Container(height: 1f, decoration: new BoxDecoration(Color: theme.SecondaryTextColor.WithAlpha(0.15f))),
                                SizedBox.FromSize(height: theme.Spacing * 1.5f),
                                content,
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
                            new FrameSlider(value: _masterVolume, min: 0f, max: 100f,
                                label: $"Master Volume  {(int)_masterVolume}%",
                                onChanged: v => SetState(() => _masterVolume = v)),
                            SizedBox.FromSize(height: theme.Spacing),
                            new FrameSlider(value: _musicVolume, min: 0f, max: 100f,
                                label: $"Music Volume  {(int)_musicVolume}%",
                                onChanged: v => SetState(() => _musicVolume = v)),
                            SizedBox.FromSize(height: theme.Spacing),
                            new FrameSlider(value: _sfxVolume, min: 0f, max: 100f,
                                label: $"SFX Volume  {(int)_sfxVolume}%",
                                onChanged: v => SetState(() => _sfxVolume = v)),
                        }
                    };
                }

                private Element BuildGraphicsSection(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Dropdown(options: QualityOptions, selectedIndex: _qualityIndex, label: "Quality Preset",
                                onChanged: i => SetState(() => _qualityIndex = i)),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Dropdown(options: ResolutionOptions, selectedIndex: _resolutionIndex, label: "Resolution",
                                onChanged: i => SetState(() => _resolutionIndex = i)),
                            SizedBox.FromSize(height: theme.Spacing),
                            new FrameToggle(value: _fullscreen, label: "Fullscreen",
                                onChanged: v => SetState(() => _fullscreen = v)),
                            SizedBox.FromSize(height: theme.Spacing),
                            new FrameToggle(value: _vsync, label: "VSync",
                                onChanged: v => SetState(() => _vsync = v)),
                        }
                    };
                }

                private Element BuildGameplaySection(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new FrameSlider(value: _mouseSensitivity, min: 0.1f, max: 2f,
                                label: $"Mouse Sensitivity  {_mouseSensitivity:F1}",
                                onChanged: v => SetState(() => _mouseSensitivity = v)),
                            SizedBox.FromSize(height: theme.Spacing),
                            new FrameToggle(value: _controllerVibration, label: "Controller Vibration",
                                onChanged: v => SetState(() => _controllerVibration = v)),
                            SizedBox.FromSize(height: theme.Spacing),
                            new FrameToggle(value: _showFpsCounter, label: "Show FPS Counter",
                                onChanged: v => SetState(() => _showFpsCounter = v)),
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
                            new Button(label: "Save Changes", onPressed: () => { }),
                        }
                    };
                }
            }
        }
    }
}
