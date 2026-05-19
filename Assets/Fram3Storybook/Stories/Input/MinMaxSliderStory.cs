using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Input
{
    public sealed class MinMaxSliderStory : StatefulElement
    {
        public override State CreateState() => new MinMaxSliderStoryState();

        private sealed class MinMaxSliderStoryState : State<MinMaxSliderStory>
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
                            "Default min-max slider (0-1):",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: 4f),
                        new MinMaxSlider(
                            minValue: 0.2f,
                            maxValue: 0.8f
                        ),
                        SizedBox.FromSize(height: theme.Spacing),
                        new Text(
                            "Min-max slider with label:",
                            style: new TextStyle(
                                Color: theme.SecondaryTextColor,
                                FontSize: theme.FontSizeSmall
                            )
                        ),
                        SizedBox.FromSize(height: 4f),
                        new MinMaxSlider(
                            minValue: 20f, maxValue: 80f,
                            lowLimit: 0f, highLimit: 100f,
                            label: "Level range filter"
                        )
                    }
                };
            }
        }
    }
}