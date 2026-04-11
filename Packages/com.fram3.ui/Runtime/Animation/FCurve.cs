#nullable enable
using System;
using Fram3.UI.Elements;

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
            return t switch
            {
                <= 0f => 0f,
                >= 1f => 1f,
                _ => MathF.Pow(2f, -10f * t) * MathF.Sin((t * 10f - 0.75f) * c4) + 1f
            };
        };

        /// <summary>
        /// Bounces at the end, simulating a ball dropping onto a surface.
        /// </summary>
        public static readonly FCurve BounceOut = t =>
        {
            const float parabolicScale = 7.5625f;
            const float segmentDivisor = 2.75f;

            const float segment1End = 1f / segmentDivisor; // 0.3636...
            const float segment2End = 2f / segmentDivisor; // 0.7272...
            const float segment3End = 2.5f / segmentDivisor; // 0.9090...

            const float segment2Centre = 1.5f / segmentDivisor;
            const float segment3Centre = 2.25f / segmentDivisor;
            const float segment4Centre = 2.625f / segmentDivisor;

            const float segment2Floor = 0.75f;
            const float segment3Floor = 0.9375f;
            const float segment4Floor = 0.984375f;

            switch (t)
            {
                case < segment1End:
                    return parabolicScale * t * t;
                case < segment2End:
                    t -= segment2Centre;
                    return parabolicScale * t * t + segment2Floor;
                case < segment3End:
                    t -= segment3Centre;
                    return parabolicScale * t * t + segment3Floor;
                default:
                    t -= segment4Centre;
                    return parabolicScale * t * t + segment4Floor;
            }
        };
    }
}