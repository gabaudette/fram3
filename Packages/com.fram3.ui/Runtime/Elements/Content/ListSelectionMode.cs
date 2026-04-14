#nullable enable

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// Specifies how items in an <see cref="ListView{T}"/> may be selected.
    /// </summary>
    public enum ListSelectionMode
    {
        /// <summary>No items can be selected.</summary>
        None,

        /// <summary>At most one item can be selected at a time.</summary>
        Single,

        /// <summary>Multiple items can be selected simultaneously.</summary>
        Multiple
    }
}