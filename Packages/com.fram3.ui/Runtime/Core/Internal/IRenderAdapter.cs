#nullable enable
namespace Fram3.UI.Core.Internal
{
    /// <summary>
    /// Receives lifecycle notifications from <see cref="NodeExpander"/> so a render
    /// bridge can keep its native object tree in sync with the framework node tree.
    /// Implement this interface inside the framework assembly; it is not part of the
    /// public API because it references the internal <see cref="Node"/> type.
    /// </summary>
    internal interface IRenderAdapter
    {
        /// <summary>
        /// Called after a node has been fully mounted and its subtree has been expanded.
        /// </summary>
        void OnMounted(Node node);

        /// <summary>
        /// Called before a node's children are unmounted and its state is disposed.
        /// Use this to release the native object associated with the node.
        /// </summary>
        void OnUnmounting(Node node);

        /// <summary>
        /// Called after a node has been rebuilt and its child list updated. Use this
        /// to synchronize properties on the node's existing native object.
        /// </summary>
        void OnRebuilt(Node node);
    }
}