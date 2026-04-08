using Fram3.UI.Core;
using System.Collections.Generic;

namespace Fram3.UI.Core.Internal
{
    /// <summary>
    /// Produces a minimal list of <see cref="FDiffOp"/> operations that transform an
    /// existing list of child <see cref="FNode"/>s to reflect a new list of child
    /// <see cref="FElement"/>s. The algorithm prioritises keyed matching, then falls
    /// back to positional matching for unkeyed elements of the same type.
    /// This is an internal framework type not intended for direct use.
    /// </summary>
    internal static class FTreeDiffer
    {
        /// <summary>
        /// Computes the diff between an old node list and a new element list.
        /// </summary>
        /// <param name="oldNodes">Current child nodes of the parent.</param>
        /// <param name="newElements">Desired child elements from the latest build.</param>
        /// <returns>
        /// An ordered list of operations to apply. Operations are ordered so that
        /// <see cref="FDiffOpKind.Remove"/> operations appear first, followed by
        /// <see cref="FDiffOpKind.Insert"/>, <see cref="FDiffOpKind.Update"/>, and
        /// <see cref="FDiffOpKind.Move"/> operations in new-index order.
        /// </returns>
        internal static IReadOnlyList<FDiffOp> Diff(
            IReadOnlyList<FNode> oldNodes,
            IReadOnlyList<FElement> newElements)
        {
            var ops = new List<FDiffOp>();
            var matched = new bool[oldNodes.Count];

            var keyedOldNodes = BuildKeyedIndex(oldNodes);
            var positionalOldByType = BuildPositionalIndex(oldNodes);

            for (int newIndex = 0; newIndex < newElements.Count; newIndex++)
            {
                FElement newElement = newElements[newIndex];
                FNode candidate = FindMatch(
                    newElement,
                    oldNodes,
                    matched,
                    keyedOldNodes,
                    positionalOldByType,
                    out int oldIndex);

                if (candidate == null)
                {
                    ops.Add(FDiffOp.Insert(newIndex, newElement));
                }
                else
                {
                    matched[oldIndex] = true;
                    bool isInPlace = oldIndex == newIndex && !HasUnmatchedBefore(matched, oldIndex);
                    if (isInPlace)
                    {
                        ops.Add(FDiffOp.Update(newIndex, candidate, newElement));
                    }
                    else
                    {
                        ops.Add(FDiffOp.Move(newIndex, oldIndex, candidate, newElement));
                    }
                }
            }

            AppendRemoveOps(oldNodes, matched, ops);

            return ops;
        }

        private static Dictionary<FKey, (FNode node, int index)> BuildKeyedIndex(
            IReadOnlyList<FNode> oldNodes)
        {
            var map = new Dictionary<FKey, (FNode, int)>();
            for (int i = 0; i < oldNodes.Count; i++)
            {
                FKey key = oldNodes[i].Element.Key;
                if (key != null)
                {
                    map[key] = (oldNodes[i], i);
                }
            }
            return map;
        }

        private static Dictionary<System.Type, Queue<(FNode node, int index)>> BuildPositionalIndex(
            IReadOnlyList<FNode> oldNodes)
        {
            var map = new Dictionary<System.Type, Queue<(FNode, int)>>();
            for (int i = 0; i < oldNodes.Count; i++)
            {
                FNode node = oldNodes[i];
                if (node.Element.Key != null)
                {
                    continue;
                }
                System.Type t = node.Element.GetType();
                if (!map.TryGetValue(t, out Queue<(FNode, int)> queue))
                {
                    queue = new Queue<(FNode, int)>();
                    map[t] = queue;
                }
                queue.Enqueue((node, i));
            }
            return map;
        }

        private static FNode FindMatch(
            FElement newElement,
            IReadOnlyList<FNode> oldNodes,
            bool[] matched,
            Dictionary<FKey, (FNode node, int index)> keyedOldNodes,
            Dictionary<System.Type, Queue<(FNode node, int index)>> positionalOldByType,
            out int oldIndex)
        {
            if (newElement.Key != null)
            {
                return FindKeyedMatch(newElement, matched, keyedOldNodes, out oldIndex);
            }
            return FindPositionalMatch(newElement, matched, positionalOldByType, out oldIndex);
        }

        private static FNode FindKeyedMatch(
            FElement newElement,
            bool[] matched,
            Dictionary<FKey, (FNode node, int index)> keyedOldNodes,
            out int oldIndex)
        {
            if (keyedOldNodes.TryGetValue(newElement.Key, out (FNode node, int index) entry))
            {
                if (!matched[entry.index] && FElement.CanUpdate(entry.node.Element, newElement))
                {
                    oldIndex = entry.index;
                    return entry.node;
                }
            }
            oldIndex = -1;
            return null;
        }

        private static FNode FindPositionalMatch(
            FElement newElement,
            bool[] matched,
            Dictionary<System.Type, Queue<(FNode node, int index)>> positionalOldByType,
            out int oldIndex)
        {
            System.Type type = newElement.GetType();
            if (positionalOldByType.TryGetValue(type, out Queue<(FNode node, int index)> queue))
            {
                while (queue.Count > 0)
                {
                    (FNode node, int index) = queue.Peek();
                    if (matched[index])
                    {
                        queue.Dequeue();
                        continue;
                    }
                    if (FElement.CanUpdate(node.Element, newElement))
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

        private static bool HasUnmatchedBefore(bool[] matched, int index)
        {
            for (int i = 0; i < index; i++)
            {
                if (!matched[i])
                {
                    return true;
                }
            }
            return false;
        }

        private static void AppendRemoveOps(
            IReadOnlyList<FNode> oldNodes,
            bool[] matched,
            List<FDiffOp> ops)
        {
            for (int i = 0; i < oldNodes.Count; i++)
            {
                if (!matched[i])
                {
                    ops.Insert(0, FDiffOp.Remove(oldNodes[i]));
                }
            }
        }
    }
}
