using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class AccordionStory : StatefulElement
    {
        public override State CreateState() => new AccordionStoryState();

        private sealed class AccordionStoryState : State<AccordionStory>
        {
            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);
                
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        StoryHelpers.Section(
                            label: "Single-expand",
                            content: BuildSingle(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Multi-expand",
                            content: BuildMulti(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Game Example — Codex",
                            content: BuildGame(theme),
                            theme
                        )
                    }
                };
            }

            private static Element BuildSingle(Theme theme)
            {
                return new Accordion(
                    new List<AccordionItem>
                    {
                        new AccordionItem(
                            header: "What is fram3?",
                            content: new Text(
                                "fram3 is a retained-mode UI framework for Unity built on a virtual element tree.",
                                style: new TextStyle(
                                    FontSize: theme.FontSize,
                                    Color: theme.SecondaryTextColor
                                )
                            )
                        ),
                        new AccordionItem(
                            header: "How do themes work?",
                            content: new Text(
                                "Elements pull colors, spacing, and typography from the active ThemeData via ThemeConsumer.Of.",
                                style: new TextStyle(
                                    FontSize: theme.FontSize,
                                    Color: theme.SecondaryTextColor
                                )
                            )
                        ),
                        new AccordionItem(
                            header: "Is it production-ready?",
                            content: new Text(
                                "Currently in beta — APIs are stable but the element library is still growing.",
                                style: new TextStyle(
                                    FontSize: theme.FontSize,
                                    Color: theme.SecondaryTextColor
                                )
                            )
                        )
                    },
                    initialIndex: 0
                );
            }

            private static Element BuildMulti(Theme theme)
            {
                return new Accordion(
                    new List<AccordionItem>
                    {
                        new AccordionItem(
                            header: "Movement",
                            content: new Text(
                                "WASD or left-stick to move. Hold shift to sprint.",
                                style: new TextStyle(
                                    FontSize: theme.FontSize,
                                    Color: theme.SecondaryTextColor
                                )
                            )
                        ),
                        new AccordionItem(
                            header: "Combat",
                            content: new Text(
                                "Left-click to attack. Right-click to block. Q to dodge-roll.",
                                style: new TextStyle(
                                    FontSize: theme.FontSize,
                                    Color: theme.SecondaryTextColor
                                )
                            )
                        ),
                        new AccordionItem(
                            header: "Inventory",
                            content: new Text(
                                "Press I to open inventory. Drag items to equip or combine.",
                                style: new TextStyle(
                                    FontSize: theme.FontSize,
                                    Color: theme.SecondaryTextColor
                                )
                            )
                        )
                    },
                    allowMultiple: true,
                    initialIndex: 0
                );
            }

            private static Element BuildGame(Theme theme)
            {
                return new Accordion(
                    new List<AccordionItem>
                    {
                        new AccordionItem(
                            header: "Chapter I — The Awakening",
                            content: new Text(
                                "You wake in the ruins of Vel'Kara with no memory of how you arrived.",
                                style: new TextStyle(
                                    FontSize: theme.FontSize,
                                    Color: theme.SecondaryTextColor
                                )
                            )
                        ),
                        new AccordionItem(
                            header: "Chapter II — The Sunken Archive",
                            content: new Text(
                                "Ancient texts beneath the city hint at a weapon capable of sealing the rift.",
                                style: new TextStyle(
                                    FontSize: theme.FontSize,
                                    Color: theme.SecondaryTextColor
                                )
                            )
                        ),
                        new AccordionItem(
                            header: "Chapter III — Betrayal at the Gate",
                            content: new Text(
                                "Your guide reveals their true allegiance moments before the gate is destroyed.",
                                style: new TextStyle(
                                    FontSize: theme.FontSize,
                                    Color: theme.SecondaryTextColor
                                )
                            )
                        )
                    },
                    initialIndex: 0
                );
            }
        }
    }
}