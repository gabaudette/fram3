using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Input
{
    public sealed class TextFieldStory : StatefulElement
    {
        public override State CreateState() => new TextFieldStoryState();

        private sealed class TextFieldStoryState : State<TextFieldStory>
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
                            "Empty field with placeholder:",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: 4f),
                        new TextField(placeholder: "Enter text here..."),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Text(
                            "Pre-filled field:",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: 4f),
                        new TextField(value: "Hello, Fram3!"),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Text(
                            "Read-only field:",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: 4f),
                        new TextField(
                            value: "Read-only value",
                            readOnly: true
                        )
                    }
                };
            }
        }
    }
}