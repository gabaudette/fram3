#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// Makes a value of type <typeparamref name="T"/> available to any descendant in the
    /// element tree. Descendants retrieve it with
    /// <c>context.GetInherited&lt;FProvider&lt;T&gt;&gt;().Value</c> or by using
    /// <see cref="FConsumer{T}"/> / <see cref="FSelector{TCubit,TState,TValue}"/>.
    /// </summary>
    /// <typeparam name="T">The type of value to provide. May be any type.</typeparam>
    /// <remarks>
    /// When the parent rebuilds and supplies a new <see cref="FProvider{T}"/> carrying a
    /// different value, all descendants that depend on this provider are automatically
    /// scheduled for a rebuild via the inherited element notification mechanism.
    /// </remarks>
    public sealed class FProvider<T> : FInheritedElement
    {
        /// <summary>The value made available to descendants.</summary>
        public T Value { get; }

        /// <summary>
        /// Creates an <see cref="FProvider{T}"/> that supplies <paramref name="value"/>
        /// to the subtree rooted at <paramref name="child"/>.
        /// </summary>
        /// <param name="value">The value to provide. May be null for reference types.</param>
        /// <param name="child">The child subtree that can access this provider. Must not be null.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when child is null.</exception>
        public FProvider(T value, FElement child, FKey? key = null) : base(key)
        {
            Value = value;
            Child = child ?? throw new ArgumentNullException(nameof(child));
        }

        /// <inheritdoc/>
        public override bool UpdateShouldNotify(FInheritedElement oldElement)
        {
            if (oldElement is not FProvider<T> old)
            {
                return true;
            }

            return !Equals(Value, old.Value);
        }
    }
}
