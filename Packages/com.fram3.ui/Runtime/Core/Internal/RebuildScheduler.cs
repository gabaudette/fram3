#nullable enable
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Fram3.UI.Core.Internal
{
    /// <summary>
    /// Collects nodes marked dirty via <see cref="State.SetState"/> and flushes
    /// them in ascending depth order during a single reconciliation pass. Processing
    /// ancestors before descendants avoids redundant rebuilds: if a parent is rebuilt
    /// it re-expands its entire subtree, making any enqueued descendant rebuild moot.
    /// This is an internal framework type not intended for direct use.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class RebuildScheduler
    {
        private readonly SortedSet<Node> _dirtyNodes;
        private bool _flushing;

        internal RebuildScheduler()
        {
            _dirtyNodes = new SortedSet<Node>(DepthAscendingComparer.Instance);
        }

        /// <summary>
        /// Schedules <paramref name="node"/> for a rebuild on the next flush.
        /// Safe to call during a flush; the node will be processed in the current pass
        /// provided its depth is greater than the node currently being rebuilt.
        /// </summary>
        internal void Schedule(Node node)
        {
            _dirtyNodes.Add(node);
        }

        /// <summary>
        /// Rebuilds all dirty nodes in depth order. Nodes already rebuilt because
        /// an ancestor was processed earlier are skipped.
        /// </summary>
        internal void Flush(NodeExpander expander)
        {
            if (_flushing)
            {
                return;
            }

            _flushing = true;
            try
            {
                FlushInternal(expander);
            }
            finally
            {
                _flushing = false;
            }
        }

        internal bool HasDirtyNodes => _dirtyNodes.Count > 0;

        private void FlushInternal(NodeExpander expander)
        {
            while (_dirtyNodes.Count > 0)
            {
                var node = RemoveMin();
                if (node == null)
                {
                    continue;
                }

                if (node.IsUnmounted || !node.IsDirty)
                {
                    continue;
                }

                expander.Rebuild(node);
            }
        }

        private Node? RemoveMin()
        {
            var min = _dirtyNodes.Min;
            if (min == null)
            {
                return null;
            }

            _dirtyNodes.Remove(min);

            return min;
        }

        private sealed class DepthAscendingComparer : IComparer<Node>
        {
            internal static readonly DepthAscendingComparer Instance = new();

            public int Compare(Node? x, Node? y)
            {
                if (ReferenceEquals(x, y))
                {
                    return 0;
                }

                if (x == null)
                {
                    return -1;
                }

                if (y == null)
                {
                    return 1;
                }

                var depthCompare = x.Depth.CompareTo(y.Depth);

                return depthCompare != 0
                    ? depthCompare
                    : RuntimeHelpers.GetHashCode(x).CompareTo(RuntimeHelpers.GetHashCode(y));
            }
        }
    }
}