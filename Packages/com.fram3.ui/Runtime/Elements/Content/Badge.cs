#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;
using StylingTheme = Fram3.UI.Styling.Theme;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// Overlays a small colored pip on the top-right corner of its child element.
    /// When <see cref="Count"/> is provided, the pip displays that number as a label.
    /// When <see cref="Count"/> is null, a compact dot is rendered instead.
    /// Typically used to indicate notifications, item quantity, or status on icons and avatars.
    /// </summary>
    public sealed class Badge : StatelessElement
    {
        /// <summary>
        /// The element the badge is attached to.
        /// </summary>
        public Element Child { get; }

        /// <summary>
        /// The numeric value displayed inside the pip.
        /// Null renders a compact dot with no label.
        /// </summary>
        public int? Count { get; }

        /// <summary>
        /// The background color of the pip.
        /// Null defaults to the theme's <see cref="Styling.Theme.ErrorColor"/>.
        /// </summary>
        public FrameColor? Color { get; }

        /// <summary>
        /// Creates a <see cref="Badge"/> element.
        /// </summary>
        /// <param name="child">The element to attach the badge to.</param>
        /// <param name="count">
        /// The count to display inside the pip. Null renders a dot badge.
        /// </param>
        /// <param name="color">
        /// The pip background color. Null uses the theme's error color.
        /// </param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Badge(
            Element child,
            int? count = null,
            FrameColor? color = null,
            Key? key = null
        ) : base(key)
        {
            Child = child;
            Count = count;
            Color = color;
        }

        /// <inheritdoc/>
        public override Element Build(BuildContext context)
        {
            var theme = ThemeConsumer.Of(context);
            var pipColor = Color ?? theme.ErrorColor;
            var pip = BuildPip(theme, pipColor);

            return new Stack
            {
                Children = new Element[]
                {
                    Child,
                    new Margin(EdgeInsets.OnlyTop(-theme.Spacing * 0.75f), pip)
                }
            };
        }

        private Element BuildPip(StylingTheme theme, FrameColor pipColor)
        {
            if (Count == null)
            {
                return new Container(
                    width: theme.Spacing,
                    height: theme.Spacing,
                    decoration: new BoxDecoration(
                        Color: pipColor,
                        BorderRadius: BorderRadius.All(theme.Spacing * 0.5f)
                    )
                );
            }

            var label = Count > 99 ? "99+" : Count.ToString()!;

            return new Container(
                height: theme.Spacing * 2f,
                padding: EdgeInsets.Symmetric(vertical: 0f, horizontal: theme.Spacing * 0.5f),
                decoration: new BoxDecoration(
                    Color: pipColor,
                    BorderRadius: BorderRadius.All(theme.Spacing)
                )
            )
            {
                Child = new Center
                {
                    Child = new Text(label, new TextStyle(
                        Color: theme.OnPrimaryColor,
                        FontSize: theme.FontSizeSmall,
                        Bold: true
                    ))
                }
            };
        }
    }
}
