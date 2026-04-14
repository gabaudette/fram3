#nullable enable
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
    public class BuildContext
    {
        internal BuildContext(Node node)
        {
            Node = node ?? throw new ArgumentNullException(nameof(node));
        }

        /// <summary>
        /// The element that this context is associated with.
        /// </summary>
        public Element Element => Node.Element;

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
        public T? FindAncestorOfType<T>() where T : Element
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
        public T GetAncestorOfType<T>() where T : Element
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
        /// Walks up the element tree looking for the nearest ancestor that is an
        /// <see cref="InheritedElement"/> of type <typeparamref name="T"/>, registers
        /// this element as a dependent of that ancestor, and returns the inherited element.
        /// When the inherited element is later replaced and
        /// <see cref="InheritedElement.UpdateShouldNotify"/> returns <c>true</c>,
        /// this element's subtree will be automatically scheduled for a rebuild.
        /// Returns null when no matching ancestor exists.
        /// </summary>
        /// <typeparam name="T">The concrete inherited element type to look up.</typeparam>
        /// <returns>The nearest ancestor inherited element of type T, or null.</returns>
        public T? FindInherited<T>() where T : InheritedElement
        {
            var ancestorNode = Node.FindInheritedAncestor<T>();
            if (ancestorNode is null)
            {
                return null;
            }

            ancestorNode.AddInheritedDependent(Node);
            return (T)ancestorNode.Element;
        }

        /// <summary>
        /// Walks up the element tree looking for the nearest ancestor that is an
        /// <see cref="InheritedElement"/> of type <typeparamref name="T"/>, registers
        /// this element as a dependent, and returns the inherited element.
        /// Throws when no matching ancestor exists.
        /// </summary>
        /// <typeparam name="T">The concrete inherited element type to look up.</typeparam>
        /// <returns>The nearest ancestor inherited element of type T.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no ancestor of type <typeparamref name="T"/> exists in the tree.
        /// </exception>
        public T GetInherited<T>() where T : InheritedElement
        {
            var result = FindInherited<T>();
            if (result is null)
            {
                throw new InvalidOperationException(
                    $"No inherited element of type {typeof(T).Name} found in the element tree."
                );
            }

            return result;
        }

        /// <summary>
        /// Returns the internal node associated with this context.
        /// This is intended for framework use only and should not be called by end users.
        /// </summary>
        internal Node Node { get; }
    }
}