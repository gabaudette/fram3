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
                    BuildButton),
                new Story("TextField",
                    "A single- or multi-line text input with optional placeholder, initial value, and read-only mode.",
                    BuildTextField),
                new Story("PasswordField",
                    "A text input that masks its characters; behaves identically to TextField but with a hidden-text display.",
                    BuildPasswordField),
                new Story("Checkbox",
                    "A binary toggle rendered as a checkbox, with an optional text label placed beside it.",
                    BuildCheckbox),
                new Story("FrameToggle",
                    "A binary toggle rendered as a switch, with an optional text label; functionally equivalent to Checkbox with a different visual style.",
                    BuildToggle),
                new Story("FrameSlider",
                    "A continuous value picker that slides between a minimum and maximum, with an optional label.",
                    BuildSlider),
                new Story("MinMaxSlider",
                    "A dual-handle slider for selecting a numeric range; both the lower and upper bound are independently draggable.",
                    BuildMinMaxSlider),
                new Story("RadioGroup",
                    "A list of mutually exclusive options rendered as radio buttons; at most one option can be selected at a time.",
                    BuildRadioGroup),
                new Story("Dropdown",
                    "A collapsed pick-list that expands to show all available string options; supports an optional label and a pre-selected index.",
                    BuildDropdown),
                new Story("IntField",
                    "A numeric input constrained to integers, with an optional label and initial value.",
                    BuildIntField),
                new Story("FloatField",
                    "A numeric input that accepts floating-point values, with an optional label and initial value.",
                    BuildFloatField),
                new Story("Settings Form",
                    "A complete game settings panel with Audio, Graphics, and Gameplay sections demonstrating sliders, toggles, and dropdowns working together.",
                    BuildSettingsForm),
            };
        }

        private static Element BuildButton()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("Default button:"),
                    SizedBox.FromSize(height: 4f),
                    new Button(label: "Click me"),
                    SizedBox.FromSize(height: 12f),
                    new Text("Button with callback:"),
                    SizedBox.FromSize(height: 4f),
                    new Button(label: "Save", onPressed: () => { }),
                    SizedBox.FromSize(height: 12f),
                    new Text("Button with no label:"),
                    SizedBox.FromSize(height: 4f),
                    new Button(label: null),
                }
            };
        }

        private static Element BuildTextField()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("Empty field with placeholder:"),
                    SizedBox.FromSize(height: 4f),
                    new TextField(placeholder: "Enter text here..."),
                    SizedBox.FromSize(height: 12f),
                    new Text("Pre-filled field:"),
                    SizedBox.FromSize(height: 4f),
                    new TextField(value: "Hello, Fram3!"),
                    SizedBox.FromSize(height: 12f),
                    new Text("Read-only field:"),
                    SizedBox.FromSize(height: 4f),
                    new TextField(value: "Read-only value", readOnly: true),
                    SizedBox.FromSize(height: 12f),
                    new Text("Multiline field:"),
                    SizedBox.FromSize(height: 4f),
                    new TextField(
                        value: "Line one\nLine two\nLine three",
                        multiline: true
                    ),
                }
            };
        }

        private static Element BuildPasswordField()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("Password field with placeholder:"),
                    SizedBox.FromSize(height: 4f),
                    new PasswordField(placeholder: "Enter password..."),
                    SizedBox.FromSize(height: 12f),
                    new Text("Pre-filled password field:"),
                    SizedBox.FromSize(height: 4f),
                    new PasswordField(value: "secret123"),
                }
            };
        }

        private static Element BuildCheckbox()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("Unchecked (default):"),
                    SizedBox.FromSize(height: 4f),
                    new Checkbox(),
                    SizedBox.FromSize(height: 12f),
                    new Text("Checked:"),
                    SizedBox.FromSize(height: 4f),
                    new Checkbox(value: true),
                    SizedBox.FromSize(height: 12f),
                    new Text("With label:"),
                    SizedBox.FromSize(height: 4f),
                    new Checkbox(value: false, label: "Accept terms and conditions"),
                    new Checkbox(value: true, label: "Subscribe to newsletter"),
                }
            };
        }

        private static Element BuildToggle()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("Toggle off (default):"),
                    SizedBox.FromSize(height: 4f),
                    new FrameToggle(),
                    SizedBox.FromSize(height: 12f),
                    new Text("Toggle on:"),
                    SizedBox.FromSize(height: 4f),
                    new FrameToggle(value: true),
                    SizedBox.FromSize(height: 12f),
                    new Text("Toggle with label:"),
                    SizedBox.FromSize(height: 4f),
                    new FrameToggle(value: true, label: "Enable dark mode"),
                }
            };
        }

        private static Element BuildSlider()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("Default slider (0-1, value 0.5):"),
                    SizedBox.FromSize(height: 4f),
                    new FrameSlider(value: 0.5f),
                    SizedBox.FromSize(height: 12f),
                    new Text("Slider with label and custom range:"),
                    SizedBox.FromSize(height: 4f),
                    new FrameSlider(value: 60f, min: 0f, max: 100f, label: "Volume"),
                }
            };
        }

        private static Element BuildMinMaxSlider()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("Default min-max slider (0-1):"),
                    SizedBox.FromSize(height: 4f),
                    new MinMaxSlider(minValue: 0.2f, maxValue: 0.8f),
                    SizedBox.FromSize(height: 12f),
                    new Text("Min-max slider with label and custom range:"),
                    SizedBox.FromSize(height: 4f),
                    new MinMaxSlider(
                        minValue: 20f,
                        maxValue: 80f,
                        lowLimit: 0f,
                        highLimit: 100f,
                        label: "Price range"
                    ),
                }
            };
        }

        private static Element BuildRadioGroup()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("Radio group with no selection:"),
                    SizedBox.FromSize(height: 4f),
                    new RadioGroup(
                        options: new string[] { "Option A", "Option B", "Option C" }
                    ),
                    SizedBox.FromSize(height: 12f),
                    new Text("Radio group with pre-selected value:"),
                    SizedBox.FromSize(height: 4f),
                    new RadioGroup(
                        options: new string[] { "Small", "Medium", "Large" },
                        selectedValue: "Medium"
                    ),
                }
            };
        }

        private static Element BuildDropdown()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("Dropdown with no selection:"),
                    SizedBox.FromSize(height: 4f),
                    new Dropdown(
                        options: new string[] { "Red", "Green", "Blue", "Yellow" }
                    ),
                    SizedBox.FromSize(height: 12f),
                    new Text("Dropdown with label and pre-selection:"),
                    SizedBox.FromSize(height: 4f),
                    new Dropdown(
                        options: new string[] { "USD", "EUR", "GBP", "JPY" },
                        selectedIndex: 1,
                        label: "Currency"
                    ),
                }
            };
        }

        private static Element BuildIntField()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("Integer field (default 0):"),
                    SizedBox.FromSize(height: 4f),
                    new IntField(),
                    SizedBox.FromSize(height: 12f),
                    new Text("Integer field with label and value:"),
                    SizedBox.FromSize(height: 4f),
                    new IntField(value: 42, label: "Count"),
                }
            };
        }

        private static Element BuildFloatField()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("Float field (default 0):"),
                    SizedBox.FromSize(height: 4f),
                    new FloatField(),
                    SizedBox.FromSize(height: 12f),
                    new Text("Float field with label and value:"),
                    SizedBox.FromSize(height: 4f),
                    new FloatField(value: 3.14f, label: "Pi"),
                }
            };
        }

        private static Element BuildSettingsForm()
        {
            return new SettingsFormElement();
        }

        // ---------------------------------------------------------------------------
        // Settings Form
        // ---------------------------------------------------------------------------

        private sealed class SettingsFormElement : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new SettingsFormState();

            private sealed class SettingsFormState : Fram3.UI.Core.State<SettingsFormElement>
            {
                // Audio
                private float _masterVolume = 80f;
                private float _musicVolume = 60f;
                private float _sfxVolume = 70f;

                // Graphics
                private int _qualityIndex = 2;
                private int _resolutionIndex = 1;
                private bool _fullscreen = true;
                private bool _vsync = false;

                // Gameplay
                private float _mouseSensitivity = 0.5f;
                private bool _controllerVibration = true;
                private bool _showFpsCounter = false;

                private static readonly string[] QualityOptions = new string[]
                    { "Low", "Medium", "High", "Ultra" };

                private static readonly string[] ResolutionOptions = new string[]
                    { "1280x720", "1920x1080", "2560x1440", "3840x2160" };

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
                                new Container(
                                    height: 1f,
                                    decoration: new BoxDecoration(
                                        Color: theme.SecondaryTextColor.WithAlpha(0.15f)
                                    )
                                ),
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
                            new FrameSlider(
                                value: _masterVolume,
                                min: 0f,
                                max: 100f,
                                label: $"Master Volume  {(int)_masterVolume}%",
                                onChanged: v => SetState(() => _masterVolume = v)
                            ),
                            SizedBox.FromSize(height: theme.Spacing),
                            new FrameSlider(
                                value: _musicVolume,
                                min: 0f,
                                max: 100f,
                                label: $"Music Volume  {(int)_musicVolume}%",
                                onChanged: v => SetState(() => _musicVolume = v)
                            ),
                            SizedBox.FromSize(height: theme.Spacing),
                            new FrameSlider(
                                value: _sfxVolume,
                                min: 0f,
                                max: 100f,
                                label: $"SFX Volume  {(int)_sfxVolume}%",
                                onChanged: v => SetState(() => _sfxVolume = v)
                            ),
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
                                options: ResolutionOptions,
                                selectedIndex: _resolutionIndex,
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
                            ),
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
                                onChanged: v => SetState(() => _showFpsCounter = v)
                            ),
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