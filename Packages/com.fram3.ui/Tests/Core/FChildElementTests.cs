using System;
using Fram3.UI.Core;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    public class FChildElementTests
    {
        [Test]
        public void SingleChildElement_StoresChild()
        {
            var child = new TestLeafElement("child");
            var parent = new TestSingleChildElement(child);

            Assert.That(parent.Child, Is.SameAs(child));
        }

        [Test]
        public void SingleChildElement_GetChildren_ReturnsSingleChild()
        {
            var child = new TestLeafElement("child");
            var parent = new TestSingleChildElement(child);

            var children = parent.GetChildren();

            Assert.That(children, Has.Count.EqualTo(1));
            Assert.That(children[0], Is.SameAs(child));
        }

        [Test]
        public void SingleChildElement_NullChild_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TestSingleChildElement(null));
        }

        [Test]
        public void MultiChildElement_StoresChildren()
        {
            var children = new FElement[]
            {
                new TestLeafElement("a"),
                new TestLeafElement("b"),
                new TestLeafElement("c")
            };
            var parent = new TestMultiChildElement(children);

            Assert.That(parent.ElementChildren, Has.Count.EqualTo(3));
        }

        [Test]
        public void MultiChildElement_GetChildren_ReturnsAllChildren()
        {
            var a = new TestLeafElement("a");
            var b = new TestLeafElement("b");
            var parent = new TestMultiChildElement(new FElement[] { a, b });

            var result = parent.GetChildren();

            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0], Is.SameAs(a));
            Assert.That(result[1], Is.SameAs(b));
        }

        [Test]
        public void MultiChildElement_NullArray_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TestMultiChildElement(null));
        }

        [Test]
        public void MultiChildElement_NullChildInArray_ThrowsArgumentException()
        {
            var children = new FElement[]
            {
                new TestLeafElement("a"),
                null,
                new TestLeafElement("c")
            };

            Assert.Throws<ArgumentException>(() => new TestMultiChildElement(children));
        }

        [Test]
        public void MultiChildElement_EmptyArray_IsValid()
        {
            var parent = new TestMultiChildElement(Array.Empty<FElement>());

            Assert.That(parent.GetChildren(), Is.Empty);
        }

        [Test]
        public void LeafElement_GetChildren_ReturnsEmpty()
        {
            var leaf = new TestLeafElement("leaf");

            Assert.That(leaf.GetChildren(), Is.Empty);
        }
    }
}
