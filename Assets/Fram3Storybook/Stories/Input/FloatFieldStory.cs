using Fram3.UI.Core;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Input
{
    public sealed class FloatFieldStory : StatefulElement
    {
        public override State CreateState() => new FloatFieldStoryState();

        private sealed class FloatFieldStoryState : State<FloatFieldStory>
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
                        new FloatField(),
                        SizedBox.FromSize(height: theme.Spacing),
                        new FloatField(
                            value: 1.5f,
                            label: "Move Speed Multiplier"
                        )
                    }
                };
            }
        }
    }
}