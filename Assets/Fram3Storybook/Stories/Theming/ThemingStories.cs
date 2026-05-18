#nullable enable
using System.Collections.Generic;

namespace Fram3.UI.Storybook.Stories.Theming
{
    public static class ThemingStories
    {
        public static IReadOnlyList<Story> All()
        {
            return new Story[]
            {
                new Story(
                    name: "Color Palette",
                    description: "Displays all color tokens from the active theme as labeled swatches.",
                    build: () => new ColorPaletteStory()
                ),
                new Story(
                    name: "Theme Switcher",
                    description: "Wraps content in a ThemeProvider and toggles between the dark storybook theme" +
                                 " and a light variant to show live theme propagation.",
                    build: () => new ThemeSwitcherStory()
                )
            };
        }
    }
}