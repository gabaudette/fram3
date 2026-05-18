using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class ChipStory : StatefulElement
    {
        public override State CreateState() => new ChipStoryState();

        private sealed class ChipStoryState : State<ChipStory>
        {
            private readonly List<string> _activeFilters = new List<string>
            {
                "Warrior",
                "Mage",
                "Ranger"
            };

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
                            label: "Game Example",
                            content: BuildGame(theme),
                            theme
                        )
                    }
                };
            }

            private static Element BuildBasic(Theme theme)
            {
                return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                {
                    Children = new Element[]
                    {
                        new Text(
                            "Static chip (no dismiss):",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Chip("Epic"),
                        SizedBox.FromSize(height: theme.Spacing * 2f),
                        new Text(
                            "Colored chip:",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.SecondaryTextColor
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Chip(
                            "Legendary",
                            color: FrameColor.FromHex("#FFD700").WithAlpha(0.2f)
                        )
                    }
                };
            }

            private Element BuildGame(Theme theme)
            {
                var filterColors = new Dictionary<string, FrameColor>
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
                foreach (var (color, _) in filterColors)
                {
                    if (_activeFilters.Contains(color))
                    {
                        continue;
                    }

                    if (addButtons.Count > 0)
                    {
                        addButtons.Add(SizedBox.FromSize(width: theme.Spacing));
                    }

                    addButtons.Add(
                        new Button(
                            label: $"+ {color}",
                            onPressed: () => SetState(() => _activeFilters.Add(color))
                        )
                    );
                }

                var rows = new List<Element>
                {
                    new Text(
                        "Active class filters (tap x to remove):",
                        style: new TextStyle(
                            FontSize: theme.FontSizeSmall,
                            Color: theme.SecondaryTextColor
                        )
                    ),
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
                    rows.Add(
                        new Text(
                            "No filters active.",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.DisabledTextColor
                            )
                        )
                    );
                }

                if (addButtons.Count <= 0)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                    {
                        Children = rows.ToArray()
                    };
                }

                rows.Add(SizedBox.FromSize(height: theme.Spacing * 2f));
                rows.Add(new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                {
                    Children = addButtons.ToArray()
                });

                return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                {
                    Children = rows.ToArray()
                };
            }
        }
    }
}