#nullable enable
namespace Fram3.UI.Core
{
    /// <summary>
    /// <status>live</status>
    /// Marker interface for elements that must be attached directly to the root
    /// container rather than to their logical parent in the visual tree.
    /// The renderer skips normal parent attachment and calls
    /// <c>_rootContainer.Add</c> for any element implementing this interface.
    /// </summary>
    /// <since>2.0.0-beta.2</since>
    /// <status>live</status>
    public interface IRootAttachedElement
    {
    }
}
