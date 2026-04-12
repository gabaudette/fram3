#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Animation;
using Fram3.UI.Core;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Animation
{
    /// <summary>
    /// A container element whose visual properties animate implicitly when they change
    /// between builds. Any property that differs from its previous value will smoothly
    /// tween to the new target over <see cref="Duration"/>.
    ///
    /// This is a convenience wrapper over <see cref="FImplicitAnimation"/> for the
    /// common case of animating container decoration, size, and padding.
    /// </summary>
    public sealed class FAnimatedContainer : FStatelessElement
    {
        private const string KeyDecoration = "decoration";
        private const string KeyWidth = "width";
        private const string KeyHeight = "height";
        private const string KeyPadding = "padding";

        /// <summary>
        /// The visual decoration to animate toward.
        /// </summary>
        public FBoxDecoration? Decoration { get; }

        /// <summary>
        /// The target width in logical pixels.
        /// </summary>
        public float? Width { get; }

        /// <summary>
        /// The target height in logical pixels.
        /// </summary>
        public float? Height { get; }

        /// <summary>
        /// The target inner padding.
        /// </summary>
        public FEdgeInsets? Padding { get; }

        /// <summary>
        /// Duration of one full tween in seconds. Must be greater than zero.
        /// </summary>
        public float Duration { get; }

        /// <summary>
        /// The easing curve applied to the animation progress. Defaults to
        /// <see cref="FCurves.Linear"/> when null.
        /// </summary>
        public FCurve? Curve { get; }

        /// <summary>
        /// The child element rendered inside this container.
        /// </summary>
        public FElement Child { get; }

        /// <summary>
        /// Creates a new animated container.
        /// </summary>
        /// <param name="duration">Duration of one full tween in seconds. Must be greater than zero.</param>
        /// <param name="child">The child element to render inside this container.</param>
        /// <param name="decoration">Optional visual decoration to animate toward.</param>
        /// <param name="width">Optional target width in logical pixels.</param>
        /// <param name="height">Optional target height in logical pixels.</param>
        /// <param name="padding">Optional target inner padding.</param>
        /// <param name="curve">The easing curve to apply. Defaults to linear when null.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="child"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="duration"/> is less than or equal to zero.
        /// </exception>
        public FAnimatedContainer(
            float duration,
            FElement child,
            FBoxDecoration? decoration = null,
            float? width = null,
            float? height = null,
            FEdgeInsets? padding = null,
            FCurve? curve = null,
            FKey? key = null
        ) : base(key)
        {
            if (duration <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(duration),
                    "Duration must be greater than zero."
                );
            }

            Duration = duration;
            Child = child ?? throw new ArgumentNullException(nameof(child));
            Decoration = decoration;
            Width = width;
            Height = height;
            Padding = padding;
            Curve = curve;
        }

        /// <inheritdoc/>
        public override FElement Build(FBuildContext context)
        {
            var values = BuildValues();

            return new FImplicitAnimation(
                values,
                Duration,
                (_, snapshot) => new FContainer(
                    decoration: snapshot.Get<FBoxDecoration?>(KeyDecoration),
                    width: snapshot.Get<float?>(KeyWidth),
                    height: snapshot.Get<float?>(KeyHeight),
                    padding: snapshot.Get<FEdgeInsets?>(KeyPadding)
                )
                {
                    Child = Child
                },
                Curve
            );
        }

        private IReadOnlyList<IFAnimatedValue> BuildValues() =>
            new IFAnimatedValue[]
            {
                new FAnimatedValue<FBoxDecoration?>(
                    KeyDecoration,
                    Decoration,
                    InterpolateNullableDecoration
                ),
                new FAnimatedValue<float?>(
                    KeyWidth,
                    Width,
                    FLerp.NullableFloat
                ),
                new FAnimatedValue<float?>(
                    KeyHeight,
                    Height,
                    FLerp.NullableFloat
                ),
                new FAnimatedValue<FEdgeInsets?>(
                    KeyPadding,
                    Padding,
                    InterpolateNullableEdgeInsets
                )
            };

        private static FBoxDecoration? InterpolateNullableDecoration(
            FBoxDecoration? a,
            FBoxDecoration? b,
            float t
        )
        {
            if (a == null && b == null)
            {
                return null;
            }

            var from = a ?? new FBoxDecoration();
            var to = b ?? new FBoxDecoration();
            return FLerp.BoxDecoration(from, to, t);
        }

        private static FEdgeInsets? InterpolateNullableEdgeInsets(
            FEdgeInsets? a,
            FEdgeInsets? b,
            float t
        )
        {
            if (a == null && b == null)
            {
                return null;
            }

            var from = a ?? new FEdgeInsets(0f, 0f, 0f, 0f);
            var to = b ?? new FEdgeInsets(0f, 0f, 0f, 0f);
            return FLerp.EdgeInsets(from, to, t);
        }
    }
}