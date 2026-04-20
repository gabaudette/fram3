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
    /// <summary>Stories for the Theming chapter.</summary>
    public static class ThemeStories
    {
        /// <summary>Returns all theming stories.</summary>
        public static IReadOnlyList<Story> All()
        {
            return new Story[]
            {
                new Story("Color Palette",
                    "Displays all color tokens from the active theme as labeled swatches.",
                    BuildColorPalette),
                new Story("Theme Switcher",
                    "Wraps content in a ThemeProvider and toggles between the dark storybook theme and a light variant to show live theme propagation.",
                    BuildThemeSwitcher),
            };
        }

        private static Element BuildColorPalette()
        {
            return new ColorPaletteElement();
        }

        private static Element BuildThemeSwitcher()
        {
            return new ThemeSwitcherElement();
        }

        private sealed class ColorPaletteElement : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new ColorPaletteState();

            private sealed class ColorPaletteState : Fram3.UI.Core.State<ColorPaletteElement>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);

                    var swatches = new (string label, FrameColor color)[]
                    {
                        ("PrimaryColor", theme.PrimaryColor),
                        ("SecondaryColor", theme.SecondaryColor),
                        ("BackgroundColor", theme.BackgroundColor),
                        ("SurfaceColor", theme.SurfaceColor),
                        ("OnPrimaryColor", theme.OnPrimaryColor),
                        ("ErrorColor", theme.ErrorColor),
                        ("PrimaryTextColor", theme.PrimaryTextColor),
                        ("SecondaryTextColor", theme.SecondaryTextColor),
                        ("DisabledTextColor", theme.DisabledTextColor),
                    };

                    var rows = new List<Element>();
                    foreach (var (label, color) in swatches)
                    {
                        rows.Add(BuildSwatch(label, color, theme));
                        rows.Add(SizedBox.FromSize(height: 8f));
                    }

                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = rows.ToArray()
                    };
                }

                private static string ColorHex(FrameColor c)
                {
                    var r = (int)(c.R * 255f);
                    var g = (int)(c.G * 255f);
                    var b = (int)(c.B * 255f);
                    return $"#{r:X2}{g:X2}{b:X2}";
                }

                private static Element BuildSwatch(string label, FrameColor color, Theme theme)
                {
                    return new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = new Element[]
                        {
                            new Container(
                                width: 48f,
                                height: 48f,
                                decoration: new BoxDecoration(
                                    Color: color,
                                    BorderRadius: BorderRadius.All(8f),
                                    Border: new Border(theme.PrimaryTextColor.WithAlpha(0.1f), 1f)
                                )
                            ),
                            SizedBox.FromSize(width: 12f),
                            new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                            {
                                Children = new Element[]
                                {
                                    new Text(label, new TextStyle(
                                        FontSize: theme.FontSize,
                                        Bold: true,
                                        Color: theme.PrimaryTextColor
                                    )),
                                    new Text(ColorHex(color), new TextStyle(
                                        FontSize: theme.FontSizeSmall,
                                        Color: theme.SecondaryTextColor
                                    )),
                                }
                            }
                        }
                    };
                }
            }
        }

        private sealed class ThemeSwitcherElement : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new ThemeSwitcherState();

            private sealed class ThemeSwitcherState : Fram3.UI.Core.State<ThemeSwitcherElement>
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
                    Spacing = 8f,
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
                    Spacing = 8f,
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

        private sealed class PreviewPanel : StatefulElement
        {
            public bool UseDark { get; }
            public System.Action OnToggle { get; }

            public PreviewPanel(bool useDark, System.Action onToggle)
            {
                UseDark = useDark;
                OnToggle = onToggle;
            }

            public override Fram3.UI.Core.State CreateState() => new PreviewPanelState();

            private sealed class PreviewPanelState : Fram3.UI.Core.State<PreviewPanel>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);

                    return new Container(
                        decoration: new BoxDecoration(
                            Color: theme.BackgroundColor,
                            BorderRadius: BorderRadius.All(theme.BorderRadius),
                            Border: new Border(theme.SecondaryTextColor.WithAlpha(0.2f), 1f)
                        ),
                        padding: EdgeInsets.All(theme.Spacing * 2f)
                    )
                    {
                        Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                        {
                            Children = new Element[]
                            {
                                new Button(
                                    label: Element!.UseDark ? "Switch to Light Theme" : "Switch to Dark Theme",
                                    onPressed: Element.OnToggle
                                ),
                                SizedBox.FromSize(height: theme.Spacing * 2f),
                                new Container(
                                    decoration: new BoxDecoration(
                                        Color: theme.SurfaceColor,
                                        BorderRadius: BorderRadius.All(theme.BorderRadius)
                                    ),
                                    padding: EdgeInsets.All(theme.Spacing * 2f)
                                )
                                {
                                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                                    {
                                        Children = new Element[]
                                        {
                                            new Text("Primary color sample", new TextStyle(
                                                Color: theme.PrimaryColor,
                                                Bold: true,
                                                FontSize: theme.FontSizeLarge
                                            )),
                                            SizedBox.FromSize(height: 6f),
                                            new Text("Secondary text sample", new TextStyle(
                                                Color: theme.SecondaryTextColor,
                                                FontSize: theme.FontSize
                                            )),
                                            SizedBox.FromSize(height: theme.Spacing),
                                            new Button(label: "Primary Action"),
                                        }
                                    }
                                }
                            }
                        }
                    };
                }
            }
        }
    }
}
