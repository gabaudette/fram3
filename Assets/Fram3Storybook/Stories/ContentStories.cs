#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;
using UnityEngine;

namespace Fram3.UI.Storybook.Stories
{
    public static class ContentStories
    {
        public static IReadOnlyList<Story> All()
        {
            return new Story[]
            {
                new Story(
                    "Text",
                    "Renders a string with optional font size, color, weight, style, and letter-spacing overrides.",
                    () => new TextStory()
                ),
                new Story(
                    "ProgressBar",
                    "Shows a bounded progress value between a min and max, with an optional title label above the track.",
                    () => new ProgressBarStory()
                ),
                new Story(
                    "Spinner",
                    "Displays an indeterminate loading indicator as a spinning ring, configurable in size," +
                    " stroke width, color, and rotation speed.",
                    () => new SpinnerStory()
                ),
                new Story
                ("TabView",
                    "Renders a row of tab labels and swaps the visible content panel when a tab is selected.",
                    () => new TabViewStory()
                ),
                new Story(
                    "ListView",
                    "Virtualized list with search, class filter, and pagination over a roster of game characters.",
                    BuildListView
                ),
                new Story(
                    "Tooltip", "Attaches a plain-text tooltip to its child that appears on hover.",
                    () => new TooltipStory()
                ),
                new Story(
                    "Snackbar",
                    "Shows a transient message bar triggered by a button, with and without an action label.",
                    () => new SnackbarStory()
                ),
                new Story(
                    "Images & Icons",
                    "Displays a Texture2D and Sprite loaded via Resources.Load, and an SVG icon loaded via svgPath.",
                    () => new ImageStory()
                )
            };
        }

        private static Element BuildListView() => new RosterListViewElement();

        private sealed class TextStory : StatefulElement
        {
            public override State CreateState() => new TextStoryState();

            private sealed class TextStoryState : State<TextStory>
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
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme)
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                    {
                        Children = new Element[]
                        {
                            new Text("Default text", new TextStyle(Color: theme.PrimaryTextColor)),
                            new Text("Large bold text",
                                style: new TextStyle(FontSize: 24f, Bold: true, Color: theme.PrimaryTextColor)
                            ),
                            new Text("Accent italic text", new TextStyle(
                                FontSize: 16f,
                                Color: theme.PrimaryColor,
                                Italic: true
                            )),
                            new Text("Small with letter spacing", new TextStyle(
                                FontSize: 11f,
                                LetterSpacing: 2f,
                                Color: theme.SecondaryTextColor
                            ))
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("KILL FEED", new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )),
                            SizedBox.FromSize(height: theme.Spacing),
                            KillFeedEntry(
                                "Drex Ironfoot", "eliminated", "a Goblin Scout",
                                FrameColor.FromHex("#FF6B6B"), theme
                            ),
                            KillFeedEntry(
                                "Lyria Moonwhisper", "cast", "Arcane Surge", FrameColor.FromHex("#7B61FF"),
                                theme
                            ),
                            KillFeedEntry(
                                "Seraphine Vale", "healed", "Aric for 142 HP", FrameColor.FromHex("#00D4AA"),
                                theme
                            ),
                            KillFeedEntry(
                                "Vex Nightfall", "looted", "Shadowblade", FrameColor.FromHex("#FFD700"),
                                theme
                            )
                        }
                    };
                }

                private static Element KillFeedEntry(
                    string actor,
                    string verb,
                    string target,
                    FrameColor actorColor,
                    Theme theme
                )
                {
                    return new Padding(EdgeInsets.Symmetric(vertical: 2f, horizontal: 0f))
                    {
                        Child = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = new Element[]
                            {
                                new Text(actor,
                                    style: new TextStyle(FontSize: theme.FontSizeSmall, Bold: true, Color: actorColor)
                                ),
                                new Text($" {verb} ",
                                    style: new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.SecondaryTextColor)
                                ),
                                new Text(target,
                                    style: new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.PrimaryTextColor)
                                )
                            }
                        }
                    };
                }
            }
        }

        private sealed class ProgressBarStory : StatefulElement
        {
            public override State CreateState() => new ProgressBarStoryState();

            private sealed class ProgressBarStoryState : State<ProgressBarStory>
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
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme)
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("25% progress:",
                                style: new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)
                            ),
                            SizedBox.FromSize(height: 4f),
                            ThemedBar(25f, 100f, theme.PrimaryColor),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Text("75% progress:",
                                style: new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)
                            ),
                            SizedBox.FromSize(height: 4f),
                            ThemedBar(75f, 100f, theme.PrimaryColor)
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("HERO STATS", new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )),
                            SizedBox.FromSize(height: theme.Spacing),
                            GameStatBar("HP", 87, 100, FrameColor.FromHex("#FF6B6B"), theme),
                            SizedBox.FromSize(height: theme.Spacing),
                            GameStatBar("MP", 42, 80, FrameColor.FromHex("#7B61FF"), theme),
                            SizedBox.FromSize(height: theme.Spacing),
                            GameStatBar("XP", 1240, 2000, FrameColor.FromHex("#FFD700"), theme)
                        }
                    };
                }

                private static Element GameStatBar(string label, int value, int max, FrameColor color, Theme theme)
                {
                    return new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = new Element[]
                        {
                            new Container(width: 28f)
                            {
                                Child = new Text(label, new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Bold: true,
                                    Color: color
                                ))
                            },
                            SizedBox.FromSize(width: theme.Spacing),
                            new Expanded
                            {
                                Child = ThemedBar(value, max, color)
                            },
                            SizedBox.FromSize(width: theme.Spacing),
                            new Text($"{value}/{max}", new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor
                            ))
                        }
                    };
                }

                private static Element ThemedBar(float value, float max, FrameColor fillColor)
                {
                    var fill = max > 0f ? System.Math.Min(value / max, 1f) : 0f;
                    var empty = System.Math.Max(1f - fill, 0f);
                    return new Container(
                        height: 12f,
                        decoration: new BoxDecoration(
                            Color: fillColor.WithAlpha(0.2f),
                            BorderRadius: BorderRadius.All(6f)
                        )
                    )
                    {
                        Child = new Row(crossAxisAlignment: CrossAxisAlignment.Stretch)
                        {
                            Children = new Element[]
                            {
                                new Expanded(flex: fill)
                                {
                                    Child = new Container(
                                        height: 12f,
                                        decoration: new BoxDecoration(
                                            Color: fillColor,
                                            BorderRadius: BorderRadius.All(6f)
                                        )
                                    )
                                },
                                new Expanded(flex: empty) { Child = new Container(height: 12f) }
                            }
                        }
                    };
                }
            }
        }

        private sealed class SpinnerStory : StatefulElement
        {
            public override State CreateState() => new SpinnerStoryState();

            private sealed class SpinnerStoryState : State<SpinnerStory>
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
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme)
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text("Default spinner (32px):",
                                new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                            SizedBox.FromSize(height: 4f),
                            new Spinner(),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Text("Large spinner (64px, primary color):",
                                new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)),
                            SizedBox.FromSize(height: 4f),
                            new Spinner(size: 64f, strokeWidth: 6f, color: theme.PrimaryColor, speed: 2f)
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    return new Container(
                        decoration: new BoxDecoration(
                            Color: theme.SurfaceColor,
                            BorderRadius: BorderRadius.All(theme.BorderRadius)
                        ),
                        padding: EdgeInsets.All(theme.Spacing * 3f)
                    )
                    {
                        Child = new Column(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = new Element[]
                            {
                                new Spinner(size: 48f, strokeWidth: 4f, color: theme.SecondaryColor),
                                SizedBox.FromSize(height: theme.Spacing * 2f),
                                new Text(
                                    "Connecting to server...",
                                    style: new TextStyle(
                                        FontSize: theme.FontSize,
                                        Bold: true,
                                        Color: theme.PrimaryTextColor
                                    )
                                ),
                                new Text(
                                    "Please wait",
                                    style: new TextStyle(
                                        FontSize: theme.FontSizeSmall,
                                        Color: theme.SecondaryTextColor
                                    )
                                )
                            }
                        }
                    };
                }
            }
        }

        private sealed class TabViewStory : StatefulElement
        {
            public override State CreateState() => new TabViewStoryState();

            private sealed class TabViewStoryState : State<TabViewStory>
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
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme)
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new TabView(
                        tabs: new Tab[]
                        {
                            new Tab("Alpha", new Padding(EdgeInsets.All(16f))
                            {
                                Child = new Text("Content for tab Alpha.", new TextStyle(Color: theme.PrimaryTextColor))
                            }),
                            new Tab("Beta", new Padding(EdgeInsets.All(16f))
                            {
                                Child = new Text("Content for tab Beta.", new TextStyle(Color: theme.PrimaryTextColor))
                            }),
                            new Tab("Gamma", new Padding(EdgeInsets.All(16f))
                            {
                                Child = new Text("Content for tab Gamma.", new TextStyle(Color: theme.PrimaryTextColor))
                            })
                        },
                        initialIndex: 0
                    );
                }

                private static Element BuildGame(Theme theme)
                {
                    return new TabView(
                        tabs: new Tab[]
                        {
                            new Tab("Character", new Padding(EdgeInsets.All(theme.Spacing))
                            {
                                Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                                {
                                    Children = new Element[]
                                    {
                                        new Text("Aric Stormblade",
                                            style: new TextStyle(
                                                Bold: true, FontSize: theme.FontSizeLarge,
                                                Color: theme.PrimaryTextColor
                                            )
                                        ),
                                        new Text(
                                            "Warrior - Level 42",
                                            style: new TextStyle(
                                                Color: theme.SecondaryTextColor,
                                                FontSize: theme.FontSizeSmall
                                            )
                                        )
                                    }
                                }
                            }),
                            new Tab("Inventory", new Padding(EdgeInsets.All(theme.Spacing))
                            {
                                Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                                {
                                    Children = new Element[]
                                    {
                                        new Text("Shadowblade x1", new TextStyle(Color: theme.PrimaryTextColor)),
                                        new Text("Health Potion x5", new TextStyle(Color: theme.PrimaryTextColor)),
                                        new Text(
                                            "Gold x1,240",
                                            style: new TextStyle(
                                                Color: FrameColor.FromHex("#FFD700"),
                                                Bold: true
                                            )
                                        )
                                    }
                                }
                            }),
                            new Tab("Skills", new Padding(EdgeInsets.All(theme.Spacing))
                            {
                                Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                                {
                                    Children = new Element[]
                                    {
                                        new Text("Cleave - Lv 3", new TextStyle(Color: theme.PrimaryTextColor)),
                                        new Text("Battlecry - Lv 2", new TextStyle(Color: theme.PrimaryTextColor)),
                                        new Text("Shield Bash - Lv 5", new TextStyle(Color: theme.PrimaryTextColor))
                                    }
                                }
                            })
                        },
                        initialIndex: 0
                    );
                }
            }
        }

        private sealed class TooltipStory : StatefulElement
        {
            public override State CreateState() => new TooltipStoryState();

            private sealed class TooltipStoryState : State<TooltipStory>
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
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme)
                        }
                    };
                }

                private static Element BuildBasic(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Text(
                                "Hover over the box below:",
                                style: new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)
                            ),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Tooltip("This is the tooltip message!")
                            {
                                Child = new Container(
                                    decoration: new BoxDecoration(
                                        Color: theme.PrimaryColor.WithAlpha(0.15f),
                                        Border: new Border(theme.PrimaryColor, 1f),
                                        BorderRadius: BorderRadius.All(theme.BorderRadius)
                                    ),
                                    padding: EdgeInsets.All(12f)
                                )
                                {
                                    Child = new Text("Hover me", new TextStyle(Color: theme.PrimaryTextColor))
                                }
                            }
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                    {
                        Children = new Element[]
                        {
                            new Text("Hover over an ability icon:",
                                style: new TextStyle(Color: theme.SecondaryTextColor, FontSize: theme.FontSizeSmall)
                            ),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                            {
                                Children = new Element[]
                                {
                                    AbilityIcon(
                                        "C",
                                        "Cleave",
                                        "Deals 150% weapon damage to all enemies in front.",
                                        FrameColor.FromHex("#FF6B6B"),
                                        theme
                                    ),
                                    SizedBox.FromSize(width: theme.Spacing),
                                    AbilityIcon(
                                        "B",
                                        "Battlecry",
                                        "Increases party ATK by 20% for 10 seconds.",
                                        FrameColor.FromHex("#FFD700"),
                                        theme
                                    ),
                                    SizedBox.FromSize(width: theme.Spacing),
                                    AbilityIcon(
                                        "S",
                                        "Shield Bash",
                                        "Stuns target for 2 seconds. 12s cooldown.",
                                        FrameColor.FromHex("#7B61FF"),
                                        theme
                                    )
                                }
                            }
                        }
                    };
                }

                private static Element AbilityIcon(string key, string name, string desc, FrameColor color, Theme theme)
                {
                    return new Tooltip($"{name}: {desc}")
                    {
                        Child = new Container(
                            width: 52f, height: 52f,
                            decoration: new BoxDecoration(
                                Color: color.WithAlpha(0.2f),
                                Border: new Border(color, 2f),
                                BorderRadius: BorderRadius.All(8f)
                            )
                        )
                        {
                            Child = new Center
                            {
                                Child = new Text(key, new TextStyle(
                                    Bold: true,
                                    FontSize: theme.FontSizeLarge,
                                    Color: color
                                ))
                            }
                        }
                    };
                }
            }
        }

        private sealed class SnackbarStory : StatefulElement
        {
            public override State CreateState() => new SnackbarStoryState();

            private sealed class SnackbarStoryState : State<SnackbarStory>
            {
                private bool _showSimple;
                private bool _showLoot;

                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example", BuildGame(theme), theme)
                        }
                    };
                }

                private Element BuildBasic(Theme theme)
                {
                    var children = new List<Element>
                    {
                        new Button(
                            label: _showSimple ? "Dismiss" : "Show Snackbar",
                            onPressed: () => SetState(() => _showSimple = !_showSimple)
                        )
                    };

                    if (!_showSimple)
                    {
                        return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                        {
                            Children = children.ToArray()
                        };
                    }

                    children.Add(SizedBox.FromSize(height: theme.Spacing));
                    children.Add(new Snackbar(message: "File saved successfully."));

                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = children.ToArray()
                    };
                }

                private Element BuildGame(Theme theme)
                {
                    var children = new List<Element>
                    {
                        new Button(
                            label: _showLoot ? "Dismiss" : "Pick Up Item",
                            onPressed: () => SetState(() => _showLoot = !_showLoot)
                        )
                    };

                    if (!_showLoot)
                    {
                        return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                        {
                            Children = children.ToArray()
                        };
                    }

                    children.Add(SizedBox.FromSize(height: theme.Spacing));
                    children.Add(new Snackbar(
                        message: "You picked up: Shadowblade (Epic)",
                        actionLabel: "Equip",
                        onAction: () => SetState(() => _showLoot = false)
                    ));

                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = children.ToArray()
                    };
                }
            }
        }

        private sealed class ImageStory : StatefulElement
        {
            public override State CreateState() => new ImageStoryState();

            private sealed class ImageStoryState : State<ImageStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    var texture = Resources.Load<Texture2D>("Images/sample");
                    var sprite = Resources.Load<Sprite>("Images/sample-sprite");

                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Texture2D", BuildTexture(texture, theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Sprite", BuildSprite(sprite, theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("SVG Icon", BuildIcon(theme), theme)
                        }
                    };
                }

                private static Element BuildTexture(Texture2D? texture, Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                    {
                        Children = new Element[]
                        {
                            new FrameImage(source: texture, width: 128f, height: 128f),
                            SizedBox.FromSize(height: 4f),
                            new Text(
                                "Loaded as Texture2D via Resources.Load",
                                style: new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.SecondaryTextColor)
                            )
                        }
                    };
                }

                private static Element BuildSprite(Sprite? sprite, Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                    {
                        Children = new Element[]
                        {
                            new FrameImage(source: sprite, width: 128f, height: 128f),
                            SizedBox.FromSize(height: 4f),
                            new Text(
                                "Loaded as Sprite via Resources.Load",
                                style: new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.SecondaryTextColor)
                            )
                        }
                    };
                }

                private static Element BuildIcon(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                    {
                        Children = new Element[]
                        {
                            new Icon(svgPath: "Assets/Fram3Storybook/Icons/sample.svg", width: 64f, height: 64f),
                            SizedBox.FromSize(height: 4f),
                            new Text(
                                "SVG icon loaded via svgPath",
                                style: new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.SecondaryTextColor)
                            )
                        }
                    };
                }
            }
        }

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

        private static readonly Character[] AllCharacters =
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
            new("Ophira Vex", "Healer", 35)
        };

        private static readonly string[] ClassFilters =
        {
            "All", "Warrior", "Mage", "Rogue", "Healer", "Ranger", "Paladin", "Assassin", "Berserker"
        };

        private const int PageSize = 8;

        private sealed class RosterListViewElement : StatefulElement
        {
            public override State CreateState() => new RosterListViewState();

            private sealed class RosterListViewState : State<RosterListViewElement>
            {
                private string _searchQuery = "";
                private string _classFilter = "All";
                private int _page;

                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);

                    var filtered = FilterCharacters();

                    var totalPages = (filtered.Count + PageSize - 1) / PageSize;
                    if (totalPages == 0)
                    {
                        totalPages = 1;
                    }

                    var currentPage = _page >= totalPages ? totalPages - 1 : _page;
                    var pageStart = currentPage * PageSize;
                    var pageEnd = System.Math.Min(pageStart + PageSize, filtered.Count);
                    var pageItems = new List<Character>();

                    for (var i = pageStart; i < pageEnd; i++)
                    {
                        pageItems.Add(filtered[i]);
                    }

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
                                    itemHeight: 56f,
                                    selectionMode: ListSelectionMode.Single
                                )
                            },
                            SizedBox.FromSize(height: theme.Spacing),
                            BuildPagination(currentPage, totalPages, theme)
                        }
                    };
                }

                private IReadOnlyList<Character> FilterCharacters()
                {
                    var result = new List<Character>();
                    foreach (var c in AllCharacters)
                    {
                        if (_classFilter != "All" && c.Class != _classFilter)
                        {
                            continue;
                        }

                        if (
                            _searchQuery.Length > 0 &&
                            !c.Name.ToLower().Contains(_searchQuery.ToLower())
                        )
                        {
                            continue;
                        }

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
                            )
                        }
                    };
                }

                private static Element BuildResultCount(int count, Theme theme)
                {
                    return new Text(
                        $"{count} character{(count == 1 ? "" : "s")} found",
                        style: new TextStyle(
                            FontSize: theme.FontSizeSmall,
                            Color: theme.SecondaryTextColor
                        )
                    );
                }

                private static Element BuildCharacterRow(Character c, Theme theme)
                {
                    return new Margin(new EdgeInsets(0f, 0f, 4f, 0f),
                        new Container(
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
                                                ))
                                            }
                                        }
                                    },
                                    new Text($"Lv {c.Level}", new TextStyle(
                                        FontSize: theme.FontSize,
                                        Bold: true,
                                        Color: theme.SecondaryColor
                                    ))
                                }
                            }
                        }
                    );
                }

                private Element BuildPagination(int currentPage, int totalPages, Theme theme)
                {
                    return new Row(
                        mainAxisAlignment: MainAxisAlignment.End,
                        crossAxisAlignment: CrossAxisAlignment.Center
                    )
                    {
                        Children = new Element[]
                        {
                            new Button(
                                label: "Prev",
                                onPressed: currentPage > 0
                                    ? () => SetState(() => _page = currentPage - 1)
                                    : null
                            ),
                            SizedBox.FromSize(width: theme.Spacing * 2f),
                            new Text($"Page {currentPage + 1} / {totalPages}", new TextStyle(
                                FontSize: theme.FontSize,
                                Color: theme.SecondaryTextColor
                            )),
                            SizedBox.FromSize(width: theme.Spacing * 2f),
                            new Button(
                                label: "Next",
                                onPressed: currentPage < totalPages - 1
                                    ? () => SetState(() => _page = currentPage + 1)
                                    : null
                            )
                        }
                    };
                }
            }
        }
    }
}