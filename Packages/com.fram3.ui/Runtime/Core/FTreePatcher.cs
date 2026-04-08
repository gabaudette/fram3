using System.Collections.Generic;

namespace Fram3.UI.Core
{
    /// <summary>
    /// Applies a list of <see cref="FDiffOp"/> operations produced by <see cref="FTreeDiffer"/>
    /// to the child list of an <see cref="FNode"/>. The patcher delegates to
    /// <see cref="FNodeExpander"/> for mounting and unmounting subtrees and updates
    /// element references on existing nodes.
    /// This is an internal framework type not intended for direct use.
    /// </summary>
    internal static class FTreePatcher
    {
        /// <summary>
        /// Reconciles the children of <paramref name="parent"/> against
        /// <paramref name="newElements"/> by computing a diff and applying
        /// each operation in order.
        /// </summary>
        internal static void Patch(
            FNode parent,
            IReadOnlyList<FElement> newElements,
            FNodeExpander expander)
        {
            IReadOnlyList<FDiffOp> ops = FTreeDiffer.Diff(parent.Children, newElements);
            ApplyOps(parent, ops, expander);
        }

        private static void ApplyOps(FNode parent, IReadOnlyList<FDiffOp> ops, FNodeExpander expander)
        {
            foreach (FDiffOp op in ops)
            {
                switch (op.Kind)
                {
                    case FDiffOpKind.Remove:
                        ApplyRemove(parent, op, expander);
                        break;
                    case FDiffOpKind.Insert:
                        ApplyInsert(parent, op, expander);
                        break;
                    case FDiffOpKind.Update:
                        ApplyUpdate(op, expander);
                        break;
                    case FDiffOpKind.Move:
                        ApplyMove(parent, op, expander);
                        break;
                }
            }
        }

        private static void ApplyRemove(FNode parent, FDiffOp op, FNodeExpander expander)
        {
            expander.Unmount(op.ExistingNode);
            parent.RemoveChild(op.ExistingNode);
        }

        private static void ApplyInsert(FNode parent, FDiffOp op, FNodeExpander expander)
        {
            FNode newNode = expander.Mount(op.NewElement, parent);
            InsertOrAppend(parent, op.NewIndex, newNode);
        }

        private static void ApplyUpdate(FDiffOp op, FNodeExpander expander)
        {
            expander.UpdateElement(op.ExistingNode, op.NewElement);
            expander.Rebuild(op.ExistingNode);
        }

        private static void ApplyMove(FNode parent, FDiffOp op, FNodeExpander expander)
        {
            parent.RemoveChild(op.ExistingNode);
            expander.UpdateElement(op.ExistingNode, op.NewElement);
            InsertOrAppend(parent, op.NewIndex, op.ExistingNode);
            expander.Rebuild(op.ExistingNode);
        }

        private static void InsertOrAppend(FNode parent, int index, FNode child)
        {
            if (index >= parent.Children.Count)
            {
                parent.AddChild(child);
            }
            else
            {
                parent.InsertChild(index, child);
            }
        }
    }
}
