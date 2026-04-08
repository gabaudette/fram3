using System;
using Fram3.UI.Core;
using Fram3.UI.Tests.Mocks;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    public class FNodeTests
    {
        [Test]
        public void Constructor_SetsElement()
        {
            var element = new TestLeafElement("test");
            var node = new FNode(element, null);

            Assert.That(node.Element, Is.SameAs(element));
        }

        [Test]
        public void Constructor_NullElement_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new FNode(null, null));
        }

        [Test]
        public void Constructor_SetsParent()
        {
            var parent = new FNode(new TestLeafElement("parent"), null);
            var child = new FNode(new TestLeafElement("child"), parent);

            Assert.That(child.Parent, Is.SameAs(parent));
        }

        [Test]
        public void Constructor_NullParent_IsRoot()
        {
            var node = new FNode(new TestLeafElement("root"), null);

            Assert.That(node.Parent, Is.Null);
            Assert.That(node.Depth, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_CreatesContext()
        {
            var node = new FNode(new TestLeafElement("test"), null);

            Assert.That(node.Context, Is.Not.Null);
        }

        [Test]
        public void AddChild_AddsToChildrenList()
        {
            var parent = new FNode(new TestLeafElement("parent"), null);
            var child = new FNode(new TestLeafElement("child"), parent);

            parent.AddChild(child);

            Assert.That(parent.Children, Has.Count.EqualTo(1));
            Assert.That(parent.Children[0], Is.SameAs(child));
        }

        [Test]
        public void RemoveChild_RemovesFromChildrenList()
        {
            var parent = new FNode(new TestLeafElement("parent"), null);
            var child = new FNode(new TestLeafElement("child"), parent);
            parent.AddChild(child);

            parent.RemoveChild(child);

            Assert.That(parent.Children, Is.Empty);
        }

        [Test]
        public void ClearChildren_RemovesAllChildren()
        {
            var parent = new FNode(new TestLeafElement("parent"), null);
            parent.AddChild(new FNode(new TestLeafElement("a"), parent));
            parent.AddChild(new FNode(new TestLeafElement("b"), parent));

            parent.ClearChildren();

            Assert.That(parent.Children, Is.Empty);
        }

        [Test]
        public void InsertChild_InsertsAtIndex()
        {
            var parent = new FNode(new TestLeafElement("parent"), null);
            var a = new FNode(new TestLeafElement("a"), parent);
            var b = new FNode(new TestLeafElement("b"), parent);
            var c = new FNode(new TestLeafElement("c"), parent);
            parent.AddChild(a);
            parent.AddChild(c);

            parent.InsertChild(1, b);

            Assert.That(parent.Children[0], Is.SameAs(a));
            Assert.That(parent.Children[1], Is.SameAs(b));
            Assert.That(parent.Children[2], Is.SameAs(c));
        }

        [Test]
        public void MarkDirty_SetsDirtyFlag()
        {
            var node = new FNode(new TestLeafElement("test"), null);

            Assert.That(node.IsDirty, Is.False);

            node.MarkDirty();

            Assert.That(node.IsDirty, Is.True);
        }

        [Test]
        public void IsDirty_CanBeCleared()
        {
            var node = new FNode(new TestLeafElement("test"), null);
            node.MarkDirty();

            node.IsDirty = false;

            Assert.That(node.IsDirty, Is.False);
        }

        [Test]
        public void State_CanBeAssigned()
        {
            var node = new FNode(new TestLeafElement("test"), null);
            var state = new TestState(ctx => new TestLeafElement("x"));

            node.State = state;

            Assert.That(node.State, Is.SameAs(state));
        }

        [Test]
        public void Element_CanBeUpdated()
        {
            var original = new TestLeafElement("original");
            var updated = new TestLeafElement("updated");
            var node = new FNode(original, null);

            node.Element = updated;

            Assert.That(node.Element, Is.SameAs(updated));
        }
    }
}
