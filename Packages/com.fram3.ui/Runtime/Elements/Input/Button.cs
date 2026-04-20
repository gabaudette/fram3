#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Input
{
    /// <summary>
    /// A tappable button with a text label. Renders using theme-aware primitives.
    /// Supports hover and disabled visual states.
    /// </summary>
    public sealed class Button : StatefulElement
    {
        /// <summary>The text label displayed on the button.</summary>
        public string Label { get; }

        /// <summary>
        /// The callback invoked when the button is pressed.
        /// Null means the button is disabled.
        /// </summary>
        public Action? OnPressed { get; }

        /// <summary>
        /// Creates a <see cref="Button"/> element.
        /// </summary>
        /// <param name="label">The text label to display.</param>
        /// <param name="onPressed">The callback invoked on press. Null means disabled.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Button(string? label, Action? onPressed = null, Key? key = null) : base(key)
        {
            Label = label ?? string.Empty;
            OnPressed = onPressed;
        }

        /// <inheritdoc/>
        public override Fram3.UI.Core.State CreateState() => new ButtonState();

        private sealed class ButtonState : Fram3.UI.Core.State<Button>
        {
            private bool _hovered;

            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);
                var disabled = Element!.OnPressed == null;

                FrameColor bgColor;
                FrameColor borderColor;
                FrameColor textColor;

                if (disabled)
                {
                    bgColor = theme.DisabledTextColor.WithAlpha(0.12f);
                    borderColor = theme.DisabledTextColor.WithAlpha(0.3f);
                    textColor = theme.DisabledTextColor;
                }
                else if (_hovered)
                {
                    bgColor = theme.PrimaryColor;
                    borderColor = theme.PrimaryColor;
                    textColor = theme.OnPrimaryColor;
                }
                else
                {
                    bgColor = theme.PrimaryColor.WithAlpha(0.12f);
                    borderColor = theme.PrimaryColor.WithAlpha(0.6f);
                    textColor = theme.PrimaryColor;
                }

                return new GestureDetector(
                    onTap: disabled ? null : Element.OnPressed,
                    onPointerEnter: disabled ? null : (() => SetState(() => _hovered = true)),
                    onPointerExit: disabled ? null : (() => SetState(() => _hovered = false)),
                    child: new Container(
                        padding: EdgeInsets.Symmetric(
                            vertical: theme.Spacing,
                            horizontal: theme.Spacing * 2f
                        ),
                        decoration: new BoxDecoration(
                            Color: bgColor,
                            Border: new Border(borderColor, 1f),
                            BorderRadius: BorderRadius.All(theme.BorderRadius)
                        )
                    )
                    {
                        Child = new Center
                        {
                            Child = new Text(Element.Label, new TextStyle(
                                Color: textColor,
                                FontSize: theme.FontSize,
                                Bold: true
                            ))
                        }
                    }
                );
            }
        }
    }
}
