#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;
using UnityEngine;
using Avatar = Fram3.UI.Elements.Content.Avatar;

namespace Fram3.UI.Storybook.Stories
{
    public static class ContentStories
    {
        public static IReadOnlyList<Story> All()
        {
            return new Story[]
            {
                new Story(
                    "Accordion",
                    "A vertically stacked set of collapsible panels. Each item has a header that toggles " +
                    "its content. Supports single-expand and multi-expand modes.",
                    () => new AccordionStory()
                ),
                new Story(
                    "Avatar",
                    "A circular element displaying a user image, initials placeholder, or icon. " +
                    "Available in small, medium, and large sizes with optional ring border.",
                    () => new AvatarStory()
                ),
                new Story(
                    "Alert",
                    "An inline notification surface that displays a severity-tinted title, an optional message, " +
                    "and optional action buttons. Use Info, Success, Warning, or Error severity.",
                    () => new AlertStory()
                ),
                new Story(
                    "Badge",
                    "Overlays a colored pip on a child element to show a count or status dot. " +
                    "Common on inventory slots, ability icons, and minimap markers.",
                    () => new BadgeStory()
                ),
                new Story(
                    "Card",
                    "An elevated surface container with an optional header and footer. " +
                    "Supports elevated (shadow) and outlined (border) variants.",
                    () => new CardStory()
                ),
                new Story(
                    "Chip",
                    "A compact, pill-shaped label used to represent an attribute, filter, or selection. " +
                    "Supports an optional dismiss button for removable chips.",
                    () => new ChipStory()
                ),
                new Story(
                    "Dialog",
                    "A modal overlay dialog with a title, optional body content, and action buttons. " +
                    "Centered on a dimmed backdrop. Toggled by mounting or unmounting the element.",
                    () => new DialogStory()
                ),
                new Story(
                    "Images & Icons",
                    "Displays a Texture2D and Sprite loaded via Resources.Load, and an SVG icon loaded via svgPath.",
                    () => new ImageStory()
                ),
                new Story(
                    "ListView",
                    "Virtualized list with search, class filter, and pagination over a roster of game characters.",
                    BuildListView
                ),
                new Story(
                    "ProgressBar",
                    "Shows a bounded progress value between a min and max, with an optional title label above the track.",
                    () => new ProgressBarStory()
                ),
                new Story(
                    "Snackbar",
                    "Shows a transient message bar triggered by a button, with and without an action label.",
                    () => new SnackbarStory()
                ),
                new Story(
                    "Spinner",
                    "Displays an indeterminate loading indicator as a spinning ring, configurable in size," +
                    " stroke width, color, and rotation speed.",
                    () => new SpinnerStory()
                ),
                new Story(
                    "Stepper",
                    "A numeric input with decrement and increment buttons. Supports min/max clamping, " +
                    "configurable step size, optional label, and a disabled state.",
                    () => new StepperStory()
                ),
                new Story
                ("TabView",
                    "Renders a row of tab labels and swaps the visible content panel when a tab is selected.",
                    () => new TabViewStory()
                ),
                new Story(
                    "Text",
                    "Renders a string with optional font size, color, weight, style, and letter-spacing overrides.",
                    () => new TextStory()
                ),
                new Story(
                    "Tooltip", "Attaches a plain-text tooltip to its child that appears on hover.",
                    () => new TooltipStory()
                )
            };
        }

        private static Element BuildListView() => new RosterListViewElement();

        private sealed class AccordionStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new AccordionStoryState();

            private sealed class AccordionStoryState : State<AccordionStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Single-expand", BuildSingle(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Multi-expand", BuildMulti(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example — Codex", BuildGame(theme), theme),
                        }
                    };
                }

                private static Element BuildSingle(Theme theme) =>
                    new Accordion(
                        new List<AccordionItem>
                        {
                            new AccordionItem("What is fram3?", new Text(
                                "fram3 is a retained-mode UI framework for Unity built on a virtual element tree.",
                                new TextStyle(FontSize: theme.FontSize, Color: theme.SecondaryTextColor)
                            )),
                            new AccordionItem("How do themes work?", new Text(
                                "Elements pull colors, spacing, and typography from the active ThemeData via ThemeConsumer.Of.",
                                new TextStyle(FontSize: theme.FontSize, Color: theme.SecondaryTextColor)
                            )),
                            new AccordionItem("Is it production-ready?", new Text(
                                "Currently in beta — APIs are stable but the element library is still growing.",
                                new TextStyle(FontSize: theme.FontSize, Color: theme.SecondaryTextColor)
                            )),
                        },
                        initialIndex: 0
                    );

                private static Element BuildMulti(Theme theme) =>
                    new Accordion(
                        new List<AccordionItem>
                        {
                            new AccordionItem("Movement", new Text(
                                "WASD or left-stick to move. Hold shift to sprint.",
                                new TextStyle(FontSize: theme.FontSize, Color: theme.SecondaryTextColor)
                            )),
                            new AccordionItem("Combat", new Text(
                                "Left-click to attack. Right-click to block. Q to dodge-roll.",
                                new TextStyle(FontSize: theme.FontSize, Color: theme.SecondaryTextColor)
                            )),
                            new AccordionItem("Inventory", new Text(
                                "Press I to open inventory. Drag items to equip or combine.",
                                new TextStyle(FontSize: theme.FontSize, Color: theme.SecondaryTextColor)
                            )),
                        },
                        allowMultiple: true,
                        initialIndex: 0
                    );

                private static Element BuildGame(Theme theme) =>
                    new Accordion(
                        new List<AccordionItem>
                        {
                            new AccordionItem("Chapter I — The Awakening", new Text(
                                "You wake in the ruins of Vel'Kara with no memory of how you arrived.",
                                new TextStyle(FontSize: theme.FontSize, Color: theme.SecondaryTextColor)
                            )),
                            new AccordionItem("Chapter II — The Sunken Archive", new Text(
                                "Ancient texts beneath the city hint at a weapon capable of sealing the rift.",
                                new TextStyle(FontSize: theme.FontSize, Color: theme.SecondaryTextColor)
                            )),
                            new AccordionItem("Chapter III — Betrayal at the Gate", new Text(
                                "Your guide reveals their true allegiance moments before the gate is destroyed.",
                                new TextStyle(FontSize: theme.FontSize, Color: theme.SecondaryTextColor)
                            )),
                        },
                         initialIndex: 0
                    );
            }
        }

        private sealed class AvatarStory : StatefulElement
        {
            public override Fram3.UI.Core.State CreateState() => new AvatarStoryState();

            private sealed class AvatarStoryState : State<AvatarStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Initials", BuildInitials(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Icon", BuildIcon(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Sizes", BuildSizes(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Ring border", BuildRing(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example — Party", BuildGame(theme), theme),
                        }
                    };
                }

                private static Element BuildInitials(Theme theme) =>
                    new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = new Element[]
                        {
                            new Avatar(initials: "AB"),
                            SizedBox.FromSize(width: theme.Spacing * 2f),
                            new Avatar(initials: "JD", backgroundColor: new FrameColor(0.13f, 0.59f, 0.95f, 1f)),
                            SizedBox.FromSize(width: theme.Spacing * 2f),
                            new Avatar(initials: "Z", backgroundColor: new FrameColor(0.61f, 0.15f, 0.69f, 1f)),
                        }
                    };

                private static Element BuildIcon(Theme theme) =>
                    new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = new Element[]
                        {
                            new Avatar(iconSvgPath: "ui/icons/person.svg"),
                            SizedBox.FromSize(width: theme.Spacing * 2f),
                            new Avatar(
                                iconSvgPath: "ui/icons/shield.svg",
                                backgroundColor: new FrameColor(0.18f, 0.8f, 0.44f, 1f)
                            ),
                        }
                    };

                private static Element BuildSizes(Theme theme) =>
                    new Row(
                        crossAxisAlignment: CrossAxisAlignment.Center,
                        mainAxisAlignment: MainAxisAlignment.Start
                    )
                    {
                        Children = new Element[]
                        {
                            new Avatar(initials: "S", size: AvatarSize.Small),
                            SizedBox.FromSize(width: theme.Spacing * 2f),
                            new Avatar(initials: "M", size: AvatarSize.Medium),
                            SizedBox.FromSize(width: theme.Spacing * 2f),
                            new Avatar(initials: "L", size: AvatarSize.Large),
                        }
                    };

                private static Element BuildRing(Theme theme) =>
                    new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = new Element[]
                        {
                            new Avatar(
                                initials: "AB",
                                ring: new Border(theme.PrimaryColor, 2f)
                            ),
                            SizedBox.FromSize(width: theme.Spacing * 2f),
                            new Avatar(
                                initials: "CD",
                                backgroundColor: theme.SurfaceColor,
                                foregroundColor: theme.PrimaryTextColor,
                                ring: new Border(theme.InputBorderColor, 1f)
                            ),
                        }
                    };

                private static Element BuildGame(Theme theme) =>
                    new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = new Element[]
                        {
                            new Column(crossAxisAlignment: CrossAxisAlignment.Center)
                            {
                                Children = new Element[]
                                {
                                    new Avatar(
                                        initials: "KN",
                                        size: AvatarSize.Large,
                                        backgroundColor: new FrameColor(0.83f, 0.18f, 0.18f, 1f),
                                        ring: new Border(new FrameColor(1f, 0.84f, 0f, 1f), 3f)
                                    ),
                                    SizedBox.FromSize(height: theme.Spacing),
                                    new Text("Knight", new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.SecondaryTextColor, TextAlign: UnityEngine.TextAnchor.MiddleCenter)),
                                }
                            },
                            SizedBox.FromSize(width: theme.Spacing * 3f),
                            new Column(crossAxisAlignment: CrossAxisAlignment.Center)
                            {
                                Children = new Element[]
                                {
                                    new Avatar(
                                        initials: "MG",
                                        size: AvatarSize.Large,
                                        backgroundColor: new FrameColor(0.13f, 0.59f, 0.95f, 1f)
                                    ),
                                    SizedBox.FromSize(height: theme.Spacing),
                                    new Text("Mage", new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.SecondaryTextColor, TextAlign: UnityEngine.TextAnchor.MiddleCenter)),
                                }
                            },
                            SizedBox.FromSize(width: theme.Spacing * 3f),
                            new Column(crossAxisAlignment: CrossAxisAlignment.Center)
                            {
                                Children = new Element[]
                                {
                                    new Avatar(
                                        initials: "RG",
                                        size: AvatarSize.Large,
                                        backgroundColor: new FrameColor(0.18f, 0.8f, 0.44f, 1f)
                                    ),
                                    SizedBox.FromSize(height: theme.Spacing),
                                    new Text("Ranger", new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.SecondaryTextColor, TextAlign: UnityEngine.TextAnchor.MiddleCenter)),
                                }
                            },
                        }
                    };
            }
        }

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

        private sealed class StepperStory : StatefulElement
        {
            public override State CreateState() => new StepperStoryState();

            private sealed class StepperStoryState : State<StepperStory>
            {
                private int _basic;
                private int _bounded = 5;
                private int _stepped;
                private int _quantity = 1;

                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Basic", BuildBasic(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("With Min and Max", BuildBounded(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Custom Step", BuildStepped(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Disabled", BuildDisabled(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example — Item Quantity", BuildGameExample(theme), theme),
                        }
                    };
                }

                private Element BuildBasic(Theme theme) =>
                    new Stepper(
                        value: _basic,
                        label: "Count",
                        onChanged: v => SetState(() => _basic = v)
                    );

                private Element BuildBounded(Theme theme) =>
                    new Stepper(
                        value: _bounded,
                        label: "Level (1-10)",
                        min: 1,
                        max: 10,
                        onChanged: v => SetState(() => _bounded = v)
                    );

                private Element BuildStepped(Theme theme) =>
                    new Stepper(
                        value: _stepped,
                        label: "Gold (step 50)",
                        step: 50,
                        onChanged: v => SetState(() => _stepped = v)
                    );

                private static Element BuildDisabled(Theme theme) =>
                    new Stepper(
                        value: 3,
                        label: "Locked",
                        min: 0,
                        max: 10
                    );

                private Element BuildGameExample(Theme theme) =>
                    new Card(
                        header: new Text("Potion of Healing", new TextStyle(FontSize: theme.FontSize, Bold: true, Color: theme.PrimaryTextColor)),
                        content: new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                        {
                            Children = new Element[]
                            {
                                new Text("Restores 50 HP on use.", new TextStyle(FontSize: theme.FontSize, Color: theme.PrimaryTextColor)),
                                SizedBox.FromSize(height: theme.Spacing * 2f),
                                new Stepper(
                                    value: _quantity,
                                    label: "Quantity",
                                    min: 1,
                                    max: 99,
                                    onChanged: v => SetState(() => _quantity = v)
                                ),
                                SizedBox.FromSize(height: theme.Spacing),
                                new Text($"Total cost: {_quantity * 25} gold", new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.SecondaryTextColor)),
                            }
                        },
                        footer: new Row(crossAxisAlignment: CrossAxisAlignment.Center, mainAxisAlignment: MainAxisAlignment.End)
                        {
                            Children = new Element[]
                            {
                                new Button("Cancel", () => SetState(() => _quantity = 1)),
                                SizedBox.FromSize(width: theme.Spacing),
                                new Button("Buy", () => { }),
                            }
                        }
                    );
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

        private sealed class BadgeStory : StatefulElement
        {
            public override State CreateState() => new BadgeStoryState();

            private sealed class BadgeStoryState : State<BadgeStory>
            {
                private int _notifications = 3;

                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Dot badge", BuildDot(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Count badge", BuildCount(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example — Inventory slot", BuildGame(theme), theme),
                        }
                    };
                }

                private static Element BuildDot(Theme theme)
                {
                    return new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = new Element[]
                        {
                            new Badge(
                                new Container(
                                    width: theme.Spacing * 5f,
                                    height: theme.Spacing * 5f,
                                    decoration: new BoxDecoration(
                                        Color: theme.SurfaceColor,
                                        Border: new Border(theme.InputBorderColor, 1f),
                                        BorderRadius: BorderRadius.All(theme.BorderRadius)
                                    )
                                )
                            ),
                            SizedBox.FromSize(width: theme.Spacing * 2f),
                            new Text("Dot badge — no count", new TextStyle(
                                FontSize: theme.FontSize,
                                Color: theme.SecondaryTextColor
                            ))
                        }
                    };
                }

                private static Element BuildCount(Theme theme)
                {
                    return new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = new Element[]
                        {
                            new Badge(
                                new Container(
                                    width: theme.Spacing * 5f,
                                    height: theme.Spacing * 5f,
                                    decoration: new BoxDecoration(
                                        Color: theme.SurfaceColor,
                                        Border: new Border(theme.InputBorderColor, 1f),
                                        BorderRadius: BorderRadius.All(theme.BorderRadius)
                                    )
                                ),
                                count: 42
                            ),
                            SizedBox.FromSize(width: theme.Spacing * 2f),
                            new Badge(
                                new Container(
                                    width: theme.Spacing * 5f,
                                    height: theme.Spacing * 5f,
                                    decoration: new BoxDecoration(
                                        Color: theme.SurfaceColor,
                                        Border: new Border(theme.InputBorderColor, 1f),
                                        BorderRadius: BorderRadius.All(theme.BorderRadius)
                                    )
                                ),
                                count: 150
                            ),
                            SizedBox.FromSize(width: theme.Spacing * 2f),
                            new Text("42 and 99+ capped", new TextStyle(
                                FontSize: theme.FontSize,
                                Color: theme.SecondaryTextColor
                            ))
                        }
                    };
                }

                private Element BuildGame(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                    {
                        Children = new Element[]
                        {
                            new Text(
                                "Inventory slots with pending notifications. Press the button to add more.",
                                new TextStyle(FontSize: theme.FontSize, Color: theme.SecondaryTextColor)
                            ),
                            SizedBox.FromSize(height: theme.Spacing * 2f),
                            new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                            {
                                Children = new Element[]
                                {
                                    BuildSlot(theme, "Sword", _notifications),
                                    SizedBox.FromSize(width: theme.Spacing),
                                    BuildSlot(theme, "Shield", 0),
                                    SizedBox.FromSize(width: theme.Spacing),
                                    BuildSlot(theme, "Potion", 99),
                                    SizedBox.FromSize(width: theme.Spacing * 3f),
                                    new Button(
                                        label: "+ Alert",
                                        onPressed: () => SetState(() => _notifications++)
                                    )
                                }
                            }
                        }
                    };
                }

                private static Element BuildSlot(Theme theme, string label, int count)
                {
                    var slot = new Container(
                        width: theme.Spacing * 7f,
                        height: theme.Spacing * 7f,
                        decoration: new BoxDecoration(
                            Color: theme.SurfaceColor,
                            Border: new Border(theme.InputBorderColor, 1f),
                            BorderRadius: BorderRadius.All(theme.BorderRadius)
                        )
                    )
                    {
                        Child = new Center
                        {
                            Child = new Text(label, new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.PrimaryTextColor
                            ))
                        }
                    };

                    if (count <= 0)
                    {
                        return slot;
                    }

                    return new Badge(slot, count: count, color: theme.SecondaryColor);
                }
            }
        }

        private sealed class AlertStory : StatefulElement
        {
            public override State CreateState() => new AlertStoryState();

            private sealed class AlertStoryState : State<AlertStory>
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
                            new Alert("Information", message: "Your settings have been saved."),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Alert("Success", message: "Character created successfully.", severity: AlertSeverity.Success),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Alert("Warning", message: "Your session will expire in 5 minutes.", severity: AlertSeverity.Warning),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Alert("Error", message: "Failed to connect to the server.", severity: AlertSeverity.Error)
                        }
                    };
                }

                private static Element BuildGame(Theme theme)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Alert(
                                "Low Health",
                                message: "Aric Stormblade is below 20% HP. Use a Health Potion before the next encounter.",
                                severity: AlertSeverity.Warning
                            ),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Alert(
                                "Quest Complete",
                                message: "You defeated the Goblin King. Reward: 500 XP and the Shadowblade.",
                                severity: AlertSeverity.Success,
                                actions: new List<(string, Action)>
                                {
                                    ("View Rewards", () => { }),
                                    ("Continue", () => { })
                                }
                            ),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Alert(
                                "Connection Lost",
                                message: "Lost connection to the game server. Progress from the last checkpoint has been saved.",
                                severity: AlertSeverity.Error,
                                actions: new List<(string, Action)>
                                {
                                    ("Reconnect", () => { })
                                }
                            )
                        }
                    };
                }
            }
        }

        private sealed class CardStory : StatefulElement
        {
            public override State CreateState() => new CardStoryState();

            private sealed class CardStoryState : State<CardStory>
            {
                public override Element Build(BuildContext context)
                {
                    var theme = ThemeConsumer.Of(context);
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section("Elevated", BuildElevated(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Outlined", BuildOutlined(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("With Header", BuildWithHeader(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("With Header and Footer", BuildWithHeaderAndFooter(theme), theme),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section("Game Example — Quest Card", BuildGameExample(theme), theme),
                        }
                    };
                }

                private static Element BuildElevated(Theme theme) =>
                    new Card(
                        content: new Text(
                            "This is an elevated card. It uses a shadow to convey depth above the surface.",
                            new TextStyle(FontSize: theme.FontSize, Color: theme.PrimaryTextColor)
                        )
                    );

                private static Element BuildOutlined(Theme theme) =>
                    new Card(
                        outlined: true,
                        content: new Text(
                            "This is an outlined card. It uses a border instead of a shadow for a flat look.",
                            new TextStyle(FontSize: theme.FontSize, Color: theme.PrimaryTextColor)
                        )
                    );

                private static Element BuildWithHeader(Theme theme) =>
                    new Card(
                        header: new Text(
                            "Card Header",
                            new TextStyle(FontSize: theme.FontSize, Bold: true, Color: theme.PrimaryTextColor)
                        ),
                        content: new Text(
                            "Card body content appears here. The header above is separated by a divider.",
                            new TextStyle(FontSize: theme.FontSize, Color: theme.PrimaryTextColor)
                        )
                    );

                private static Element BuildWithHeaderAndFooter(Theme theme) =>
                    new Card(
                        header: new Text(
                            "Card Header",
                            new TextStyle(FontSize: theme.FontSize, Bold: true, Color: theme.PrimaryTextColor)
                        ),
                        content: new Text(
                            "Card body content appears here.",
                            new TextStyle(FontSize: theme.FontSize, Color: theme.PrimaryTextColor)
                        ),
                        footer: new Row(crossAxisAlignment: CrossAxisAlignment.Center, mainAxisAlignment: MainAxisAlignment.End)
                        {
                            Children = new Element[]
                            {
                                new Button("Cancel", () => { }),
                                SizedBox.FromSize(width: theme.Spacing),
                                new Button("Confirm", () => { }),
                            }
                        }
                    );

                private static Element BuildGameExample(Theme theme) =>
                    new Row(crossAxisAlignment: CrossAxisAlignment.Start)
                    {
                        Children = new Element[]
                        {
                            new Expanded
                            {
                                Child = new Card(
                                    header: new Row(
                                        crossAxisAlignment: CrossAxisAlignment.Center,
                                        mainAxisAlignment: MainAxisAlignment.SpaceBetween
                                    )
                                    {
                                        Children = new Element[]
                                        {
                                            new Text("Defeat the Dragon", new TextStyle(FontSize: theme.FontSize, Bold: true, Color: theme.PrimaryTextColor)),
                                            new Badge(
                                                new Container(
                                                    width: theme.Spacing * 3f,
                                                    height: theme.Spacing * 3f,
                                                    decoration: new BoxDecoration(Color: theme.PrimaryColor, BorderRadius: BorderRadius.All(theme.BorderRadius))
                                                ),
                                                count: 3
                                            )
                                        }
                                    },
                                    content: new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                                    {
                                        Children = new Element[]
                                        {
                                            new Text("Travel to the Ember Peaks and slay the ancient dragon terrorizing the region.", new TextStyle(FontSize: theme.FontSize, Color: theme.PrimaryTextColor)),
                                            SizedBox.FromSize(height: theme.Spacing),
                                            new Text("Reward: 500 gold", new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.SecondaryTextColor)),
                                        }
                                    },
                                    footer: new Row(crossAxisAlignment: CrossAxisAlignment.Center, mainAxisAlignment: MainAxisAlignment.End)
                                    {
                                        Children = new Element[]
                                        {
                                            new Button("Abandon", () => { }),
                                            SizedBox.FromSize(width: theme.Spacing),
                                            new Button("Track Quest", () => { }),
                                        }
                                    }
                                )
                            },
                            SizedBox.FromSize(width: theme.Spacing * 3f),
                            new Expanded
                            {
                                Child = new Card(
                                    outlined: true,
                                    header: new Text("Gather Herbs", new TextStyle(FontSize: theme.FontSize, Bold: true, Color: theme.PrimaryTextColor)),
                                    content: new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                                    {
                                        Children = new Element[]
                                        {
                                            new Text("Collect 10 moonbloom herbs from the Verdant Forest.", new TextStyle(FontSize: theme.FontSize, Color: theme.PrimaryTextColor)),
                                            SizedBox.FromSize(height: theme.Spacing),
                                            new Text("Reward: 80 gold", new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.SecondaryTextColor)),
                                        }
                                    }
                                )
                            }
                        }
                    };
            }
        }

        private sealed class ChipStory : StatefulElement
        {
            public override State CreateState() => new ChipStoryState();

            private sealed class ChipStoryState : State<ChipStory>
            {
                private List<string> _activeFilters = new List<string> { "Warrior", "Mage", "Ranger" };

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
                            new Text("Static chip (no dismiss):",
                                new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.SecondaryTextColor)),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Chip("Epic"),
                            SizedBox.FromSize(height: theme.Spacing * 2f),
                            new Text("Colored chip:",
                                new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.SecondaryTextColor)),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Chip("Legendary", color: FrameColor.FromHex("#FFD700").WithAlpha(0.2f))
                        }
                    };
                }

                private Element BuildGame(Theme theme)
                {
                    var filterColors = new System.Collections.Generic.Dictionary<string, FrameColor>
                    {
                        { "Warrior", FrameColor.FromHex("#FF6B6B") },
                        { "Mage", FrameColor.FromHex("#7B61FF") },
                        { "Ranger", FrameColor.FromHex("#00D4AA") },
                        { "Rogue", FrameColor.FromHex("#FFD700") },
                        { "Healer", FrameColor.FromHex("#FF9F43") }
                    };

                    var chipElements = new List<Element>();
                    foreach (var filter in _activeFilters)
                    {
                        var captured = filter;
                        var color = filterColors.ContainsKey(filter)
                            ? filterColors[filter].WithAlpha(0.2f)
                            : theme.PrimaryColor.WithAlpha(0.15f);

                        if (chipElements.Count > 0)
                        {
                            chipElements.Add(SizedBox.FromSize(width: theme.Spacing));
                        }

                        chipElements.Add(new Chip(
                            captured,
                            onDeleted: () => SetState(() => _activeFilters.Remove(captured)),
                            color: color
                        ));
                    }

                    var addButtons = new List<Element>();
                    foreach (var kv in filterColors)
                    {
                        if (_activeFilters.Contains(kv.Key))
                        {
                            continue;
                        }

                        var captured = kv.Key;
                        if (addButtons.Count > 0)
                        {
                            addButtons.Add(SizedBox.FromSize(width: theme.Spacing));
                        }

                        addButtons.Add(new Button(
                            label: $"+ {captured}",
                            onPressed: () => SetState(() => _activeFilters.Add(captured))
                        ));
                    }

                    var rows = new List<Element>
                    {
                        new Text("Active class filters (tap x to remove):",
                            new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.SecondaryTextColor)),
                        SizedBox.FromSize(height: theme.Spacing)
                    };

                    if (chipElements.Count > 0)
                    {
                        rows.Add(new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = chipElements.ToArray()
                        });
                    }
                    else
                    {
                        rows.Add(new Text("No filters active.",
                            new TextStyle(FontSize: theme.FontSizeSmall, Color: theme.DisabledTextColor)));
                    }

                    if (addButtons.Count > 0)
                    {
                        rows.Add(SizedBox.FromSize(height: theme.Spacing * 2f));
                        rows.Add(new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = addButtons.ToArray()
                        });
                    }

                    return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                    {
                        Children = rows.ToArray()
                    };
                }
            }
        }
        private sealed class DialogStory : StatefulElement
        {
            public override State CreateState() => new DialogStoryState();

            private sealed class DialogStoryState : State<DialogStory>
            {
                private bool _showBasic;
                private bool _showGame;

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
                    var rows = new List<Element>
                    {
                        new Button(
                            label: "Open Dialog",
                            onPressed: () => SetState(() => _showBasic = true)
                        )
                    };

                    if (_showBasic)
                    {
                        rows.Add(new Dialog(
                            title: "Confirm",
                            content: new Text(
                                "Are you sure you want to proceed?",
                                new TextStyle(Color: theme.PrimaryTextColor)
                            ),
                            actions: new List<(string, Action)>
                            {
                                ("Cancel", () => SetState(() => _showBasic = false)),
                                ("OK", () => SetState(() => _showBasic = false))
                            },
                            onDismiss: () => SetState(() => _showBasic = false)
                        ));
                    }

                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = rows.ToArray()
                    };
                }

                private Element BuildGame(Theme theme)
                {
                    var rows = new List<Element>
                    {
                        new Button(
                            label: "Abandon Quest",
                            onPressed: () => SetState(() => _showGame = true)
                        )
                    };

                    if (_showGame)
                    {
                        rows.Add(new Dialog(
                            title: "Abandon Quest?",
                            content: new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                            {
                                Children = new Element[]
                                {
                                    new Text(
                                        "\"Into the Dark Forest\" will be removed from your quest log.",
                                        new TextStyle(Color: theme.PrimaryTextColor)
                                    ),
                                    SizedBox.FromSize(height: theme.Spacing),
                                    new Text(
                                        "All progress and collected items will be lost.",
                                        new TextStyle(
                                            FontSize: theme.FontSizeSmall,
                                            Color: theme.SecondaryTextColor
                                        )
                                    )
                                }
                            },
                            actions: new List<(string, Action)>
                            {
                                ("Keep Quest", () => SetState(() => _showGame = false)),
                                ("Abandon", () => SetState(() => _showGame = false))
                            },
                            onDismiss: () => SetState(() => _showGame = false)
                        ));
                    }

                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = rows.ToArray()
                    };
                }
            }
        }
    }
}
