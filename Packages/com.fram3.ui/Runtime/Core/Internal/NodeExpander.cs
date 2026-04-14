#nullable enable
using System;
using System.Collections.Generic;

namespace Fram3.UI.Core.Internal
{
    /// <summary>
    /// Mounts, unmounts, and expands <see cref="Node"/>s from <see cref="Element"/>
    /// descriptions. Expanding a node means recursively building the subtree beneath it:
    /// for stateless elements by calling <c>Build</c>, for stateful elements by invoking
    /// the state's <c>Build</c>, and for structural elements by walking their declared
    /// children. Unmounting a node recursively disposes state and clears child references.
    /// This is an internal framework type not intended for direct use.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class NodeExpander
    {
        private readonly RebuildScheduler _scheduler;
        private readonly IRenderAdapter? _adapter;

        internal NodeExpander(RebuildScheduler scheduler, IRenderAdapter? adapter = null)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _adapter = adapter;
        }

        /// <summary>
        /// Creates a new <see cref="Node"/> for <paramref name="element"/>, attaches it
        /// to <paramref name="parent"/>, mounts any required <see cref="State"/>, and
        /// recursively expands child nodes.
        /// </summary>
        internal Node Mount(Element element, Node? parent)
        {
            var node = new Node(element, parent, _scheduler);

            if (element is StatefulElement stateful)
            {
                MountState(node, stateful);
            }

            ExpandChildren(node);
            _adapter?.OnMounted(node);
            return node;
        }

        /// <summary>
        /// Recursively unmounts a node subtree: disposes state bottom-up and
        /// clears child references from the node.
        /// </summary>
        internal void Unmount(Node node)
        {
            node.MarkUnmounted();
            _adapter?.OnUnmounting(node);
            UnmountChildren(node);
            DisposeState(node);
            RemoveInheritedDependencies(node);
            node.ClearChildren();
        }

        /// <summary>
        /// Re-expands the children of a dirty node after a rebuild. The node's element
        /// must already have been updated to the new description before calling this.
        /// </summary>
        internal void Rebuild(Node node)
        {
            var newChildren = ResolveChildren(node);
            TreePatcher.Patch(node, newChildren, this);
            node.IsDirty = false;
            _adapter?.OnRebuilt(node);
        }

        /// <summary>
        /// Updates the element reference on a node and, for stateful nodes, invokes
        /// <see cref="State.DidUpdateElement"/> with the previous element.
        /// For inherited element nodes, notifies dependents when
        /// <see cref="InheritedElement.UpdateShouldNotify"/> returns true.
        /// </summary>
        internal void UpdateElement(Node node, Element newElement)
        {
            var oldElement = node.Element;
            node.Element = newElement;

            if (node.State != null && oldElement is StatefulElement oldStateful)
            {
                node.State.DidUpdateElement(oldStateful);
            }

            if (oldElement is not InheritedElement oldInherited || newElement is not InheritedElement element)
            {
                return;
            }

            if (element.UpdateShouldNotify(oldInherited))
            {
                node.NotifyInheritedDependents();
            }
        }

        private void MountState(Node node, StatefulElement stateful)
        {
            var state = stateful.CreateState();
            state.Mount(node);
            node.State = state;
            state.InitState();
        }

        private void ExpandChildren(Node node)
        {
            var children = ResolveChildren(node);
            foreach (var child in children)
            {
                var childNode = Mount(child, node);
                node.AddChild(childNode);
            }
        }

        private IReadOnlyList<Element> ResolveChildren(Node node)
        {
            return node.Element switch
            {
                StatefulElement => ResolveStatefulChildren(node),
                StatelessElement stateless => ResolveStatelessChildren(stateless, node),
                _ => node.Element.GetChildren()
            };
        }

        private IReadOnlyList<Element> ResolveStatefulChildren(Node node)
        {
            if (node.State == null)
            {
                throw new InvalidOperationException(
                    "Cannot rebuild a stateful node whose state has been disposed. " +
                    "The node may have been unmounted before its scheduled rebuild ran."
                );
            }

            var built = node.State.Build(node.Context);
            return new[] { built };
        }

        private IReadOnlyList<Element> ResolveStatelessChildren(StatelessElement stateless, Node node)
        {
            var built = stateless.Build(node.Context);
            return new[] { built };
        }

        private void UnmountChildren(Node node)
        {
            foreach (var child in node.Children)
            {
                Unmount(child);
            }
        }

        private static void RemoveInheritedDependencies(Node node)
        {
            var ancestorNode = node.FindInheritedAncestor<InheritedElement>();
            ancestorNode?.RemoveInheritedDependent(node);
        }

        private static void DisposeState(Node node)
        {
            if (node.State == null)
            {
                return;
            }

            node.State.Unmount();
            node.State.Dispose();
            node.State = null;
        }
    }
}