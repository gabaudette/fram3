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
    public sealed class DialogStory : StatefulElement
    {
        public override State CreateState() => new DialogStoryState();

        private sealed class DialogStoryState : State<DialogStory>
        {
            private bool _showBasic;
            private bool _showGame;

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

            private Element BuildBasic(Theme theme)
            {
                var rows = new List<Element>
                {
                    new Button(
                        label: "Open Dialog",
                        onPressed: () => SetState(() => _showBasic = true)
                    )
                };

                if (_showBasic)
                {
                    rows.Add(
                        new Dialog(
                            title: "Confirm",
                            content: new Text(
                                "Are you sure you want to proceed?",
                                style: new TextStyle(Color: theme.PrimaryTextColor)
                            ),
                            actions: new List<(string, Action)>
                            {
                                ("Cancel", () => SetState(() => _showBasic = false)),
                                ("OK", () => SetState(() => _showBasic = false))
                            },
                            onDismiss: () => SetState(() => _showBasic = false)
                        )
                    );
                }

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = rows.ToArray()
                };
            }

            private Element BuildGame(Theme theme)
            {
                var rows = new List<Element>
                {
                    new Button(
                        label: "Abandon Quest",
                        onPressed: () => SetState(() => _showGame = true)
                    )
                };

                if (_showGame)
                {
                    rows.Add(new Dialog(
                        title: "Abandon Quest?",
                        content: new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                        {
                            Children = new Element[]
                            {
                                new Text(
                                    "\"Into the Dark Forest\" will be removed from your quest log.",
                                    style: new TextStyle(
                                        Color: theme.PrimaryTextColor
                                    )
                                ),
                                SizedBox.FromSize(height: theme.Spacing),
                                new Text(
                                    "All progress and collected items will be lost.",
                                    style: new TextStyle(
                                        FontSize: theme.FontSizeSmall,
                                        Color: theme.SecondaryTextColor
                                    )
                                )
                            }
                        },
                        actions: new List<(string, Action)>
                        {
                            ("Keep Quest", () => SetState(() => _showGame = false)),
                            ("Abandon", () => SetState(() => _showGame = false))
                        },
                        onDismiss: () => SetState(() => _showGame = false)
                    ));
                }

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = rows.ToArray()
                };
            }
        }
    }
}