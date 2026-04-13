#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
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
                new Story("FButton",        "A pressable button with a text label and an optional callback invoked on click.",                                                                        BuildButton),
                new Story("FTextField",     "A single- or multi-line text input with optional placeholder, initial value, and read-only mode.",                                                       BuildTextField),
                new Story("FPasswordField", "A text input that masks its characters; behaves identically to FTextField but with a hidden-text display.",                                              BuildPasswordField),
                new Story("FCheckbox",      "A binary toggle rendered as a checkbox, with an optional text label placed beside it.",                                                                  BuildCheckbox),
                new Story("FToggle",        "A binary toggle rendered as a switch, with an optional text label; functionally equivalent to FCheckbox with a different visual style.",                BuildToggle),
                new Story("FSlider",        "A continuous value picker that slides between a minimum and maximum, with an optional label.",                                                           BuildSlider),
                new Story("FMinMaxSlider",  "A dual-handle slider for selecting a numeric range; both the lower and upper bound are independently draggable.",                                        BuildMinMaxSlider),
                new Story("FRadioGroup",    "A list of mutually exclusive options rendered as radio buttons; at most one option can be selected at a time.",                                          BuildRadioGroup),
                new Story("FDropdown",      "A collapsed pick-list that expands to show all available string options; supports an optional label and a pre-selected index.",                          BuildDropdown),
                new Story("FIntField",      "A numeric input constrained to integers, with an optional label and initial value.",                                                                     BuildIntField),
                new Story("FFloatField",    "A numeric input that accepts floating-point values, with an optional label and initial value.",                                                          BuildFloatField),
            };
        }

        private static FElement BuildButton()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("Default button:"),
                    FSizedBox.FromSize(height: 4f),
                    new FButton(label: "Click me"),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Button with callback:"),
                    FSizedBox.FromSize(height: 4f),
                    new FButton(label: "Save", onPressed: () => { }),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Button with no label:"),
                    FSizedBox.FromSize(height: 4f),
                    new FButton(label: null),
                }
            };
        }

        private static FElement BuildTextField()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("Empty field with placeholder:"),
                    FSizedBox.FromSize(height: 4f),
                    new FTextField(placeholder: "Enter text here..."),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Pre-filled field:"),
                    FSizedBox.FromSize(height: 4f),
                    new FTextField(value: "Hello, Fram3!"),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Read-only field:"),
                    FSizedBox.FromSize(height: 4f),
                    new FTextField(value: "Read-only value", readOnly: true),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Multiline field:"),
                    FSizedBox.FromSize(height: 4f),
                    new FTextField(
                        value: "Line one\nLine two\nLine three",
                        multiline: true
                    ),
                }
            };
        }

        private static FElement BuildPasswordField()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("Password field with placeholder:"),
                    FSizedBox.FromSize(height: 4f),
                    new FPasswordField(placeholder: "Enter password..."),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Pre-filled password field:"),
                    FSizedBox.FromSize(height: 4f),
                    new FPasswordField(value: "secret123"),
                }
            };
        }

        private static FElement BuildCheckbox()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("Unchecked (default):"),
                    FSizedBox.FromSize(height: 4f),
                    new FCheckbox(),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Checked:"),
                    FSizedBox.FromSize(height: 4f),
                    new FCheckbox(value: true),
                    FSizedBox.FromSize(height: 12f),
                    new FText("With label:"),
                    FSizedBox.FromSize(height: 4f),
                    new FCheckbox(value: false, label: "Accept terms and conditions"),
                    new FCheckbox(value: true, label: "Subscribe to newsletter"),
                }
            };
        }

        private static FElement BuildToggle()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("Toggle off (default):"),
                    FSizedBox.FromSize(height: 4f),
                    new FToggle(),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Toggle on:"),
                    FSizedBox.FromSize(height: 4f),
                    new FToggle(value: true),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Toggle with label:"),
                    FSizedBox.FromSize(height: 4f),
                    new FToggle(value: true, label: "Enable dark mode"),
                }
            };
        }

        private static FElement BuildSlider()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("Default slider (0-1, value 0.5):"),
                    FSizedBox.FromSize(height: 4f),
                    new FSlider(value: 0.5f),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Slider with label and custom range:"),
                    FSizedBox.FromSize(height: 4f),
                    new FSlider(value: 60f, min: 0f, max: 100f, label: "Volume"),
                }
            };
        }

        private static FElement BuildMinMaxSlider()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("Default min-max slider (0-1):"),
                    FSizedBox.FromSize(height: 4f),
                    new FMinMaxSlider(minValue: 0.2f, maxValue: 0.8f),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Min-max slider with label and custom range:"),
                    FSizedBox.FromSize(height: 4f),
                    new FMinMaxSlider(
                        minValue: 20f,
                        maxValue: 80f,
                        lowLimit: 0f,
                        highLimit: 100f,
                        label: "Price range"
                    ),
                }
            };
        }

        private static FElement BuildRadioGroup()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("Radio group with no selection:"),
                    FSizedBox.FromSize(height: 4f),
                    new FRadioGroup(
                        options: new string[] { "Option A", "Option B", "Option C" }
                    ),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Radio group with pre-selected value:"),
                    FSizedBox.FromSize(height: 4f),
                    new FRadioGroup(
                        options: new string[] { "Small", "Medium", "Large" },
                        selectedValue: "Medium"
                    ),
                }
            };
        }

        private static FElement BuildDropdown()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("Dropdown with no selection:"),
                    FSizedBox.FromSize(height: 4f),
                    new FDropdown(
                        options: new string[] { "Red", "Green", "Blue", "Yellow" }
                    ),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Dropdown with label and pre-selection:"),
                    FSizedBox.FromSize(height: 4f),
                    new FDropdown(
                        options: new string[] { "USD", "EUR", "GBP", "JPY" },
                        selectedIndex: 1,
                        label: "Currency"
                    ),
                }
            };
        }

        private static FElement BuildIntField()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("Integer field (default 0):"),
                    FSizedBox.FromSize(height: 4f),
                    new FIntField(),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Integer field with label and value:"),
                    FSizedBox.FromSize(height: 4f),
                    new FIntField(value: 42, label: "Count"),
                }
            };
        }

        private static FElement BuildFloatField()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("Float field (default 0):"),
                    FSizedBox.FromSize(height: 4f),
                    new FFloatField(),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Float field with label and value:"),
                    FSizedBox.FromSize(height: 4f),
                    new FFloatField(value: 3.14f, label: "Pi"),
                }
            };
        }
    }
}
