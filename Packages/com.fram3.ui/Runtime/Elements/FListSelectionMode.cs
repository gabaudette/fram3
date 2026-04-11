#nullable enable

namespace Fram3.UI.Elements
{
    /// <summary>
    /// Specifies how items in an <see cref="FListView{T}"/> may be selected.
    /// </summary>
    public enum FListSelectionMode
    {
        /// <summary>No items can be selected.</summary>
        None,

        /// <summary>At most one item can be selected at a time.</summary>
        Single,

        /// <summary>Multiple items can be selected simultaneously.</summary>
        Multiple
    }
}
