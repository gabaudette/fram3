using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Content
{
    public sealed class StepperStory : StatefulElement
    {
        public override State CreateState() => new StepperStoryState();

        private sealed class StepperStoryState : State<StepperStory>
        {
            private int _basic;
            private int _bounded = 5;
            private int _stepped;
            private int _quantity = 1;

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
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "With Min and Max",
                            content: BuildBounded(),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Custom Step",
                            content: BuildStepped(),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Disabled",
                            content: BuildDisabled(),
                            theme
                        ),
                        SizedBox.FromSize(height: theme.Spacing * 3f),
                        StoryHelpers.Section(
                            label: "Game Example — Item Quantity",
                            content: BuildGameExample(theme),
                            theme
                        )
                    }
                };
            }

            private Element BuildBasic() =>
                new Stepper(
                    value: _basic,
                    label: "Count",
                    onChanged: v => SetState(() => _basic = v)
                );

            private Element BuildBounded() =>
                new Stepper(
                    value: _bounded,
                    label: "Level (1-10)",
                    min: 1,
                    max: 10,
                    onChanged: v => SetState(() => _bounded = v)
                );

            private Element BuildStepped() =>
                new Stepper(
                    value: _stepped,
                    label: "Gold (step 50)",
                    step: 50,
                    onChanged: v => SetState(() => _stepped = v)
                );

            private static Element BuildDisabled() =>
                new Stepper(
                    value: 3,
                    label: "Locked",
                    min: 0,
                    max: 10
                );

            private Element BuildGameExample(Theme theme) =>
                new Card(
                    header: new Text(
                        "Potion of Healing",
                        style: new TextStyle(
                            FontSize: theme.FontSize,
                            Bold: true,
                            Color: theme.PrimaryTextColor
                        )
                    ),
                    content: new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                    {
                        Children = new Element[]
                        {
                            new Text(
                                "Restores 50 HP on use.",
                                style: new TextStyle(
                                    FontSize: theme.FontSize,
                                    Color: theme.PrimaryTextColor
                                )
                            ),
                            SizedBox.FromSize(height: theme.Spacing * 2f),
                            new Stepper(
                                value: _quantity,
                                label: "Quantity",
                                min: 1,
                                max: 99,
                                onChanged: v => SetState(() => _quantity = v)
                            ),
                            SizedBox.FromSize(height: theme.Spacing),
                            new Text(
                                $"Total cost: {_quantity * 25} gold",
                                style: new TextStyle(
                                    FontSize: theme.FontSizeSmall,
                                    Color: theme.SecondaryTextColor
                                )
                            )
                        }
                    },
                    footer: new Row(
                        crossAxisAlignment: CrossAxisAlignment.Center,
                        mainAxisAlignment: MainAxisAlignment.End
                    )
                    {
                        Children = new Element[]
                        {
                            new Button(
                                "Cancel",
                                onPressed: () => SetState(() => _quantity = 1)
                            ),
                            SizedBox.FromSize(width: theme.Spacing),
                            new Button(
                                "Buy",
                                onPressed: () => { }
                            )
                        }
                    }
                );
        }
    }
}