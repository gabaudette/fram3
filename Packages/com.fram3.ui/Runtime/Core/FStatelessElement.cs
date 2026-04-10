#nullable enable
namespace Fram3.UI.Core
{
    /// <summary>
    /// An element that composes other elements but holds no mutable state.
    /// Override the Build method to describe the UI subtree this element represents.
    /// The framework calls Build whenever the parent rebuilds, and the returned
    /// element tree is reconciled against the previous output.
    /// </summary>
    public abstract class FStatelessElement : FElement
    {
        /// <summary>
        /// Creates a new stateless element with an optional key.
        /// </summary>
        /// <param name="key">An optional key for reconciliation identity.</param>
        protected FStatelessElement(FKey? key = null) : base(key)
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
        public abstract FElement Build(FBuildContext context);
    }
}