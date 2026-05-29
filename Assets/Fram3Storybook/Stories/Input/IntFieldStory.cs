using Fram3.UI.Core;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Input
{
    public sealed class IntFieldStory : StatefulElement
    {
        public override State CreateState() => new IntFieldStoryState();

        private sealed class IntFieldStoryState : State<IntFieldStory>
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
                        new IntField(),
                        SizedBox.FromSize(height: theme.Spacing),
                        new IntField(
                            value: 42,
                            label: "Max Inventory Size"
                        )
                    }
                };
            }
        }
    }
}