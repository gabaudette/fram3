#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
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
                new Story("FColumn",
                    "Arranges children vertically in a single column, with configurable main-axis and cross-axis alignment.",
                    BuildColumn),
                new Story("FRow",
                    "Arranges children horizontally in a single row, with configurable main-axis and cross-axis alignment.",
                    BuildRow),
                new Story("FStack",
                    "Layers children on top of each other using absolute positioning, useful for overlapping elements.",
                    BuildStack),
                new Story("FWrap",
                    "Flows children left-to-right and wraps onto new lines when the available width is exhausted.",
                    BuildWrap),
                new Story("FPadding",
                    "Inserts empty space between a single child and its parent boundaries using per-side insets.",
                    BuildPadding),
                new Story("FMargin", "Adds outer spacing around a single child, pushing surrounding siblings away.",
                    BuildMargin),
                new Story("FSizedBox",
                    "Occupies a fixed width and/or height; can hold an optional child or act as a gap element between siblings.",
                    BuildSizedBox),
                new Story("FCenter", "Centers its child both horizontally and vertically within the available space.",
                    BuildCenter),
                new Story("FExpanded",
                    "Fills all remaining space along the parent axis, optionally weighted by a flex factor.",
                    BuildExpanded),
                new Story("FContainer",
                    "A versatile single-child box that combines a background decoration, explicit sizing, and inner padding in one element.",
                    BuildContainer),
                new Story("FDivider",
                    "Renders a thin horizontal or vertical rule, useful as a visual separator between sections.",
                    BuildDivider),
                new Story("FScrollView",
                    "Makes its child scrollable along one axis when the content exceeds the available viewport size.",
                    BuildScrollView),
            };
        }

        private static FElement BuildColumn()
        {
            return new FColumn(mainAxisAlignment: FMainAxisAlignment.Start,
                crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FContainer(
                        decoration: new FBoxDecoration(Color: FColor.Blue.WithAlpha(0.15f)),
                        padding: FEdgeInsets.All(8f)
                    ) { Child = new FText("First item") },
                    FSizedBox.FromSize(height: 4f),
                    new FContainer(
                        decoration: new FBoxDecoration(Color: FColor.Green.WithAlpha(0.15f)),
                        padding: FEdgeInsets.All(8f)
                    ) { Child = new FText("Second item") },
                    FSizedBox.FromSize(height: 4f),
                    new FContainer(
                        decoration: new FBoxDecoration(Color: FColor.Red.WithAlpha(0.15f)),
                        padding: FEdgeInsets.All(8f)
                    ) { Child = new FText("Third item") },
                }
            };
        }

        private static FElement BuildRow()
        {
            return new FRow(
                mainAxisAlignment: FMainAxisAlignment.SpaceBetween,
                crossAxisAlignment: FCrossAxisAlignment.Center
            )
            {
                Children = new FElement[]
                {
                    new FContainer(decoration: new FBoxDecoration(Color: FColor.Blue), width: 60f, height: 60f),
                    new FContainer(decoration: new FBoxDecoration(Color: FColor.Green), width: 60f, height: 60f),
                    new FContainer(decoration: new FBoxDecoration(Color: FColor.Red), width: 60f, height: 60f),
                }
            };
        }

        private static FElement BuildStack()
        {
            return new FContainer(width: 140f, height: 140f)
            {
                Child = new FStack
                {
                    Children = new FElement[]
                    {
                        new FContainer(decoration: new FBoxDecoration(Color: FColor.Blue), width: 120f, height: 120f),
                        new FContainer(decoration: new FBoxDecoration(Color: FColor.Red.WithAlpha(0.7f)), width: 80f,
                            height: 80f),
                        new FContainer(
                            decoration: new FBoxDecoration(
                                Color: FColor.Black.WithAlpha(0.5f),
                                BorderRadius: FBorderRadius.All(2f)
                            ),
                            padding: FEdgeInsets.Symmetric(vertical: 2f, horizontal: 6f)
                        )
                        {
                            Child = new FText("Stacked label", new FTextStyle(Color: FColor.White))
                        },
                    }
                }
            };
        }

        private static FElement BuildWrap()
        {
            return new FWrap
            {
                Children = new FElement[]
                {
                    WrapTag("Tag A", FColor.Blue),
                    WrapTag("Tag B", FColor.Green),
                    WrapTag("Tag C", FColor.Red),
                    WrapTag("Tag D", FColor.Blue.WithAlpha(0.5f)),
                }
            };
        }

        private static FElement WrapTag(string label, FColor color)
        {
            return new FContainer(
                decoration: new FBoxDecoration(Color: color),
                width: 60f, height: 40f,
                padding: FEdgeInsets.All(4f)
            )
            {
                Child = new FText(label, new FTextStyle(Color: FColor.White))
            };
        }

        private static FElement BuildPadding()
        {
            return new FContainer(decoration: new FBoxDecoration(Color: FColor.FromHex("#EEEEEE")))
            {
                Child = new FPadding(FEdgeInsets.All(24f))
                {
                    Child = new FText("This text has 24px padding on all sides.")
                }
            };
        }

        private static FElement BuildMargin()
        {
            var blockA = new FContainer(
                decoration: new FBoxDecoration(Color: FColor.Blue.WithAlpha(0.2f)),
                height: 40f
            )
            {
                Child = new FText("Block A")
            };

            var blockB = new FContainer(
                decoration: new FBoxDecoration(Color: FColor.Green.WithAlpha(0.2f)),
                height: 40f
            )
            {
                Child = new FText("Block B (16px vertical margin)")
            };

            var blockC = new FContainer(
                decoration: new FBoxDecoration(Color: FColor.Red.WithAlpha(0.2f)),
                height: 40f
            )
            {
                Child = new FText("Block C")
            };

            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    blockA,
                    new FMargin(FEdgeInsets.Symmetric(vertical: 16f, horizontal: 0f), blockB),
                    blockC,
                }
            };
        }

        private static FElement BuildSizedBox()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("FSizedBox as a 200x80 spacer (no child):"),
                    new FContainer(decoration: new FBoxDecoration(Color: FColor.FromHex("#EEEEEE")))
                    {
                        Child = new FRow
                        {
                            Children = new FElement[]
                            {
                                new FContainer(decoration: new FBoxDecoration(Color: FColor.Blue.WithAlpha(0.3f)),
                                    height: 20f)
                                {
                                    Child = new FText("A")
                                },
                                FSizedBox.FromSize(width: 200f, height: 80f),
                                new FContainer(decoration: new FBoxDecoration(Color: FColor.Blue.WithAlpha(0.3f)),
                                    height: 20f)
                                {
                                    Child = new FText("B")
                                },
                            }
                        }
                    },
                    new FText("FSizedBox.Square(60) as a vertical gap:"),
                    new FContainer(decoration: new FBoxDecoration(Color: FColor.FromHex("#EEEEEE")))
                    {
                        Child = new FColumn
                        {
                            Children = new FElement[]
                            {
                                new FText("Above"),
                                FSizedBox.Square(60f),
                                new FText("Below"),
                            }
                        }
                    },
                    new FText("FSizedBox.Expand() filling available width:"),
                    new FContainer(
                        decoration: new FBoxDecoration(Color: FColor.FromHex("#EEEEEE")),
                        height: 40f
                    )
                    {
                        Child = new FRow
                        {
                            Children = new FElement[]
                            {
                                new FText("Left"),
                                FSizedBox.Expand(),
                                new FText("Right"),
                            }
                        }
                    },
                }
            };
        }

        private static FElement BuildCenter()
        {
            return new FContainer(
                decoration: new FBoxDecoration(Color: FColor.FromHex("#F0F0F0")),
                width: 300f, height: 200f
            )
            {
                Child = new FCenter
                {
                    Child = new FText("Centered content")
                }
            };
        }

        private static FElement BuildExpanded()
        {
            return new FContainer(height: 60f)
            {
                Child = new FRow
                {
                    Children = new FElement[]
                    {
                        new FContainer(
                            decoration: new FBoxDecoration(Color: FColor.Blue.WithAlpha(0.3f)),
                            width: 80f
                        )
                        {
                            Child = new FText("Fixed")
                        },
                        new FExpanded
                        {
                            Child = new FContainer(
                                decoration: new FBoxDecoration(Color: FColor.Green.WithAlpha(0.3f))
                            )
                            {
                                Child = new FText("Expanded (fills remaining space)")
                            }
                        },
                    }
                }
            };
        }

        private static FElement BuildContainer()
        {
            return new FContainer(
                decoration: new FBoxDecoration(
                    Color: FColor.FromHex("#6200EE").WithAlpha(0.1f),
                    Border: new FBorder(FColor.FromHex("#6200EE"), 2f),
                    BorderRadius: FBorderRadius.All(8f),
                    Shadow: new FShadow(FColor.Black.WithAlpha(0.2f), OffsetX: 2f, OffsetY: 2f, BlurRadius: 8f)
                ),
                padding: FEdgeInsets.All(16f),
                width: 280f
            )
            {
                Child = new FText("FContainer with decoration, border, radius, and shadow.")
            };
        }

        private static FElement BuildDivider()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("Above horizontal divider"),
                    new FDivider(color: FColor.FromHex("#BDBDBD")),
                    new FText("Below horizontal divider"),
                }
            };
        }

        private static FElement BuildScrollView()
        {
            var items = new FElement[20];
            for (var i = 0; i < 20; i++)
            {
                var label = $"Scrollable item {i + 1}";
                items[i] = new FPadding(FEdgeInsets.Symmetric(vertical: 4f, horizontal: 0f))
                {
                    Child = new FText(label)
                };
            }

            return new FContainer(height: 200f)
            {
                Child = new FScrollView()
                {
                    Child = new FColumn
                    {
                        Children = items
                    }
                }
            };
        }
    }
}