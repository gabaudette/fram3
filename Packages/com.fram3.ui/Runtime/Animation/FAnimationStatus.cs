#nullable enable
namespace Fram3.UI.Animation
{
    /// <summary>
    /// Describes the current playback state of an <see cref="FAnimationController"/>.
    /// </summary>
    public enum FAnimationStatus
    {
        /// <summary>The animation is not running and the progress is at the beginning.</summary>
        Idle,

        /// <summary>The animation is actively advancing forward toward 1.</summary>
        Forward,

        /// <summary>The animation is actively advancing in reverse toward 0.</summary>
        Reverse,

        /// <summary>The animation has finished and progress is at its final value.</summary>
        Completed
    }
}
