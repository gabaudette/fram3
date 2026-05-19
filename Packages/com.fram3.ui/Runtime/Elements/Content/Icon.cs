#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// Displays a vector icon asset. Maps to UIToolkit's native <c>Image</c> element
    /// with a <c>VectorImage</c> source.
    /// <para>
    /// Supply a preloaded <see cref="Source"/>, a <see cref="ResourcePath"/> for
    /// <c>Resources.Load</c> (works in player builds), or a project-relative
    /// <see cref="SvgPath"/> for Editor-only <c>AssetDatabase</c> loading.
    /// When multiple are provided, precedence is: <see cref="Source"/> first,
    /// then <see cref="ResourcePath"/>, then <see cref="SvgPath"/>.
    /// </para>
    /// </summary>
    /// <since>2.0.0-beta.1</since>
    /// <status>live</status>
    public sealed class Icon : LeafElement
    {
        /// <summary>
        /// A preloaded <c>UnityEngine.UIElements.VectorImage</c>.
        /// Takes precedence over <see cref="ResourcePath"/> and <see cref="SvgPath"/>.
        /// </summary>
        public object? Source { get; }

        /// <summary>
        /// Path relative to any <c>Resources/</c> folder (e.g. <c>"Icons/star"</c>).
        /// Loaded via <c>Resources.Load&lt;VectorImage&gt;</c> — works in player builds.
        /// Ignored when <see cref="Source"/> is non-null.
        /// </summary>
        public string? ResourcePath { get; }

        /// <summary>
        /// Project-relative path to a <c>.svg</c> asset (e.g. <c>"Assets/Icons/arrow.svg"</c>).
        /// Loaded via <c>AssetDatabase.LoadAssetAtPath</c> — Editor only.
        /// Ignored when <see cref="Source"/> or <see cref="ResourcePath"/> is non-null.
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
        /// Creates an <see cref="Icon"/> element.
        /// </summary>
        /// <param name="source">
        /// A preloaded <c>UnityEngine.UIElements.VectorImage</c>. Takes precedence over all other sources.
        /// </param>
        /// <param name="resourcePath">
        /// Path relative to a <c>Resources/</c> folder. Used when <paramref name="source"/> is null.
        /// Works in player builds.
        /// </param>
        /// <param name="svgPath">
        /// Project-relative path to a <c>.svg</c> asset. Editor only. Used when both
        /// <paramref name="source"/> and <paramref name="resourcePath"/> are null.
        /// </param>
        /// <param name="width">Optional explicit width in logical pixels.</param>
        /// <param name="height">Optional explicit height in logical pixels.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Icon(
            object? source = null,
            string? resourcePath = null,
            string? svgPath = null,
            float? width = null,
            float? height = null,
            Key? key = null
        ) : base(key)
        {
            Source = source;
            ResourcePath = resourcePath;
            SvgPath = svgPath;
            Width = width;
            Height = height;
        }
    }
}