#nullable enable
using System;
using Fram3.UI.Core.Internal;
using UnityEngine.UIElements;

namespace Fram3.UI.Rendering.Internal
{
    /// <summary>
    /// Tracks the native <see cref="VisualElement"/> produced for a single
    /// <see cref="FNode"/>. The <see cref="FRenderer"/> maintains a dictionary
    /// of these handles keyed by node, updated on every mount, unmount, and rebuild.
    /// This is an internal framework type not intended for direct use.
    /// </summary>
    internal sealed class FRenderHandle
    {
        internal FNode Node { get; }
        internal VisualElement NativeElement { get; set; }

        internal FRenderHandle(FNode node, VisualElement nativeElement)
        {
            Node = node ?? throw new ArgumentNullException(nameof(node));
            NativeElement = nativeElement ?? throw new ArgumentNullException(nameof(nativeElement));
        }
    }
}
