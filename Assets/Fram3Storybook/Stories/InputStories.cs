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
                new Story("FButton",       BuildButton),
                new Story("FTextField",    BuildTextField),
                new Story("FPasswordField", BuildPasswordField),
                new Story("FCheckbox",     BuildCheckbox),
                new Story("FToggle",       BuildToggle),
                new Story("FSlider",       BuildSlider),
                new Story("FMinMaxSlider", BuildMinMaxSlider),
                new Story("FRadioGroup",   BuildRadioGroup),
                new Story("FDropdown",     BuildDropdown),
                new Story("FIntField",     BuildIntField),
                new Story("FFloatField",   BuildFloatField),
            };
        }

        private static FElement BuildButton()
        {
            return new FColumn
            {
                Children = new FElement[]
                {
                    new FText("Default button:"),
                    new FButton(label: "Click me"),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Button with callback:")
                    },
                    new FButton(label: "Save", onPressed: () => { }),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Button with no label:")
                    },
                    new FButton(label: null),
                }
            };
        }

        private static FElement BuildTextField()
        {
            return new FColumn
            {
                Children = new FElement[]
                {
                    new FText("Empty field with placeholder:"),
                    new FTextField(placeholder: "Enter text here..."),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Pre-filled field:")
                    },
                    new FTextField(value: "Hello, Fram3!"),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Read-only field:")
                    },
                    new FTextField(value: "Read-only value", readOnly: true),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Multiline field:")
                    },
                    new FTextField(
                        value: "Line one\nLine two\nLine three",
                        multiline: true
                    ),
                }
            };
        }

        private static FElement BuildPasswordField()
        {
            return new FColumn
            {
                Children = new FElement[]
                {
                    new FText("Password field with placeholder:"),
                    new FPasswordField(placeholder: "Enter password..."),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Pre-filled password field:")
                    },
                    new FPasswordField(value: "secret123"),
                }
            };
        }

        private static FElement BuildCheckbox()
        {
            return new FColumn
            {
                Children = new FElement[]
                {
                    new FText("Unchecked (default):"),
                    new FCheckbox(),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Checked:")
                    },
                    new FCheckbox(value: true),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("With label:")
                    },
                    new FCheckbox(value: false, label: "Accept terms and conditions"),
                    new FCheckbox(value: true, label: "Subscribe to newsletter"),
                }
            };
        }

        private static FElement BuildToggle()
        {
            return new FColumn
            {
                Children = new FElement[]
                {
                    new FText("Toggle off (default):"),
                    new FToggle(),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Toggle on:")
                    },
                    new FToggle(value: true),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Toggle with label:")
                    },
                    new FToggle(value: true, label: "Enable dark mode"),
                }
            };
        }

        private static FElement BuildSlider()
        {
            return new FColumn
            {
                Children = new FElement[]
                {
                    new FText("Default slider (0-1, value 0.5):"),
                    new FSlider(value: 0.5f),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Slider with label and custom range:")
                    },
                    new FSlider(value: 60f, min: 0f, max: 100f, label: "Volume"),
                }
            };
        }

        private static FElement BuildMinMaxSlider()
        {
            return new FColumn
            {
                Children = new FElement[]
                {
                    new FText("Default min-max slider (0-1):"),
                    new FMinMaxSlider(minValue: 0.2f, maxValue: 0.8f),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Min-max slider with label and custom range:")
                    },
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
            return new FColumn
            {
                Children = new FElement[]
                {
                    new FText("Radio group with no selection:"),
                    new FRadioGroup(
                        options: new string[] { "Option A", "Option B", "Option C" }
                    ),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Radio group with pre-selected value:")
                    },
                    new FRadioGroup(
                        options: new string[] { "Small", "Medium", "Large" },
                        selectedValue: "Medium"
                    ),
                }
            };
        }

        private static FElement BuildDropdown()
        {
            return new FColumn
            {
                Children = new FElement[]
                {
                    new FText("Dropdown with no selection:"),
                    new FDropdown(
                        options: new string[] { "Red", "Green", "Blue", "Yellow" }
                    ),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Dropdown with label and pre-selection:")
                    },
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
            return new FColumn
            {
                Children = new FElement[]
                {
                    new FText("Integer field (default 0):"),
                    new FIntField(),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Integer field with label and value:")
                    },
                    new FIntField(value: 42, label: "Count"),
                }
            };
        }

        private static FElement BuildFloatField()
        {
            return new FColumn
            {
                Children = new FElement[]
                {
                    new FText("Float field (default 0):"),
                    new FFloatField(),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Float field with label and value:")
                    },
                    new FFloatField(value: 3.14f, label: "Pi"),
                }
            };
        }
    }
}
