#nullable enable
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Mocks;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    internal sealed class TreeDifferTests
    {
        private static Node MakeNode(Element element, Node? parent = null)
        {
            return new Node(element, parent);
        }

        [Test]
        public void Diff_BothEmpty_ReturnsNoOps()
        {
            var ops = TreeDiffer.Diff(
                new List<Node>(),
                new List<Element>());

            Assert.That(ops, Is.Empty);
        }

        [Test]
        public void Diff_NoOldNodes_InsertsAll()
        {
            var elements = new Element[]
            {
                new TestLeafElement("a"),
                new TestLeafElement("b")
            };

            var ops = TreeDiffer.Diff(new List<Node>(), elements);

            Assert.That(ops.Count, Is.EqualTo(2));
            Assert.That(ops[0].Kind, Is.EqualTo(DiffOpKind.Insert));
            Assert.That(ops[0].NewIndex, Is.EqualTo(0));
            Assert.That(ops[1].Kind, Is.EqualTo(DiffOpKind.Insert));
            Assert.That(ops[1].NewIndex, Is.EqualTo(1));
        }

        [Test]
        public void Diff_NoNewElements_RemovesAll()
        {
            var nodes = new List<Node>
            {
                MakeNode(new TestLeafElement("a")),
                MakeNode(new TestLeafElement("b"))
            };

            var ops = TreeDiffer.Diff(nodes, new List<Element>());

            Assert.That(ops.Count, Is.EqualTo(2));
            Assert.That(ops[0].Kind, Is.EqualTo(DiffOpKind.Remove));
            Assert.That(ops[1].Kind, Is.EqualTo(DiffOpKind.Remove));
        }

        [Test]
        public void Diff_SameTypeAndPosition_ProducesUpdate()
        {
            var oldNode = MakeNode(new TestLeafElement("a"));
            var newElement = new TestLeafElement("b");

            var ops = TreeDiffer.Diff(
                new List<Node> { oldNode },
                new List<Element> { newElement });

            Assert.That(ops.Count, Is.EqualTo(1));
            Assert.That(ops[0].Kind, Is.EqualTo(DiffOpKind.Update));
            Assert.That(ops[0].ExistingNode, Is.SameAs(oldNode));
            Assert.That(ops[0].NewElement, Is.SameAs(newElement));
        }

        [Test]
        public void Diff_DifferentType_RemovesThenInserts()
        {
            var oldNode = MakeNode(new TestLeafElement("a"));
            var newElement = new TestSingleChildElement { Child = new TestLeafElement("child") };

            var ops = TreeDiffer.Diff(
                new List<Node> { oldNode },
                new List<Element> { newElement });

            Assert.That(ops.Count, Is.EqualTo(2));
            Assert.That(ops[0].Kind, Is.EqualTo(DiffOpKind.Remove));
            Assert.That(ops[0].ExistingNode, Is.SameAs(oldNode));
            Assert.That(ops[1].Kind, Is.EqualTo(DiffOpKind.Insert));
            Assert.That(ops[1].NewElement, Is.SameAs(newElement));
        }

        [Test]
        public void Diff_KeyedMatch_ProducesUpdateRegardlessOfPosition()
        {
            var key = new ValueKey<string>("k");
            var oldNode = MakeNode(new TestLeafElement("a", key));
            var newElement = new TestLeafElement("b", key);

            var unrelatedOldNode = MakeNode(new TestLeafElement("x"));

            var ops = TreeDiffer.Diff(
                new List<Node> { unrelatedOldNode, oldNode },
                new List<Element> { newElement });

            var updateOrMove = ops.Where(o =>
                o.Kind == DiffOpKind.Update || o.Kind == DiffOpKind.Move).ToList();
            Assert.That(updateOrMove.Count, Is.EqualTo(1));
            Assert.That(updateOrMove[0].ExistingNode, Is.SameAs(oldNode));
            Assert.That(updateOrMove[0].NewElement, Is.SameAs(newElement));
        }

        [Test]
        public void Diff_KeyedNodeMovedForward_ProducesMove()
        {
            var key = new ValueKey<string>("k");
            var keyedNode = MakeNode(new TestLeafElement("a", key));
            var unkeyedNode = MakeNode(new TestLeafElement("b"));

            var newElements = new Element[]
            {
                new TestLeafElement("b"),
                new TestLeafElement("c", key)
            };

            var ops = TreeDiffer.Diff(
                new List<Node> { keyedNode, unkeyedNode },
                newElements);

            var moveOp = ops.FirstOrDefault(o => o.Kind == DiffOpKind.Move && o.NewIndex == 1);
            Assert.That(moveOp, Is.Not.Null);
            Assert.That(moveOp?.ExistingNode, Is.SameAs(keyedNode));
            if (moveOp != null)
            {
                Assert.That(moveOp.NewIndex, Is.EqualTo(1));
            }
        }

        [Test]
        public void Diff_RemoveOps_PrecedeInsertAndUpdateOps()
        {
            var nodeToRemove = MakeNode(new TestSingleChildElement { Child = new TestLeafElement("x") });
            var nodeToKeep = MakeNode(new TestLeafElement("a"));
            var newElement = new TestLeafElement("b");

            var ops = TreeDiffer.Diff(
                new List<Node> { nodeToRemove, nodeToKeep },
                new List<Element> { newElement });

            Assert.That(ops[0].Kind, Is.EqualTo(DiffOpKind.Remove));
        }

        [Test]
        public void Diff_SameKeyDifferentType_DoesNotMatch()
        {
            var key = new ValueKey<int>(1);
            var oldNode = MakeNode(new TestLeafElement("a", key));
            var newElement = new TestSingleChildElement(key) { Child = new TestLeafElement("child") };

            var ops = TreeDiffer.Diff(
                new List<Node> { oldNode },
                new List<Element> { newElement });

            Assert.That(ops.Any(o => o.Kind == DiffOpKind.Remove), Is.True);
            Assert.That(ops.Any(o => o.Kind == DiffOpKind.Insert), Is.True);
        }
    }
}
