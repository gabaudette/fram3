using Fram3.UI.Core;

namespace Fram3.UI.Rendering
{
    /// <summary>
    /// A synthetic single-child element that serves as the invisible root of every
    /// framework tree. <see cref="FRenderer"/> wraps the user-supplied root element
    /// inside an <see cref="FRootElement"/> so the reconciler always has a stable
    /// anchor node at depth zero whose native counterpart is the UIToolkit container
    /// passed to <see cref="FRenderer.Mount"/>.
    /// </summary>
    /// <remarks>
    /// You do not instantiate this type directly. Pass your root element to
    /// <see cref="FRenderer.Mount"/> and the renderer creates the wrapping root
    /// automatically.
    /// </remarks>
    public sealed class FRootElement : FSingleChildElement
    {
    }
}
