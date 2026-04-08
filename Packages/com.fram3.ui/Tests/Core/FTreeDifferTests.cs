using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Fram3.UI.Core;
using Fram3.UI.Tests.Mocks;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    internal sealed class FTreeDifferTests
    {
        private static FNode MakeNode(FElement element, FNode parent = null)
        {
            return new FNode(element, parent);
        }

        [Test]
        public void Diff_BothEmpty_ReturnsNoOps()
        {
            var ops = FTreeDiffer.Diff(
                new List<FNode>(),
                new List<FElement>());

            Assert.That(ops, Is.Empty);
        }

        [Test]
        public void Diff_NoOldNodes_InsertsAll()
        {
            var elements = new FElement[]
            {
                new TestLeafElement("a"),
                new TestLeafElement("b")
            };

            var ops = FTreeDiffer.Diff(new List<FNode>(), elements);

            Assert.That(ops.Count, Is.EqualTo(2));
            Assert.That(ops[0].Kind, Is.EqualTo(FDiffOpKind.Insert));
            Assert.That(ops[0].NewIndex, Is.EqualTo(0));
            Assert.That(ops[1].Kind, Is.EqualTo(FDiffOpKind.Insert));
            Assert.That(ops[1].NewIndex, Is.EqualTo(1));
        }

        [Test]
        public void Diff_NoNewElements_RemovesAll()
        {
            var nodes = new List<FNode>
            {
                MakeNode(new TestLeafElement("a")),
                MakeNode(new TestLeafElement("b"))
            };

            var ops = FTreeDiffer.Diff(nodes, new List<FElement>());

            Assert.That(ops.Count, Is.EqualTo(2));
            Assert.That(ops[0].Kind, Is.EqualTo(FDiffOpKind.Remove));
            Assert.That(ops[1].Kind, Is.EqualTo(FDiffOpKind.Remove));
        }

        [Test]
        public void Diff_SameTypeAndPosition_ProducesUpdate()
        {
            var oldNode = MakeNode(new TestLeafElement("a"));
            var newElement = new TestLeafElement("b");

            var ops = FTreeDiffer.Diff(
                new List<FNode> { oldNode },
                new List<FElement> { newElement });

            Assert.That(ops.Count, Is.EqualTo(1));
            Assert.That(ops[0].Kind, Is.EqualTo(FDiffOpKind.Update));
            Assert.That(ops[0].ExistingNode, Is.SameAs(oldNode));
            Assert.That(ops[0].NewElement, Is.SameAs(newElement));
        }

        [Test]
        public void Diff_DifferentType_RemovesThenInserts()
        {
            var oldNode = MakeNode(new TestLeafElement("a"));
            var newElement = new TestSingleChildElement(new TestLeafElement("child"));

            var ops = FTreeDiffer.Diff(
                new List<FNode> { oldNode },
                new List<FElement> { newElement });

            Assert.That(ops.Count, Is.EqualTo(2));
            Assert.That(ops[0].Kind, Is.EqualTo(FDiffOpKind.Remove));
            Assert.That(ops[0].ExistingNode, Is.SameAs(oldNode));
            Assert.That(ops[1].Kind, Is.EqualTo(FDiffOpKind.Insert));
            Assert.That(ops[1].NewElement, Is.SameAs(newElement));
        }

        [Test]
        public void Diff_KeyedMatch_ProducesUpdateRegardlessOfPosition()
        {
            var key = new FValueKey<string>("k");
            var oldNode = MakeNode(new TestLeafElement("a", key));
            var newElement = new TestLeafElement("b", key);

            var unrelatedOldNode = MakeNode(new TestLeafElement("x"));

            var ops = FTreeDiffer.Diff(
                new List<FNode> { unrelatedOldNode, oldNode },
                new List<FElement> { newElement });

            var updateOrMove = ops.Where(o =>
                o.Kind == FDiffOpKind.Update || o.Kind == FDiffOpKind.Move).ToList();
            Assert.That(updateOrMove.Count, Is.EqualTo(1));
            Assert.That(updateOrMove[0].ExistingNode, Is.SameAs(oldNode));
            Assert.That(updateOrMove[0].NewElement, Is.SameAs(newElement));
        }

        [Test]
        public void Diff_KeyedNodeMovedForward_ProducesMove()
        {
            var key = new FValueKey<string>("k");
            var keyedNode = MakeNode(new TestLeafElement("a", key));
            var unkeyedNode = MakeNode(new TestLeafElement("b"));

            var newElements = new FElement[]
            {
                new TestLeafElement("b"),
                new TestLeafElement("c", key)
            };

            var ops = FTreeDiffer.Diff(
                new List<FNode> { keyedNode, unkeyedNode },
                newElements);

            var moveOp = ops.FirstOrDefault(o => o.Kind == FDiffOpKind.Move && o.NewIndex == 1);
            Assert.That(moveOp, Is.Not.Null);
            Assert.That(moveOp.ExistingNode, Is.SameAs(keyedNode));
            Assert.That(moveOp.NewIndex, Is.EqualTo(1));
        }

        [Test]
        public void Diff_RemoveOps_PrecedeInsertAndUpdateOps()
        {
            var nodeToRemove = MakeNode(new TestSingleChildElement(new TestLeafElement("x")));
            var nodeToKeep = MakeNode(new TestLeafElement("a"));
            var newElement = new TestLeafElement("b");

            var ops = FTreeDiffer.Diff(
                new List<FNode> { nodeToRemove, nodeToKeep },
                new List<FElement> { newElement });

            Assert.That(ops[0].Kind, Is.EqualTo(FDiffOpKind.Remove));
        }

        [Test]
        public void Diff_SameKeyDifferentType_DoesNotMatch()
        {
            var key = new FValueKey<int>(1);
            var oldNode = MakeNode(new TestLeafElement("a", key));
            var newElement = new TestSingleChildElement(new TestLeafElement("child"), key);

            var ops = FTreeDiffer.Diff(
                new List<FNode> { oldNode },
                new List<FElement> { newElement });

            Assert.That(ops.Any(o => o.Kind == FDiffOpKind.Remove), Is.True);
            Assert.That(ops.Any(o => o.Kind == FDiffOpKind.Insert), Is.True);
        }
    }
}
