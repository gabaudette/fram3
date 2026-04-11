#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// Displays a sprite or texture as a UI image. Maps to UIToolkit's native
    /// <c>Image</c> element.
    /// <para>
    /// The <see cref="Source"/> property accepts a <c>UnityEngine.Sprite</c> or a
    /// <c>UnityEngine.Texture2D</c>. It is typed as <c>object?</c> so the element
    /// can be declared in files that are compiled without Unity.
    /// </para>
    /// </summary>
    public sealed class FImage : FLeafElement
    {
        /// <summary>
        /// The image source. Expects a <c>UnityEngine.Sprite</c> or
        /// <c>UnityEngine.Texture2D</c> at runtime. Null displays nothing.
        /// </summary>
        public object? Source { get; }

        /// <summary>
        /// The explicit width in logical pixels. Null means unconstrained.
        /// </summary>
        public float? Width { get; }

        /// <summary>
        /// The explicit height in logical pixels. Null means unconstrained.
        /// </summary>
        public float? Height { get; }

        /// <summary>
        /// Creates an <see cref="FImage"/> element.
        /// </summary>
        /// <param name="source">
        /// A <c>UnityEngine.Sprite</c> or <c>UnityEngine.Texture2D</c> to display.
        /// </param>
        /// <param name="width">Optional explicit width in logical pixels.</param>
        /// <param name="height">Optional explicit height in logical pixels.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FImage(
            object? source = null,
            float? width = null,
            float? height = null,
            FKey? key = null
        ) : base(key)
        {
            Source = source;
            Width = width;
            Height = height;
        }
    }
}
