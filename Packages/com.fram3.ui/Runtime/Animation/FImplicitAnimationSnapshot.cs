#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Elements;

namespace Fram3.UI.Animation
{
    /// <summary>
    /// An immutable snapshot of the current interpolated values produced by
    /// <see cref="FImplicitAnimation"/> on each animation tick.
    /// Use <see cref="Get{T}"/> inside the builder delegate to read the
    /// current animated value for a given key.
    /// </summary>
    public sealed class FImplicitAnimationSnapshot
    {
        private readonly IReadOnlyDictionary<string, object> _values;

        internal FImplicitAnimationSnapshot(IReadOnlyDictionary<string, object> values)
        {
            _values = values;
        }

        /// <summary>
        /// Returns the current interpolated value for the animated property identified
        /// by <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">The type of the animated property.</typeparam>
        /// <param name="key">The key provided when the <see cref="FAnimatedValue{T}"/> was created.</param>
        /// <returns>The current interpolated value.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when no value with <paramref name="key"/> exists in the snapshot.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Thrown when the stored value cannot be cast to <typeparamref name="T"/>.
        /// </exception>
        public T Get<T>(string key) => (T)_values[key];

        internal object GetRaw(string key) => _values[key];
    }
}