using Fram3.UI.Core;
using System;
using System.Collections.Generic;

namespace Fram3.UI.Core.Internal
{
    /// <summary>
    /// Mounts, unmounts, and expands <see cref="FNode"/>s from <see cref="FElement"/>
    /// descriptions. Expanding a node means recursively building the subtree beneath it:
    /// for stateless elements by calling <c>Build</c>, for stateful elements by invoking
    /// the state's <c>Build</c>, and for structural elements by walking their declared
    /// children. Unmounting a node recursively disposes state and clears child references.
    /// This is an internal framework type not intended for direct use.
    /// </summary>
    internal sealed class FNodeExpander
    {
        private readonly FRebuildScheduler _scheduler;

        internal FNodeExpander(FRebuildScheduler scheduler)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        }

        /// <summary>
        /// Creates a new <see cref="FNode"/> for <paramref name="element"/>, attaches it
        /// to <paramref name="parent"/>, mounts any required <see cref="FState"/>, and
        /// recursively expands child nodes.
        /// </summary>
        internal FNode Mount(FElement element, FNode parent)
        {
            var node = new FNode(element, parent, _scheduler);

            if (element is FStatefulElement stateful)
            {
                MountState(node, stateful);
            }

            ExpandChildren(node);
            return node;
        }

        /// <summary>
        /// Recursively unmounts a node subtree: disposes state bottom-up and
        /// clears child references from the node.
        /// </summary>
        internal void Unmount(FNode node)
        {
            UnmountChildren(node);
            DisposeState(node);
            node.ClearChildren();
        }

        /// <summary>
        /// Re-expands the children of a dirty node after a rebuild. The node's element
        /// must already have been updated to the new description before calling this.
        /// </summary>
        internal void Rebuild(FNode node)
        {
            IReadOnlyList<FElement> newChildren = ResolveChildren(node);
            FTreePatcher.Patch(node, newChildren, this);
            node.IsDirty = false;
        }

        /// <summary>
        /// Updates the element reference on a node and, for stateful nodes, invokes
        /// <see cref="FState.DidUpdateElement"/> with the previous element.
        /// </summary>
        internal void UpdateElement(FNode node, FElement newElement)
        {
            FElement oldElement = node.Element;
            node.Element = newElement;

            if (node.State != null && oldElement is FStatefulElement oldStateful)
            {
                node.State.DidUpdateElement(oldStateful);
            }
        }

        private void MountState(FNode node, FStatefulElement stateful)
        {
            FState state = stateful.CreateState();
            state.Mount(node);
            node.State = state;
            state.InitState();
        }

        private void ExpandChildren(FNode node)
        {
            IReadOnlyList<FElement> children = ResolveChildren(node);
            foreach (FElement child in children)
            {
                FNode childNode = Mount(child, node);
                node.AddChild(childNode);
            }
        }

        private IReadOnlyList<FElement> ResolveChildren(FNode node)
        {
            return node.Element switch
            {
                FStatefulElement => ResolveStatefulChildren(node),
                FStatelessElement stateless => ResolveStatelessChildren(stateless, node),
                _ => node.Element.GetChildren()
            };
        }

        private IReadOnlyList<FElement> ResolveStatefulChildren(FNode node)
        {
            FElement built = node.State.Build(node.Context);
            return new[] { built };
        }

        private IReadOnlyList<FElement> ResolveStatelessChildren(FStatelessElement stateless, FNode node)
        {
            FElement built = stateless.Build(node.Context);
            return new[] { built };
        }

        private void UnmountChildren(FNode node)
        {
            foreach (FNode child in node.Children)
            {
                Unmount(child);
            }
        }

        private static void DisposeState(FNode node)
        {
            if (node.State != null)
            {
                node.State.Unmount();
                node.State.Dispose();
                node.State = null;
            }
        }
    }
}
