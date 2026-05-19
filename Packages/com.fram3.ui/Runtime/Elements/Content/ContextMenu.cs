#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// A floating menu that appears at a given screen position and displays a list of
    /// <see cref="ContextMenuItem"/> entries. Attaches directly to the root container
    /// like <see cref="Fram3.UI.Elements.Gesture.Modal"/>, rendered with absolute
    /// positioning at <see cref="X"/>, <see cref="Y"/>.
    /// Tapping any item or the backdrop dismisses the menu via <see cref="OnDismiss"/>.
    /// </summary>
    /// <since>2.0.0-beta.2</since>
    /// <status>live</status>
    public sealed class ContextMenu : Element, IRootAttachedElement
    {
        /// <summary>The menu items to display. Must not be null or empty.</summary>
        public IReadOnlyList<ContextMenuItem> Items { get; }

        /// <summary>Horizontal position in logical pixels from the left of the root container.</summary>
        public float X { get; }

        /// <summary>Vertical position in logical pixels from the top of the root container.</summary>
        public float Y { get; }

        /// <summary>
        /// Callback invoked when the menu requests dismissal — after an item tap or a
        /// backdrop tap. The caller is responsible for removing the <see cref="ContextMenu"/>
        /// from the tree in response.
        /// </summary>
        public Action OnDismiss { get; }

        /// <summary>
        /// Creates a <see cref="ContextMenu"/>.
        /// </summary>
        /// <param name="items">Menu items. Must not be null or empty.</param>
        /// <param name="x">Horizontal anchor position in logical pixels.</param>
        /// <param name="y">Vertical anchor position in logical pixels.</param>
        /// <param name="onDismiss">Callback invoked when dismissal is requested. Must not be null.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> or <paramref name="onDismiss"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="items"/> is empty.</exception>
        public ContextMenu(
            IReadOnlyList<ContextMenuItem> items,
            float x,
            float y,
            Action onDismiss,
            Key? key = null
        ) : base(key)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (items.Count == 0)
            {
                throw new ArgumentException("Items must not be empty.", nameof(items));
            }

            Items = items;
            X = x;
            Y = y;
            OnDismiss = onDismiss ?? throw new ArgumentNullException(nameof(onDismiss));
        }
    }
}