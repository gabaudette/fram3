#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// The visual severity of an <see cref="Alert"/>, which controls the accent color
    /// applied to the left border and title.
    /// </summary>
    /// <status>live</status>
    public enum AlertSeverity
    {
        /// <summary>Informational message. Uses the theme's primary color.</summary>
        Info,

        /// <summary>Operation succeeded. Uses a green accent.</summary>
        Success,

        /// <summary>Something may need attention. Uses an amber accent.</summary>
        Warning,

        /// <summary>An error or destructive action. Uses the theme's error color.</summary>
        Error
    }

    /// <summary>
    /// An inline notification surface that displays a title, an optional message, and
    /// optional action buttons. The left border is tinted by <see cref="Severity"/> to
    /// communicate urgency. Unlike <see cref="Dialog"/> the alert does not use an overlay;
    /// it is placed directly in the element tree.
    /// </summary>
    public sealed class Alert : StatelessElement
    {
        /// <summary>The short heading text displayed at the top of the alert.</summary>
        public string Title { get; }

        /// <summary>The optional body text displayed below the title.</summary>
        public string? Message { get; }

        /// <summary>Controls the accent color used for the left border and title.</summary>
        public AlertSeverity Severity { get; }

        /// <summary>
        /// Optional action buttons rendered at the bottom of the alert.
        /// Each entry is a (label, callback) pair.
        /// </summary>
        public IReadOnlyList<(string Label, Action OnPressed)> Actions { get; }

        /// <summary>
        /// Creates an <see cref="Alert"/> element.
        /// </summary>
        /// <param name="title">The heading text. Must not be null.</param>
        /// <param name="message">The optional body text.</param>
        /// <param name="severity">The visual severity level. Defaults to <see cref="AlertSeverity.Info"/>.</param>
        /// <param name="actions">Optional action buttons as (label, callback) pairs.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="title"/> is null.</exception>
        public Alert(
            string title,
            string? message = null,
            AlertSeverity severity = AlertSeverity.Info,
            IReadOnlyList<(string Label, Action OnPressed)>? actions = null,
            Key? key = null
        ) : base(key)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Message = message;
            Severity = severity;
            Actions = actions ?? Array.Empty<(string, Action)>();
        }

        /// <inheritdoc/>
        public override Element Build(BuildContext context)
        {
            var theme = ThemeConsumer.Of(context);
            var accentColor = ResolveAccentColor(theme);

            var contentRows = new List<Element>
            {
                new Text(
                    Title,
                    style: new TextStyle(
                        FontSize: theme.FontSize,
                        Bold: true,
                        Color: accentColor
                    )
                )
            };

            if (Message != null)
            {
                contentRows.Add(SizedBox.FromSize(height: theme.Spacing * 0.5f));
                contentRows.Add(
                    new Text(
                        Message,
                        style: new TextStyle(
                            FontSize: theme.FontSize,
                            Color: theme.PrimaryTextColor
                        )
                    )
                );
            }

            if (Actions.Count <= 0)
            {
                return new Container(
                    decoration: new BoxDecoration(
                        Color: accentColor.WithAlpha(0.08f),
                        BorderRadius: BorderRadius.All(theme.BorderRadius),
                        Border: new Border(accentColor, 1f)
                    ),
                    padding: EdgeInsets.All(theme.Spacing * 1.5f)
                )
                {
                    Child = new Row(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new Container(
                                width: 3f,
                                decoration: new BoxDecoration(
                                    Color: accentColor,
                                    BorderRadius: BorderRadius.All(theme.BorderRadius)
                                )
                            ),
                            SizedBox.FromSize(width: theme.Spacing),
                            new Expanded
                            {
                                Child = new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                                {
                                    Children = contentRows.ToArray()
                                }
                            }
                        }
                    }
                };
            }

            contentRows.Add(SizedBox.FromSize(height: theme.Spacing));
            var buttons = new List<Element>();

            foreach (var action in Actions)
            {
                if (buttons.Count > 0)
                {
                    buttons.Add(SizedBox.FromSize(width: theme.Spacing));
                }

                buttons.Add(new Button(action.Label, action.OnPressed));
            }

            contentRows.Add(new Row(crossAxisAlignment: CrossAxisAlignment.Center)
            {
                Children = buttons.ToArray()
            });

            return new Container(
                decoration: new BoxDecoration(
                    Color: accentColor.WithAlpha(0.08f),
                    BorderRadius: BorderRadius.All(theme.BorderRadius),
                    Border: new Border(accentColor, 1f)
                ),
                padding: EdgeInsets.All(theme.Spacing * 1.5f)
            )
            {
                Child = new Row(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        new Container(
                            width: 3f,
                            decoration: new BoxDecoration(
                                Color: accentColor,
                                BorderRadius: BorderRadius.All(theme.BorderRadius)
                            )
                        ),
                        SizedBox.FromSize(width: theme.Spacing),
                        new Expanded
                        {
                            Child = new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                            {
                                Children = contentRows.ToArray()
                            }
                        }
                    }
                }
            };
        }

        private FrameColor ResolveAccentColor(Styling.Theme theme)
        {
            return Severity switch
            {
                AlertSeverity.Success => FrameColor.FromHex("#22C55E"),
                AlertSeverity.Warning => FrameColor.FromHex("#F59E0B"),
                AlertSeverity.Error => theme.ErrorColor,
                _ => theme.PrimaryColor
            };
        }
    }
}