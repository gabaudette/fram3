#nullable enable
using System.Collections.Generic;
using Fram3.UI.Rendering;

namespace Fram3.UI.Animation
{
    /// <summary>
    /// Global registry that tracks all live <see cref="AnimationController"/> instances
    /// and advances them each frame. <see cref="Renderer"/> calls <see cref="Tick"/> once
    /// per frame before flushing the rebuild scheduler.
    /// </summary>
    public static class AnimationSystem
    {
        private static readonly List<AnimationController> _controllers = new();
        private static readonly List<AnimationController> _tickBuffer = new();

        internal static void Register(AnimationController controller)
        {
            _controllers.Add(controller);
        }

        internal static void Unregister(AnimationController controller)
        {
            _controllers.Remove(controller);
        }

        /// <summary>
        /// Advances all registered animation controllers by <paramref name="deltaTime"/> seconds.
        /// Called automatically by <see cref="Renderer.Tick"/>.
        /// </summary>
        /// <param name="deltaTime">Elapsed time in seconds since the previous frame.</param>
        public static void Tick(float deltaTime)
        {
            if (_controllers.Count == 0)
            {
                return;
            }

            _tickBuffer.Clear();
            _tickBuffer.AddRange(_controllers);

            foreach (var tickBuffer in _tickBuffer)
            {
                tickBuffer.Tick(deltaTime);
            }
        }

        /// <summary>
        /// Returns the number of currently registered animation controllers.
        /// Intended for testing and debugging.
        /// </summary>
        public static int RegisteredCount => _controllers.Count;

        /// <summary>
        /// Clears all registered controllers without disposing them.
        /// Intended for test isolation only.
        /// </summary>
        internal static void Reset()
        {
            _controllers.Clear();
            _tickBuffer.Clear();
        }
    }
}