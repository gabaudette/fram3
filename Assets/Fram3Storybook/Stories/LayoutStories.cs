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
                new Story("FColumn",    BuildColumn),
                new Story("FRow",       BuildRow),
                new Story("FStack",     BuildStack),
                new Story("FWrap",      BuildWrap),
                new Story("FPadding",   BuildPadding),
                new Story("FMargin",    BuildMargin),
                new Story("FSizedBox",  BuildSizedBox),
                new Story("FCenter",    BuildCenter),
                new Story("FExpanded",  BuildExpanded),
                new Story("FContainer", BuildContainer),
                new Story("FDivider",   BuildDivider),
                new Story("FScrollView", BuildScrollView),
            };
        }

        private static FElement BuildColumn()
        {
            return new FColumn(mainAxisAlignment: FMainAxisAlignment.Start)
            {
                Children = new FElement[]
                {
                    new FText("First item"),
                    new FText("Second item"),
                    new FText("Third item"),
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
                    new FContainer(decoration: new FBoxDecoration(Color: FColor.Blue),   width: 60f, height: 60f),
                    new FContainer(decoration: new FBoxDecoration(Color: FColor.Green),  width: 60f, height: 60f),
                    new FContainer(decoration: new FBoxDecoration(Color: FColor.Red),    width: 60f, height: 60f),
                }
            };
        }

        private static FElement BuildStack()
        {
            return new FStack
            {
                Children = new FElement[]
                {
                    new FContainer(decoration: new FBoxDecoration(Color: FColor.Blue),               width: 120f, height: 120f),
                    new FContainer(decoration: new FBoxDecoration(Color: FColor.Red.WithAlpha(0.7f)), width:  80f, height:  80f),
                    new FText("Stacked label"),
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
                Child = new FText(label)
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

            return new FColumn
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
            return new FColumn
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
                                new FContainer(decoration: new FBoxDecoration(Color: FColor.Blue.WithAlpha(0.3f)), height: 20f)
                                {
                                    Child = new FText("A")
                                },
                                FSizedBox.FromSize(width: 200f, height: 80f),
                                new FContainer(decoration: new FBoxDecoration(Color: FColor.Blue.WithAlpha(0.3f)), height: 20f)
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
            return new FRow
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
            return new FColumn
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
                Child = new FScrollView(FScrollDirection.Vertical)
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
