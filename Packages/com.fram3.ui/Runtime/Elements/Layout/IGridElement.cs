#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Layout
{
    /// <summary>
    /// Non-generic view of a <see cref="Grid{T}"/> used by the painter to build
    /// the native UIToolkit VE subtree without knowing the concrete item type.
    /// </summary>
    internal interface IGridElement
    {
        int ColumnCount { get; }
        float ColumnSpacing { get; }
        float RowSpacing { get; }

        /// <summary>Total number of data items.</summary>
        int ItemCount { get; }

        /// <summary>Builds the element for the item at <paramref name="index"/>.</summary>
        Element BuildItemAt(int index);
    }
}