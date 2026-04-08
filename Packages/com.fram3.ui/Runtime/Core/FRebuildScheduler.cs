using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Fram3.UI.Core
{
    /// <summary>
    /// Collects nodes marked dirty via <see cref="FState.SetState"/> and flushes
    /// them in ascending depth order during a single reconciliation pass. Processing
    /// ancestors before descendants avoids redundant rebuilds: if a parent is rebuilt
    /// it re-expands its entire subtree, making any enqueued descendant rebuild moot.
    /// This is an internal framework type not intended for direct use.
    /// </summary>
    internal sealed class FRebuildScheduler
    {
        private readonly SortedSet<FNode> _dirtyNodes;
        private bool _flushing;

        internal FRebuildScheduler()
        {
            _dirtyNodes = new SortedSet<FNode>(DepthAscendingComparer.Instance);
        }

        /// <summary>
        /// Schedules <paramref name="node"/> for a rebuild on the next flush.
        /// Safe to call during a flush; the node will be processed in the current pass
        /// provided its depth is greater than the node currently being rebuilt.
        /// </summary>
        internal void Schedule(FNode node)
        {
            _dirtyNodes.Add(node);
        }

        /// <summary>
        /// Rebuilds all dirty nodes in depth order. Nodes already rebuilt because
        /// an ancestor was processed earlier are skipped.
        /// </summary>
        internal void Flush(FNodeExpander expander)
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

        private void FlushInternal(FNodeExpander expander)
        {
            while (_dirtyNodes.Count > 0)
            {
                FNode node = RemoveMin();

                if (!node.IsDirty)
                {
                    continue;
                }

                expander.Rebuild(node);
            }
        }

        private FNode RemoveMin()
        {
            FNode min = _dirtyNodes.Min;
            _dirtyNodes.Remove(min);
            return min;
        }

        private sealed class DepthAscendingComparer : IComparer<FNode>
        {
            internal static readonly DepthAscendingComparer Instance = new DepthAscendingComparer();

            public int Compare(FNode x, FNode y)
            {
                if (ReferenceEquals(x, y))
                {
                    return 0;
                }
                int depthCompare = x.Depth.CompareTo(y.Depth);
                if (depthCompare != 0)
                {
                    return depthCompare;
                }
                return RuntimeHelpers.GetHashCode(x).CompareTo(RuntimeHelpers.GetHashCode(y));
            }
        }
    }
}
