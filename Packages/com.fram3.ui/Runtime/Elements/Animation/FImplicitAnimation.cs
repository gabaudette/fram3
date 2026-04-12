#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Animation;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Animation
{
    /// <summary>
    /// An element that automatically animates a set of typed values whenever their
    /// targets change between builds. Each property change triggers a tween from
    /// the previous snapshot to the new target over the configured duration.
    ///
    /// Declare the properties to animate as <see cref="FAnimatedValue{T}"/> instances
    /// in the <c>values</c> list. Read the current interpolated values inside the
    /// <c>builder</c> delegate via <see cref="FImplicitAnimationSnapshot.Get{T}"/>.
    ///
    /// A single <see cref="FAnimationController"/> is shared across all properties.
    /// When any target value changes, the controller restarts from the current
    /// interpolated snapshot.
    /// </summary>
    public sealed class FImplicitAnimation : FStatefulElement
    {
        /// <summary>
        /// The list of animated values. Each entry must have a unique <see cref="IFAnimatedValue.Key"/>.
        /// </summary>
        public IReadOnlyList<IFAnimatedValue> Values { get; }

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
        /// A delegate that receives the build context and the current animated snapshot,
        /// and returns the element subtree.
        /// </summary>
        public Func<FBuildContext, FImplicitAnimationSnapshot, FElement> Builder { get; }

        /// <summary>
        /// Creates a new implicit animation element.
        /// </summary>
        /// <param name="values">
        /// The list of <see cref="FAnimatedValue{T}"/> entries to animate.
        /// Each entry must have a unique key.
        /// </param>
        /// <param name="duration">Duration of one full tween in seconds. Must be greater than zero.</param>
        /// <param name="builder">
        /// A delegate that receives the build context and the current animated snapshot.
        /// </param>
        /// <param name="curve">The easing curve to apply. Defaults to linear when null.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="values"/> or <paramref name="builder"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="duration"/> is less than or equal to zero.
        /// </exception>
        public FImplicitAnimation(
            IReadOnlyList<IFAnimatedValue> values,
            float duration,
            Func<FBuildContext, FImplicitAnimationSnapshot, FElement> builder,
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

            Values = values ?? throw new ArgumentNullException(nameof(values));
            Duration = duration;
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            Curve = curve;
        }

        /// <inheritdoc/>
        public override FState CreateState() => new FImplicitAnimationState();

        private sealed class FImplicitAnimationState : FState<FImplicitAnimation>
        {
            private FAnimationController? _controller;
            private Action<float>? _listener;
            private IReadOnlyList<IFAnimatedValue>? _previousValues;
            private float _animationProgress;

            public override void InitState()
            {
                _previousValues = Element!.Values;
                _animationProgress = 1f;
                _controller = new FAnimationController(Element.Duration, Element.Curve);
                _listener = OnTick;
                _controller.AddListener(_listener);
            }

            public override FElement Build(FBuildContext context)
            {
                var snapshot = BuildSnapshot(_previousValues!, Element!.Values, _animationProgress);
                return Element!.Builder(context, snapshot);
            }

            public override void DidUpdateElement(FStatefulElement oldElement)
            {
                var old = (FImplicitAnimation)oldElement;
                var newValues = Element!.Values;

                if (!AnyValueChanged(old.Values, newValues))
                {
                    return;
                }

                var snapshotAtCurrentProgress = BuildSnapshot(_previousValues!, old.Values, _animationProgress);
                _previousValues = SnapshotToValues(snapshotAtCurrentProgress, newValues);

                _animationProgress = 0f;

                _controller!.RemoveListener(_listener!);
                _controller.Dispose();

                _controller = new FAnimationController(Element.Duration, Element.Curve);
                _controller.AddListener(_listener!);
                _controller.Forward();
            }

            public override void Dispose()
            {
                _controller?.RemoveListener(_listener!);
                _controller?.Dispose();
                _controller = null;
                _listener = null;
            }

            private void OnTick(float value)
            {
                _animationProgress = value;
                SetState(null);
            }

            private static FImplicitAnimationSnapshot BuildSnapshot(
                IReadOnlyList<IFAnimatedValue> previousValues,
                IReadOnlyList<IFAnimatedValue> targetValues,
                float t
            )
            {
                var dict = new Dictionary<string, object>();

                foreach (var targetValue in targetValues)
                {
                    var previousValue = FindByKey(previousValues, targetValue.Key) ?? targetValue;
                    dict[targetValue.Key] = targetValue.Interpolate(previousValue, t);
                }

                return new FImplicitAnimationSnapshot(dict);
            }

            private static IReadOnlyList<IFAnimatedValue> SnapshotToValues(
                FImplicitAnimationSnapshot snapshot,
                IReadOnlyList<IFAnimatedValue> template
            )
            {
                var frozen = new IFAnimatedValue[template.Count];
                for (var i = 0; i < template.Count; i++)
                {
                    var key = template[i].Key;
                    var frozenTarget = snapshot.GetRaw(key);
                    frozen[i] = template[i].WithTarget(frozenTarget);
                }

                return frozen;
            }

            private static bool AnyValueChanged(
                IReadOnlyList<IFAnimatedValue> previous,
                IReadOnlyList<IFAnimatedValue> current
            )
            {
                foreach (var animatedValue in current)
                {
                    var prev = FindByKey(previous, animatedValue.Key);
                    if (prev == null || animatedValue.HasChangedFrom(prev))
                    {
                        return true;
                    }
                }

                return false;
            }

            private static IFAnimatedValue? FindByKey(IReadOnlyList<IFAnimatedValue> list, string key)
            {
                foreach (var animatedValue in list)
                {
                    if (animatedValue.Key == key)
                    {
                        return animatedValue;
                    }
                }

                return null;
            }
        }
    }
}