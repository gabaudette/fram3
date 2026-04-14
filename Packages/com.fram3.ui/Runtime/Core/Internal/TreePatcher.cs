#nullable enable
using System;
using System.Collections.Generic;

namespace Fram3.UI.Core.Internal
{
    /// <summary>
    /// Applies a list of <see cref="DiffOp"/> operations produced by <see cref="TreeDiffer"/>
    /// to the child list of an <see cref="Node"/>. The patcher delegates to
    /// <see cref="NodeExpander"/> for mounting and unmounting subtrees and updates
    /// element references on existing nodes.
    /// This is an internal framework type not intended for direct use.
    /// </summary>
    internal static class TreePatcher
    {
        /// <summary>
        /// Reconciles the children of <paramref name="parent"/> against
        /// <paramref name="newElements"/> by computing a diff and applying
        /// each operation in order.
        /// </summary>
        internal static void Patch(
            Node parent,
            IReadOnlyList<Element> newElements,
            NodeExpander expander
        )
        {
            var ops = TreeDiffer.Diff(parent.Children, newElements);
            ApplyOps(parent, ops, expander);
        }

        private static void ApplyOps(Node parent, IReadOnlyList<DiffOp> ops, NodeExpander expander)
        {
            foreach (var op in ops)
            {
                switch (op.Kind)
                {
                    case DiffOpKind.Remove:
                        ApplyRemove(parent, op, expander);
                        break;
                    case DiffOpKind.Insert:
                        ApplyInsert(parent, op, expander);
                        break;
                    case DiffOpKind.Update:
                        ApplyUpdate(op, expander);
                        break;
                    case DiffOpKind.Move:
                        ApplyMove(parent, op, expander);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static void ApplyRemove(Node parent, DiffOp op, NodeExpander expander)
        {
            if (op.ExistingNode == null)
            {
                return;
            }

            expander.Unmount(op.ExistingNode);
            parent.RemoveChild(op.ExistingNode);
        }

        private static void ApplyInsert(Node parent, DiffOp op, NodeExpander expander)
        {
            if (op.NewElement == null)
            {
                return;
            }

            var newNode = expander.Mount(op.NewElement, parent);
            InsertOrAppend(parent, op.NewIndex, newNode);
        }

        private static void ApplyUpdate(DiffOp op, NodeExpander expander)
        {
            if (op.ExistingNode == null)
            {
                return;
            }

            if (op.NewElement != null)
            {
                expander.UpdateElement(op.ExistingNode, op.NewElement);
            }

            expander.Rebuild(op.ExistingNode);
        }

        private static void ApplyMove(Node parent, DiffOp op, NodeExpander expander)
        {
            if (op.ExistingNode == null)
            {
                return;
            }

            parent.RemoveChild(op.ExistingNode);

            if (op.NewElement != null)
            {
                expander.UpdateElement(op.ExistingNode, op.NewElement);
            }

            InsertOrAppend(parent, op.NewIndex, op.ExistingNode);
            expander.Rebuild(op.ExistingNode);
        }

        private static void InsertOrAppend(Node parent, int index, Node child)
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