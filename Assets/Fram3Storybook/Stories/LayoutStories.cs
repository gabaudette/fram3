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
                new Story("Column",
                    "Arranges children vertically in a single column, with configurable main-axis and cross-axis alignment.",
                    BuildColumn),
                new Story("Row",
                    "Arranges children horizontally in a single row, with configurable main-axis and cross-axis alignment.",
                    BuildRow),
                new Story("Stack",
                    "Layers children on top of each other using absolute positioning, useful for overlapping elements.",
                    BuildStack),
                new Story("Wrap",
                    "Flows children left-to-right and wraps onto new lines when the available width is exhausted.",
                    BuildWrap),
                new Story("Padding",
                    "Inserts empty space between a single child and its parent boundaries using per-side insets.",
                    BuildPadding),
                new Story("Margin", "Adds outer spacing around a single child, pushing surrounding siblings away.",
                    BuildMargin),
                new Story("SizedBox",
                    "Occupies a fixed width and/or height; can hold an optional child or act as a gap element between siblings.",
                    BuildSizedBox),
                new Story("Center", "Centers its child both horizontally and vertically within the available space.",
                    BuildCenter),
                new Story("Expanded",
                    "Fills all remaining space along the parent axis, optionally weighted by a flex factor.",
                    BuildExpanded),
                new Story("Container",
                    "A versatile single-child box that combines a background decoration, explicit sizing, and inner padding in one element.",
                    BuildContainer),
                new Story("Divider",
                    "Renders a thin horizontal or vertical rule, useful as a visual separator between sections.",
                    BuildDivider),
                new Story("ScrollView",
                    "Makes its child scrollable along one axis when the content exceeds the available viewport size.",
                    BuildScrollView),
            };
        }

        private static Element BuildColumn()
        {
            return new Column(mainAxisAlignment: MainAxisAlignment.Start,
                crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Container(
                        decoration: new BoxDecoration(Color: FrameColor.Blue.WithAlpha(0.15f)),
                        padding: EdgeInsets.All(8f)
                    ) { Child = new Text("First item") },
                    SizedBox.FromSize(height: 4f),
                    new Container(
                        decoration: new BoxDecoration(Color: FrameColor.Green.WithAlpha(0.15f)),
                        padding: EdgeInsets.All(8f)
                    ) { Child = new Text("Second item") },
                    SizedBox.FromSize(height: 4f),
                    new Container(
                        decoration: new BoxDecoration(Color: FrameColor.Red.WithAlpha(0.15f)),
                        padding: EdgeInsets.All(8f)
                    ) { Child = new Text("Third item") },
                }
            };
        }

        private static Element BuildRow()
        {
            return new Row(
                mainAxisAlignment: MainAxisAlignment.SpaceBetween,
                crossAxisAlignment: CrossAxisAlignment.Center
            )
            {
                Children = new Element[]
                {
                    new Container(decoration: new BoxDecoration(Color: FrameColor.Blue), width: 60f, height: 60f),
                    new Container(decoration: new BoxDecoration(Color: FrameColor.Green), width: 60f, height: 60f),
                    new Container(decoration: new BoxDecoration(Color: FrameColor.Red), width: 60f, height: 60f),
                }
            };
        }

        private static Element BuildStack()
        {
            return new Container(width: 140f, height: 140f)
            {
                Child = new Stack
                {
                    Children = new Element[]
                    {
                        new Container(decoration: new BoxDecoration(Color: FrameColor.Blue), width: 120f, height: 120f),
                        new Container(decoration: new BoxDecoration(Color: FrameColor.Red.WithAlpha(0.7f)), width: 80f,
                            height: 80f),
                        new Container(
                            decoration: new BoxDecoration(
                                Color: FrameColor.Black.WithAlpha(0.5f),
                                BorderRadius: BorderRadius.All(2f)
                            ),
                            padding: EdgeInsets.Symmetric(vertical: 2f, horizontal: 6f)
                        )
                        {
                            Child = new Text("Stacked label", new TextStyle(Color: FrameColor.White))
                        },
                    }
                }
            };
        }

        private static Element BuildWrap()
        {
            return new Wrap
            {
                Children = new Element[]
                {
                    WrapTag("Tag A", FrameColor.Blue),
                    WrapTag("Tag B", FrameColor.Green),
                    WrapTag("Tag C", FrameColor.Red),
                    WrapTag("Tag D", FrameColor.Blue.WithAlpha(0.5f)),
                }
            };
        }

        private static Element WrapTag(string label, FrameColor color)
        {
            return new Container(
                decoration: new BoxDecoration(Color: color),
                width: 60f, height: 40f,
                padding: EdgeInsets.All(4f)
            )
            {
                Child = new Text(label, new TextStyle(Color: FrameColor.White))
            };
        }

        private static Element BuildPadding()
        {
            return new Container(decoration: new BoxDecoration(Color: FrameColor.FromHex("#EEEEEE")))
            {
                Child = new Padding(EdgeInsets.All(24f))
                {
                    Child = new Text("This text has 24px padding on all sides.")
                }
            };
        }

        private static Element BuildMargin()
        {
            var blockA = new Container(
                decoration: new BoxDecoration(Color: FrameColor.Blue.WithAlpha(0.2f)),
                height: 40f
            )
            {
                Child = new Text("Block A")
            };

            var blockB = new Container(
                decoration: new BoxDecoration(Color: FrameColor.Green.WithAlpha(0.2f)),
                height: 40f
            )
            {
                Child = new Text("Block B (16px vertical margin)")
            };

            var blockC = new Container(
                decoration: new BoxDecoration(Color: FrameColor.Red.WithAlpha(0.2f)),
                height: 40f
            )
            {
                Child = new Text("Block C")
            };

            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    blockA,
                    new Margin(EdgeInsets.Symmetric(vertical: 16f, horizontal: 0f), blockB),
                    blockC,
                }
            };
        }

        private static Element BuildSizedBox()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("SizedBox as a 200x80 spacer (no child):"),
                    new Container(decoration: new BoxDecoration(Color: FrameColor.FromHex("#EEEEEE")))
                    {
                        Child = new Row
                        {
                            Children = new Element[]
                            {
                                new Container(decoration: new BoxDecoration(Color: FrameColor.Blue.WithAlpha(0.3f)),
                                    height: 20f)
                                {
                                    Child = new Text("A")
                                },
                                SizedBox.FromSize(width: 200f, height: 80f),
                                new Container(decoration: new BoxDecoration(Color: FrameColor.Blue.WithAlpha(0.3f)),
                                    height: 20f)
                                {
                                    Child = new Text("B")
                                },
                            }
                        }
                    },
                    new Text("SizedBox.Square(60) as a vertical gap:"),
                    new Container(decoration: new BoxDecoration(Color: FrameColor.FromHex("#EEEEEE")))
                    {
                        Child = new Column
                        {
                            Children = new Element[]
                            {
                                new Text("Above"),
                                SizedBox.Square(60f),
                                new Text("Below"),
                            }
                        }
                    },
                    new Text("SizedBox.Expand() filling available width:"),
                    new Container(
                        decoration: new BoxDecoration(Color: FrameColor.FromHex("#EEEEEE")),
                        height: 40f
                    )
                    {
                        Child = new Row
                        {
                            Children = new Element[]
                            {
                                new Text("Left"),
                                SizedBox.Expand(),
                                new Text("Right"),
                            }
                        }
                    },
                }
            };
        }

        private static Element BuildCenter()
        {
            return new Container(
                decoration: new BoxDecoration(Color: FrameColor.FromHex("#F0F0F0")),
                width: 300f, height: 200f
            )
            {
                Child = new Center
                {
                    Child = new Text("Centered content")
                }
            };
        }

        private static Element BuildExpanded()
        {
            return new Container(height: 60f)
            {
                Child = new Row
                {
                    Children = new Element[]
                    {
                        new Container(
                            decoration: new BoxDecoration(Color: FrameColor.Blue.WithAlpha(0.3f)),
                            width: 80f
                        )
                        {
                            Child = new Text("Fixed")
                        },
                        new Expanded
                        {
                            Child = new Container(
                                decoration: new BoxDecoration(Color: FrameColor.Green.WithAlpha(0.3f))
                            )
                            {
                                Child = new Text("Expanded (fills remaining space)")
                            }
                        },
                    }
                }
            };
        }

        private static Element BuildContainer()
        {
            return new Container(
                decoration: new BoxDecoration(
                    Color: FrameColor.FromHex("#6200EE").WithAlpha(0.1f),
                    Border: new Border(FrameColor.FromHex("#6200EE"), 2f),
                    BorderRadius: BorderRadius.All(8f),
                    Shadow: new Shadow(FrameColor.Black.WithAlpha(0.2f), OffsetX: 2f, OffsetY: 2f, BlurRadius: 8f)
                ),
                padding: EdgeInsets.All(16f),
                width: 280f
            )
            {
                Child = new Text("Container with decoration, border, radius, and shadow.")
            };
        }

        private static Element BuildDivider()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("Above horizontal divider"),
                    new Divider(color: FrameColor.FromHex("#BDBDBD")),
                    new Text("Below horizontal divider"),
                }
            };
        }

        private static Element BuildScrollView()
        {
            var items = new Element[20];
            for (var i = 0; i < 20; i++)
            {
                var label = $"Scrollable item {i + 1}";
                items[i] = new Padding(EdgeInsets.Symmetric(vertical: 4f, horizontal: 0f))
                {
                    Child = new Text(label)
                };
            }

            return new Container(height: 200f)
            {
                Child = new ScrollView()
                {
                    Child = new Column
                    {
                        Children = items
                    }
                }
            };
        }
    }
}