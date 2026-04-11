#nullable enable

namespace Fram3.UI.Styling
{
    /// <summary>
    /// An immutable set of design tokens that define the visual language of the application.
    /// Pass a theme through the tree with <c>FThemeProvider</c> and read it in any descendant
    /// with <c>FThemeConsumer.Of(context)</c>.
    /// </summary>
    public sealed record FTheme
    {
        /// <summary>The primary brand color, used for prominent interactive elements.</summary>
        public FColor PrimaryColor { get; init; }

        /// <summary>The secondary brand color, used for complementary accents.</summary>
        public FColor SecondaryColor { get; init; }

        /// <summary>The background color of the root canvas or page.</summary>
        public FColor BackgroundColor { get; init; }

        /// <summary>The surface color for cards, dialogs, and elevated containers.</summary>
        public FColor SurfaceColor { get; init; }

        /// <summary>Text or icon color intended for use on top of the primary color.</summary>
        public FColor OnPrimaryColor { get; init; }

        /// <summary>Color used to indicate errors and destructive actions.</summary>
        public FColor ErrorColor { get; init; }

        /// <summary>Default body text color.</summary>
        public FColor PrimaryTextColor { get; init; }

        /// <summary>Subdued text color for secondary labels and captions.</summary>
        public FColor SecondaryTextColor { get; init; }

        /// <summary>Text color applied to disabled controls and placeholder text.</summary>
        public FColor DisabledTextColor { get; init; }

        /// <summary>Base font size in pixels used for body text.</summary>
        public float FontSize { get; init; }

        /// <summary>Smaller font size in pixels used for captions and helper text.</summary>
        public float FontSizeSmall { get; init; }

        /// <summary>Larger font size in pixels used for headings and titles.</summary>
        public float FontSizeLarge { get; init; }

        /// <summary>Corner radius in pixels applied to buttons, cards, and input fields.</summary>
        public float BorderRadius { get; init; }

        /// <summary>Base spacing unit in pixels used for padding and gaps between elements.</summary>
        public float Spacing { get; init; }

        /// <summary>
        /// A sensible light-theme set of default tokens. Use this as a starting point and override
        /// individual tokens with <c>with { ... }</c> syntax.
        /// </summary>
        public static FTheme Default { get; } = new FTheme
        {
            PrimaryColor = FColor.FromHex("#6200EE"),
            SecondaryColor = FColor.FromHex("#03DAC6"),
            BackgroundColor = FColor.FromHex("#FFFFFF"),
            SurfaceColor = FColor.FromHex("#F5F5F5"),
            OnPrimaryColor = FColor.FromHex("#FFFFFF"),
            ErrorColor = FColor.FromHex("#B00020"),
            PrimaryTextColor = FColor.FromHex("#212121"),
            SecondaryTextColor = FColor.FromHex("#757575"),
            DisabledTextColor = FColor.FromHex("#BDBDBD"),
            FontSize = 14f,
            FontSizeSmall = 12f,
            FontSizeLarge = 20f,
            BorderRadius = 4f,
            Spacing = 8f
        };
    }
}
