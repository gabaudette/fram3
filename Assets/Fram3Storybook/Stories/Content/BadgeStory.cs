using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class BadgeStory : StatefulElement
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
                        StoryHelpers.Section(
                            label: "Dot badge",
                            content: BuildDot(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Count badge",
                            content: BuildCount(theme),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Game Example — Inventory slot",
                            content: BuildGame(theme),
                            theme
                        )
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
                        new Text(
                            "Dot badge — no count",
                            style: new TextStyle(
                                FontSize: theme.FontSize,
                                Color: theme.SecondaryTextColor
                            )
                        )
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
                        new Text(
                            "42 and 99+ capped",
                            style: new TextStyle(
                                FontSize: theme.FontSize,
                                Color: theme.SecondaryTextColor
                            )
                        )
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
                            style: new TextStyle(
                                FontSize: theme.FontSize,
                                Color: theme.SecondaryTextColor
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 2f),
                        new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = new Element[]
                            {
                                BuildSlot(
                                    theme,
                                    label: "Sword",
                                    count: _notifications
                                ),
                                SizedBox.FromSize(width: theme.Spacing),
                                BuildSlot(
                                    theme,
                                    label: "Shield",
                                    count: 0
                                ),
                                SizedBox.FromSize(width: theme.Spacing),
                                BuildSlot(
                                    theme,
                                    label: "Potion",
                                    count: 99
                                ),
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
                        Child = new Text(
                            label,
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: theme.PrimaryTextColor
                            )
                        )
                    }
                };

                if (count <= 0)
                {
                    return slot;
                }

                return new Badge(
                    child: slot,
                    count: count,
                    color: theme.SecondaryColor
                );
            }
        }
    }
}