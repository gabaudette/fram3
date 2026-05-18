using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Theming
{
    public sealed class ColorPaletteStory : StatefulElement
    {
        public override State CreateState() => new ColorPaletteState();

        private sealed class ColorPaletteState : State<ColorPaletteStory>
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
                    ("DisabledTextColor", theme.DisabledTextColor)
                };

                var rows = new List<Element>();
                foreach (var (label, color) in swatches)
                {
                    rows.Add(
                        BuildSwatch(
                            label,
                            color,
                            theme
                        )
                    );
                    rows.Add(SizedBox.FromSize(height: 8f));
                }

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = rows.ToArray()
                };
            }

            private static string GetColorHexString(FrameColor color)
            {
                var r = (int)(color.R * 255f);
                var g = (int)(color.G * 255f);
                var b = (int)(color.B * 255f);

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
                                Border: new Border(
                                    Color: theme.PrimaryTextColor.WithAlpha(0.1f),
                                    Width: 1f
                                )
                            )
                        ),
                        SizedBox.FromSize(width: 12f),
                        new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                        {
                            Children = new Element[]
                            {
                                new Text(
                                    label,
                                    style: new TextStyle(
                                        FontSize: theme.FontSize,
                                        Bold: true,
                                        Color: theme.PrimaryTextColor
                                    )
                                ),
                                new Text(
                                    GetColorHexString(color),
                                    style: new TextStyle(
                                        FontSize: theme.FontSizeSmall,
                                        Color: theme.SecondaryTextColor
                                    )
                                )
                            }
                        }
                    }
                };
            }
        }
    }
}