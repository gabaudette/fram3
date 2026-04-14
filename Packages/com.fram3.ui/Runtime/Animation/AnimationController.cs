#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Rendering;

namespace Fram3.UI.Animation
{
    /// <summary>
    /// A time-driven animation controller that progresses from 0 to 1 (or 1 to 0)
    /// over a configurable duration. The controller applies an easing curve to the
    /// raw linear progress before notifying listeners.
    ///
    /// Create an <see cref="AnimationController"/> inside <c>State.InitState</c>,
    /// call <see cref="Forward"/> or <see cref="Reverse"/> to start playback, and
    /// dispose it inside <c>State.Dispose</c>. The controller registers itself with
    /// <see cref="AnimationSystem"/> automatically and is ticked once per frame by
    /// <see cref="Renderer.Tick"/>.
    /// </summary>
    public sealed class AnimationController : IDisposable
    {
        private readonly List<Action<float>> _listeners = new();
        private float _rawProgress;
        private bool _disposed;

        /// <summary>
        /// The duration of one full forward or reverse pass in seconds. Must be greater than zero.
        /// </summary>
        public float Duration { get; }

        /// <summary>
        /// The easing curve applied to the raw linear progress before exposing <see cref="Value"/>
        /// and notifying listeners. Defaults to <see cref="Curves.Linear"/>.
        /// </summary>
        public Curve Curve { get; }

        /// <summary>
        /// The current playback status of the controller.
        /// </summary>
        public AnimationStatus Status { get; private set; } = AnimationStatus.Idle;

        /// <summary>
        /// The current curved animation value in the range [0, 1] (or beyond for overshoot curves).
        /// </summary>
        public float Value => Curve(_rawProgress);

        /// <summary>
        /// Creates an animation controller with the given duration and optional curve.
        /// </summary>
        /// <param name="duration">Duration of one full pass in seconds. Must be greater than zero.</param>
        /// <param name="curve">
        /// The easing curve to apply. Defaults to <see cref="Curves.Linear"/> when null.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="duration"/> is less than or equal to zero.
        /// </exception>
        public AnimationController(float duration, Curve? curve = null)
        {
            if (duration <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(duration),
                    "Duration must be greater than zero."
                );
            }

            Duration = duration;
            Curve = curve ?? Curves.Linear;

            AnimationSystem.Register(this);
        }

        /// <summary>
        /// Starts or resumes playback in the forward direction (toward 1).
        /// If the controller is already at 1, it restarts from 0.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown when the controller has been disposed.</exception>
        public void Forward()
        {
            ThrowIfDisposed();

            if (_rawProgress >= 1f)
            {
                _rawProgress = 0f;
            }

            Status = AnimationStatus.Forward;
        }

        /// <summary>
        /// Starts or resumes playback in the reverse direction (toward 0).
        /// If the controller is already at 0, it restarts from 1.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown when the controller has been disposed.</exception>
        public void Reverse()
        {
            ThrowIfDisposed();

            if (_rawProgress <= 0f)
            {
                _rawProgress = 1f;
            }

            Status = AnimationStatus.Reverse;
        }

        /// <summary>
        /// Stops playback and resets progress to 0 without notifying listeners.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown when the controller has been disposed.</exception>
        public void Reset()
        {
            ThrowIfDisposed();

            _rawProgress = 0f;
            Status = AnimationStatus.Idle;
        }

        /// <summary>
        /// Registers a callback to be invoked with the current curved <see cref="Value"/>
        /// on every tick during which the animation is running.
        /// </summary>
        /// <param name="listener">The callback to register. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="listener"/> is null.</exception>
        /// <exception cref="ObjectDisposedException">Thrown when the controller has been disposed.</exception>
        public void AddListener(Action<float> listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            ThrowIfDisposed();
            _listeners.Add(listener);
        }

        /// <summary>
        /// Removes a previously registered listener. If the listener is not registered, this is a no-op.
        /// </summary>
        /// <param name="listener">The callback to remove. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="listener"/> is null.</exception>
        public void RemoveListener(Action<float> listener)
        {
            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            _listeners.Remove(listener);
        }

        /// <summary>
        /// Stops playback, deregisters from the animation system, and clears all listeners.
        /// Safe to call multiple times.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            AnimationSystem.Unregister(this);
            _listeners.Clear();
            Status = AnimationStatus.Idle;
            _disposed = true;
        }

        internal void Tick(float deltaTime)
        {
            if (Status != AnimationStatus.Forward && Status != AnimationStatus.Reverse)
            {
                return;
            }

            var delta = deltaTime / Duration;

            if (Status == AnimationStatus.Forward)
            {
                _rawProgress = Math.Min(_rawProgress + delta, 1f);

                if (_rawProgress >= 1f)
                {
                    Status = AnimationStatus.Completed;
                }
            }
            else
            {
                _rawProgress = Math.Max(_rawProgress - delta, 0f);

                if (_rawProgress <= 0f)
                {
                    Status = AnimationStatus.Completed;
                }
            }

            NotifyListeners();
        }

        private void NotifyListeners()
        {
            var value = Value;
            foreach (var listener in _listeners)
            {
                listener(value);
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(AnimationController));
            }
        }
    }
}