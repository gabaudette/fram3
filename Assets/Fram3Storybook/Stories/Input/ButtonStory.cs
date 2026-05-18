using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Input
{
    public sealed class ButtonStory : StatefulElement
    {
        public override State CreateState() => new ButtonStoryState();

        private sealed class ButtonStoryState : State<ButtonStory>
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
                        new Text(
                            "Enabled button:",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: 4f),
                        new Button(
                            label: "Save",
                            onPressed: () => { }
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Text(
                            "Disabled button (onPressed: null):",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: 4f),
                        new Button(label: "Disabled")
                    }
                };
            }

            private static Element BuildGame(Theme theme)
            {
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new Text(
                            "COMBAT ACTIONS",
                            style: new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Bold: true,
                                Color: theme.SecondaryColor,
                                LetterSpacing: 1.5f
                            )
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = new Element[]
                            {
                                new Expanded
                                {
                                    Child = new Button(
                                        label: "ATTACK",
                                        onPressed: () => { }
                                    )
                                },
                                SizedBox.FromSize(width: theme.Spacing),
                                new Expanded
                                {
                                    Child = new Button(
                                        label: "DEFEND",
                                        onPressed: () => { }
                                    )
                                },
                                SizedBox.FromSize(width: theme.Spacing),
                                new Expanded
                                {
                                    Child = new Button(label: "FLEE")
                                }
                            }
                        }
                    }
                };
            }
        }
    }
}