#nullable enable
namespace Fram3.UI.Core
{
    /// <summary>
    /// An element that composes other elements but holds no mutable state.
    /// Override the Build method to describe the UI subtree this element represents.
    /// The framework calls Build whenever the parent rebuilds, and the returned
    /// element tree is reconciled against the previous output.
    /// </summary>
    public abstract class StatelessElement : Element
    {
        /// <summary>
        /// Creates a new stateless element with an optional key.
        /// </summary>
        /// <param name="key">An optional key for reconciliation identity.</param>
        protected StatelessElement(Key? key = null) : base(key)
        {
        }

        /// <summary>
        /// Describes the UI subtree that this element represents.
        /// Called by the framework whenever a rebuild is needed.
        /// </summary>
        /// <param name="context">
        /// The build context providing access to the element's position
        /// in the tree and ancestor lookup capabilities.
        /// </param>
        /// <returns>An element describing the UI subtree.</returns>
        public abstract Element Build(BuildContext context);

        /// <summary>
        /// Called by the framework before rebuilding this element when its parent
        /// rebuilds. Return <c>false</c> to skip the rebuild and keep the existing
        /// subtree. Return <c>true</c> (the default) to rebuild as normal.
        /// Override this to avoid rebuilding when the new element description is
        /// equivalent to the previous one.
        /// </summary>
        /// <param name="oldElement">The previous element description.</param>
        /// <param name="newElement">The incoming element description from the parent.</param>
        /// <returns><c>true</c> to rebuild; <c>false</c> to skip.</returns>
        public virtual bool ShouldRebuild(StatelessElement oldElement, StatelessElement newElement)
        {
            return true;
        }
    }
}