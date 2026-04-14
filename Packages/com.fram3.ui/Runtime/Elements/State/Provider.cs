#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.State
{
    /// <summary>
    /// Makes a value of type <typeparamref name="T"/> available to any descendant in the
    /// element tree. Descendants retrieve it with
    /// <c>context.GetInherited&lt;Provider&lt;T&gt;&gt;().Value</c> or by using
    /// <see cref="Consumer{T}"/> / <see cref="Selector{TCubit,TState,TValue}"/>.
    /// </summary>
    /// <typeparam name="T">The type of value to provide. May be any type.</typeparam>
    /// <remarks>
    /// When the parent rebuilds and supplies a new <see cref="Provider{T}"/> carrying a
    /// different value, all descendants that depend on this provider are automatically
    /// scheduled for a rebuild via the inherited element notification mechanism.
    /// </remarks>
    public sealed class Provider<T> : InheritedElement
    {
        /// <summary>The value made available to descendants.</summary>
        public T Value { get; }

        /// <summary>
        /// Creates an <see cref="Provider{T}"/> that supplies <paramref name="value"/>
        /// to the subtree rooted at <paramref name="child"/>.
        /// </summary>
        /// <param name="value">The value to provide. May be null for reference types.</param>
        /// <param name="child">The child subtree that can access this provider. Must not be null.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when child is null.</exception>
        public Provider(T value, Element child, Key? key = null) : base(key)
        {
            Value = value;
            Child = child ?? throw new ArgumentNullException(nameof(child));
        }

        /// <inheritdoc/>
        public override bool UpdateShouldNotify(InheritedElement oldElement)
        {
            if (oldElement is not Provider<T> old)
            {
                return true;
            }

            return !Equals(Value, old.Value);
        }
    }
}