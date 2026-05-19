#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// A modal overlay dialog with a title, optional body content, and action buttons.
    /// The dialog is centered on a dimmed full-screen backdrop. Tapping the backdrop
    /// invokes <see cref="OnDismiss"/> when <see cref="BarrierDismissible"/> is true.
    /// Visibility is controlled by the caller mounting or unmounting this element.
    /// </summary>
    /// <since>2.0.0-beta.2</since>
    /// <status>live</status>
    public sealed class Dialog : StatelessElement
    {
        /// <summary>The heading text displayed at the top of the dialog panel.</summary>
        public string Title { get; }

        /// <summary>The optional body element displayed between the title and actions.</summary>
        public Element? Content { get; }

        /// <summary>
        /// Action buttons rendered at the bottom of the dialog.
        /// Each entry is a (label, callback) pair.
        /// </summary>
        public IReadOnlyList<(string Label, Action OnPressed)> Actions { get; }

        /// <summary>
        /// Callback invoked when the user requests dismissal via the backdrop or a cancel action.
        /// </summary>
        public Action? OnDismiss { get; }

        /// <summary>
        /// When true, tapping the backdrop invokes <see cref="OnDismiss"/>.
        /// Defaults to true.
        /// </summary>
        public bool BarrierDismissible { get; }

        /// <summary>
        /// Creates a <see cref="Dialog"/> element.
        /// </summary>
        /// <param name="title">The heading text. Must not be null.</param>
        /// <param name="content">The optional body element.</param>
        /// <param name="actions">Action buttons as (label, callback) pairs.</param>
        /// <param name="onDismiss">Callback invoked when dismissal is requested.</param>
        /// <param name="barrierDismissible">Whether tapping the backdrop triggers <paramref name="onDismiss"/>.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="title"/> is null.</exception>
        public Dialog(
            string title,
            Element? content = null,
            IReadOnlyList<(string Label, Action OnPressed)>? actions = null,
            Action? onDismiss = null,
            bool barrierDismissible = true,
            Key? key = null
        ) : base(key)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Content = content;
            Actions = actions ?? Array.Empty<(string, Action)>();
            OnDismiss = onDismiss;
            BarrierDismissible = barrierDismissible;
        }

        /// <inheritdoc/>
        public override Element Build(BuildContext context)
        {
            var theme = ThemeConsumer.Of(context);

            return new Modal(
                child: BuildPanel(theme),
                onDismiss: OnDismiss,
                barrierDismissible: BarrierDismissible
            );
        }

        private Element BuildPanel(Styling.Theme theme)
        {
            var panelRows = new List<Element>
            {
                new Text(Title, new TextStyle(
                    FontSize: theme.FontSizeLarge,
                    Bold: true,
                    Color: theme.PrimaryTextColor
                ))
            };

            if (Content != null)
            {
                panelRows.Add(SizedBox.FromSize(height: theme.Spacing * 1.5f));
                panelRows.Add(Content);
            }

            if (Actions.Count > 0)
            {
                panelRows.Add(SizedBox.FromSize(height: theme.Spacing * 2f));

                var buttons = new List<Element>();
                foreach (var action in Actions)
                {
                    if (buttons.Count > 0)
                    {
                        buttons.Add(SizedBox.FromSize(width: theme.Spacing));
                    }

                    buttons.Add(new Button(action.Label, action.OnPressed));
                }

                panelRows.Add(new Row(
                    mainAxisAlignment: MainAxisAlignment.End,
                    crossAxisAlignment: CrossAxisAlignment.Center
                )
                {
                    Children = buttons.ToArray()
                });
            }

            return new Center
            {
                Child = new Container(
                    decoration: new BoxDecoration(
                        Color: theme.SurfaceColor,
                        BorderRadius: BorderRadius.All(theme.BorderRadius),
                        Border: new Border(FrameColor.FromHex("#1E2235"), 1f)
                    ),
                    padding: EdgeInsets.All(theme.Spacing * 3f)
                )
                {
                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = panelRows.ToArray()
                    }
                }
            };
        }
    }
}