#nullable enable
namespace Fram3.UI.Core
{
    /// <summary>
    /// An element that has mutable state persisted across rebuilds.
    /// The framework creates the state object once via CreateState,
    /// and preserves it even when the element description is recreated.
    /// Use this when a piece of UI needs to track information that changes
    /// over time, such as user input, animation progress, or toggle state.
    /// </summary>
    public abstract class FStatefulElement : FElement
    {
        /// <summary>
        /// Creates a new stateful element with an optional key.
        /// </summary>
        /// <param name="key">An optional key for reconciliation identity.</param>
        protected FStatefulElement(FKey? key = null) : base(key)
        {
        }

        /// <summary>
        /// Creates the mutable state object for this element.
        /// Called exactly once when the element is first mounted into the tree.
        /// The returned state object persists across rebuilds until the element
        /// is unmounted.
        /// </summary>
        /// <returns>A new state instance for this element.</returns>
        public abstract FState CreateState();
    }
}
