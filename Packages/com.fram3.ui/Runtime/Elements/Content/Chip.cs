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
    /// A compact, pill-shaped label used to represent an attribute, filter, or selection.
    /// When <see cref="OnDeleted"/> is provided a dismiss button is shown on the trailing
    /// edge; tapping it invokes the callback so the caller can remove the chip from the tree.
    /// Colors default to a tinted surface derived from the active theme's primary color.
    /// </summary>
    public sealed class Chip : StatefulElement
    {
        /// <summary>The text label displayed inside the chip.</summary>
        public string Label { get; }

        /// <summary>
        /// Callback invoked when the user taps the dismiss button.
        /// Null means no dismiss button is rendered.
        /// </summary>
        public Action? OnDeleted { get; }

        /// <summary>
        /// Background color of the chip surface.
        /// Null defaults to a tinted surface derived from the theme's primary color.
        /// </summary>
        public FrameColor? Color { get; }

        /// <summary>
        /// Creates a <see cref="Chip"/> element.
        /// </summary>
        /// <param name="label">The text to display. Must not be null.</param>
        /// <param name="onDeleted">
        /// Callback invoked when the dismiss button is tapped.
        /// Null suppresses the dismiss button entirely.
        /// </param>
        /// <param name="color">
        /// The chip background color. Null uses a primary-tinted surface from the theme.
        /// </param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="label"/> is null.</exception>
        public Chip(
            string label,
            Action? onDeleted = null,
            FrameColor? color = null,
            Key? key = null
        ) : base(key)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            OnDeleted = onDeleted;
            Color = color;
        }

        /// <inheritdoc/>
        public override Fram3.UI.Core.State CreateState() => new ChipState();

        private sealed class ChipState : State<Chip>
        {
            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);
                var bgColor = Element!.Color ?? theme.PrimaryColor.WithAlpha(0.15f);
                var labelColor = theme.PrimaryTextColor;
                var deleteColor = theme.SecondaryTextColor;

                Element? deleteButton = null;
                if (Element.OnDeleted != null)
                {
                    var callback = Element.OnDeleted;
                    deleteButton = new GestureDetector(
                        onTap: callback,
                        child: new Padding(EdgeInsets.OnlyLeft(theme.Spacing * 0.5f))
                        {
                            Child = new Text("x", new TextStyle(
                                FontSize: theme.FontSizeSmall,
                                Color: deleteColor,
                                Bold: true
                            ))
                        }
                    );
                }

                var children = deleteButton != null
                    ? new Element[]
                    {
                        new Text(Element.Label, new TextStyle(
                            FontSize: theme.FontSizeSmall,
                            Color: labelColor
                        )),
                        deleteButton
                    }
                    : new Element[]
                    {
                        new Text(Element.Label, new TextStyle(
                            FontSize: theme.FontSizeSmall,
                            Color: labelColor
                        ))
                    };

                return new Container(
                    decoration: new BoxDecoration(
                        Color: bgColor,
                        BorderRadius: BorderRadius.All(theme.BorderRadius)
                    ),
                    padding: EdgeInsets.Symmetric(
                        horizontal: theme.Spacing,
                        vertical: theme.Spacing * 0.5f
                    )
                )
                {
                    Child = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = children
                    }
                };
            }
        }
    }
}
