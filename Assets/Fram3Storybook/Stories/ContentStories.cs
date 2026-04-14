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
                new Story("Text",
                    "Renders a string with optional font size, color, weight, style, and letter-spacing overrides.",
                    BuildText),
                new Story("ProgressBar",
                    "Shows a bounded progress value between a min and max, with an optional title label above the track.",
                    BuildProgressBar),
                new Story("Spinner",
                    "Displays an indeterminate loading indicator as a spinning ring, configurable in size, stroke width, color, and rotation speed.",
                    BuildSpinner),
                new Story("TabView",
                    "Renders a row of tab labels and swaps the visible content panel when a tab is selected.",
                    BuildTabView),
                new Story("ListView",
                    "Renders a large, virtualized list of items with a fixed item height and optional single- or multi-select support.",
                    BuildListView),
                new Story("Tooltip", "Attaches a plain-text tooltip to its child that appears on hover.",
                    BuildTooltip),
                new Story("Snackbar",
                    "Shows a transient message bar at the bottom of its parent with an optional action button and auto-dismiss duration.",
                    BuildSnackbar),
            };
        }

        private static Element BuildText()
        {
            return new Column
            {
                Children = new Element[]
                {
                    new Text("Default text"),
                    new Text("Large bold text", new TextStyle(FontSize: 24f, Bold: true)),
                    new Text("Colored italic text", new TextStyle(
                        FontSize: 16f,
                        Color: FrameColor.FromHex("#6200EE"),
                        Italic: true
                    )),
                    new Text("Underlined text", new TextStyle(Underline: true)),
                    new Text("Small with letter spacing", new TextStyle(
                        FontSize: 11f,
                        LetterSpacing: 2f,
                        Color: FrameColor.FromHex("#757575")
                    )),
                }
            };
        }

        private static Element BuildProgressBar()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("25% progress:"),
                    SizedBox.FromSize(height: 4f),
                    new ProgressBar(value: 25f, title: "Loading..."),
                    SizedBox.FromSize(height: 12f),
                    new Text("75% progress:"),
                    SizedBox.FromSize(height: 4f),
                    new ProgressBar(value: 75f),
                    SizedBox.FromSize(height: 12f),
                    new Text("Custom range (0-50), value 30:"),
                    SizedBox.FromSize(height: 4f),
                    new ProgressBar(value: 30f, min: 0f, max: 50f),
                }
            };
        }

        private static Element BuildSpinner()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("Default spinner (32px):"),
                    SizedBox.FromSize(height: 4f),
                    new Spinner(),
                    SizedBox.FromSize(height: 12f),
                    new Text("Large spinner (64px, 6px stroke, blue, 2s):"),
                    SizedBox.FromSize(height: 4f),
                    new Spinner(size: 64f, strokeWidth: 6f, color: FrameColor.FromHex("#6200EE"), speed: 2f),
                    SizedBox.FromSize(height: 12f),
                    new Text("Small fast spinner (20px, fast):"),
                    SizedBox.FromSize(height: 4f),
                    new Spinner(size: 20f, strokeWidth: 3f, speed: 0.5f),
                }
            };
        }

        private static Element BuildTabView()
        {
            return new TabView(
                tabs: new Tab[]
                {
                    new Tab("Alpha", new Padding(EdgeInsets.All(16f))
                    {
                        Child = new Text("Content for tab Alpha.")
                    }),
                    new Tab("Beta", new Padding(EdgeInsets.All(16f))
                    {
                        Child = new Text("Content for tab Beta.")
                    }),
                    new Tab("Gamma", new Padding(EdgeInsets.All(16f))
                    {
                        Child = new Text("Content for tab Gamma.")
                    }),
                },
                initialIndex: 0
            );
        }

        private static Element BuildListView()
        {
            var fruits = new string[]
            {
                "Apple", "Banana", "Cherry", "Date", "Elderberry",
                "Fig", "Grape", "Honeydew", "Kiwi", "Lemon",
            };

            return new ListView<string>(
                items: fruits,
                itemBuilder: fruit => new Padding(EdgeInsets.Symmetric(vertical: 4f, horizontal: 8f))
                {
                    Child = new Text(fruit)
                },
                itemHeight: 36f,
                selectionMode: ListSelectionMode.Single
            );
        }

        private static Element BuildTooltip()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("Hover over the box below to see the tooltip:"),
                    SizedBox.FromSize(height: 8f),
                    new Tooltip("This is the tooltip message!")
                    {
                        Child = new Container(
                            decoration: new BoxDecoration(
                                Color: FrameColor.FromHex("#6200EE").WithAlpha(0.15f),
                                Border: new Border(FrameColor.FromHex("#6200EE"), 1f),
                                BorderRadius: BorderRadius.All(4f)
                            ),
                            padding: EdgeInsets.All(12f)
                        )
                        {
                            Child = new Text("Hover me")
                        }
                    },
                }
            };
        }

        private static Element BuildSnackbar()
        {
            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = new Element[]
                {
                    new Text("Snackbar (visible when rendered):"),
                    SizedBox.FromSize(height: 8f),
                    new Snackbar(
                        message: "File saved successfully.",
                        actionLabel: "Undo",
                        duration: 8f
                    ),
                    SizedBox.FromSize(height: 8f),
                    new Snackbar(
                        message: "No action button variant.",
                        duration: 8f
                    ),
                }
            };
        }
    }
}