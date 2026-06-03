#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Layout
{
    /// <summary>
    /// Wraps a single child and causes it to expand along the main axis of the
    /// parent flex container, proportional to its <see cref="Flex"/> factor.
    /// Maps to UIToolkit's <c>flexGrow</c> style property.
    /// </summary>
    /// <status>live</status>
    public sealed class Expanded : SingleChildElement
    {
        /// <summary>
        /// The flex factor that controls how much of the remaining space this element
        /// claims relative to other <see cref="Expanded"/> siblings. Defaults to 1.
        /// </summary>
        public float Flex { get; }

        /// <summary>
        /// Optional inner padding applied to the expanded container.
        /// </summary>
        public EdgeInsets? Padding { get; }

        /// <summary>
        /// Optional tap callback registered directly on the native element. Use this instead
        /// of wrapping in a <see cref="Fram3.UI.Elements.Gesture.GestureDetector"/> to avoid
        /// introducing an extra layout node inside a flex container.
        /// </summary>
        public Action? OnTap { get; }

        /// <summary>
        /// Creates an <see cref="Expanded"/> element.
        /// </summary>
        /// <param name="flex">The flex grow factor. Must be greater than zero. Defaults to 1.</param>
        /// <param name="padding">Optional inner padding.</param>
        /// <param name="onTap">Optional tap callback registered directly on the native element.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Expanded(float flex = 1f, EdgeInsets? padding = null, Action? onTap = null, Key? key = null) : base(key)
        {
            Flex = flex;
            Padding = padding;
            OnTap = onTap;
        }

        /// <inheritdoc/>
        public override bool ShouldRebuild(Element oldEl, Element newEl)
        {
            var o = (Expanded)oldEl;
            var n = (Expanded)newEl;
            return o.Flex != n.Flex || o.Padding != n.Padding || o.OnTap != n.OnTap;
        }
    }
}