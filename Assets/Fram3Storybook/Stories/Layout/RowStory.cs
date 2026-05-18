using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Layout
{
    public sealed class RowStory : StatefulElement
    {
        public override State CreateState() => new RowStoryState();

        private sealed class RowStoryState : State<RowStory>
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
                            content: StoryHelpers.HalfWidth(BuildGame(theme)),
                            theme
                        )
                    }
                };
            }

            private static Element BuildBasic(Theme theme)
            {
                return new Row(
                    mainAxisAlignment: MainAxisAlignment.SpaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.Center
                )
                {
                    Children = new Element[]
                    {
                        new Container(
                            decoration: new BoxDecoration(
                                Color: theme.PrimaryColor
                            ),
                            width: 60f,
                            height: 60f
                        ),
                        new Container(
                            decoration: new BoxDecoration(
                                Color: theme.SecondaryColor
                            ),
                            width: 60f,
                            height: 60f
                        ),
                        new Container(
                            decoration: new BoxDecoration(
                                Color: theme.ErrorColor
                            ),
                            width: 60f,
                            height: 60f
                        )
                    }
                };
            }

            private static Element BuildGame(Theme theme)
            {
                return InventoryItemRow(
                    itemName: "Shadowblade",
                    itemType: "Sword",
                    level: 42,
                    rarityColor: FrameColor.FromHex("#B04AFF"),
                    theme: theme
                );
            }

            private static Element InventoryItemRow(
                string itemName,
                string itemType,
                int level,
                FrameColor rarityColor,
                Theme theme
            )
            {
                return new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                {
                    Children = new Element[]
                    {
                        new Container(
                            width: 48f, height: 48f,
                            decoration: new BoxDecoration(
                                Color: rarityColor.WithAlpha(0.2f),
                                Border: new Border(
                                    Color: rarityColor,
                                    Width: 2f
                                ),
                                BorderRadius: BorderRadius.All(6f)
                            )
                        )
                        {
                            Child = new Center
                            {
                                Child = new Text(
                                    "S",
                                    style: new TextStyle(
                                        Bold: true,
                                        FontSize: theme.FontSizeLarge,
                                        Color: rarityColor
                                    )
                                )
                            }
                        },
                        SizedBox.FromSize(width: theme.Spacing),
                        new Expanded
                        {
                            Child = new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                            {
                                Children = new Element[]
                                {
                                    new Text(
                                        itemName,
                                        style: new TextStyle(
                                            Bold: true,
                                            FontSize: theme.FontSize,
                                            Color: theme.PrimaryTextColor
                                        )
                                    ),
                                    new Text(
                                        itemType,
                                        style: new TextStyle(
                                            FontSize: theme.FontSizeSmall,
                                            Color: theme.SecondaryTextColor
                                        )
                                    )
                                }
                            }
                        },
                        new Container(
                            decoration: new BoxDecoration(
                                Color: theme.SecondaryColor.WithAlpha(0.15f),
                                BorderRadius: BorderRadius.All(4f),
                                Border: new Border(
                                    Color: theme.SecondaryColor.WithAlpha(0.4f),
                                    Width: 1f
                                )
                            ),
                            padding: EdgeInsets.Symmetric(horizontal: 8f, vertical: 4f)
                        )
                        {
                            Child = new Text(
                                $"Lv {level}",
                                style: new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Bold: true,
                                    Color: theme.SecondaryColor
                                )
                            )
                        }
                    }
                };
            }
        }
    }
}