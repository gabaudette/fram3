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
        private readonly List<FNode> _children = new List<FNode>();
        private readonly FRebuildScheduler? _scheduler;

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
    }
}
