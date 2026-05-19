using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class DraggablePanelStory : StatefulElement
    {
        public override State CreateState() => new DraggablePanelStoryState();

        private sealed class DraggablePanelStoryState : State<DraggablePanelStory>
        {
            private bool _showBasic;
            private bool _showStats;
            private bool _showInventory;

            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);

                var panels = new List<Element>();

                panels.Add(
                    new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            StoryHelpers.Section(
                                label: "Basic",
                                content: BuildBasicControls(theme),
                                theme
                            ),
                            SizedBox.FromSize(height: theme.Spacing * 3f),
                            StoryHelpers.Section(
                                label: "Game Example — Floating Panels",
                                content: BuildGameControls(theme),
                                theme
                            )
                        }
                    }
                );

                if (_showBasic)
                {
                    panels.Add(new DraggablePanel(
                        child: new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                        {
                            Children = new Element[]
                            {
                                new Text(
                                    "Drag the title bar to move this panel.",
                                    style: new TextStyle(Color: theme.PrimaryTextColor)
                                ),
                                SizedBox.FromSize(height: theme.Spacing),
                                new Text(
                                    "Close it with the button in the top-right corner.",
                                    style: new TextStyle(
                                        FontSize: theme.FontSizeSmall,
                                        Color: theme.SecondaryTextColor
                                    )
                                )
                            }
                        },
                        initialX: 300f,
                        initialY: 120f,
                        title: "Basic Panel",
                        width: 280f,
                        onClose: () => SetState(() => _showBasic = false)
                    ));
                }

                if (_showStats)
                {
                    panels.Add(new DraggablePanel(
                        child: new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                        {
                            Children = new Element[]
                            {
                                BuildStatRow("HP", "142 / 200", theme),
                                BuildStatRow("MP", "88 / 120", theme),
                                BuildStatRow("STR", "34", theme),
                                BuildStatRow("DEF", "18", theme),
                                BuildStatRow("SPD", "27", theme)
                            }
                        },
                        initialX: 80f,
                        initialY: 140f,
                        title: "Character Stats",
                        width: 240f,
                        onClose: () => SetState(() => _showStats = false)
                    ));
                }

                if (_showInventory)
                {
                    panels.Add(new DraggablePanel(
                        child: new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                        {
                            Children = new Element[]
                            {
                                BuildInventoryRow("Health Potion", "x3", theme),
                                BuildInventoryRow("Iron Sword", "x1", theme),
                                BuildInventoryRow("Leather Armor", "x1", theme),
                                BuildInventoryRow("Gold Coins", "x128", theme),
                                BuildInventoryRow("Magic Scroll", "x2", theme)
                            }
                        },
                        initialX: 360f,
                        initialY: 200f,
                        title: "Inventory",
                        width: 300f,
                        onClose: () => SetState(() => _showInventory = false)
                    ));
                }

                return new Stack
                {
                    Children = panels.ToArray()
                };
            }

            private Element BuildBasicControls(Theme theme)
            {
                return new Button(
                    label: _showBasic ? "Hide Panel" : "Show Panel",
                    onPressed: () => SetState(() => _showBasic = !_showBasic)
                );
            }

            private Element BuildGameControls(Theme theme)
            {
                return new Row(mainAxisAlignment: MainAxisAlignment.Start)
                {
                    Children = new Element[]
                    {
                        new Button(
                            label: _showStats ? "Hide Stats" : "Show Stats",
                            onPressed: () => SetState(() => _showStats = !_showStats)
                        ),
                        SizedBox.FromSize(width: theme.Spacing),
                        new Button(
                            label: _showInventory ? "Hide Inventory" : "Show Inventory",
                            onPressed: () => SetState(() => _showInventory = !_showInventory)
                        )
                    }
                };
            }

            private static Element BuildStatRow(string label, string value, Theme theme)
            {
                return new Row(mainAxisAlignment: MainAxisAlignment.SpaceBetween)
                {
                    Children = new Element[]
                    {
                        new Text(label, style: new TextStyle(Color: theme.SecondaryTextColor)),
                        new Text(value, style: new TextStyle(Color: theme.PrimaryTextColor))
                    }
                };
            }

            private static Element BuildInventoryRow(string name, string qty, Theme theme)
            {
                return new Row(mainAxisAlignment: MainAxisAlignment.SpaceBetween)
                {
                    Children = new Element[]
                    {
                        new Text(name, style: new TextStyle(Color: theme.PrimaryTextColor)),
                        new Text(qty, style: new TextStyle(Color: theme.SecondaryTextColor))
                    }
                };
            }
        }
    }
}
