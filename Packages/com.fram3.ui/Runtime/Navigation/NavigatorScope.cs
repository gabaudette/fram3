#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Navigation
{
    /// <summary>
    /// An inherited element that carries a <see cref="NavigatorHandle"/> through the element
    /// tree. Descendants retrieve it with
    /// <c>context.GetInherited&lt;NavigatorScope&gt;().Navigator</c>.
    /// </summary>
    public sealed class NavigatorScope : InheritedElement
    {
        /// <summary>The navigator handle available to descendants.</summary>
        public NavigatorHandle Navigator { get; }

        /// <summary>
        /// Creates an <see cref="NavigatorScope"/> that propagates <paramref name="navigator"/>
        /// to the subtree rooted at <paramref name="child"/>.
        /// </summary>
        /// <param name="navigator">The navigator handle. Must not be null.</param>
        /// <param name="child">The child subtree. Must not be null.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="navigator"/> or <paramref name="child"/> is null.
        /// </exception>
        public NavigatorScope(NavigatorHandle navigator, Element child, Key? key = null)
            : base(key)
        {
            Navigator = navigator ?? throw new System.ArgumentNullException(nameof(navigator));
            Child = child ?? throw new System.ArgumentNullException(nameof(child));
        }

        /// <inheritdoc/>
        public override bool UpdateShouldNotify(InheritedElement oldElement)
        {
            return oldElement is not NavigatorScope old
                   || !ReferenceEquals(Navigator, old.Navigator);
        }
    }
}