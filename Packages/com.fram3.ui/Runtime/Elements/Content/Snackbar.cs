#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// A brief notification bar that appears temporarily at the bottom of the screen.
    /// Supports an optional action button that the user can tap.
    /// Renders itself using Container, Row, Text, and GestureDetector primitives.
    /// </summary>
    public sealed class Snackbar : StatefulElement
    {
        /// <summary>The message text displayed in the snackbar.</summary>
        public string Message { get; }

        /// <summary>
        /// The label for the optional action button.
        /// Null means no action button is shown.
        /// </summary>
        public string? ActionLabel { get; }

        /// <summary>
        /// Callback invoked when the user taps the action button.
        /// Only relevant when <see cref="ActionLabel"/> is non-null.
        /// </summary>
        public Action? OnAction { get; }

        /// <summary>
        /// How long the snackbar remains visible, in seconds.
        /// Defaults to 4 seconds.
        /// </summary>
        public float Duration { get; }

        /// <summary>
        /// Creates a <see cref="Snackbar"/> element.
        /// </summary>
        /// <param name="message">The notification text. Must not be null.</param>
        /// <param name="actionLabel">Label for the optional action button.</param>
        /// <param name="onAction">Callback invoked when the action button is tapped.</param>
        /// <param name="duration">Visible duration in seconds. Defaults to 4.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        public Snackbar(
            string message,
            string? actionLabel = null,
            Action? onAction = null,
            float duration = 4f,
            Key? key = null
        ) : base(key)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            ActionLabel = actionLabel;
            OnAction = onAction;
            Duration = duration;
        }

        /// <inheritdoc/>
        public override Fram3.UI.Core.State CreateState() => new SnackbarState();

        private sealed class SnackbarState : Fram3.UI.Core.State<Snackbar>
        {
            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);

                var bg = FrameColor.FromHex("#2D3142");
                var textColor = FrameColor.FromHex("#E2E8F0");
                var actionColor = theme.SecondaryColor;

                Element? actionButton = null;
                if (Element!.ActionLabel != null)
                {
                    actionButton = new GestureDetector(
                        onTap: Element.OnAction ?? (() => { }),
                        child: new Padding(EdgeInsets.Symmetric(horizontal: 12f, vertical: 4f))
                        {
                            Child = new Text(Element.ActionLabel, new TextStyle(
                                Color: actionColor,
                                Bold: true,
                                FontSize: theme.FontSize
                            ))
                        }
                    );
                }

                var rowChildren = actionButton != null
                    ? new Element[]
                    {
                        new Expanded
                        {
                            Child = new Text(Element.Message, new TextStyle(
                                Color: textColor,
                                FontSize: theme.FontSize
                            ))
                        },
                        actionButton,
                    }
                    : new Element[]
                    {
                        new Expanded
                        {
                            Child = new Text(Element.Message, new TextStyle(
                                Color: textColor,
                                FontSize: theme.FontSize
                            ))
                        },
                    };

                return new Container(
                    decoration: new BoxDecoration(
                        Color: bg,
                        BorderRadius: BorderRadius.All(theme.BorderRadius)
                    ),
                    padding: EdgeInsets.Symmetric(horizontal: theme.Spacing * 2f, vertical: theme.Spacing * 1.5f)
                )
                {
                    Child = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = rowChildren
                    }
                };
            }
        }
    }
}
