#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// Predefined size steps for an <see cref="Avatar"/>.
    /// The pixel value of each step is derived from the active theme's
    /// <c>Spacing</c> token at render time.
    /// </summary>
    public enum AvatarSize
    {
        /// <summary>Small avatar — 4× theme spacing (32 px at default spacing).</summary>
        Small,

        /// <summary>Medium avatar — 5× theme spacing (40 px at default spacing). Default.</summary>
        Medium,

        /// <summary>Large avatar — 7× theme spacing (56 px at default spacing).</summary>
        Large
    }

    /// <summary>
    /// A circular avatar element that displays either an image, a text initials
    /// placeholder, or an icon placeholder inside a clipped round container.
    ///
    /// <para>
    /// Exactly one of <see cref="Source"/>, <see cref="Initials"/>, or
    /// <see cref="IconSource"/> / <see cref="IconSvgPath"/> should be provided.
    /// When more than one is set the priority order is:
    /// image source → icon → initials → blank circle.
    /// </para>
    /// </summary>
    public sealed class Avatar : StatefulElement
    {
        /// <summary>
        /// A <c>UnityEngine.Texture2D</c> or <c>UnityEngine.Sprite</c> to display
        /// inside the avatar circle. Takes priority over all other content.
        /// </summary>
        public object? Source { get; }

        /// <summary>
        /// One or two characters shown as centered text when no image or icon is provided.
        /// Typically, the user's initials, e.g. <c>"AB"</c>.
        /// </summary>
        public string? Initials { get; }

        /// <summary>
        /// A <c>UnityEngine.Texture2D</c> or <c>UnityEngine.Sprite</c> used as an icon
        /// placeholder when no <see cref="Source"/> image is provided.
        /// </summary>
        public object? IconSource { get; }

        /// <summary>
        /// An SVG resource path used as an icon placeholder when no <see cref="Source"/>
        /// image is provided.
        /// </summary>
        public string? IconSvgPath { get; }

        /// <summary>
        /// Background color of the circle. When null, <c>theme.PrimaryColor</c> is used
        /// for the initials variant and <c>theme.SurfaceColor</c> for image/icon variants.
        /// </summary>
        public FrameColor? BackgroundColor { get; }

        /// <summary>
        /// Foreground color applied to initials text and icon tint.
        /// When null, <c>theme.OnPrimaryColor</c> is used for the initials variant
        /// and <c>theme.PrimaryTextColor</c> for the icon variant.
        /// </summary>
        public FrameColor? ForegroundColor { get; }

        /// <summary>
        /// Optional border drawn around the avatar circle.
        /// When null, no border is drawn.
        /// </summary>
        public Border? Ring { get; }

        /// <summary>The display size of the avatar. Defaults to <see cref="AvatarSize.Medium"/>.</summary>
        public AvatarSize Size { get; }

        /// <summary>
        /// Creates an <see cref="Avatar"/> element.
        /// </summary>
        /// <param name="source">
        /// Optional image source (<c>Texture2D</c> or <c>Sprite</c>). Takes highest priority.
        /// </param>
        /// <param name="initials">
        /// Optional initials string (1-2 chars) shown when no image or icon is provided.
        /// </param>
        /// <param name="iconSource">
        /// Optional icon image source used when no <paramref name="source"/> is set.
        /// </param>
        /// <param name="iconSvgPath">
        /// Optional SVG path for an icon used when no <paramref name="source"/> is set.
        /// </param>
        /// <param name="backgroundColor">
        /// Optional circle background color override.
        /// </param>
        /// <param name="foregroundColor">
        /// Optional initials/icon color override.
        /// </param>
        /// <param name="ring">Optional border ring drawn around the circle.</param>
        /// <param name="size">Avatar size step. Defaults to <see cref="AvatarSize.Medium"/>.</param>
        /// <param name="key">Optional reconciliation key.</param>
        public Avatar(
            object? source = null,
            string? initials = null,
            object? iconSource = null,
            string? iconSvgPath = null,
            FrameColor? backgroundColor = null,
            FrameColor? foregroundColor = null,
            Border? ring = null,
            AvatarSize size = AvatarSize.Medium,
            Key? key = null
        ) : base(key)
        {
            Source = source;
            Initials = initials;
            IconSource = iconSource;
            IconSvgPath = iconSvgPath;
            BackgroundColor = backgroundColor;
            ForegroundColor = foregroundColor;
            Ring = ring;
            Size = size;
        }

        /// <inheritdoc/>
        public override Fram3.UI.Core.State CreateState() => new AvatarState();

        private sealed class AvatarState : State<Avatar>
        {
            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);
                var element = Element!;
                var diameter = Diameter(element.Size, theme);
                var radius = diameter / 2f;

                var hasImage = element.Source != null;
                var hasIcon = element.IconSource != null || element.IconSvgPath != null;
                var hasInitials = !string.IsNullOrEmpty(element.Initials);

                FrameColor bgColor;
                Element inner;

                if (hasImage)
                {
                    bgColor = element.BackgroundColor ?? theme.SurfaceColor;
                    inner = new FrameImage(
                        source: element.Source,
                        width: diameter,
                        height: diameter
                    );
                }
                else if (hasIcon)
                {
                    bgColor = element.BackgroundColor ?? theme.SurfaceColor;
                    var iconSize = diameter * 0.5f;
                    inner = new Icon(
                        source: element.IconSource,
                        svgPath: element.IconSvgPath,
                        width: iconSize,
                        height: iconSize
                    );
                }
                else if (hasInitials)
                {
                    bgColor = element.BackgroundColor ?? theme.PrimaryColor;
                    var fgColor = element.ForegroundColor ?? theme.OnPrimaryColor;
                    inner = new Text(
                        element.Initials!,
                        style: new TextStyle(
                            FontSize: InitialsFontSize(element.Size, theme),
                            Bold: true,
                            Color: fgColor,
                            ResetPadding: true,
#if !FRAM3_PURE_TESTS && !FRAM3_DOC_BUILD
                            TextAlign: UnityEngine.TextAnchor.MiddleCenter
#else
                            TextAlign: 4
#endif
                        )
                    );
                }
                else
                {
                    bgColor = element.BackgroundColor ?? theme.SurfaceColor;
                    inner = SizedBox.FromSize();
                }

                return new Container(
                    width: diameter,
                    height: diameter,
                    centerChild: true,
                    decoration: new BoxDecoration(
                        Color: bgColor,
                        BorderRadius: BorderRadius.All(radius),
                        Border: element.Ring
                    )
                )
                {
                    Child = inner
                };
            }

            private static float Diameter(AvatarSize size, Styling.Theme theme) =>
                size switch
                {
                    AvatarSize.Small => theme.Spacing * 4f,
                    AvatarSize.Large => theme.Spacing * 7f,
                    _ => theme.Spacing * 5f
                };

            private static float InitialsFontSize(AvatarSize size, Styling.Theme theme) =>
                size switch
                {
                    AvatarSize.Small => theme.FontSizeSmall,
                    AvatarSize.Large => theme.FontSizeLarge,
                    _ => theme.FontSize
                };
        }
    }
}