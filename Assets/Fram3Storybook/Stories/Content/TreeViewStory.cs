#nullable enable
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
                            content: BuildBasic(),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "With Icons",
                            content: BuildWithIcons(),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Pre-expanded",
                            content: BuildPreExpanded(),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Game Example — Skill Tree",
                            content: BuildGameExample(theme),
                            theme
                        )
                    }
                };
            }

            private static Element BuildBasic() =>
                new TreeView(
                    nodes: new[]
                    {
                        new TreeViewNode(
                            label: "Weapons",
                            children: new[]
                            {
                                new TreeViewNode(label: "Swords"),
                                new TreeViewNode(label: "Axes"),
                                new TreeViewNode(label: "Bows")
                            }
                        ),
                        new TreeViewNode(
                            label: "Armor",
                            children: new[]
                            {
                                new TreeViewNode(label: "Helmets"),
                                new TreeViewNode(label: "Chestplates"),
                                new TreeViewNode(label: "Boots")
                            }
                        ),
                        new TreeViewNode(label: "Consumables")
                    }
                );

            private static Element BuildWithIcons() =>
                new TreeView(
                    nodes: new[]
                    {
                        new TreeViewNode(
                            label: "Quests",
                            children: new[]
                            {
                                new TreeViewNode(
                                    label: "Into the Dark Forest",
                                    icon: "!"
                                ),
                                new TreeViewNode(
                                    label: "The Lost Artifact",
                                    icon: "!"
                                ),
                                new TreeViewNode(
                                    label: "Dragon's Lair",
                                    icon: "★"
                                )
                            },
                            icon: "Q"
                        ),
                        new TreeViewNode(
                            label: "Characters",
                            children: new[]
                            {
                                new TreeViewNode(
                                    label: "Warrior",
                                    icon: "W"
                                ),
                                new TreeViewNode
                                (
                                    label: "Mage",
                                    icon: "M"
                                ),
                                new TreeViewNode(
                                    label: "Rogue",
                                    icon: "R"
                                )
                            },
                            icon: "C"
                        ),
                        new TreeViewNode(
                            label: "Settings",
                            icon: "S"
                        )
                    },
                    indent: 24f
                );

            private static Element BuildPreExpanded() =>
                new TreeView(
                    nodes: new[]
                    {
                        new TreeViewNode(
                            label: "World Map",
                            children: new[]
                            {
                                new TreeViewNode(
                                    label: "Ashenvale",
                                    children: new[]
                                    {
                                        new TreeViewNode(label: "The Ruins"),
                                        new TreeViewNode(label: "Moonwell Clearing")
                                    },
                                    initiallyExpanded: true
                                ),
                                new TreeViewNode(
                                    label: "Ironforge Peaks",
                                    children: new[]
                                    {
                                        new TreeViewNode(label: "Summit Pass"),
                                        new TreeViewNode(label: "Miner's Camp")
                                    }
                                )
                            },
                            initiallyExpanded: true
                        )
                    }
                );

            private Element BuildGameExample(Theme theme)
            {
                var nodes = new[]
                {
                    new TreeViewNode(
                        label: "Combat",
                        children: new[]
                        {
                            new TreeViewNode(label: "Slash (Rank 1)"),
                            new TreeViewNode(label: "Power Strike (Rank 2)"),
                            new TreeViewNode(
                                label: "Whirlwind (Rank 3)",
                                children: new[]
                                {
                                    new TreeViewNode(label: "Bladestorm (Rank 4)")
                                }
                            )
                        },
                        icon: "⚔",
                        initiallyExpanded: true
                    ),
                    new TreeViewNode(
                        label: "Defence",
                        children: new[]
                        {
                            new TreeViewNode(label: "Shield Block (Rank 1)"),
                            new TreeViewNode(label: "Iron Skin (Rank 2)")
                        },
                        icon: "🛡"
                    ),
                    new TreeViewNode(
                        label: "Utility",
                        children: new[]
                        {
                            new TreeViewNode(label: "Sprint (Rank 1)"),
                            new TreeViewNode(label: "Rally (Rank 2)")
                        },
                        icon: "✦"
                    )
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