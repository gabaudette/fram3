using Fram3.UI.Core;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Input
{
    public sealed class CheckboxStory : StatefulElement
    {
        public override State CreateState() => new CheckboxStoryState();

        private sealed class CheckboxStoryState : State<CheckboxStory>
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
                            content: BuildBasic(),
                            theme
                        )
                    }
                };
            }

            private static Element BuildBasic()
            {
                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new Checkbox(
                            value: false,
                            label: "Accept terms and conditions"
                        ),
                        new Checkbox(
                            value: true,
                            label: "Subscribe to newsletter"
                        )
                    }
                };
            }
        }
    }
}