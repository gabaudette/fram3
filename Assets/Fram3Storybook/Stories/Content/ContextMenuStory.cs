using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class ContextMenuStory : StatefulElement
    {
        public override State CreateState() => new ContextMenuStoryState();

        private sealed class ContextMenuStoryState : State<ContextMenuStory>
        {
            private bool _showBasic;
            private bool _showGame;
            private bool _showDisabled;

            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        StoryHelpers.Section(
                            label: "Basic (right-click region)",
                            content: BuildBasic(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "With Disabled Items",
                            content: BuildDisabled(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Game Example — Unit Actions",
                            content: BuildGame(theme),
                            theme
                        )
                    }
                };
            }

            private Element BuildBasic(Theme theme)
            {
                var items = new List<Element>
                {
                    new GestureDetector(
                        onSecondaryTap: (x, y) => SetState(() => _showBasic = true),
                        child: new Container(
                            decoration: new BoxDecoration(
                                Color: theme.SurfaceColor,
                                BorderRadius: theme.BorderRadius
                            ),
                            padding: EdgeInsets.All(theme.Spacing * 2f)
                        )
                        {
                            Child = new Text(
                                "Right-click anywhere in this box to open a context menu.",
                                style: new TextStyle(Color: theme.PrimaryTextColor)
                            )
                        }
                    )
                };

                if (_showBasic)
                {
                    items.Add(new ContextMenu(
                        items: new[]
                        {
                            new ContextMenuItem("Copy", () => SetState(() => _showBasic = false)),
                            new ContextMenuItem("Paste", () => SetState(() => _showBasic = false)),
                            new ContextMenuItem("Delete", () => SetState(() => _showBasic = false))
                        },
                        x: 100f,
                        y: 80f,
                        onDismiss: () => SetState(() => _showBasic = false)
                    ));
                }

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = items.ToArray()
                };
            }

            private Element BuildDisabled(Theme theme)
            {
                var items = new List<Element>
                {
                    new GestureDetector(
                        onSecondaryTap: (x, y) => SetState(() => _showDisabled = true),
                        child: new Container(
                            decoration: new BoxDecoration(
                                Color: theme.SurfaceColor,
                                BorderRadius: theme.BorderRadius
                            ),
                            padding: EdgeInsets.All(theme.Spacing * 2f)
                        )
                        {
                            Child = new Text(
                                "Right-click to see a menu with a disabled item.",
                                style: new TextStyle(Color: theme.PrimaryTextColor)
                            )
                        }
                    )
                };

                if (_showDisabled)
                {
                    items.Add(new ContextMenu(
                        items: new[]
                        {
                            new ContextMenuItem("Edit", () => SetState(() => _showDisabled = false)),
                            new ContextMenuItem("Export", () => { }, disabled: true),
                            new ContextMenuItem("Close", () => SetState(() => _showDisabled = false))
                        },
                        x: 100f,
                        y: 80f,
                        onDismiss: () => SetState(() => _showDisabled = false)
                    ));
                }

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = items.ToArray()
                };
            }

            private Element BuildGame(Theme theme)
            {
                var items = new List<Element>
                {
                    new GestureDetector(
                        onSecondaryTap: (x, y) => SetState(() => _showGame = true),
                        child: new Container(
                            decoration: new BoxDecoration(
                                Color: theme.SurfaceColor,
                                BorderRadius: theme.BorderRadius
                            ),
                            padding: EdgeInsets.All(theme.Spacing * 2f)
                        )
                        {
                            Child = new Text(
                                "Warrior (right-click for unit actions)",
                                style: new TextStyle(Color: theme.PrimaryTextColor)
                            )
                        }
                    )
                };

                if (_showGame)
                {
                    items.Add(new ContextMenu(
                        items: new[]
                        {
                            new ContextMenuItem("Move", () => SetState(() => _showGame = false)),
                            new ContextMenuItem("Attack", () => SetState(() => _showGame = false)),
                            new ContextMenuItem("Use Item", () => SetState(() => _showGame = false)),
                            new ContextMenuItem("Fortify", () => { }, disabled: true),
                            new ContextMenuItem("End Turn", () => SetState(() => _showGame = false))
                        },
                        x: 120f,
                        y: 100f,
                        onDismiss: () => SetState(() => _showGame = false)
                    ));
                }

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = items.ToArray()
                };
            }
        }
    }
}
