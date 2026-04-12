#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// Displays a vector icon asset. Maps to UIToolkit's native <c>Image</c> element
    /// with a <c>VectorImage</c> source.
    /// <para>
    /// Supply either a preloaded <see cref="Source"/> (<c>UnityEngine.UIElements.VectorImage</c>)
    /// or a project-relative asset path via <see cref="SvgPath"/>. When both are provided,
    /// <see cref="Source"/> takes precedence.
    /// </para>
    /// </summary>
    public sealed class FIcon : FLeafElement
    {
        /// <summary>
        /// The icon source. Expects a <c>UnityEngine.UIElements.VectorImage</c> at runtime.
        /// When set, takes precedence over <see cref="SvgPath"/>.
        /// Null displays nothing unless <see cref="SvgPath"/> is provided.
        /// </summary>
        public object? Source { get; }

        /// <summary>
        /// Project-relative path to a <c>.svg</c> asset (e.g. <c>"Assets/Icons/arrow.svg"</c>).
        /// The asset is loaded via <c>AssetDatabase.LoadAssetAtPath&lt;VectorImage&gt;</c> at runtime.
        /// Ignored when <see cref="Source"/> is non-null.
        /// </summary>
        public string? SvgPath { get; }

        /// <summary>
        /// The explicit width in logical pixels. Null means unconstrained.
        /// </summary>
        public float? Width { get; }

        /// <summary>
        /// The explicit height in logical pixels. Null means unconstrained.
        /// </summary>
        public float? Height { get; }

        /// <summary>
        /// Creates an <see cref="FIcon"/> element.
        /// </summary>
        /// <param name="source">
        /// A preloaded <c>UnityEngine.UIElements.VectorImage</c>. Takes precedence over
        /// <paramref name="svgPath"/> when both are provided.
        /// </param>
        /// <param name="svgPath">
        /// Project-relative path to a <c>.svg</c> asset. Used only when <paramref name="source"/>
        /// is null.
        /// </param>
        /// <param name="width">Optional explicit width in logical pixels.</param>
        /// <param name="height">Optional explicit height in logical pixels.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FIcon(
            object? source = null,
            string? svgPath = null,
            float? width = null,
            float? height = null,
            FKey? key = null
        ) : base(key)
        {
            Source = source;
            SvgPath = svgPath;
            Width = width;
            Height = height;
        }
    }
}