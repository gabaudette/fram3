#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Rendering
{
    /// <summary>
    /// <status>live</status>
    /// A synthetic single-child element that serves as the invisible root of every
    /// framework tree. <see cref="Renderer"/> wraps the user-supplied root element
    /// inside an <see cref="RootElement"/> so the reconciler always has a stable
    /// anchor node at depth zero whose native counterpart is the UIToolkit container
    /// passed to <see cref="Renderer.Mount"/>.
    /// </summary>
    /// <since>2.0.0-beta.1</since>
    /// <status>live</status>
    /// <remarks>
    /// You do not instantiate this type directly. Pass your root element to
    /// <see cref="Renderer.Mount"/> and the renderer creates the wrapping root
    /// automatically.
    /// </remarks>
    public sealed class RootElement : SingleChildElement
    {
    }
}