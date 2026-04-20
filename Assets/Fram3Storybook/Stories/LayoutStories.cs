#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories
{
    /// <summary>Stories for the Layout chapter.</summary>
    public static class LayoutStories
    {
        /// <summary>Returns all layout stories.</summary>
        public static IReadOnlyList<Story> All()
        {
            return new Story[]
            {
                new Story("Column",
                    "Arranges children vertically in a single column, with configurable main-axis and cross-axis alignment.",
                    () => new ColumnStory()),
                new Story("Row",
                    "Arranges children horizontally in a single row, with configurable main-axis and cross-axis alignment.",
                    () => new RowStory()),
                new Story("Stack",
                    "Layers children on top of each other using absolute positioning, useful for overlapping elements.",
                    () => new StackStory()),
                new Story("Wrap",
                    "Flows children left-to-right and wraps onto new lines when the available width is exhausted.",
                    () => new WrapStory()),
                new Story("Padding",
                    "Inserts empty space between a single child and its parent boundaries using per-side insets.",
                    () => new PaddingStory()),
                new Story("Margin", "Adds outer spacing around a single child, pushing surrounding siblings away.",
                    () => new MarginStory()),
                new Story("SizedBox",
                    "Occupies a fixed width and/or height; can hold an optional child or act as a gap element between siblings.",
                    () => new SizedBoxStory()),
                new Story("Center", "Centers its child both horizontally and vertically within the available space.",
                    () => new CenterStory()),
                new Story("Expanded",
                    "Fills all remaining space along the parent axis, optionally weighted by a flex factor.",
                    () => new ExpandedStory()),
                new Story("Container",
                    "A versatile single-child box that combines a background decoration, explicit sizing, and inner padding in one element.",
                    () => new ContainerStory()),
                new Story("Divider",
                    "Renders a thin horizontal or vertical rule, useful as a visual separator between sections.",
                    () => new DividerStory()),
                new Story("ScrollView",
                    "Makes its child scrollable along one axis when the content exceeds the available viewport size.",
                    () => new ScrollViewStory()),
            };
        }

        // ---------------------------------------------------------------------------
        // Column
        // ---------------------------------------------------------------------------

        private sealed class ColumnStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new ColumnStoryState();

            private sealed class ColumnStoryState : Fram3.UI.Core.State<ColumnStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(mainAxisAlignment: MainAxisAlignment.Start,
                        crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Container(
                                decoration: new BoxDecoration(Color: theme.PrimaryColor.WithAlpha(0.15f)),
                                padding: EdgeInsets.All(8f)
                            ) { Child = new Text("First item", new TextStyle(Color: theme.PrimaryTextColor)) },
                            SizedBox.FromSize(height: 4f),
                            new Container(
                                decoration: new BoxDecoration(Color: theme.SecondaryColor.WithAlpha(0.15f)),
                                padding: EdgeInsets.All(8f)
                            ) { Child = new Text("Second item", new TextStyle(Color: theme.PrimaryTextColor)) },
                            SizedBox.FromSize(height: 4f),
                            new Container(
                                decoration: new BoxDecoration(Color: theme.ErrorColor.WithAlpha(0.15f)),
                                padding: EdgeInsets.All(8f)
                            ) { Child = new Text("Third item", new TextStyle(Color: theme.PrimaryTextColor)) },
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("PLAYER STATS", new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )),
                            SizedBox.FromSize(height: theme.Spacing),
                            StatRow("Health", 87, 100, FrameColor.FromHex("#FF6B6B"), theme),
                            SizedBox.FromSize(height: 6f),
                            StatRow("Mana", 42, 80, FrameColor.FromHex("#7B61FF"), theme),
                            SizedBox.FromSize(height: 6f),
                            StatRow("Stamina", 65, 100, FrameColor.FromHex("#00D4AA"), theme),
                        }
                    };
                }

                private static Element StatRow(string label, int value, int max, FrameColor color, Theme theme)
                {
                    return new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = new Element[]
                        {
                            new Container(width: 64f)
                            {
                                Child = new Text(label, new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Color: theme.SecondaryTextColor
                                ))
                            },
                            new Expanded
                            {
                                Child = new Container(
                                    height: 8f,
                                    decoration: new BoxDecoration(
                                        Color: color.WithAlpha(0.2f),
                                        BorderRadius: BorderRadius.All(4f)
                                    )
                                )
                                {
                                    Child = new Container(
                                        height: 8f,
                                        width: 160f * value / max,
                                        decoration: new BoxDecoration(
                                            Color: color,
                                            BorderRadius: BorderRadius.All(4f)
                                        )
                                    )
                                }
                            },
                            SizedBox.FromSize(width: 8f),
                            new Text($"{value}/{max}", new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor
                            )),
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // Row
        // ---------------------------------------------------------------------------

        private sealed class RowStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new RowStoryState();

            private sealed class RowStoryState : Fram3.UI.Core.State<RowStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme),
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
                            new Container(decoration: new BoxDecoration(Color: theme.PrimaryColor), width: 60f, height: 60f),
                            new Container(decoration: new BoxDecoration(Color: theme.SecondaryColor), width: 60f, height: 60f),
                            new Container(decoration: new BoxDecoration(Color: theme.ErrorColor), width: 60f, height: 60f),
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

                private static Element InventoryItemRow(string itemName, string itemType, int level,
                    FrameColor rarityColor, Theme theme)
                {
                    return new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = new Element[]
                        {
                            new Container(
                                width: 48f, height: 48f,
                                decoration: new BoxDecoration(
                                    Color: rarityColor.WithAlpha(0.2f),
                                    Border: new Border(rarityColor, 2f),
                                    BorderRadius: BorderRadius.All(6f)
                                )
                            )
                            {
                                Child = new Center
                                {
                                    Child = new Text("S", new TextStyle(
                                        Bold: true,
                                        FontSize: theme.FontSizeLarge,
                                        Color: rarityColor
                                    ))
                                }
                            },
                            SizedBox.FromSize(width: theme.Spacing),
                            new Expanded
                            {
                                Child = new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                                {
                                    Children = new Element[]
                                    {
                                        new Text(itemName, new TextStyle(
                                            Bold: true,
                                            FontSize: theme.FontSize,
                                            Color: theme.PrimaryTextColor
                                        )),
                                        new Text(itemType, new TextStyle(
                                            FontSize: theme.FontSizeSmall,
                                            Color: theme.SecondaryTextColor
                                        )),
                                    }
                                }
                            },
                            new Container(
                                decoration: new BoxDecoration(
                                    Color: theme.SecondaryColor.WithAlpha(0.15f),
                                    BorderRadius: BorderRadius.All(4f),
                                    Border: new Border(theme.SecondaryColor.WithAlpha(0.4f), 1f)
                                ),
                                padding: EdgeInsets.Symmetric(horizontal: 8f, vertical: 4f)
                            )
                            {
                                Child = new Text($"Lv {level}", new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Bold: true,
                                    Color: theme.SecondaryColor
                                ))
                            },
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // Stack
        // ---------------------------------------------------------------------------

        private sealed class StackStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new StackStoryState();

            private sealed class StackStoryState : Fram3.UI.Core.State<StackStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Container(width: 140f, height: 140f)
                    {
                        Child = new Stack
                        {
                            Children = new Element[]
                            {
                                new Container(decoration: new BoxDecoration(Color: theme.PrimaryColor.WithAlpha(0.4f)), width: 120f, height: 120f),
                                new Container(decoration: new BoxDecoration(Color: theme.SecondaryColor.WithAlpha(0.5f)), width: 80f, height: 80f),
                                new Container(
                                    decoration: new BoxDecoration(
                                        Color: FrameColor.Black.WithAlpha(0.6f),
                                        BorderRadius: BorderRadius.All(2f)
                                    ),
                                    padding: EdgeInsets.Symmetric(vertical: 2f, horizontal: 6f)
                                )
                                {
                                    Child = new Text("Stacked", new TextStyle(Color: FrameColor.White, FontSize: 10f))
                                },
                            }
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    var mapBg = FrameColor.FromHex("#0A1628");
                    var pingColor = FrameColor.FromHex("#FF6B6B");
                    var playerColor = FrameColor.FromHex("#00D4AA");

                    return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                    {
                        Children = new Element[]
                        {
                            new Text("Minimap overlay", new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor
                            )),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Container(
                                width: 160f, height: 160f,
                                decoration: new BoxDecoration(
                                    Color: mapBg,
                                    BorderRadius: BorderRadius.All(8f),
                                    Border: new Border(theme.SecondaryTextColor.WithAlpha(0.3f), 1f)
                                )
                            )
                            {
                                Child = new Stack
                                {
                                    Children = new Element[]
                                    {
                                        new Container(
                                            decoration: new BoxDecoration(
                                                Color: FrameColor.FromHex("#0D2040").WithAlpha(0.8f)
                                            ),
                                            width: 160f, height: 160f
                                        ),
                                        new Container(
                                            width: 10f, height: 10f,
                                            decoration: new BoxDecoration(Color: pingColor, BorderRadius: BorderRadius.All(5f))
                                        ),
                                        new Container(
                                            width: 8f, height: 8f,
                                            decoration: new BoxDecoration(Color: playerColor, BorderRadius: BorderRadius.All(4f))
                                        ),
                                        new Container(
                                            padding: EdgeInsets.Symmetric(horizontal: 4f, vertical: 2f),
                                            decoration: new BoxDecoration(
                                                Color: FrameColor.Black.WithAlpha(0.7f),
                                                BorderRadius: BorderRadius.All(3f)
                                            )
                                        )
                                        {
                                            Child = new Text("YOU", new TextStyle(
                                                FontSize: 9f,
                                                Bold: true,
                                                Color: playerColor
                                            ))
                                        },
                                    }
                                }
                            },
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // Wrap
        // ---------------------------------------------------------------------------

        private sealed class WrapStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new WrapStoryState();

            private sealed class WrapStoryState : Fram3.UI.Core.State<WrapStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Wrap
                    {
                        Children = new Element[]
                        {
                            Tag("Alpha", theme.PrimaryColor, theme),
                            Tag("Beta", theme.SecondaryColor, theme),
                            Tag("Gamma", theme.ErrorColor, theme),
                            Tag("Delta", theme.PrimaryColor.WithAlpha(0.5f), theme),
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    var classes = new (string name, FrameColor color)[]
                    {
                        ("Warrior", FrameColor.FromHex("#FF6B6B")),
                        ("Mage", FrameColor.FromHex("#7B61FF")),
                        ("Rogue", FrameColor.FromHex("#2E3347").WithAlpha(0f)),
                        ("Healer", FrameColor.FromHex("#00D4AA")),
                        ("Ranger", FrameColor.FromHex("#FFB347")),
                        ("Paladin", FrameColor.FromHex("#FFD700")),
                        ("Berserker", FrameColor.FromHex("#FF4444")),
                    };

                    var tags = new List<Element>();
                    foreach (var (name, color) in classes)
                    {
                        tags.Add(Tag(name, color, theme));
                    }

                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("Active class filters", new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor
                            )),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Wrap { Children = tags.ToArray() },
                        }
                    };
                }

                private static Element Tag(string label, FrameColor color, Theme theme)
                {
                    return new Margin(new EdgeInsets(0f, 6f, 6f, 0f),
                        new Container(
                            decoration: new BoxDecoration(
                                Color: color.WithAlpha(0.15f),
                                Border: new Border(color.WithAlpha(0.4f), 1f),
                                BorderRadius: BorderRadius.All(theme.BorderRadius)
                            ),
                            padding: EdgeInsets.Symmetric(horizontal: 10f, vertical: 4f)
                        )
                        {
                            Child = new Text(label, new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: color
                            ))
                        }
                    );
                }
            }
        }

        // ---------------------------------------------------------------------------
        // Padding
        // ---------------------------------------------------------------------------

        private sealed class PaddingStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new PaddingStoryState();

            private sealed class PaddingStoryState : Fram3.UI.Core.State<PaddingStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Container(
                        decoration: new BoxDecoration(
                            Color: theme.PrimaryColor.WithAlpha(0.08f),
                            Border: new Border(theme.PrimaryColor.WithAlpha(0.3f), 1f),
                            BorderRadius: BorderRadius.All(theme.BorderRadius)
                        )
                    )
                    {
                        Child = new Padding(EdgeInsets.All(24f))
                        {
                            Child = new Text("This text has 24px padding on all sides.",
                                new TextStyle(Color: theme.PrimaryTextColor))
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    return new Container(
                        decoration: new BoxDecoration(
                            Color: theme.SurfaceColor,
                            BorderRadius: BorderRadius.All(theme.BorderRadius),
                            Border: new Border(theme.SecondaryTextColor.WithAlpha(0.2f), 1f)
                        )
                    )
                    {
                        Child = new Padding(EdgeInsets.All(theme.Spacing * 2f))
                        {
                            Child = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                            {
                                Children = new Element[]
                                {
                                    new Container(
                                        width: 40f, height: 40f,
                                        decoration: new BoxDecoration(
                                            Color: FrameColor.FromHex("#FFD700").WithAlpha(0.2f),
                                            BorderRadius: BorderRadius.All(6f),
                                            Border: new Border(FrameColor.FromHex("#FFD700"), 1f)
                                        )
                                    )
                                    {
                                        Child = new Center
                                        {
                                            Child = new Text("G", new TextStyle(
                                                Bold: true, Color: FrameColor.FromHex("#FFD700")
                                            ))
                                        }
                                    },
                                    SizedBox.FromSize(width: theme.Spacing),
                                    new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                                    {
                                        Children = new Element[]
                                        {
                                            new Text("Gold Chest", new TextStyle(
                                                Bold: true, Color: theme.PrimaryTextColor
                                            )),
                                            new Text("Padded card layout",
                                                new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                                        }
                                    },
                                }
                            }
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // Margin
        // ---------------------------------------------------------------------------

        private sealed class MarginStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new MarginStoryState();

            private sealed class MarginStoryState : Fram3.UI.Core.State<MarginStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Container(
                                decoration: new BoxDecoration(Color: theme.PrimaryColor.WithAlpha(0.2f)),
                                height: 40f
                            ) { Child = new Text("Block A", new TextStyle(Color: theme.PrimaryTextColor)) },
                            new Margin(EdgeInsets.Symmetric(vertical: 16f, horizontal: 0f),
                                new Container(
                                    decoration: new BoxDecoration(Color: theme.SecondaryColor.WithAlpha(0.2f)),
                                    height: 40f
                                ) { Child = new Text("Block B (16px vertical margin)", new TextStyle(Color: theme.PrimaryTextColor)) }
                            ),
                            new Container(
                                decoration: new BoxDecoration(Color: theme.ErrorColor.WithAlpha(0.2f)),
                                height: 40f
                            ) { Child = new Text("Block C", new TextStyle(Color: theme.PrimaryTextColor)) },
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("HUD section spacing", new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor
                            )),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Container(
                                decoration: new BoxDecoration(
                                    Color: theme.PrimaryColor.WithAlpha(0.1f),
                                    BorderRadius: BorderRadius.All(6f)
                                ),
                                padding: EdgeInsets.All(theme.Spacing)
                            ) { Child = new Text("Kills: 12", new TextStyle(Color: theme.PrimaryTextColor)) },
                            new Margin(EdgeInsets.Symmetric(vertical: theme.Spacing * 1.5f, horizontal: 0f),
                                new Container(
                                    decoration: new BoxDecoration(
                                        Color: theme.SecondaryColor.WithAlpha(0.1f),
                                        BorderRadius: BorderRadius.All(6f)
                                    ),
                                    padding: EdgeInsets.All(theme.Spacing)
                                ) { Child = new Text("Assists: 5", new TextStyle(Color: theme.PrimaryTextColor)) }
                            ),
                            new Container(
                                decoration: new BoxDecoration(
                                    Color: theme.ErrorColor.WithAlpha(0.1f),
                                    BorderRadius: BorderRadius.All(6f)
                                ),
                                padding: EdgeInsets.All(theme.Spacing)
                            ) { Child = new Text("Deaths: 2", new TextStyle(Color: theme.PrimaryTextColor)) },
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // SizedBox
        // ---------------------------------------------------------------------------

        private sealed class SizedBoxStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new SizedBoxStoryState();

            private sealed class SizedBoxStoryState : Fram3.UI.Core.State<SizedBoxStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("200x40 fixed spacer between A and B:", new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                            new Container(decoration: new BoxDecoration(Color: theme.SurfaceColor))
                            {
                                Child = new Row
                                {
                                    Children = new Element[]
                                    {
                                        new Container(decoration: new BoxDecoration(Color: theme.PrimaryColor.WithAlpha(0.3f)), height: 20f)
                                        { Child = new Text("A", new TextStyle(Color: theme.PrimaryTextColor)) },
                                        SizedBox.FromSize(width: 200f, height: 40f),
                                        new Container(decoration: new BoxDecoration(Color: theme.PrimaryColor.WithAlpha(0.3f)), height: 20f)
                                        { Child = new Text("B", new TextStyle(Color: theme.PrimaryTextColor)) },
                                    }
                                }
                            },
                            SizedBox.FromSize(height: theme.Spacing * 2f),
                            new Text("SizedBox.Expand() fills remaining space:", new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                            new Container(
                                decoration: new BoxDecoration(Color: theme.SurfaceColor),
                                height: 40f
                            )
                            {
                                Child = new Row
                                {
                                    Children = new Element[]
                                    {
                                        new Text("Left", new TextStyle(Color: theme.PrimaryTextColor)),
                                        SizedBox.Expand(),
                                        new Text("Right", new TextStyle(Color: theme.PrimaryTextColor)),
                                    }
                                }
                            },
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("XP bar with fixed icon + expanded fill", new TextStyle(
                                FontSize: theme.FontSizeSmall, Color: theme.SecondaryTextColor
                            )),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                            {
                                Children = new Element[]
                                {
                                    new Text("XP", new TextStyle(
                                        FontSize: theme.FontSizeSmall,
                                        Bold: true,
                                        Color: FrameColor.FromHex("#FFD700")
                                    )),
                                    SizedBox.FromSize(width: theme.Spacing),
                                    new Expanded
                                    {
                                        Child = new Container(
                                            height: 10f,
                                            decoration: new BoxDecoration(
                                                Color: FrameColor.FromHex("#FFD700").WithAlpha(0.15f),
                                                BorderRadius: BorderRadius.All(5f)
                                            )
                                        )
                                        {
                                            Child = new Container(
                                                height: 10f,
                                                width: 120f,
                                                decoration: new BoxDecoration(
                                                    Color: FrameColor.FromHex("#FFD700"),
                                                    BorderRadius: BorderRadius.All(5f)
                                                )
                                            )
                                        }
                                    },
                                    SizedBox.FromSize(width: theme.Spacing),
                                    new Text("Lv 12", new TextStyle(
                                        FontSize: theme.FontSizeSmall,
                                        Bold: true,
                                        Color: theme.PrimaryTextColor
                                    )),
                                }
                            },
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // Center
        // ---------------------------------------------------------------------------

        private sealed class CenterStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new CenterStoryState();

            private sealed class CenterStoryState : Fram3.UI.Core.State<CenterStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Container(
                        decoration: new BoxDecoration(
                            Color: theme.PrimaryColor.WithAlpha(0.08f),
                            BorderRadius: BorderRadius.All(theme.BorderRadius)
                        ),
                        width: 300f, height: 120f
                    )
                    {
                        Child = new Center
                        {
                            Child = new Text("Centered content", new TextStyle(Color: theme.PrimaryTextColor))
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    return new Container(
                        decoration: new BoxDecoration(
                            Color: theme.SurfaceColor,
                            BorderRadius: BorderRadius.All(theme.BorderRadius),
                            Border: new Border(theme.SecondaryTextColor.WithAlpha(0.2f), 1f)
                        ),
                        width: 300f, height: 120f
                    )
                    {
                        Child = new Center
                        {
                            Child = new Column(crossAxisAlignment: CrossAxisAlignment.Center)
                            {
                                Children = new Element[]
                                {
                                    new Text("ROUND OVER", new TextStyle(
                                        FontSize: theme.FontSizeLarge,
                                        Bold: true,
                                        Color: theme.ErrorColor
                                    )),
                                    new Text("Respawning in 5s", new TextStyle(
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

        // ---------------------------------------------------------------------------
        // Expanded
        // ---------------------------------------------------------------------------

        private sealed class ExpandedStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new ExpandedStoryState();

            private sealed class ExpandedStoryState : Fram3.UI.Core.State<ExpandedStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Container(height: 60f)
                    {
                        Child = new Row
                        {
                            Children = new Element[]
                            {
                                new Container(
                                    decoration: new BoxDecoration(Color: theme.PrimaryColor.WithAlpha(0.3f)),
                                    width: 80f
                                )
                                { Child = new Text("Fixed", new TextStyle(Color: theme.PrimaryTextColor)) },
                                new Expanded
                                {
                                    Child = new Container(
                                        decoration: new BoxDecoration(Color: theme.SecondaryColor.WithAlpha(0.3f))
                                    )
                                    { Child = new Text("Expanded", new TextStyle(Color: theme.PrimaryTextColor)) }
                                },
                            }
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("XP Progress", new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor
                            )),
                            SizedBox.FromSize(height: 6f),
                            new Container(height: 32f)
                            {
                                Child = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                                {
                                    Children = new Element[]
                                    {
                                        new Text("Lv 11", new TextStyle(
                                            FontSize: theme.FontSizeSmall,
                                            Color: theme.SecondaryTextColor
                                        )),
                                        SizedBox.FromSize(width: theme.Spacing),
                                        new Expanded
                                        {
                                            Child = new Container(
                                                height: 12f,
                                                decoration: new BoxDecoration(
                                                    Color: FrameColor.FromHex("#FFD700").WithAlpha(0.15f),
                                                    BorderRadius: BorderRadius.All(6f)
                                                )
                                            )
                                            {
                                                Child = new Container(
                                                    height: 12f, width: 100f,
                                                    decoration: new BoxDecoration(
                                                        Color: FrameColor.FromHex("#FFD700"),
                                                        BorderRadius: BorderRadius.All(6f)
                                                    )
                                                )
                                            }
                                        },
                                        SizedBox.FromSize(width: theme.Spacing),
                                        new Text("Lv 12", new TextStyle(
                                            FontSize: theme.FontSizeSmall,
                                            Color: theme.PrimaryTextColor
                                        )),
                                    }
                                }
                            },
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // Container
        // ---------------------------------------------------------------------------

        private sealed class ContainerStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new ContainerStoryState();

            private sealed class ContainerStoryState : Fram3.UI.Core.State<ContainerStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme),
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
                            Shadow: new Shadow(FrameColor.Black.WithAlpha(0.2f), OffsetX: 2f, OffsetY: 2f, BlurRadius: 8f)
                        ),
                        padding: EdgeInsets.All(16f),
                        width: 280f
                    )
                    {
                        Child = new Text("Container with decoration, border, radius, and shadow.",
                            new TextStyle(Color: theme.PrimaryTextColor))
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
                                        new Text("Emberfall Gauntlets", new TextStyle(
                                            Bold: true,
                                            FontSize: theme.FontSize,
                                            Color: rarityColor
                                        )),
                                    }
                                },
                                new Text("Epic Gloves", new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Color: theme.SecondaryTextColor
                                )),
                                SizedBox.FromSize(height: theme.Spacing),
                                new Text("+48 Armor", new TextStyle(Color: theme.PrimaryTextColor, FontSize: theme.FontSizeSmall)),
                                new Text("+12% Fire Resist", new TextStyle(Color: FrameColor.FromHex("#FF6B35"), FontSize: theme.FontSizeSmall)),
                            }
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // Divider
        // ---------------------------------------------------------------------------

        private sealed class DividerStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new DividerStoryState();

            private sealed class DividerStoryState : Fram3.UI.Core.State<DividerStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("Above divider", new TextStyle(Color: theme.PrimaryTextColor)),
                            new Divider(color: theme.SecondaryTextColor.WithAlpha(0.3f)),
                            new Text("Below divider", new TextStyle(Color: theme.PrimaryTextColor)),
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("INVENTORY", new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )),
                            SizedBox.FromSize(height: 4f),
                            new Divider(color: theme.SecondaryColor.WithAlpha(0.4f)),
                            SizedBox.FromSize(height: 4f),
                            new Text("Shadowblade x1", new TextStyle(Color: theme.PrimaryTextColor)),
                            new Divider(color: theme.SecondaryTextColor.WithAlpha(0.15f), indent: 8f),
                            new Text("Health Potion x5", new TextStyle(Color: theme.PrimaryTextColor)),
                            new Divider(color: theme.SecondaryTextColor.WithAlpha(0.15f), indent: 8f),
                            new Text("Emberfall Gauntlets x1", new TextStyle(Color: theme.PrimaryTextColor)),
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // ScrollView
        // ---------------------------------------------------------------------------

        private sealed class ScrollViewStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new ScrollViewStoryState();

            private sealed class ScrollViewStoryState : Fram3.UI.Core.State<ScrollViewStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme),
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    var items = new Element[12];
                    for (var i = 0; i < 12; i++)
                    {
                        var label = $"Scrollable item {i + 1}";
                        items[i] = new Padding(EdgeInsets.Symmetric(vertical: 4f, horizontal: 0f))
                        {
                            Child = new Text(label, new TextStyle(Color: theme.PrimaryTextColor))
                        };
                    }

                    return new Container(height: 160f)
                    {
                        Child = new ScrollView()
                        {
                            Child = new Column { Children = items }
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    var logs = new (string text, FrameColor color)[]
                    {
                        ("Aric Stormblade slew a Goblin.", FrameColor.FromHex("#00D4AA")),
                        ("Lyria Moonwhisper cast Fireball.", FrameColor.FromHex("#FF8C00")),
                        ("Drex Ironfoot picked up Gold x50.", FrameColor.FromHex("#FFD700")),
                        ("Vex Nightfall used Smoke Bomb.", theme.SecondaryTextColor),
                        ("Seraphine Vale healed Aric for 80 HP.", FrameColor.FromHex("#00D4AA")),
                        ("Korroth was defeated by a Troll.", FrameColor.FromHex("#FF6B6B")),
                        ("Nira Emberveil found a Rare Helm.", FrameColor.FromHex("#B04AFF")),
                        ("Theron Ashwood gained 200 XP.", FrameColor.FromHex("#FFD700")),
                    };

                    var entries = new List<Element>();
                    foreach (var (text, color) in logs)
                    {
                        entries.Add(new Padding(EdgeInsets.Symmetric(vertical: 2f, horizontal: 0f))
                        {
                            Child = new Text(text, new TextStyle(FontSize: theme.FontSizeSmall, Color: color))
                        });
                    }

                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("COMBAT LOG", new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Container(height: 140f)
                            {
                                Child = new ScrollView()
                                {
                                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                                    {
                                        Children = entries.ToArray()
                                    }
                                }
                            },
                        }
                    };
                }
            }
        }
    }
}
