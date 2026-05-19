using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Layout
{
    public sealed class GridStory : StatefulElement
    {
        public override State CreateState() => new GridStoryState();

        private sealed class GridStoryState : State<GridStory>
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
                            content: StoryHelpers.HalfWidth(BuildBasic(theme)),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Game Example",
                            content: BuildInventory(theme),
                            theme
                        )
                    }
                };
            }

            private static Element BuildBasic(Theme theme)
            {
                var colors = new[]
                {
                    FrameColor.FromHex("#FF6B6B"),
                    FrameColor.FromHex("#FFD700"),
                    FrameColor.FromHex("#00D4AA"),
                    FrameColor.FromHex("#7B61FF"),
                    FrameColor.FromHex("#FF8C00"),
                    FrameColor.FromHex("#4FC3F7")
                };

                return new Grid<FrameColor>(
                    columnCount: 3,
                    items: colors,
                    itemBuilder: color => new Container(
                        decoration: new BoxDecoration(
                            Color: color,
                            BorderRadius: BorderRadius.All(theme.BorderRadius)
                        ),
                        height: 48f,
                        padding: EdgeInsets.All(theme.Spacing))
                    {
                        Child = new Center
                        {
                            Child = new Text(
                                $"#{(int)(color.R * 255):X2}" +
                                $"{(int)(color.G * 255):X2}" +
                                $"{(int)(color.B * 255):X2}",
                                style: new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Color: FrameColor.White,
                                    Bold: true
                                )
                            )
                        }
                    },
                    columnSpacing: theme.Spacing,
                    rowSpacing: theme.Spacing
                );
            }

            private static Element BuildInventory(Theme theme)
            {
                var items = new[]
                {
                    ("Shadowblade", "Epic", "#B04AFF"),
                    ("Iron Shield", "Common", "#9E9E9E"),
                    ("Fireball Tome", "Rare", "#2196F3"),
                    ("Ember Gauntlets", "Epic", "#B04AFF"),
                    ("Healing Potion", "Common", "#9E9E9E"),
                    ("Storm Bow", "Legendary", "#FFD700"),
                    ("Shadow Cloak", "Rare", "#2196F3"),
                    ("Troll Bone", "Common", "#9E9E9E")
                };

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new Text(
                            "INVENTORY",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Grid<(string Name, string Rarity, string RarityHex)>(
                            columnCount: 4,
                            items: items,
                            itemBuilder: item =>
                            {
                                var rarityColor = FrameColor.FromHex(item.RarityHex);
                                return new Container(
                                    decoration: new BoxDecoration(
                                        Color: theme.SurfaceColor,
                                        BorderRadius: BorderRadius.All(theme.BorderRadius),
                                        Border: new Border(rarityColor, 1.5f)
                                    ),
                                    padding: EdgeInsets.All(theme.Spacing))
                                {
                                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Center)
                                    {
                                        Children = new Element[]
                                        {
                                            new Container(
                                                decoration: new BoxDecoration(
                                                    Color: rarityColor.WithAlpha(0.15f),
                                                    BorderRadius: BorderRadius.All(theme.BorderRadius * 0.5f)
                                                ),
                                                width: 36f,
                                                height: 36f
                                            )
                                            {
                                                Child = new Center
                                                {
                                                    Child = new Text(
                                                        "[ ]",
                                                        style: new TextStyle(
                                                            FontSize: theme.FontSizeSmall,
                                                            Color: rarityColor
                                                        )
                                                    )
                                                }
                                            },
                                            SizedBox.FromSize(height: theme.Spacing * 0.5f),
                                            new Text(
                                                item.Name,
                                                style: new TextStyle(
                                                    FontSize: theme.FontSizeSmall,
                                                    Bold: true,
                                                    Color: theme.PrimaryTextColor
                                                )
                                            ),
                                            new Text(
                                                item.Rarity,
                                                style: new TextStyle(
                                                    FontSize: theme.FontSizeSmall * 0.85f,
                                                    Color: rarityColor
                                                )
                                            )
                                        }
                                    }
                                };
                            },
                            columnSpacing: theme.Spacing,
                            rowSpacing: theme.Spacing
                        )
                    }
                };
            }
        }
    }
}