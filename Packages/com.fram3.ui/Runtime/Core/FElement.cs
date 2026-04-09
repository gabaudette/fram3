#nullable enable
using System;
using System.Collections.Generic;

namespace Fram3.UI.Core
{
    /// <summary>
    /// The base class for all Fram3 UI descriptions.
    /// An FElement is an immutable configuration object that describes a piece of user interface.
    /// Elements are lightweight and intended to be recreated on every build cycle.
    /// The framework uses elements as blueprints to create and update the actual
    /// rendered UI through the reconciliation process.
    /// </summary>
    /// <remarks>
    /// Do not subclass <see cref="FElement"/> directly. Extend one of the provided archetypes:
    /// <see cref="FStatelessElement"/> for composition without state,
    /// <see cref="FStatefulElement"/> for composition with mutable state,
    /// <see cref="FSingleChildElement"/> for structural wrappers with one child,
    /// <see cref="FMultiChildElement"/> for layout elements with multiple children, or
    /// <see cref="FLeafElement"/> for terminal elements with no children.
    /// </remarks>
    public abstract class FElement
    {
        /// <summary>
        /// An optional key that controls how this element is matched against
        /// other elements during reconciliation. Elements with matching runtime
        /// types and keys are updated in place rather than replaced.
        /// </summary>
        public FKey? Key { get; }

        /// <summary>
        /// Creates a new element with an optional key.
        /// </summary>
        /// <param name="key">
        /// An optional key to control reconciliation identity.
        /// When null, elements are matched by position and type only.
        /// </param>
        protected FElement(FKey? key = null)
        {
            Key = key;
        }

        /// <summary>
        /// Determines whether an existing element can be updated with a new element
        /// rather than being replaced. Two elements can be updated when they share
        /// the same runtime type and the same key.
        /// </summary>
        /// <param name="oldElement">The existing element in the tree.</param>
        /// <param name="newElement">The new element to compare against.</param>
        /// <returns>True if the old element can be updated with the new element's configuration.</returns>
        public static bool CanUpdate(FElement? oldElement, FElement? newElement)
        {
            if (oldElement is null || newElement is null)
            {
                return false;
            }

            var isSameType = oldElement.GetType() == newElement.GetType();
            var bothUnkeyed = oldElement.Key == null && newElement.Key == null;
            var bothKeyedAndEqual = oldElement.Key != null && newElement.Key != null && oldElement.Key == newElement.Key;

            return isSameType && (bothUnkeyed || bothKeyedAndEqual);
        }

        /// <summary>
        /// Returns the children of this element for tree traversal.
        /// Override this in subclasses that contain child elements.
        /// The default implementation returns an empty collection.
        /// </summary>
        /// <returns>The child elements of this element.</returns>
        public virtual IReadOnlyList<FElement> GetChildren()
        {
            return Array.Empty<FElement>();
        }
    }
}