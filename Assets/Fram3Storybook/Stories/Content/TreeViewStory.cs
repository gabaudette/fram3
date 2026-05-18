#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class TreeViewStory : StatefulElement
    {
        public override State CreateState() => new TreeViewStoryState();

        private sealed class TreeViewStoryState : State<TreeViewStory>
        {
            private string _lastTapped = "—";

            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        StoryHelpers.Section(
                            label: "Basic",
                            content: BuildBasic(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "With Icons",
                            content: BuildWithIcons(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Pre-expanded",
                            content: BuildPreExpanded(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Game Example — Skill Tree",
                            content: BuildGameExample(theme),
                            theme
                        ),
                    }
                };
            }

            private static Element BuildBasic(Theme theme) =>
                new TreeView(
                    nodes: new[]
                    {
                        new TreeViewNode("Weapons", new[]
                        {
                            new TreeViewNode("Swords"),
                            new TreeViewNode("Axes"),
                            new TreeViewNode("Bows")
                        }),
                        new TreeViewNode("Armor", new[]
                        {
                            new TreeViewNode("Helmets"),
                            new TreeViewNode("Chestplates"),
                            new TreeViewNode("Boots")
                        }),
                        new TreeViewNode("Consumables")
                    }
                );

            private static Element BuildWithIcons(Theme theme) =>
                new TreeView(
                    nodes: new[]
                    {
                        new TreeViewNode("Quests", new[]
                        {
                            new TreeViewNode("Into the Dark Forest", icon: "!"),
                            new TreeViewNode("The Lost Artifact", icon: "!"),
                            new TreeViewNode("Dragon's Lair", icon: "★")
                        }, icon: "Q"),
                        new TreeViewNode("Characters", new[]
                        {
                            new TreeViewNode("Warrior", icon: "W"),
                            new TreeViewNode("Mage", icon: "M"),
                            new TreeViewNode("Rogue", icon: "R")
                        }, icon: "C"),
                        new TreeViewNode("Settings", icon: "S")
                    },
                    indent: 24f
                );

            private static Element BuildPreExpanded(Theme theme) =>
                new TreeView(
                    nodes: new[]
                    {
                        new TreeViewNode("World Map", new[]
                        {
                            new TreeViewNode("Ashenvale", new[]
                            {
                                new TreeViewNode("The Ruins"),
                                new TreeViewNode("Moonwell Clearing")
                            }, initiallyExpanded: true),
                            new TreeViewNode("Ironforge Peaks", new[]
                            {
                                new TreeViewNode("Summit Pass"),
                                new TreeViewNode("Miner's Camp")
                            })
                        }, initiallyExpanded: true)
                    }
                );

            private Element BuildGameExample(Theme theme)
            {
                var nodes = new[]
                {
                    new TreeViewNode("Combat", new[]
                    {
                        new TreeViewNode("Slash (Rank 1)"),
                        new TreeViewNode("Power Strike (Rank 2)"),
                        new TreeViewNode("Whirlwind (Rank 3)", new[]
                        {
                            new TreeViewNode("Bladestorm (Rank 4)")
                        })
                    }, icon: "⚔", initiallyExpanded: true),
                    new TreeViewNode("Defence", new[]
                    {
                        new TreeViewNode("Shield Block (Rank 1)"),
                        new TreeViewNode("Iron Skin (Rank 2)")
                    }, icon: "🛡"),
                    new TreeViewNode("Utility", new[]
                    {
                        new TreeViewNode("Sprint (Rank 1)"),
                        new TreeViewNode("Rally (Rank 2)")
                    }, icon: "✦")
                };

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new TreeView(
                            nodes: nodes,
                            onNodeTap: n => SetState(() => _lastTapped = n.Label)
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 2f),
                        new Text(
                            $"Selected skill: {_lastTapped}",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor
                            )
                        )
                    }
                };
            }
        }
    }
}
