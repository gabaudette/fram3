#nullable enable
using System;

namespace Fram3.UI.Core.Internal
{
    internal enum DiffOpKind
    {
        Insert,
        Update,
        Remove,
        Move
    }

    /// <summary>
    /// Represents a single patch operation produced by the tree differ.
    /// Operations are applied in order by TreePatcher to transform the
    /// existing Node child list to match a new Element child list.
    /// This is an internal framework type not intended for direct use.
    /// </summary>
    internal sealed class DiffOp
    {
        private DiffOp()
        {
        }

        internal DiffOpKind Kind { get; private init; }

        /// <summary>Index in the new child list this operation targets.</summary>
        internal int NewIndex { get; private init; }

        /// <summary>The new element to insert or update with.</summary>
        internal Element? NewElement { get; private init; }

        /// <summary>The existing node being updated, moved, or removed.</summary>
        internal Node? ExistingNode { get; private init; }

        /// <summary>
        /// Original index of the node in the old child list (used for Move).
        /// </summary>
        internal int OldIndex { get; private init; }

        internal static DiffOp Insert(int newIndex, Element? newElement) =>
            new()
            {
                Kind = DiffOpKind.Insert,
                NewIndex = newIndex,
                NewElement = newElement ?? throw new ArgumentNullException(nameof(newElement))
            };

        internal static DiffOp Update(int newIndex, Node existingNode, Element newElement) =>
            new()
            {
                Kind = DiffOpKind.Update,
                NewIndex = newIndex,
                ExistingNode = existingNode ?? throw new ArgumentNullException(nameof(existingNode)),
                NewElement = newElement ?? throw new ArgumentNullException(nameof(newElement))
            };

        internal static DiffOp Remove(Node existingNode) =>
            new()
            {
                Kind = DiffOpKind.Remove,
                ExistingNode = existingNode ?? throw new ArgumentNullException(nameof(existingNode))
            };

        internal static DiffOp Move(int newIndex, int oldIndex, Node existingNode, Element newElement) =>
            new()
            {
                Kind = DiffOpKind.Move,
                NewIndex = newIndex,
                OldIndex = oldIndex,
                ExistingNode = existingNode ?? throw new ArgumentNullException(nameof(existingNode)),
                NewElement = newElement ?? throw new ArgumentNullException(nameof(newElement))
            };
    }
}