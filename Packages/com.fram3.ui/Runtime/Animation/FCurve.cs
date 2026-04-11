#nullable enable
using System;

namespace Fram3.UI.Animation
{
    /// <summary>
    /// An easing function that maps a normalized linear progress value in [0, 1]
    /// to a curved output value. The input and output are both conventionally in
    /// [0, 1], though overshoot curves may return values outside that range.
    /// </summary>
    public delegate float FCurve(float t);

    /// <summary>
    /// Provides a set of pre-built easing curves for use with
    /// <see cref="FAnimationController"/> and <see cref="FAnimationBuilder"/>.
    /// </summary>
    public static class FCurves
    {
        /// <summary>Constant linear progression. Output equals input.</summary>
        public static readonly FCurve Linear = t => t;

        /// <summary>Starts slowly and accelerates toward the end (quadratic ease-in).</summary>
        public static readonly FCurve EaseIn = t => t * t;

        /// <summary>Starts quickly and decelerates toward the end (quadratic ease-out).</summary>
        public static readonly FCurve EaseOut = t => t * (2f - t);

        /// <summary>Accelerates in the first half and decelerates in the second half (cubic ease-in-out).</summary>
        public static readonly FCurve EaseInOut = t =>
            t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;

        /// <summary>
        /// Overshoots the target slightly before settling, producing a spring-like feel.
        /// Uses a cubic back ease-out.
        /// </summary>
        public static readonly FCurve ElasticOut = t =>
        {
            const float c4 = (2f * MathF.PI) / 3f;
            if (t <= 0f) { return 0f; }
            if (t >= 1f) { return 1f; }
            return MathF.Pow(2f, -10f * t) * MathF.Sin((t * 10f - 0.75f) * c4) + 1f;
        };

        /// <summary>
        /// Bounces at the end, simulating a ball dropping onto a surface.
        /// </summary>
        public static readonly FCurve BounceOut = t =>
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            if (t < 1f / d1)
            {
                return n1 * t * t;
            }
            else if (t < 2f / d1)
            {
                t -= 1.5f / d1;
                return n1 * t * t + 0.75f;
            }
            else if (t < 2.5f / d1)
            {
                t -= 2.25f / d1;
                return n1 * t * t + 0.9375f;
            }
            else
            {
                t -= 2.625f / d1;
                return n1 * t * t + 0.984375f;
            }
        };
    }
}
