#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// Wraps a single child and causes it to expand along the main axis of the
    /// parent flex container, proportional to its <see cref="Flex"/> factor.
    /// Maps to UIToolkit's <c>flexGrow</c> style property.
    /// </summary>
    public sealed class FExpanded : FSingleChildElement
    {
        /// <summary>
        /// The flex factor that controls how much of the remaining space this element
        /// claims relative to other <see cref="FExpanded"/> siblings. Defaults to 1.
        /// </summary>
        public float Flex { get; }

        /// <summary>
        /// Creates an <see cref="FExpanded"/> element.
        /// </summary>
        /// <param name="flex">The flex grow factor. Must be greater than zero. Defaults to 1.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FExpanded(float flex = 1f, FKey? key = null) : base(key)
        {
            Flex = flex;
        }
    }
}