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
            private float _basicX;
            private float _basicY;

            private bool _showGame;
            private float _gameX;
            private float _gameY;

            private bool _showDisabled;
            private float _disabledX;
            private float _disabledY;

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
                        onSecondaryTap: (x, y) => SetState(() => { _showBasic = true; _basicX = x; _basicY = y; }),
                        child: new Container(
                            decoration: new BoxDecoration(
                                Color: theme.SurfaceColor,
                                BorderRadius: BorderRadius.All(theme.BorderRadius)
                            ),
                            padding: EdgeInsets.All(theme.Spacing * 2f)
                        )
                        {
                            Child = new Text(
                                "Right-click anywhere in this box to open a context menu.",
                                style: new TextStyle(
                                    Color: theme.PrimaryTextColor
                                )
                            )
                        }
                    )
                };

                if (_showBasic)
                {
                    items.Add(new ContextMenu(
                        items: new[]
                        {
                            new ContextMenuItem(
                                label: "Copy",
                                onTap: () => SetState(() => _showBasic = false)
                            ),
                            new ContextMenuItem(
                                label: "Paste",
                                onTap: () => SetState(() => _showBasic = false)
                            ),
                            new ContextMenuItem(
                                label: "Delete",
                                onTap: () => SetState(() => _showBasic = false)
                            )
                        },
                        x: _basicX,
                        y: _basicY,
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
                        onSecondaryTap: (x, y) => SetState(() => { _showDisabled = true; _disabledX = x; _disabledY = y; }),
                        child: new Container(
                            decoration: new BoxDecoration(
                                Color: theme.SurfaceColor,
                                BorderRadius: BorderRadius.All(theme.BorderRadius)
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
                            new ContextMenuItem(
                                label: "Edit",
                                onTap: () => SetState(() => _showDisabled = false)
                            ),
                            new ContextMenuItem(
                                label: "Export",
                                onTap: () => { }, disabled: true
                            ),
                            new ContextMenuItem(
                                label: "Close",
                                onTap: () => SetState(() => _showDisabled = false)
                            )
                        },
                        x: _disabledX,
                        y: _disabledY,
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
                        onSecondaryTap: (x, y) => SetState(() => { _showGame = true; _gameX = x; _gameY = y; }),
                        child: new Container(
                            decoration: new BoxDecoration(
                                Color: theme.SurfaceColor,
                                BorderRadius: BorderRadius.All(theme.BorderRadius)
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
                            new ContextMenuItem(
                                label: "Move",
                                onTap: () => SetState(() => _showGame = false)
                            ),
                            new ContextMenuItem(
                                label: "Attack",
                                onTap: () => SetState(() => _showGame = false)
                            ),
                            new ContextMenuItem(
                                label: "Use Item",
                                onTap: () => SetState(() => _showGame = false)
                            ),
                            new ContextMenuItem(
                                label: "Fortify",
                                onTap: () => { }, disabled: true),
                            new ContextMenuItem(
                                label: "End Turn",
                                onTap: () => SetState(() => _showGame = false)
                            )
                        },
                        x: _gameX,
                        y: _gameY,
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