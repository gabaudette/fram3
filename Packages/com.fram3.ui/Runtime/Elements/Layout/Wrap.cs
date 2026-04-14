#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Layout
{
    /// <summary>
    /// Lays out its children in a horizontal sequence that wraps onto the next line
    /// when there is not enough space. Useful for tag clouds, item grids, and ability bars.
    /// Maps to a UIToolkit <c>VisualElement</c> with <c>FlexDirection.Row</c> and
    /// <c>FlexWrap.Wrap</c>.
    /// </summary>
    public sealed class Wrap : MultiChildElement
    {
        /// <summary>
        /// Creates an <see cref="Wrap"/> element.
        /// </summary>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Wrap(Key? key = null) : base(key)
        {
        }
    }
}