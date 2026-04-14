#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// Attaches a tooltip text string to its single child. When the user hovers
    /// over the child, the tooltip is displayed. Maps to UIToolkit's
    /// <c>VisualElement.tooltip</c> property set on the child's native element.
    /// </summary>
    public sealed class Tooltip : SingleChildElement
    {
        /// <summary>
        /// The text shown in the tooltip on hover.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Creates an <see cref="Tooltip"/> element.
        /// </summary>
        /// <param name="message">The tooltip text.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Tooltip(string message, Key? key = null) : base(key)
        {
            Message = message;
        }
    }
}