#nullable enable
using System;
using Fram3.UI.Core.Internal;
using UnityEngine.UIElements;

namespace Fram3.UI.Rendering.Internal
{
    /// <summary>
    /// Tracks the native <see cref="VisualElement"/> produced for a single
    /// <see cref="Node"/>. The <see cref="Renderer"/> maintains a dictionary
    /// of these handles keyed by node, updated on every mount, unmount, and rebuild.
    /// This is an internal framework type not intended for direct use.
    /// </summary>
    internal sealed class RenderHandle
    {
        internal Node Node { get; }
        internal VisualElement NativeElement { get; set; }

        internal RenderHandle(Node node, VisualElement nativeElement)
        {
            Node = node ?? throw new ArgumentNullException(nameof(node));
            NativeElement = nativeElement ?? throw new ArgumentNullException(nameof(nativeElement));
        }
    }
}