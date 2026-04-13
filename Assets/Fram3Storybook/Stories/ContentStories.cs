#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories
{
    /// <summary>Stories for the Content chapter.</summary>
    public static class ContentStories
    {
        /// <summary>Returns all content stories.</summary>
        public static IReadOnlyList<Story> All()
        {
            return new Story[]
            {
                new Story("FText",
                    "Renders a string with optional font size, color, weight, style, and letter-spacing overrides.",
                    BuildText),
                new Story("FProgressBar",
                    "Shows a bounded progress value between a min and max, with an optional title label above the track.",
                    BuildProgressBar),
                new Story("FSpinner",
                    "Displays an indeterminate loading indicator as a spinning ring, configurable in size, stroke width, color, and rotation speed.",
                    BuildSpinner),
                new Story("FTabView",
                    "Renders a row of tab labels and swaps the visible content panel when a tab is selected.",
                    BuildTabView),
                new Story("FListView",
                    "Renders a large, virtualized list of items with a fixed item height and optional single- or multi-select support.",
                    BuildListView),
                new Story("FTooltip", "Attaches a plain-text tooltip to its child that appears on hover.",
                    BuildTooltip),
                new Story("FSnackbar",
                    "Shows a transient message bar at the bottom of its parent with an optional action button and auto-dismiss duration.",
                    BuildSnackbar),
            };
        }

        private static FElement BuildText()
        {
            return new FColumn
            {
                Children = new FElement[]
                {
                    new FText("Default text"),
                    new FText("Large bold text", new FTextStyle(FontSize: 24f, Bold: true)),
                    new FText("Colored italic text", new FTextStyle(
                        FontSize: 16f,
                        Color: FColor.FromHex("#6200EE"),
                        Italic: true
                    )),
                    new FText("Underlined text", new FTextStyle(Underline: true)),
                    new FText("Small with letter spacing", new FTextStyle(
                        FontSize: 11f,
                        LetterSpacing: 2f,
                        Color: FColor.FromHex("#757575")
                    )),
                }
            };
        }

        private static FElement BuildProgressBar()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("25% progress:"),
                    FSizedBox.FromSize(height: 4f),
                    new FProgressBar(value: 25f, title: "Loading..."),
                    FSizedBox.FromSize(height: 12f),
                    new FText("75% progress:"),
                    FSizedBox.FromSize(height: 4f),
                    new FProgressBar(value: 75f),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Custom range (0-50), value 30:"),
                    FSizedBox.FromSize(height: 4f),
                    new FProgressBar(value: 30f, min: 0f, max: 50f),
                }
            };
        }

        private static FElement BuildSpinner()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("Default spinner (32px):"),
                    FSizedBox.FromSize(height: 4f),
                    new FSpinner(),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Large spinner (64px, 6px stroke, blue, 2s):"),
                    FSizedBox.FromSize(height: 4f),
                    new FSpinner(size: 64f, strokeWidth: 6f, color: FColor.FromHex("#6200EE"), speed: 2f),
                    FSizedBox.FromSize(height: 12f),
                    new FText("Small fast spinner (20px, fast):"),
                    FSizedBox.FromSize(height: 4f),
                    new FSpinner(size: 20f, strokeWidth: 3f, speed: 0.5f),
                }
            };
        }

        private static FElement BuildTabView()
        {
            return new FTabView(
                tabs: new FTab[]
                {
                    new FTab("Alpha", new FPadding(FEdgeInsets.All(16f))
                    {
                        Child = new FText("Content for tab Alpha.")
                    }),
                    new FTab("Beta", new FPadding(FEdgeInsets.All(16f))
                    {
                        Child = new FText("Content for tab Beta.")
                    }),
                    new FTab("Gamma", new FPadding(FEdgeInsets.All(16f))
                    {
                        Child = new FText("Content for tab Gamma.")
                    }),
                },
                initialIndex: 0
            );
        }

        private static FElement BuildListView()
        {
            var fruits = new string[]
            {
                "Apple", "Banana", "Cherry", "Date", "Elderberry",
                "Fig", "Grape", "Honeydew", "Kiwi", "Lemon",
            };

            return new FListView<string>(
                items: fruits,
                itemBuilder: fruit => new FPadding(FEdgeInsets.Symmetric(vertical: 4f, horizontal: 8f))
                {
                    Child = new FText(fruit)
                },
                itemHeight: 36f,
                selectionMode: FListSelectionMode.Single
            );
        }

        private static FElement BuildTooltip()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("Hover over the box below to see the tooltip:"),
                    FSizedBox.FromSize(height: 8f),
                    new FTooltip("This is the tooltip message!")
                    {
                        Child = new FContainer(
                            decoration: new FBoxDecoration(
                                Color: FColor.FromHex("#6200EE").WithAlpha(0.15f),
                                Border: new FBorder(FColor.FromHex("#6200EE"), 1f),
                                BorderRadius: FBorderRadius.All(4f)
                            ),
                            padding: FEdgeInsets.All(12f)
                        )
                        {
                            Child = new FText("Hover me")
                        }
                    },
                }
            };
        }

        private static FElement BuildSnackbar()
        {
            return new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch)
            {
                Children = new FElement[]
                {
                    new FText("FSnackbar (visible when rendered):"),
                    FSizedBox.FromSize(height: 8f),
                    new FSnackbar(
                        message: "File saved successfully.",
                        actionLabel: "Undo",
                        duration: 8f
                    ),
                    FSizedBox.FromSize(height: 8f),
                    new FSnackbar(
                        message: "No action button variant.",
                        duration: 8f
                    ),
                }
            };
        }
    }
}