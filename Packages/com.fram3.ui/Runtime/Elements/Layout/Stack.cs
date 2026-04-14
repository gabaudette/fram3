#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Layout
{
    /// <summary>
    /// Stacks its children on top of each other using absolute positioning.
    /// Children are painted in order; the last child appears on top.
    /// Maps to a UIToolkit <c>VisualElement</c> with <c>position: absolute</c>
    /// applied to each child.
    /// </summary>
    public sealed class Stack : MultiChildElement
    {
        /// <summary>
        /// Creates an <see cref="Stack"/> element.
        /// </summary>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Stack(Key? key = null) : base(key)
        {
        }
    }
}
