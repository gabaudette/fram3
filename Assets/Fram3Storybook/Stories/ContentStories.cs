#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
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
                    "Virtualized list with search, class filter, and pagination over a roster of game characters.",
                    BuildListView),
                new Story("Tooltip", "Attaches a plain-text tooltip to its child that appears on hover.",
                    BuildTooltip),
                new Story("Snackbar",
                    "Shows a transient message bar triggered by a button, with and without an action label.",
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
            return new RosterListViewElement();
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
            return new SnackbarDemoElement();
        }

        // ---------------------------------------------------------------------------
        // ListView story
        // ---------------------------------------------------------------------------

        private sealed class Character
        {
            public string Name { get; }
            public string Class { get; }
            public int Level { get; }
            public Character(string name, string @class, int level)
            {
                Name = name;
                Class = @class;
                Level = level;
            }
        }

        private static readonly Character[] AllCharacters = new Character[]
        {
            new("Aric Stormblade", "Warrior", 42),
            new("Lyria Moonwhisper", "Mage", 38),
            new("Drex Ironfoot", "Rogue", 55),
            new("Seraphine Vale", "Healer", 30),
            new("Korroth the Unyielding", "Warrior", 61),
            new("Vex Nightfall", "Assassin", 47),
            new("Nira Emberveil", "Mage", 29),
            new("Theron Ashwood", "Ranger", 50),
            new("Zola Brightforge", "Paladin", 33),
            new("Skar Blackthorn", "Warrior", 58),
            new("Elyndra Swift", "Ranger", 44),
            new("Grom Ironjaw", "Berserker", 62),
            new("Calyx Frostweave", "Mage", 36),
            new("Ren Duskwalker", "Rogue", 40),
            new("Mira Sunspire", "Healer", 27),
            new("Torvyn Grimcrest", "Paladin", 53),
            new("Axis the Hollow", "Assassin", 49),
            new("Deva Starcaller", "Mage", 31),
            new("Brix Ironveil", "Berserker", 65),
            new("Solene Dawnmere", "Ranger", 22),
            new("Varg Stonehide", "Warrior", 57),
            new("Ophira Vex", "Healer", 35),
        };

        private static readonly string[] ClassFilters = new string[]
        {
            "All", "Warrior", "Mage", "Rogue", "Healer", "Ranger", "Paladin", "Assassin", "Berserker",
        };

        private const int PageSize = 8;

        private sealed class RosterListViewElement : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new RosterListViewState();

            private sealed class RosterListViewState : Fram3.UI.Core.State<RosterListViewElement>
            {
                private string _searchQuery = "";
                private string _classFilter = "All";
                private int _page = 0;

                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);

                    var filtered = FilterCharacters();
                    var totalPages = (filtered.Count + PageSize - 1) / PageSize;
                    if (totalPages == 0) totalPages = 1;
                    var currentPage = _page >= totalPages ? totalPages - 1 : _page;
                    var pageStart = currentPage * PageSize;
                    var pageEnd = System.Math.Min(pageStart + PageSize, filtered.Count);
                    var pageItems = new List<Character>();
                    for (var i = pageStart; i < pageEnd; i++) pageItems.Add(filtered[i]);

                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            BuildSearchAndFilter(theme),
                            SizedBox.FromSize(height: theme.Spacing),
                            BuildResultCount(filtered.Count, theme),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Container(height: 320f)
                            {
                                Child = new ListView<Character>(
                                    items: pageItems,
                                    itemBuilder: c => BuildCharacterRow(c, theme),
                                    itemHeight: 52f,
                                    selectionMode: ListSelectionMode.Single
                                )
                            },
                            SizedBox.FromSize(height: theme.Spacing),
                            BuildPagination(currentPage, totalPages, theme),
                        }
                    };
                }

                private IReadOnlyList<Character> FilterCharacters()
                {
                    var result = new List<Character>();
                    foreach (var c in AllCharacters)
                    {
                        if (_classFilter != "All" && c.Class != _classFilter) continue;
                        if (_searchQuery.Length > 0 &&
                            !c.Name.ToLower().Contains(_searchQuery.ToLower())) continue;
                        result.Add(c);
                    }
                    return result;
                }

                private Element BuildSearchAndFilter(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new TextField(
                                placeholder: "Search by name...",
                                value: _searchQuery,
                                onChanged: v => SetState(() =>
                                {
                                    _searchQuery = v ?? "";
                                    _page = 0;
                                })
                            ),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Dropdown(
                                options: ClassFilters,
                                selectedIndex: System.Array.IndexOf(ClassFilters, _classFilter),
                                label: "Filter by class",
                                onChanged: i => SetState(() =>
                                {
                                    _classFilter = ClassFilters[i];
                                    _page = 0;
                                })
                            ),
                        }
                    };
                }

                private static Element BuildResultCount(int count, Theme theme)
                {
                    return new Text($"{count} character{(count == 1 ? "" : "s")} found", new TextStyle(
                        FontSize: theme.FontSizeSmall,
                        Color: theme.SecondaryTextColor
                    ));
                }

                private static Element BuildCharacterRow(Character c, Theme theme)
                {
                    return new Container(
                        padding: EdgeInsets.Symmetric(vertical: 8f, horizontal: 12f),
                        decoration: new BoxDecoration(
                            Border: new Border(theme.SecondaryTextColor.WithAlpha(0.1f), 1f),
                            BorderRadius: BorderRadius.All(6f)
                        )
                    )
                    {
                        Child = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = new Element[]
                            {
                                new Expanded
                                {
                                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                                    {
                                        Children = new Element[]
                                        {
                                            new Text(c.Name, new TextStyle(
                                                Bold: true,
                                                FontSize: theme.FontSize,
                                                Color: theme.PrimaryTextColor
                                            )),
                                            new Text(c.Class, new TextStyle(
                                                FontSize: theme.FontSizeSmall,
                                                Color: theme.SecondaryTextColor
                                            )),
                                        }
                                    }
                                },
                                new Text($"Lv {c.Level}", new TextStyle(
                                    FontSize: theme.FontSize,
                                    Bold: true,
                                    Color: theme.SecondaryColor
                                )),
                            }
                        }
                    };
                }

                private Element BuildPagination(int currentPage, int totalPages, Theme theme)
                {
                    return new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = new Element[]
                        {
                            new Button(
                                label: "Prev",
                                onPressed: currentPage > 0
                                    ? () => SetState(() => _page = currentPage - 1)
                                    : null
                            ),
                            SizedBox.FromSize(width: theme.Spacing),
                            new Text($"Page {currentPage + 1} / {totalPages}", new TextStyle(
                                FontSize: theme.FontSize,
                                Color: theme.SecondaryTextColor
                            )),
                            SizedBox.FromSize(width: theme.Spacing),
                            new Button(
                                label: "Next",
                                onPressed: currentPage < totalPages - 1
                                    ? () => SetState(() => _page = currentPage + 1)
                                    : null
                            ),
                        }
                    };
                }
            }
        }

        // ---------------------------------------------------------------------------
        // Snackbar story
        // ---------------------------------------------------------------------------

        private sealed class SnackbarDemoElement : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new SnackbarDemoState();

            private sealed class SnackbarDemoState : Fram3.UI.Core.State<SnackbarDemoElement>
            {
                private bool _showSimple = false;
                private bool _showWithAction = false;

                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);

                    var children = new List<Element>
                    {
                        new Text("Simple snackbar (no action button):", new TextStyle(
                            Color: theme.SecondaryTextColor,
                            FontSize: theme.FontSizeSmall
                        )),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Button(
                            label: _showSimple ? "Dismiss" : "Show Snackbar",
                            onPressed: () => SetState(() => _showSimple = !_showSimple)
                        ),
                    };

                    if (_showSimple)
                    {
                        children.Add(SizedBox.FromSize(height: theme.Spacing));
                        children.Add(new Snackbar(message: "File saved successfully."));
                    }

                    children.Add(SizedBox.FromSize(height: theme.Spacing * 3f));
                    children.Add(new Text("Snackbar with action button:", new TextStyle(
                        Color: theme.SecondaryTextColor,
                        FontSize: theme.FontSizeSmall
                    )));
                    children.Add(SizedBox.FromSize(height: theme.Spacing));
                    children.Add(new Button(
                        label: _showWithAction ? "Dismiss" : "Show Snackbar with Action",
                        onPressed: () => SetState(() => _showWithAction = !_showWithAction)
                    ));

                    if (_showWithAction)
                    {
                        children.Add(SizedBox.FromSize(height: theme.Spacing));
                        children.Add(new Snackbar(
                            message: "Changes discarded.",
                            actionLabel: "Undo",
                            onAction: () => SetState(() => _showWithAction = false)
                        ));
                    }

                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = children.ToArray()
                    };
                }
            }
        }
    }
}
