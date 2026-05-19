using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class SnackbarStory : StatefulElement
    {
        public override State CreateState() => new SnackbarStoryState();

        private sealed class SnackbarStoryState : State<SnackbarStory>
        {
            private bool _showSimple;
            private bool _showLoot;

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
                var children = new List<Element>
                {
                    new Button(
                        label: _showSimple ? "Dismiss" : "Show Snackbar",
                        onPressed: () => SetState(() => _showSimple = !_showSimple)
                    )
                };

                if (!_showSimple)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = children.ToArray()
                    };
                }

                children.Add(SizedBox.FromSize(height: theme.Spacing));
                children.Add(
                    new Snackbar(
                        message: "File saved successfully."
                    )
                );

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = children.ToArray()
                };
            }

            private Element BuildGame(Theme theme)
            {
                var children = new List<Element>
                {
                    new Button(
                        label: _showLoot ? "Dismiss" : "Pick Up Item",
                        onPressed: () => SetState(() => _showLoot = !_showLoot)
                    )
                };

                if (!_showLoot)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = children.ToArray()
                    };
                }

                children.Add(SizedBox.FromSize(height: theme.Spacing));
                children.Add(
                    new Snackbar(
                        message: "You picked up: Shadowblade (Epic)",
                        actionLabel: "Equip",
                        onAction: () => SetState(() => _showLoot = false)
                    )
                );

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = children.ToArray()
                };
            }
        }
    }
}