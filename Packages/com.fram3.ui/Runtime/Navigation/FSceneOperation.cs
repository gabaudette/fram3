#nullable enable
using System;

namespace Fram3.UI.Navigation
{
    /// <summary>
    /// Represents an in-progress or completed scene load initiated by
    /// <see cref="FSceneNavigator.GoTo"/>. Poll <see cref="Progress"/> each frame or
    /// subscribe to <see cref="Completed"/> to react when the scene is fully loaded.
    /// </summary>
    public sealed class FSceneOperation
    {
        /// <summary>
        /// Raised once when the scene load reaches completion and
        /// <see cref="IsCompleted"/> transitions to <c>true</c>.
        /// </summary>
        public event Action? Completed;

        /// <summary>
        /// The normalized load progress in the range [0, 1].
        /// Updated continuously during an async load. Equals <c>1</c> when
        /// <see cref="IsCompleted"/> is <c>true</c>.
        /// </summary>
        public float Progress { get; internal set; }

        /// <summary>
        /// <c>true</c> once the scene has finished loading and is active.
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// Marks the operation as completed, sets <see cref="Progress"/> to <c>1</c>,
        /// and raises <see cref="Completed"/>. Safe to call multiple times; subsequent
        /// calls are no-ops.
        /// </summary>
        internal void Complete()
        {
            if (IsCompleted)
            {
                return;
            }

            Progress = 1f;
            IsCompleted = true;
            Completed?.Invoke();
        }
    }
}