#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// Centers its single child both horizontally and vertically within the
    /// available space. Maps to a UIToolkit <c>VisualElement</c> with
    /// <c>alignItems: center</c> and <c>justifyContent: center</c>.
    /// </summary>
    public sealed class FCenter : FSingleChildElement
    {
        /// <summary>
        /// Creates an <see cref="FCenter"/> element.
        /// </summary>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FCenter(FKey? key = null) : base(key)
        {
        }
    }
}
