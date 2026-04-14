using NUnit.Framework;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Mocks;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    internal sealed class DiffOpTests
    {
        [Test]
        public void Insert_SetsKindAndNewIndexAndNewElement()
        {
            var element = new TestLeafElement("a");
            var op = DiffOp.Insert(2, element);

            Assert.That(op.Kind, Is.EqualTo(DiffOpKind.Insert));
            Assert.That(op.NewIndex, Is.EqualTo(2));
            Assert.That(op.NewElement, Is.SameAs(element));
        }

        [Test]
        public void Insert_NullElement_ThrowsArgumentNullException()
        {
            Assert.That(() => DiffOp.Insert(0, null!), Throws.ArgumentNullException);
        }

        [Test]
        public void Update_SetsKindAndFields()
        {
            var element = new TestLeafElement("a");
            var node = new Node(element, null);
            var newElement = new TestLeafElement("b");

            var op = DiffOp.Update(1, node, newElement);

            Assert.That(op.Kind, Is.EqualTo(DiffOpKind.Update));
            Assert.That(op.NewIndex, Is.EqualTo(1));
            Assert.That(op.ExistingNode, Is.SameAs(node));
            Assert.That(op.NewElement, Is.SameAs(newElement));
        }

        [Test]
        public void Update_NullNode_ThrowsArgumentNullException()
        {
            Assert.That(
                () => DiffOp.Update(0, null!, new TestLeafElement("a")),
                Throws.ArgumentNullException);
        }

        [Test]
        public void Remove_SetsKindAndExistingNode()
        {
            var element = new TestLeafElement("a");
            var node = new Node(element, null);

            DiffOp op = DiffOp.Remove(node);

            Assert.That(op.Kind, Is.EqualTo(DiffOpKind.Remove));
            Assert.That(op.ExistingNode, Is.SameAs(node));
        }

        [Test]
        public void Remove_NullNode_ThrowsArgumentNullException()
        {
            Assert.That(() => DiffOp.Remove(null!), Throws.ArgumentNullException);
        }

        [Test]
        public void Move_SetsAllFields()
        {
            var element = new TestLeafElement("a");
            var node = new Node(element, null);
            var newElement = new TestLeafElement("b");

            DiffOp op = DiffOp.Move(3, 1, node, newElement);

            Assert.That(op.Kind, Is.EqualTo(DiffOpKind.Move));
            Assert.That(op.NewIndex, Is.EqualTo(3));
            Assert.That(op.OldIndex, Is.EqualTo(1));
            Assert.That(op.ExistingNode, Is.SameAs(node));
            Assert.That(op.NewElement, Is.SameAs(newElement));
        }
    }
}
