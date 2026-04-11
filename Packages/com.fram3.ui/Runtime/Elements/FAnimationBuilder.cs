#nullable enable
using System;
using Fram3.UI.Animation;
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// An element that creates and drives an <see cref="FAnimationController"/> and
    /// rebuilds its subtree on every animation tick. The controller's curved value is
    /// passed to the builder delegate so that descendant elements can read the
    /// current animation progress.
    ///
    /// The controller is created automatically when the element mounts and disposed
    /// when the element unmounts. Call <see cref="FAnimationController.Forward"/> or
    /// <see cref="FAnimationController.Reverse"/> from within the builder or from a
    /// gesture handler to start playback.
    /// </summary>
    public sealed class FAnimationBuilder : FStatefulElement
    {
        /// <summary>
        /// Duration of one full forward or reverse pass in seconds. Must be greater than zero.
        /// </summary>
        public float Duration { get; }

        /// <summary>
        /// The easing curve applied to the raw linear progress before passing the value
        /// to <see cref="Builder"/>. Defaults to <see cref="FCurves.Linear"/> when null.
        /// </summary>
        public FCurve? Curve { get; }

        /// <summary>
        /// A delegate that receives the build context and the animation controller, and
        /// returns the element subtree. The controller's <see cref="FAnimationController.Value"/>
        /// reflects the current curved progress in [0, 1].
        /// </summary>
        public Func<FBuildContext, FAnimationController, FElement> Builder { get; }

        /// <summary>
        /// Creates a new animation builder.
        /// </summary>
        /// <param name="duration">Duration of one full pass in seconds. Must be greater than zero.</param>
        /// <param name="builder">
        /// A delegate that receives the build context and the animation controller and
        /// returns the element subtree.
        /// </param>
        /// <param name="curve">
        /// The easing curve to apply. Defaults to <see cref="FCurves.Linear"/> when null.
        /// </param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="builder"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="duration"/> is less than or equal to zero.
        /// </exception>
        public FAnimationBuilder(
            float duration,
            Func<FBuildContext, FAnimationController, FElement> builder,
            FCurve? curve = null,
            FKey? key = null) : base(key)
        {
            if (duration <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(duration),
                    "Duration must be greater than zero.");
            }

            Duration = duration;
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            Curve = curve;
        }

        /// <inheritdoc/>
        public override FState CreateState() => new FAnimationBuilderState();

        private sealed class FAnimationBuilderState : FState<FAnimationBuilder>
        {
            private FAnimationController? _controller;
            private Action<float>? _listener;

            public override void InitState()
            {
                _controller = new FAnimationController(Element!.Duration, Element.Curve);
                _listener = OnTick;
                _controller.AddListener(_listener);
            }

            public override FElement Build(FBuildContext context)
            {
                return Element!.Builder(context, _controller!);
            }

            public override void DidUpdateElement(FStatefulElement oldElement)
            {
                var old = (FAnimationBuilder)oldElement;
                if (old.Duration == Element!.Duration && ReferenceEquals(old.Curve, Element.Curve))
                {
                    return;
                }

                _controller!.RemoveListener(_listener!);
                var wasRunning = _controller.Status == FAnimationStatus.Forward
                    || _controller.Status == FAnimationStatus.Reverse;
                var previousStatus = _controller.Status;
                _controller.Dispose();

                _controller = new FAnimationController(Element.Duration, Element.Curve);
                _controller.AddListener(_listener!);

                if (wasRunning && previousStatus == FAnimationStatus.Forward)
                {
                    _controller.Forward();
                }
                else if (wasRunning && previousStatus == FAnimationStatus.Reverse)
                {
                    _controller.Reverse();
                }
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
                SetState(null);
            }
        }
    }
}
