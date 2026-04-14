#nullable enable
using System;
using System.Collections.Generic;

namespace Fram3.UI.Core.Internal
{
    /// <summary>
    /// Represents the instantiation of an Element in the live element tree.
    /// Nodes are the mutable, long-lived counterparts to immutable Element descriptions.
    /// They track parent-child relationships, manage lifecycle state, and serve as the
    /// bridge between the declarative element descriptions and the rendered output.
    /// This is an internal framework type not intended for direct use by end users.
    /// </summary>
    internal class Node
    {
        private readonly List<Node> _children = new();
        private readonly RebuildScheduler? _scheduler;

        /// <summary>
        /// Nodes that depend on this node when this node holds an InheritedElement.
        /// These are scheduled for rebuild when UpdateShouldNotify returns true.
        /// </summary>
        private readonly HashSet<Node> _inheritedDependents = new();

        internal Node(Element? element, Node? parent, RebuildScheduler? scheduler = null)
        {
            Element = element ?? throw new ArgumentNullException(nameof(element));
            _scheduler = scheduler;
            Parent = parent;
            Depth = parent != null ? parent.Depth + 1 : 0;
            Context = new BuildContext(this);
        }

        internal Element Element { get; set; }

        internal Node? Parent { get; }

        internal int Depth { get; }

        internal BuildContext Context { get; }

        internal IReadOnlyList<Node> Children => _children;

        internal bool IsDirty { get; set; }

        /// <summary>
        /// True once this node has been unmounted. A node that is unmounted may still
        /// be in the rebuild scheduler queue (scheduled before the parent rebuilt and
        /// replaced this subtree). The scheduler skips unmounted nodes.
        /// </summary>
        internal bool IsUnmounted { get; private set; }

        internal State? State { get; set; }

        internal void AddChild(Node child)
        {
            _children.Add(child);
        }

        internal void RemoveChild(Node child)
        {
            _children.Remove(child);
        }

        internal void ClearChildren()
        {
            _children.Clear();
        }

        internal void InsertChild(int index, Node child)
        {
            _children.Insert(index, child);
        }

        internal void MarkDirty()
        {
            IsDirty = true;
            _scheduler?.Schedule(this);
        }

        /// <summary>
        /// Marks this node as unmounted so the rebuild scheduler can skip it if it
        /// was queued before the parent rebuilt and replaced this subtree.
        /// </summary>
        internal void MarkUnmounted()
        {
            IsUnmounted = true;
        }

        /// <summary>
        /// Registers <paramref name="dependent"/> as a node that depends on the inherited
        /// data carried by this node. Called when a descendant's build context invokes
        /// DependOnInherited and resolves to this node.
        /// </summary>
        internal void AddInheritedDependent(Node dependent)
        {
            _inheritedDependents.Add(dependent);
        }

        /// <summary>
        /// Removes <paramref name="dependent"/> from the inherited dependents set.
        /// Called when the dependent node is unmounted.
        /// </summary>
        internal void RemoveInheritedDependent(Node dependent)
        {
            _inheritedDependents.Remove(dependent);
        }

        /// <summary>
        /// Marks all registered inherited dependents dirty so they are rebuilt
        /// on the next frame. Called by TreePatcher when an InheritedElement
        /// update returns true from UpdateShouldNotify.
        /// </summary>
        internal void NotifyInheritedDependents()
        {
            foreach (var dependent in _inheritedDependents)
            {
                dependent.MarkDirty();
            }
        }

        /// <summary>
        /// Walks up the ancestor chain to find the nearest Node whose element is
        /// an InheritedElement of type <typeparamref name="T"/>.
        /// Returns null when no such ancestor exists.
        /// </summary>
        internal Node? FindInheritedAncestor<T>() where T : InheritedElement
        {
            var current = Parent;
            while (current != null)
            {
                if (current.Element is T)
                {
                    return current;
                }

                current = current.Parent;
            }

            return null;
        }
    }
}