#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Layout
{
    /// <summary>
    /// A zero-height element that stretches to fill its parent's width and fires
    /// <see cref="OnWidth"/> with the resolved pixel width whenever layout changes.
    /// Use this to measure available width before committing to explicit column widths.
    /// </summary>
    /// <status>live</status>
    public sealed class WidthProbe : Element
    {
        /// <summary>
        /// Invoked with the resolved width in logical pixels each time the layout changes.
        /// </summary>
        public Action<float> OnWidth { get; }

        /// <summary>
        /// Creates a <see cref="WidthProbe"/>.
        /// </summary>
        /// <param name="onWidth">Callback invoked with the resolved width. Must not be null.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public WidthProbe(Action<float> onWidth, Key? key = null) : base(key)
        {
            OnWidth = onWidth ?? throw new ArgumentNullException(nameof(onWidth));
        }
    }
}