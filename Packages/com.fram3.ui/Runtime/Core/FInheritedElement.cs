#nullable enable

namespace Fram3.UI.Core
{
    /// <summary>
    /// An element that propagates data down the element tree, accessible to descendants
    /// via <see cref="FBuildContext.GetInherited{T}"/> or
    /// <see cref="FBuildContext.FindInherited{T}"/>.
    /// When the parent rebuilds and provides a new inherited element of the same type,
    /// all descendants that declared a dependency via <c>DependOnInherited</c> are
    /// automatically scheduled for a rebuild -- but only when
    /// <see cref="UpdateShouldNotify"/> returns <c>true</c>.
    /// </summary>
    /// <remarks>
    /// Subclass <see cref="FInheritedElement"/> and expose the data you want to share
    /// as properties. Override <see cref="UpdateShouldNotify"/> to suppress rebuilds
    /// when the new element carries equivalent data.
    /// </remarks>
    public abstract class FInheritedElement : FSingleChildElement
    {
        /// <summary>
        /// Creates a new inherited element with an optional key.
        /// </summary>
        /// <param name="key">An optional key for reconciliation identity.</param>
        protected FInheritedElement(FKey? key = null) : base(key)
        {
        }

        /// <summary>
        /// Determines whether dependent descendants should be rebuilt after this element
        /// is updated. Called by the framework when a new element of the same type
        /// replaces this element in the tree.
        /// Return <c>false</c> to suppress rebuilds when the data has not meaningfully changed.
        /// </summary>
        /// <param name="oldElement">The previous element that this one replaced.</param>
        /// <returns>
        /// <c>true</c> if dependents should be rebuilt; <c>false</c> otherwise.
        /// </returns>
        public abstract bool UpdateShouldNotify(FInheritedElement oldElement);
    }
}