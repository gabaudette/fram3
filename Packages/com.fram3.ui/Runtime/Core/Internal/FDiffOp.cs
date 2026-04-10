#nullable enable
using System;

namespace Fram3.UI.Core.Internal
{
    internal enum FDiffOpKind
    {
        Insert,
        Update,
        Remove,
        Move
    }

    /// <summary>
    /// Represents a single patch operation produced by the tree differ.
    /// Operations are applied in order by FTreePatcher to transform the
    /// existing FNode child list to match a new FElement child list.
    /// This is an internal framework type not intended for direct use.
    /// </summary>
    internal sealed class FDiffOp
    {
        private FDiffOp()
        {
        }

        internal FDiffOpKind Kind { get; private init; }

        /// <summary>Index in the new child list this operation targets.</summary>
        internal int NewIndex { get; private init; }

        /// <summary>The new element to insert or update with.</summary>
        internal FElement? NewElement { get; private init; }

        /// <summary>The existing node being updated, moved, or removed.</summary>
        internal FNode? ExistingNode { get; private init; }

        /// <summary>
        /// Original index of the node in the old child list (used for Move).
        /// </summary>
        internal int OldIndex { get; private init; }

        internal static FDiffOp Insert(int newIndex, FElement? newElement) =>
            new FDiffOp
            {
                Kind = FDiffOpKind.Insert,
                NewIndex = newIndex,
                NewElement = newElement ?? throw new ArgumentNullException(nameof(newElement))
            };

        internal static FDiffOp Update(int newIndex, FNode existingNode, FElement newElement) =>
            new FDiffOp
            {
                Kind = FDiffOpKind.Update,
                NewIndex = newIndex,
                ExistingNode = existingNode ?? throw new ArgumentNullException(nameof(existingNode)),
                NewElement = newElement ?? throw new ArgumentNullException(nameof(newElement))
            };

        internal static FDiffOp Remove(FNode existingNode) =>
            new FDiffOp
            {
                Kind = FDiffOpKind.Remove,
                ExistingNode = existingNode ?? throw new ArgumentNullException(nameof(existingNode))
            };

        internal static FDiffOp Move(int newIndex, int oldIndex, FNode existingNode, FElement newElement) =>
            new FDiffOp
            {
                Kind = FDiffOpKind.Move,
                NewIndex = newIndex,
                OldIndex = oldIndex,
                ExistingNode = existingNode ?? throw new ArgumentNullException(nameof(existingNode)),
                NewElement = newElement ?? throw new ArgumentNullException(nameof(newElement))
            };
    }
}