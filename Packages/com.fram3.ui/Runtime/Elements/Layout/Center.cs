#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Layout
{
    /// <summary>
    /// Centers its single child both horizontally and vertically within the
    /// available space. Maps to a UIToolkit <c>VisualElement</c> with
    /// <c>alignItems: center</c> and <c>justifyContent: center</c>.
    /// </summary>
    public sealed class Center : SingleChildElement
    {
        /// <summary>
        /// Creates an <see cref="Center"/> element.
        /// </summary>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Center(Key? key = null) : base(key)
        {
        }
    }
}