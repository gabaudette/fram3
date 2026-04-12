#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.State
{
    /// <summary>
    /// Reads the value from the nearest ancestor <see cref="FProvider{T}"/> and passes it
    /// to a builder function. Rebuilds whenever that value changes.
    /// </summary>
    /// <typeparam name="T">The type of value to consume from the nearest provider.</typeparam>
    public sealed class FConsumer<T> : FStatelessElement
    {
        /// <summary>
        /// The builder function that receives the current value and the build context
        /// and returns an element describing the UI.
        /// </summary>
        public Func<FBuildContext, T, FElement> Builder { get; }

        /// <summary>
        /// Creates an <see cref="FConsumer{T}"/>.
        /// </summary>
        /// <param name="builder">
        /// A function that receives the current provided value and returns the UI subtree.
        /// Must not be null.
        /// </param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when builder is null.</exception>
        public FConsumer(Func<FBuildContext, T, FElement> builder, FKey? key = null) : base(key)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc/>
        public override FElement Build(FBuildContext context)
        {
            var provider = context.GetInherited<FProvider<T>>();
            return Builder(context, provider.Value);
        }
    }
}