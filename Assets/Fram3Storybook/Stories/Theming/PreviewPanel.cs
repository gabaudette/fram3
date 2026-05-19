using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Storybook.Stories.Theming
{
    public sealed class PreviewPanel : StatefulElement
    {
        private bool UseDark { get; }
        private System.Action OnToggle { get; }

        public PreviewPanel(bool useDark, System.Action onToggle)
        {
            UseDark = useDark;
            OnToggle = onToggle;
        }

        public override State CreateState() => new PreviewPanelState();

        private sealed class PreviewPanelState : State<PreviewPanel>
        {
            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);

                return new Container(
                    decoration: new BoxDecoration(
                        Color: theme.BackgroundColor,
                        BorderRadius: BorderRadius.All(theme.BorderRadius),
                        Border: new Border(
                            Color: theme.SecondaryTextColor.WithAlpha(0.2f),
                            Width: 1f
                        )
                    ),
                    padding: EdgeInsets.All(theme.Spacing * 2f)
                )
                {
                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Button(
                                label: Element!.UseDark ? "Switch to Light Theme" : "Switch to Dark Theme",
                                onPressed: Element.OnToggle
                            ),
                            SizedBox.FromSize(height: theme.Spacing * 2f),
                            new Container(
                                decoration: new BoxDecoration(
                                    Color: theme.SurfaceColor,
                                    BorderRadius: BorderRadius.All(theme.BorderRadius)
                                ),
                                padding: EdgeInsets.All(theme.Spacing * 2f)
                            )
                            {
                                Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                                {
                                    Children = new Element[]
                                    {
                                        new Text(
                                            "Primary color sample",
                                            style: new TextStyle(
                                                Color: theme.PrimaryColor,
                                                Bold: true,
                                                FontSize: theme.FontSizeLarge
                                            )
                                        ),
                                        SizedBox.FromSize(height: 6f),
                                        new Text(
                                            "Secondary text sample",
                                            style: new TextStyle(
                                                Color: theme.SecondaryTextColor,
                                                FontSize: theme.FontSize
                                            )
                                        ),
                                        SizedBox.FromSize(height: theme.Spacing),
                                        new Button(label: "Primary Action")
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }
    }
}