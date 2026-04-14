#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// Displays a string of text with an optional <see cref="TextStyle"/>.
    /// This is a leaf element that maps directly to a UIToolkit <c>Label</c>.
    /// </summary>
    public sealed class Text : LeafElement
    {
        /// <summary>The text content to display.</summary>
        public string Content { get; }

        /// <summary>
        /// The visual style applied to the text.
        /// Null inherits all values from the parent.
        /// </summary>
        public TextStyle? Style { get; }

        /// <summary>
        /// Creates an <see cref="Text"/> element.
        /// </summary>
        /// <param name="text">The text content to display.</param>
        /// <param name="style">An optional text style. Null inherits from the parent.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Text(string? text, TextStyle? style = null, Key? key = null) : base(key)
        {
            Content = text ?? string.Empty;
            Style = style;
        }
    }
}