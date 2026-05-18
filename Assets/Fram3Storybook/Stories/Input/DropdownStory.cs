using Fram3.UI.Core;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Input
{
    public sealed class DropdownStory : StatefulElement
    {
        public override State CreateState() => new DropdownStoryState();

        private sealed class DropdownStoryState : State<DropdownStory>
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
                        new Dropdown(
                            options: new string[]
                            {
                                "Warrior",
                                "Mage",
                                "Rogue",
                                "Healer",
                                "Ranger"
                            },
                            selectedIndex: 0,
                            label: "Class"
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Dropdown(
                            options: new string[]
                            {
                                "1280x720",
                                "1920x1080",
                                "2560x1440",
                                "3840x2160"
                            },
                            selectedIndex: 1,
                            label: "Resolution"
                        )
                    }
                };
            }
        }
    }
}