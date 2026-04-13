#nullable enable
using System;
using System.Collections.Generic;

namespace Fram3.UI.Core.Internal
{
    /// <summary>
    /// Represents the instantiation of an FElement in the live element tree.
    /// Nodes are the mutable, long-lived counterparts to immutable FElement descriptions.
    /// They track parent-child relationships, manage lifecycle state, and serve as the
    /// bridge between the declarative element descriptions and the rendered output.
    /// This is an internal framework type not intended for direct use by end users.
    /// </summary>
    internal class FNode
    {
        private readonly List<FNode> _children = new();
        private readonly FRebuildScheduler? _scheduler;

        /// <summary>
        /// Nodes that depend on this node when this node holds an FInheritedElement.
        /// These are scheduled for rebuild when UpdateShouldNotify returns true.
        /// </summary>
        private readonly HashSet<FNode> _inheritedDependents = new();

        internal FNode(FElement? element, FNode? parent, FRebuildScheduler? scheduler = null)
        {
            Element = element ?? throw new ArgumentNullException(nameof(element));
            _scheduler = scheduler;
            Parent = parent;
            Depth = parent != null ? parent.Depth + 1 : 0;
            Context = new FBuildContext(this);
        }

        internal FElement Element { get; set; }

        internal FNode? Parent { get; }

        internal int Depth { get; }

        internal FBuildContext Context { get; }

        internal IReadOnlyList<FNode> Children => _children;

        internal bool IsDirty { get; set; }

        /// <summary>
        /// True once this node has been unmounted. A node that is unmounted may still
        /// be in the rebuild scheduler queue (scheduled before the parent rebuilt and
        /// replaced this subtree). The scheduler skips unmounted nodes.
        /// </summary>
        internal bool IsUnmounted { get; private set; }

        internal FState? State { get; set; }

        internal void AddChild(FNode child)
        {
            _children.Add(child);
        }

        internal void RemoveChild(FNode child)
        {
            _children.Remove(child);
        }

        internal void ClearChildren()
        {
            _children.Clear();
        }

        internal void InsertChild(int index, FNode child)
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
        internal void AddInheritedDependent(FNode dependent)
        {
            _inheritedDependents.Add(dependent);
        }

        /// <summary>
        /// Removes <paramref name="dependent"/> from the inherited dependents set.
        /// Called when the dependent node is unmounted.
        /// </summary>
        internal void RemoveInheritedDependent(FNode dependent)
        {
            _inheritedDependents.Remove(dependent);
        }

        /// <summary>
        /// Marks all registered inherited dependents dirty so they are rebuilt
        /// on the next frame. Called by FTreePatcher when an FInheritedElement
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
        /// Walks up the ancestor chain to find the nearest FNode whose element is
        /// an FInheritedElement of type <typeparamref name="T"/>.
        /// Returns null when no such ancestor exists.
        /// </summary>
        internal FNode? FindInheritedAncestor<T>() where T : FInheritedElement
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