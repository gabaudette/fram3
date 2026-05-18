using Fram3.UI.Core;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Theming
{
    public sealed class ThemeSwitcherStory : StatefulElement
    {
        public override State CreateState() => new ThemeSwitcherState();

        private sealed class ThemeSwitcherState : State<ThemeSwitcherStory>
        {
            private bool _useDark = true;

            private static readonly Theme DarkTheme = new Theme
            {
                PrimaryColor = FrameColor.FromHex("#7B61FF"),
                SecondaryColor = FrameColor.FromHex("#00D4AA"),
                BackgroundColor = FrameColor.FromHex("#0C0E1A"),
                SurfaceColor = FrameColor.FromHex("#141726"),
                OnPrimaryColor = FrameColor.FromHex("#FFFFFF"),
                ErrorColor = FrameColor.FromHex("#FF6B6B"),
                PrimaryTextColor = FrameColor.FromHex("#E2E8F0"),
                SecondaryTextColor = FrameColor.FromHex("#6B7691"),
                DisabledTextColor = FrameColor.FromHex("#2E3347"),
                FontSize = 14f,
                FontSizeSmall = 12f,
                FontSizeLarge = 20f,
                BorderRadius = 10f,
                Spacing = 8f
            };

            private static readonly Theme LightTheme = new Theme
            {
                PrimaryColor = FrameColor.FromHex("#6200EE"),
                SecondaryColor = FrameColor.FromHex("#03DAC6"),
                BackgroundColor = FrameColor.FromHex("#FFFFFF"),
                SurfaceColor = FrameColor.FromHex("#F5F5F5"),
                OnPrimaryColor = FrameColor.FromHex("#FFFFFF"),
                ErrorColor = FrameColor.FromHex("#B00020"),
                PrimaryTextColor = FrameColor.FromHex("#212121"),
                SecondaryTextColor = FrameColor.FromHex("#757575"),
                DisabledTextColor = FrameColor.FromHex("#BDBDBD"),
                FontSize = 14f,
                FontSizeSmall = 12f,
                FontSizeLarge = 20f,
                BorderRadius = 4f,
                Spacing = 8f
            };

            public override Element Build(BuildContext context)
            {
                var activeTheme = _useDark ? DarkTheme : LightTheme;

                return new ThemeProvider(
                    activeTheme,
                    new PreviewPanel(
                        useDark: _useDark,
                        onToggle: () => SetState(() => _useDark = !_useDark)
                    )
                );
            }
        }
    }
}