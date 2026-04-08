using System;
using Fram3.UI.Core.Internal;

namespace Fram3.UI.Core
{
    /// <summary>
    /// A handle to the location of an element in the element tree.
    /// Provides access to ancestor elements and framework services
    /// from within a Build method. Each element in the tree receives
    /// its own build context, which can be used to look up inherited
    /// data or access navigation and state management facilities.
    /// </summary>
    public class FBuildContext
    {
        internal FBuildContext(FNode node)
        {
            Node = node ?? throw new ArgumentNullException(nameof(node));
        }

        /// <summary>
        /// The element that this context is associated with.
        /// </summary>
        public FElement Element => Node.Element;

        /// <summary>
        /// The depth of this context in the element tree. The root element has depth 0.
        /// </summary>
        public int Depth => Node.Depth;

        /// <summary>
        /// Walks up the element tree looking for the nearest ancestor element of
        /// the specified type. Returns null if no matching ancestor is found.
        /// </summary>
        /// <typeparam name="T">The type of element to search for.</typeparam>
        /// <returns>The nearest ancestor element of type T, or null.</returns>
        public T? FindAncestorOfType<T>() where T : FElement
        {
            var current = Node.Parent;
            while (current != null)
            {
                if (current.Element is T match)
                {
                    return match;
                }
                current = current.Parent;
            }
            
            return null;
        }

        /// <summary>
        /// Walks up the element tree looking for the nearest ancestor element of
        /// the specified type. Throws if no matching ancestor is found.
        /// </summary>
        /// <typeparam name="T">The type of element to search for.</typeparam>
        /// <returns>The nearest ancestor element of type T.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no ancestor of type T exists.</exception>
        public T GetAncestorOfType<T>() where T : FElement
        {
            var result = FindAncestorOfType<T>();
            if (result is null)
            {
                throw new InvalidOperationException(
                    $"No ancestor of type {typeof(T).Name} found in the element tree."
                );
            }
            
            return result;
        }

        /// <summary>
        /// Returns the internal node associated with this context.
        /// This is intended for framework use only and should not be called by end users.
        /// </summary>
        internal FNode Node { get; }
    }
}
