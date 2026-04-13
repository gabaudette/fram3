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
                new Story("FText",        BuildText),
                new Story("FProgressBar", BuildProgressBar),
                new Story("FSpinner",     BuildSpinner),
                new Story("FTabView",     BuildTabView),
                new Story("FListView",    BuildListView),
                new Story("FTooltip",     BuildTooltip),
                new Story("FSnackbar",    BuildSnackbar),
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
            return new FColumn
            {
                Children = new FElement[]
                {
                    new FText("25% progress:"),
                    new FProgressBar(value: 25f, title: "Loading..."),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("75% progress:")
                    },
                    new FProgressBar(value: 75f),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Custom range (0-50), value 30:")
                    },
                    new FProgressBar(value: 30f, min: 0f, max: 50f),
                }
            };
        }

        private static FElement BuildSpinner()
        {
            return new FColumn
            {
                Children = new FElement[]
                {
                    new FText("Default spinner (32px):"),
                    new FSpinner(),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Large spinner (64px, 6px stroke, blue, 2s):")
                    },
                    new FSpinner(size: 64f, strokeWidth: 6f, color: FColor.FromHex("#6200EE"), speed: 2f),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FText("Small fast spinner (20px, fast):")
                    },
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

            return new FContainer(height: 260f)
            {
                Child = new FListView<string>(
                    items: fruits,
                    itemBuilder: fruit => new FPadding(FEdgeInsets.Symmetric(vertical: 4f, horizontal: 8f))
                    {
                        Child = new FText(fruit)
                    },
                    itemHeight: 36f,
                    selectionMode: FListSelectionMode.Single
                )
            };
        }

        private static FElement BuildTooltip()
        {
            return new FColumn
            {
                Children = new FElement[]
                {
                    new FText("Hover over the box below to see the tooltip:"),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FTooltip("This is the tooltip message!")
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
                        }
                    },
                }
            };
        }

        private static FElement BuildSnackbar()
        {
            return new FColumn
            {
                Children = new FElement[]
                {
                    new FText("FSnackbar (visible when rendered):"),
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FSnackbar(
                            message: "File saved successfully.",
                            actionLabel: "Undo",
                            duration: 8f
                        )
                    },
                    new FPadding(FEdgeInsets.Symmetric(vertical: 8f, horizontal: 0f))
                    {
                        Child = new FSnackbar(
                            message: "No action button variant.",
                            duration: 8f
                        )
                    },
                }
            };
        }
    }
}
