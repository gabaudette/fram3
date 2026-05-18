using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class ListViewStory : StatefulElement
    {
        public override State CreateState() => new ListViewStoryState();

        private sealed class ListViewStoryState : State<ListViewStory>
        {
            private const int PageSize = 8;
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
                var pageEnd = Math.Min(pageStart + PageSize, filtered.Count);
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

            private static readonly Character[] Characters =
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
                "All",
                "Warrior",
                "Mage",
                "Rogue",
                "Healer",
                "Ranger",
                "Paladin",
                "Assassin",
                "Berserker"
            };


            private IReadOnlyList<Character> FilterCharacters()
            {
                var result = new List<Character>();
                foreach (var character in Characters)
                {
                    if (_classFilter != "All" && character.Class != _classFilter)
                    {
                        continue;
                    }

                    if (
                        _searchQuery.Length > 0 &&
                        !character.Name.ToLower().Contains(_searchQuery.ToLower())
                    )
                    {
                        continue;
                    }

                    result.Add(character);
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
                            selectedIndex: Array.IndexOf(ClassFilters, _classFilter),
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

            private static Element BuildCharacterRow(Character character, Theme theme)
            {
                return new Margin(
                    new EdgeInsets(0f, 0f, 4f, 0f),
                    child: new Container(
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
                                            new Text(
                                                character.Name,
                                                style: new TextStyle(
                                                    Bold: true,
                                                    FontSize: theme.FontSize,
                                                    Color: theme.PrimaryTextColor
                                                )
                                            ),
                                            new Text(
                                                character.Class,
                                                style: new TextStyle(
                                                    FontSize: theme.FontSizeSmall,
                                                    Color: theme.SecondaryTextColor
                                                )
                                            )
                                        }
                                    }
                                },
                                new Text(
                                    $"Lv {character.Level}",
                                    style: new TextStyle(
                                        FontSize: theme.FontSize,
                                        Bold: true,
                                        Color: theme.SecondaryColor
                                    )
                                )
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
                        new Text(
                            $"Page {currentPage + 1} / {totalPages}",
                            style: new TextStyle(
                                FontSize: theme.FontSize,
                                Color: theme.SecondaryTextColor
                            )
                        ),
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