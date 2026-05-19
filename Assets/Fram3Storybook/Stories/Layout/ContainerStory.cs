using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Layout
{
    public sealed class ContainerStory : StatefulElement
    {
        public override State CreateState() => new ContainerStoryState();

        private sealed class ContainerStoryState : State<ContainerStory>
        {
            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);
                
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        StoryHelpers.Section(
                            label: "Basic",
                            content: BuildBasic(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Game Example",
                            content: BuildGame(theme),
                            theme
                        )
                    }
                };
            }

            private static Element BuildBasic(Theme theme)
            {
                return new Container(
                    decoration: new BoxDecoration(
                        Color: theme.PrimaryColor.WithAlpha(0.1f),
                        Border: new Border(theme.PrimaryColor, 2f),
                        BorderRadius: BorderRadius.All(theme.BorderRadius),
                        Shadow: new Shadow(
                            FrameColor.Black.WithAlpha(0.2f),
                            OffsetX: 2f, OffsetY: 2f,
                            BlurRadius: 8f
                        )
                    ),
                    padding: EdgeInsets.All(16f)
                )
                {
                    Child = new Text(
                        "Container with decoration, border, radius, and shadow.",
                        style: new TextStyle(
                            Color: theme.PrimaryTextColor
                        )
                    )
                };
            }

            private static Element BuildGame(Theme theme)
            {
                var rarityColor = FrameColor.FromHex("#FF8C00");

                return new Container(
                    decoration: new BoxDecoration(
                        Color: theme.SurfaceColor,
                        BorderRadius: BorderRadius.All(theme.BorderRadius),
                        Border: new Border(rarityColor, 1f)
                    ),
                    width: 220f,
                    padding: EdgeInsets.All(theme.Spacing * 1.5f)
                )
                {
                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                            {
                                Children = new Element[]
                                {
                                    new Text(
                                        "Emberfall Gauntlets",
                                        style: new TextStyle(
                                            Bold: true,
                                            FontSize: theme.FontSize,
                                            Color: rarityColor
                                        )
                                    )
                                }
                            },
                            new Text(
                                "Epic Gloves",
                                style: new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Color: theme.SecondaryTextColor
                                )
                            ),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Text(
                                "+48 Armor",
                                style: new TextStyle(
                                    Color: theme.PrimaryTextColor,
                                    FontSize: theme.FontSizeSmall
                                )
                            ),
                            new Text(
                                "+12% Fire Resist",
                                style: new TextStyle(
                                    Color: FrameColor.FromHex("#FF6B35"),
                                    FontSize: theme.FontSizeSmall
                                )
                            )
                        }
                    }
                };
            }
        }
    }
}