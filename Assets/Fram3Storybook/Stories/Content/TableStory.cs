#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class TableStory : StatefulElement
    {
        public override State CreateState() => new TableStoryState();

        private sealed class TableStoryState : State<TableStory>
        {
            // ReSharper disable once NotAccessedField.Local
            private string _lastSelected = "—";

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
                        StoryHelpers.Section(
                            label: "Sortable",
                            content: BuildSortable(),
                            theme
                        ),
                        StoryHelpers.Section(
                            label: "Selectable",
                            content: BuildSelectable(),
                            theme
                        ),
                        StoryHelpers.Section(
                            label: "Leaderboard",
                            content: BuildLeaderboard(),
                            theme
                        )
                    }
                };
            }

            private sealed class Spell
            {
                public string Name { get; }
                public string School { get; }
                public string Damage { get; }

                public Spell(string name, string school, string damage)
                {
                    Name = name;
                    School = school;
                    Damage = damage;
                }
            }

            private static IReadOnlyList<Spell> Spells() => new[]
            {
                new Spell("Fireball", "Evocation", "8d6"),
                new Spell("Ice Lance", "Conjuration", "4d8"),
                new Spell("Lightning Bolt", "Evocation", "8d6"),
                new Spell("Acid Arrow", "Conjuration", "4d4"),
                new Spell("Magic Missile", "Evocation", "3d4")
            };

            private Element BuildBasic() =>
                new Table<Spell>(
                    columns: new[]
                    {
                        new TableColumn<Spell>(header: "Spell", cellText: spell => spell.Name, sortable: false),
                        new TableColumn<Spell>(header: "School", cellText: spell => spell.School, sortable: false),
                        new TableColumn<Spell>(header: "Damage", cellText: spell => spell.Damage, sortable: false,
                            width: 100f)
                    },
                    rows: Spells(),
                    stripedRows: true
                );

            private sealed class Item
            {
                public string Name { get; }
                public string Type { get; }
                public int Value { get; }
                public int Weight { get; }

                public Item(string name, string type, int value, int weight)
                {
                    Name = name;
                    Type = type;
                    Value = value;
                    Weight = weight;
                }
            }

            private static IReadOnlyList<Item> Inventory() => new[]
            {
                new Item("Iron Sword", "Weapon", 150, 4),
                new Item("Shield of Dawn", "Armor", 320, 12),
                new Item("Health Potion", "Consumable", 50, 1),
                new Item("Elven Bow", "Weapon", 480, 3),
                new Item("Mana Crystal", "Consumable", 200, 1),
                new Item("Plate Helm", "Armor", 210, 7)
            };

            private Element BuildSortable() =>
                new Table<Item>(
                    columns: new[]
                    {
                        new TableColumn<Item>(
                            header: "Name",
                            cellText: item => item.Name
                        ),
                        new TableColumn<Item>(
                            header: "Type",
                            cellText: item => item.Type
                        ),
                        new TableColumn<Item>(
                            header: "Value (g)",
                            cellText: item => item.Value.ToString(),
                            width: 100f
                        ),
                        new TableColumn<Item>(
                            header: "Weight",
                            cellText: item => item.Weight.ToString(),
                            width: 80f
                        )
                    },
                    rows: Inventory(),
                    stripedRows: true
                );

            private Element BuildSelectable() =>
                new Table<Item>(
                    columns: new[]
                    {
                        new TableColumn<Item>(
                            header: "Name",
                            cellText: item => item.Name
                        ),
                        new TableColumn<Item>(
                            header: "Type",
                            cellText: item => item.Type
                        ),
                        new TableColumn<Item>(
                            header: "Value (g)",
                            cellText: item => item.Value.ToString(),
                            width: 100f
                        )
                    },
                    rows: Inventory(),
                    onRowSelected: row => { SetState(() => _lastSelected = row.Name); },
                    stripedRows: false
                );


            private sealed class Player
            {
                public int Rank { get; }
                public string Name { get; }
                public string Class { get; }
                public int Score { get; }
                public string KD { get; }

                public Player(int rank, string name, string cls, int score, string kd)
                {
                    Rank = rank;
                    Name = name;
                    Class = cls;
                    Score = score;
                    KD = kd;
                }
            }

            private static IReadOnlyList<Player> Leaderboard() => new[]
            {
                new Player(1, "Aethon", "Mage", 9840, "18.2"),
                new Player(2, "Seraph", "Rogue", 8710, "14.7"),
                new Player(3, "Grimveil", "Warrior", 7650, "9.1"),
                new Player(4, "Lyra", "Ranger", 6990, "11.4"),
                new Player(5, "Duskfang", "Warlock", 6120, "8.8"),
                new Player(6, "Torvik", "Paladin", 5430, "6.3"),
                new Player(7, "Niara", "Druid", 4870, "7.0"),
                new Player(8, "Caelyx", "Bard", 3950, "5.5")
            };

            private Element BuildLeaderboard() =>
                new Table<Player>(
                    columns: new[]
                    {
                        new TableColumn<Player>(
                            header: "#",
                            cellText: player => player.Rank.ToString(),
                            sortable: false,
                            width: 40f
                        ),
                        new TableColumn<Player>(
                            header: "Player",
                            cellText: player => player.Name,
                            sortable: false
                        ),
                        new TableColumn<Player>(
                            header: "Class",
                            cellText: player => player.Class
                        ),
                        new TableColumn<Player>(
                            header: "Score",
                            cellText: player => player.Score.ToString()),
                        new TableColumn<Player>(
                            header: "K/D",
                            cellText: player => player.KD,
                            sortable: false,
                            width: 70f
                        )
                    },
                    rows: Leaderboard(),
                    onRowSelected: _ => { },
                    stripedRows: true
                );
        }
    }
}