#nullable enable
using System;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// A single entry in a <see cref="ContextMenu"/>.
    /// </summary>
    public sealed class ContextMenuItem
    {
        /// <summary>The label displayed for this menu item.</summary>
        public string Label { get; }

        /// <summary>
        /// Callback invoked when the user taps this item.
        /// Not invoked when <see cref="Disabled"/> is true.
        /// </summary>
        public Action OnTap { get; }

        /// <summary>
        /// When true the item is rendered with reduced opacity and does not respond to taps.
        /// Defaults to false.
        /// </summary>
        public bool Disabled { get; }

        /// <summary>
        /// Creates a <see cref="ContextMenuItem"/>.
        /// </summary>
        /// <param name="label">Display label. Must not be null.</param>
        /// <param name="onTap">Callback invoked on tap. Must not be null.</param>
        /// <param name="disabled">Whether the item is non-interactive. Defaults to false.</param>
        public ContextMenuItem(string label, Action onTap, bool disabled = false)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            OnTap = onTap ?? throw new ArgumentNullException(nameof(onTap));
            Disabled = disabled;
        }
    }
}