#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// Displays a string of text with an optional <see cref="FTextStyle"/>.
    /// This is a leaf element that maps directly to a UIToolkit <c>Label</c>.
    /// </summary>
    public sealed class FText : FLeafElement
    {
        /// <summary>The text content to display.</summary>
        public string Text { get; }

        /// <summary>
        /// The visual style applied to the text.
        /// Null inherits all values from the parent.
        /// </summary>
        public FTextStyle? Style { get; }

        /// <summary>
        /// Creates an <see cref="FText"/> element.
        /// </summary>
        /// <param name="text">The text content to display.</param>
        /// <param name="style">An optional text style. Null inherits from the parent.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FText(string text, FTextStyle? style = null, FKey? key = null) : base(key)
        {
            Text = text ?? string.Empty;
            Style = style;
        }
    }
}
