using NUnit.Framework;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Mocks;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    internal sealed class FDiffOpTests
    {
        [Test]
        public void Insert_SetsKindAndNewIndexAndNewElement()
        {
            var element = new TestLeafElement("a");
            var op = FDiffOp.Insert(2, element);

            Assert.That(op.Kind, Is.EqualTo(FDiffOpKind.Insert));
            Assert.That(op.NewIndex, Is.EqualTo(2));
            Assert.That(op.NewElement, Is.SameAs(element));
        }

        [Test]
        public void Insert_NullElement_ThrowsArgumentNullException()
        {
            if (Throws.ArgumentNullException != null)
            {
                Assert.That(() => FDiffOp.Insert(0, null), Throws.ArgumentNullException);
            }
        }

        [Test]
        public void Update_SetsKindAndFields()
        {
            var element = new TestLeafElement("a");
            var node = new FNode(element, null);
            var newElement = new TestLeafElement("b");

            var op = FDiffOp.Update(1, node, newElement);

            Assert.That(op.Kind, Is.EqualTo(FDiffOpKind.Update));
            Assert.That(op.NewIndex, Is.EqualTo(1));
            Assert.That(op.ExistingNode, Is.SameAs(node));
            Assert.That(op.NewElement, Is.SameAs(newElement));
        }

        [Test]
        public void Update_NullNode_ThrowsArgumentNullException()
        {
            Assert.That(
                () => FDiffOp.Update(0, null, new TestLeafElement("a")),
                Throws.ArgumentNullException);
        }

        [Test]
        public void Remove_SetsKindAndExistingNode()
        {
            var element = new TestLeafElement("a");
            var node = new FNode(element, null);

            FDiffOp op = FDiffOp.Remove(node);

            Assert.That(op.Kind, Is.EqualTo(FDiffOpKind.Remove));
            Assert.That(op.ExistingNode, Is.SameAs(node));
        }

        [Test]
        public void Remove_NullNode_ThrowsArgumentNullException()
        {
            Assert.That(() => FDiffOp.Remove(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Move_SetsAllFields()
        {
            var element = new TestLeafElement("a");
            var node = new FNode(element, null);
            var newElement = new TestLeafElement("b");

            FDiffOp op = FDiffOp.Move(3, 1, node, newElement);

            Assert.That(op.Kind, Is.EqualTo(FDiffOpKind.Move));
            Assert.That(op.NewIndex, Is.EqualTo(3));
            Assert.That(op.OldIndex, Is.EqualTo(1));
            Assert.That(op.ExistingNode, Is.SameAs(node));
            Assert.That(op.NewElement, Is.SameAs(newElement));
        }
    }
}
