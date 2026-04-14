#nullable enable
using System;
using Fram3.UI.Animation;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Animation
{
    /// <summary>
    /// An element that creates and drives an <see cref="AnimationController"/> and
    /// rebuilds its subtree on every animation tick. The controller's curved value is
    /// passed to the builder delegate so that descendant elements can read the
    /// current animation progress.
    ///
    /// The controller is created automatically when the element mounts and disposed
    /// when the element unmounts. Call <see cref="AnimationController.Forward"/> or
    /// <see cref="AnimationController.Reverse"/> from within the builder or from a
    /// gesture handler to start playback.
    /// </summary>
    public sealed class AnimationBuilder : StatefulElement
    {
        /// <summary>
        /// Duration of one full forward or reverse pass in seconds. Must be greater than zero.
        /// </summary>
        public float Duration { get; }

        /// <summary>
        /// The easing curve applied to the raw linear progress before passing the value
        /// to <see cref="Builder"/>. Defaults to <see cref="Curves.Linear"/> when null.
        /// </summary>
        public Curve? Curve { get; }

        /// <summary>
        /// A delegate that receives the build context and the animation controller, and
        /// returns the element subtree. The controller's <see cref="AnimationController.Value"/>
        /// reflects the current curved progress in [0, 1].
        /// </summary>
        public Func<BuildContext, AnimationController, Element> Builder { get; }

        /// <summary>
        /// Creates a new animation builder.
        /// </summary>
        /// <param name="duration">Duration of one full pass in seconds. Must be greater than zero.</param>
        /// <param name="builder">
        /// A delegate that receives the build context and the animation controller and
        /// returns the element subtree.
        /// </param>
        /// <param name="curve">
        /// The easing curve to apply. Defaults to <see cref="Curves.Linear"/> when null.
        /// </param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="builder"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="duration"/> is less than or equal to zero.
        /// </exception>
        public AnimationBuilder(
            float duration,
            Func<BuildContext, AnimationController, Element> builder,
            Curve? curve = null,
            Key? key = null
        ) : base(key)
        {
            if (duration <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(duration),
                    "Duration must be greater than zero."
                );
            }

            Duration = duration;
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            Curve = curve;
        }

        /// <inheritdoc/>
        public override Fram3.UI.Core.State CreateState() => new AnimationBuilderState();

        private sealed class AnimationBuilderState : Fram3.UI.Core.State<AnimationBuilder>
        {
            private const float DurationTolerance = 0.0001f;
            private AnimationController? _controller;
            private Action<float>? _listener;

            public override void InitState()
            {
                _controller = new AnimationController(Element!.Duration, Element.Curve);
                _listener = OnTick;
                _controller.AddListener(_listener);
            }

            public override Element Build(BuildContext context)
            {
                return Element!.Builder(context, _controller!);
            }

            public override void DidUpdateElement(StatefulElement oldElement)
            {
                var old = (AnimationBuilder)oldElement;
                if (
                    Math.Abs(old.Duration - Element!.Duration) < DurationTolerance &&
                    ReferenceEquals(old.Curve, Element.Curve)
                )
                {
                    return;
                }

                _controller!.RemoveListener(_listener!);
                var wasRunning = _controller.Status == AnimationStatus.Forward
                                 || _controller.Status == AnimationStatus.Reverse;

                var previousStatus = _controller.Status;
                _controller.Dispose();

                _controller = new AnimationController(Element.Duration, Element.Curve);
                _controller.AddListener(_listener!);

                switch (wasRunning)
                {
                    case true when previousStatus == AnimationStatus.Forward:
                        _controller.Forward();
                        break;
                    case true when previousStatus == AnimationStatus.Reverse:
                        _controller.Reverse();
                        break;
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