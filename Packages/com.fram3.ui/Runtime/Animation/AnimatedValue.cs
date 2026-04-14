#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Elements.Animation;

namespace Fram3.UI.Animation
{
    /// <summary>
    /// Non-generic interface implemented by all <see cref="AnimatedValue{T}"/> instances.
    /// Used by <see cref="ImplicitAnimationSnapshot"/> internally to store and interpolate
    /// heterogeneous animated values without knowing their concrete types.
    /// </summary>
    public interface IFAnimatedValue
    {
        /// <summary>
        /// The key that identifies this value within an <see cref="ImplicitAnimation"/>.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Returns true when the target value differs from <paramref name="other"/>'s target.
        /// Used by <see cref="ImplicitAnimation"/> to detect whether any property changed.
        /// </summary>
        bool HasChangedFrom(IFAnimatedValue other);

        /// <summary>
        /// Creates an interpolated snapshot entry for this value, starting from
        /// <paramref name="previous"/> and ending at the current target, at position
        /// <paramref name="t"/>.
        /// </summary>
        object Interpolate(IFAnimatedValue previous, float t);

        /// <summary>
        /// Returns a new <see cref="IFAnimatedValue"/> of the same type and key,
        /// but with its target replaced by <paramref name="frozenTarget"/>.
        /// Used internally to capture the mid-tween snapshot as a new baseline
        /// when a property changes while already animating.
        /// </summary>
        IFAnimatedValue WithTarget(object frozenTarget);
    }

    /// <summary>
    /// A typed animated value that pairs a target value with a lerp function.
    /// Pass instances of this type in the <c>values</c> list of
    /// <see cref="ImplicitAnimation"/>. When the target changes between builds,
    /// the framework automatically tweens from the previous value to the new target.
    ///
    /// Read the current interpolated result inside the builder via
    /// <see cref="ImplicitAnimationSnapshot.Get{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of value to animate.</typeparam>
    public sealed class AnimatedValue<T> : IFAnimatedValue
    {
        private readonly Func<T, T, float, T> _lerp;

        /// <inheritdoc/>
        public string Key { get; }

        /// <summary>
        /// The current target value for this animated property.
        /// </summary>
        public T Target { get; }

        /// <summary>
        /// Creates a new animated value.
        /// </summary>
        /// <param name="key">
        /// A string key that uniquely identifies this value within the
        /// <see cref="ImplicitAnimation"/>'s values list.
        /// </param>
        /// <param name="target">The current target value.</param>
        /// <param name="lerp">
        /// A function that interpolates between two values of type <typeparamref name="T"/>.
        /// The signature is <c>(from, to, t) => interpolated</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="key"/> or <paramref name="lerp"/> is null.
        /// </exception>
        public AnimatedValue(string key, T target, Func<T, T, float, T> lerp)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Target = target;
            _lerp = lerp ?? throw new ArgumentNullException(nameof(lerp));
        }

        /// <inheritdoc/>
        public bool HasChangedFrom(IFAnimatedValue other)
        {
            if (other is not AnimatedValue<T> typed)
            {
                return true;
            }

            return !EqualityComparer<T>.Default.Equals(Target, typed.Target);
        }

        /// <inheritdoc/>
        public object Interpolate(IFAnimatedValue previous, float t)
        {
            var from = previous is AnimatedValue<T> typed ? typed.Target : Target;
            return _lerp(from, Target, t)!;
        }

        /// <inheritdoc/>
        public IFAnimatedValue WithTarget(object frozenTarget) =>
            new AnimatedValue<T>(Key, (T)frozenTarget, _lerp);
    }
}