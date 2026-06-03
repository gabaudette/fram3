#nullable enable
using System;
using System.Collections.Generic;
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
    /// Produces a minimal list of <see cref="DiffOp"/> operations that transform an
    /// existing list of child <see cref="Node"/>s to reflect a new list of child
    /// <see cref="Element"/>s. The algorithm prioritizes keyed matching, then falls
    /// back to positional matching for unkeyed elements of the same type.
    /// This is an internal framework type not intended for direct use.
    /// </summary>
    /// <since>2.0.0-beta.1</since>
    /// <status>live</status>
    internal static class TreeDiffer
    {
#if !FRAM3_PURE_TESTS && !FRAM3_DOC_BUILD
        private static readonly ProfilerMarker s_DiffMarker = new("Fram3.Diff");
#endif
        /// <summary>
        /// Computes the diff between an old node list and a new element list.
        /// </summary>
        /// <param name="oldNodes">Current child nodes of the parent.</param>
        /// <param name="newElements">Desired child elements from the latest build.</param>
        /// <returns>
        /// An ordered list of operations to apply. Operations are ordered so that
        /// <see cref="DiffOpKind.Remove"/> operations appear first, followed by
        /// <see cref="DiffOpKind.Insert"/>, <see cref="DiffOpKind.Update"/>, and
        /// <see cref="DiffOpKind.Move"/> operations in new-index order.
        /// </returns>
        internal static IReadOnlyList<DiffOp> Diff(
            IReadOnlyList<Node> oldNodes,
            IReadOnlyList<Element> newElements)
        {
#if !FRAM3_PURE_TESTS && !FRAM3_DOC_BUILD
            using var _ = s_DiffMarker.Auto();
#endif
#if FRAM3_FRAMEWORK_DIAGNOSTICS
            var diffStart = Stopwatch.GetTimestamp();
#endif
            var ops = new List<DiffOp>();

            // unmatchedOldIndices tracks old-node indices that have not yet been
            // claimed by a new element. Using a SortedSet allows O(log n) remove
            // and O(1) Min access, making the in-place check O(log n) per element
            // instead of the previous O(n) linear scan.
            var unmatchedOldIndices = new SortedSet<int>();
            for (var i = 0; i < oldNodes.Count; i++)
                unmatchedOldIndices.Add(i);

            var keyedOldNodes = BuildKeyedIndex(oldNodes);
            var positionalOldByType = BuildPositionalIndex(oldNodes);

            for (var newIndex = 0; newIndex < newElements.Count; newIndex++)
            {
                var newElement = newElements[newIndex];
                var candidate = FindMatch(
                    newElement,
                    unmatchedOldIndices,
                    keyedOldNodes,
                    positionalOldByType,
                    out var oldIndex
                );

                if (candidate == null)
                {
                    ops.Add(DiffOp.Insert(newIndex, newElement));
#if FRAM3_FRAMEWORK_DIAGNOSTICS
                    Fram3Diagnostics.CurrentFrame.DiffOpsInsert++;
#endif
                }
                else
                {
                    // A node is in-place when its old index equals the new index and
                    // no old node with a smaller index is still unmatched (which would
                    // shift this node's effective position). Check before removing so
                    // that SortedSet.Min reflects all still-unmatched indices.
                    var isInPlace = oldIndex == newIndex &&
                                    unmatchedOldIndices.Min == oldIndex;

                    unmatchedOldIndices.Remove(oldIndex);

                    if (isInPlace)
                    {
                        ops.Add(DiffOp.Update(newIndex, candidate, newElement));
#if FRAM3_FRAMEWORK_DIAGNOSTICS
                        Fram3Diagnostics.CurrentFrame.DiffOpsUpdate++;
#endif
                    }
                    else
                    {
                        ops.Add(DiffOp.Move(newIndex, oldIndex, candidate, newElement));
#if FRAM3_FRAMEWORK_DIAGNOSTICS
                        Fram3Diagnostics.CurrentFrame.DiffOpsMove++;
#endif
                    }
                }
            }

            AppendRemoveOps(oldNodes, unmatchedOldIndices, ops);
#if FRAM3_FRAMEWORK_DIAGNOSTICS
            Fram3Diagnostics.CurrentFrame.DiffOpsRemove += unmatchedOldIndices.Count;
            Fram3Diagnostics.CurrentFrame.DiffDurationTicks += Stopwatch.GetTimestamp() - diffStart;
#endif

            return ops;
        }

        private static Dictionary<Key, (Node node, int index)> BuildKeyedIndex(
            IReadOnlyList<Node> oldNodes
        )
        {
            var map = new Dictionary<Key, (Node, int)>();
            for (var i = 0; i < oldNodes.Count; i++)
            {
                var key = oldNodes[i].Element.Key;
                if (key != null)
                {
                    map[key] = (oldNodes[i], i);
                }
            }

            return map;
        }

        private static Dictionary<Type, Queue<(Node node, int index)>> BuildPositionalIndex(
            IReadOnlyList<Node> oldNodes
        )
        {
            var map = new Dictionary<Type, Queue<(Node, int)>>();
            for (var i = 0; i < oldNodes.Count; i++)
            {
                var node = oldNodes[i];
                if (node.Element.Key != null)
                {
                    continue;
                }

                var t = node.Element.GetType();
                if (!map.TryGetValue(t, out var queue))
                {
                    queue = new Queue<(Node, int)>();
                    map[t] = queue;
                }

                queue.Enqueue((node, i));
            }

            return map;
        }

        private static Node? FindMatch(
            Element newElement,
            SortedSet<int> unmatchedOldIndices,
            Dictionary<Key, (Node node, int index)> keyedOldNodes,
            Dictionary<Type, Queue<(Node node, int index)>> positionalOldByType,
            out int oldIndex
        )
        {
            return newElement.Key != null
                ? FindKeyedMatch(newElement, unmatchedOldIndices, keyedOldNodes, out oldIndex)
                : FindPositionalMatch(newElement, unmatchedOldIndices, positionalOldByType, out oldIndex);
        }

        private static Node? FindKeyedMatch(
            Element newElement,
            SortedSet<int> unmatchedOldIndices,
            Dictionary<Key, (Node node, int index)> keyedOldNodes,
            out int oldIndex
        )
        {
            if (newElement.Key != null && keyedOldNodes.TryGetValue(newElement.Key, out var entry))
            {
                if (unmatchedOldIndices.Contains(entry.index) &&
                    Element.CanUpdate(entry.node.Element, newElement))
                {
                    oldIndex = entry.index;
                    return entry.node;
                }
            }

            oldIndex = -1;
            return null;
        }

        private static Node? FindPositionalMatch(
            Element newElement,
            SortedSet<int> unmatchedOldIndices,
            Dictionary<Type, Queue<(Node node, int index)>> positionalOldByType,
            out int oldIndex
        )
        {
            var type = newElement.GetType();
            if (positionalOldByType.TryGetValue(type, out var queue))
            {
                while (queue.Count > 0)
                {
                    var (node, index) = queue.Peek();
                    if (!unmatchedOldIndices.Contains(index))
                    {
                        queue.Dequeue();
                        continue;
                    }

                    if (Element.CanUpdate(node.Element, newElement))
                    {
                        queue.Dequeue();
                        oldIndex = index;
                        return node;
                    }

                    break;
                }
            }

            oldIndex = -1;

            return null;
        }

        private static void AppendRemoveOps(
            IReadOnlyList<Node> oldNodes,
            SortedSet<int> unmatchedOldIndices,
            List<DiffOp> ops
        )
        {
            foreach (var i in unmatchedOldIndices)
            {
                ops.Insert(0, DiffOp.Remove(oldNodes[i]));
            }
        }
    }
}