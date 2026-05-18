using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class AlertStory : StatefulElement
    {
        public override State CreateState() => new AlertStoryState();

        private sealed class AlertStoryState : State<AlertStory>
        {
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
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new Alert(
                            title: "Information",
                            message: "Your settings have been saved."
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Alert(
                            title: "Success",
                            message: "Character created successfully.",
                            severity: AlertSeverity.Success
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Alert(
                            title: "Warning",
                            message: "Your session will expire in 5 minutes.",
                            severity: AlertSeverity.Warning
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Alert(
                            title: "Error",
                            message: "Failed to connect to the server.",
                            severity: AlertSeverity.Error
                        )
                    }
                };
            }

            private static Element BuildGame(Theme theme)
            {
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new Alert(
                            title: "Low Health",
                            message: "Aric Stormblade is below 20% HP. " +
                                     "Use a Health Potion before the next encounter.",
                            severity: AlertSeverity.Warning
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Alert(
                            title: "Quest Complete",
                            message: "You defeated the Goblin King. Reward: 500 XP and the Shadowblade.",
                            severity: AlertSeverity.Success,
                            actions: new List<(string, Action)>
                            {
                                ("View Rewards", () => { }),
                                ("Continue", () => { })
                            }
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Alert(
                            title: "Connection Lost",
                            message: "Lost connection to the game server. " +
                                     "Progress from the last checkpoint has been saved.",
                            severity: AlertSeverity.Error,
                            actions: new List<(string, Action)>
                            {
                                ("Reconnect", () => { })
                            }
                        )
                    }
                };
            }
        }
    }
}