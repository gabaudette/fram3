#nullable enable
using System.Collections.Generic;
using System.Runtime.CompilerServices;
#if !FRAM3_PURE_TESTS && !FRAM3_DOC_BUILD
using Unity.Profiling;
#endif
#if FRAM3_FRAMEWORK_DIAGNOSTICS
using System.Diagnostics;
using Fram3.UI.Diagnostics;
#endif

namespace Fram3.UI.Core.Internal
{
    /// <summary>
    /// <status>live</status>
    /// Collects nodes marked dirty via <see cref="State.SetState"/> and flushes
    /// them in ascending depth order during a single reconciliation pass. Processing
    /// ancestors before descendants avoids redundant rebuilds: if a parent is rebuilt
    /// it re-expands its entire subtree, making any enqueued descendant rebuild moot.
    /// This is an internal framework type not intended for direct use.
    /// </summary>
    /// <since>2.0.0-beta.1</since>
    /// <status>live</status>
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class RebuildScheduler
    {
        private readonly SortedSet<Node> _dirtyNodes;
        private bool _flushing;
#if !FRAM3_PURE_TESTS && !FRAM3_DOC_BUILD
        private static readonly ProfilerMarker s_FlushMarker = new("Fram3.Flush");
#endif

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

#if !FRAM3_PURE_TESTS && !FRAM3_DOC_BUILD
            using var _ = s_FlushMarker.Auto();
#endif
#if FRAM3_FRAMEWORK_DIAGNOSTICS
            var flushStart = Stopwatch.GetTimestamp();
#endif
            _flushing = true;
            try
            {
                FlushInternal(expander);
            }
            finally
            {
                _flushing = false;
#if FRAM3_FRAMEWORK_DIAGNOSTICS
                Fram3Diagnostics.CurrentFrame.FlushDurationTicks += Stopwatch.GetTimestamp() - flushStart;
#endif
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

                if (node.IsUnmounted || !node.IsDirty || node.IsFaulted)
                {
                    continue;
                }

                expander.Rebuild(node);
#if FRAM3_FRAMEWORK_DIAGNOSTICS
                Fram3Diagnostics.CurrentFrame.DirtyNodesFlushed++;
#endif
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