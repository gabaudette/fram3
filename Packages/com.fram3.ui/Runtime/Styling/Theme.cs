#nullable enable

namespace Fram3.UI.Styling
{
    /// <summary>
    /// An immutable set of design tokens that define the visual language of the application.
    /// Pass a theme through the tree with <c>ThemeProvider</c> and read it in any descendant
    /// with <c>ThemeConsumer.Of(context)</c>.
    /// </summary>
    public sealed record Theme
    {
        /// <summary>The primary brand color, used for prominent interactive elements.</summary>
        public FrameColor PrimaryColor { get; init; }

        /// <summary>The secondary brand color, used for complementary accents.</summary>
        public FrameColor SecondaryColor { get; init; }

        /// <summary>The background color of the root canvas or page.</summary>
        public FrameColor BackgroundColor { get; init; }

        /// <summary>The surface color for cards, dialogs, and elevated containers.</summary>
        public FrameColor SurfaceColor { get; init; }

        /// <summary>Text or icon color intended for use on top of the primary color.</summary>
        public FrameColor OnPrimaryColor { get; init; }

        /// <summary>Color used to indicate errors and destructive actions.</summary>
        public FrameColor ErrorColor { get; init; }

        /// <summary>Default body text color.</summary>
        public FrameColor PrimaryTextColor { get; init; }

        /// <summary>Subdued text color for secondary labels and captions.</summary>
        public FrameColor SecondaryTextColor { get; init; }

        /// <summary>Text color applied to disabled controls and placeholder text.</summary>
        public FrameColor DisabledTextColor { get; init; }

        /// <summary>Border color for input fields, dropdowns, scrollbars, and other widget outlines.</summary>
        public FrameColor InputBorderColor { get; init; }

        /// <summary>Background color for slider tracks and scrollbar track areas.</summary>
        public FrameColor TrackColor { get; init; }

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
        public static Theme Default { get; } = new Theme
        {
            PrimaryColor = FrameColor.FromHex("#6200EE"),
            SecondaryColor = FrameColor.FromHex("#03DAC6"),
            BackgroundColor = FrameColor.FromHex("#FFFFFF"),
            SurfaceColor = FrameColor.FromHex("#F5F5F5"),
            OnPrimaryColor = FrameColor.FromHex("#FFFFFF"),
            ErrorColor = FrameColor.FromHex("#B00020"),
            PrimaryTextColor = FrameColor.FromHex("#212121"),
            SecondaryTextColor = FrameColor.FromHex("#757575"),
            DisabledTextColor = FrameColor.FromHex("#BDBDBD"),
            InputBorderColor = FrameColor.FromHex("#E0E0E0"),
            TrackColor = FrameColor.FromHex("#EEEEEE"),
            FontSize = 14f,
            FontSizeSmall = 12f,
            FontSizeLarge = 20f,
            BorderRadius = 4f,
            Spacing = 8f
        };
    }
}