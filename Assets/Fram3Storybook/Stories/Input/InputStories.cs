#nullable enable
using System.Collections.Generic;

namespace Fram3.UI.Storybook.Stories.Input
{
    public static class InputStories
    {
        public static IReadOnlyList<Story> All()
        {
            return new Story[]
            {
                new Story(
                    name: "Button",
                    description: "A pressable button with a text label and an optional callback invoked on click.",
                    build: () => new ButtonStory()
                ),
                new Story(
                    name: "Checkbox",
                    description:
                    "A binary toggle rendered as a checkbox, with an optional text label placed beside it.",
                    build: () => new CheckboxStory()
                ),
                new Story(
                    name: "Dropdown",
                    description: "A collapsed pick-list that expands to show all available string options;" +
                                 " supports an optional label and a pre-selected index.",
                    build: () => new DropdownStory()
                ),
                new Story(
                    name: "FloatField",
                    description: "A numeric input that accepts floating-point values, " +
                                 "with an optional label and initial value.",
                    build: () => new FloatFieldStory()
                ),
                new Story(
                    name: "FrameSlider",
                    description: "A continuous value picker that slides between" +
                                 " a minimum and maximum, with an optional label.",
                    build: () => new SliderStory()
                ),
                new Story(
                    name: "FrameToggle",
                    description: "A binary toggle rendered as a switch, with an optional text label;" +
                                 " functionally equivalent to Checkbox with a different visual style.",
                    build: () => new ToggleStory()
                ),
                new Story(
                    name: "IntField",
                    description: "A numeric input constrained to integers, with an optional label and initial value.",
                    () => new IntFieldStory()
                ),
                new Story(
                    name: "MinMaxSlider",
                    description: "A dual-handle slider for selecting a numeric range;" +
                                 " both the lower and upper bound are independently draggable.",
                    build: () => new MinMaxSliderStory()
                ),
                new Story(
                    name: "PasswordField",
                    description: "A text input that masks its characters; " +
                                 "behaves identically to TextField but with a hidden-text display.",
                    build: () => new PasswordFieldStory()
                ),
                new Story(
                    name: "RadioGroup",
                    description: "A list of mutually exclusive options rendered as radio buttons;" +
                                 " at most one option can be selected at a time.",
                    () => new RadioGroupStory()
                ),
                new Story(
                    name: "Settings Form",
                    description: "A complete game settings panel with Audio, Graphics, " +
                                 "and Gameplay sections demonstrating sliders," +
                                 " toggles, and dropdowns working together.",
                    build: () => new SettingsFormStory()
                ),
                new Story(
                    name: "TextField",
                    description: "A single- or multi-line text input with optional placeholder," +
                                 " initial value, and read-only mode.",
                    build: () => new TextFieldStory()
                )
            };
        }
    }
}