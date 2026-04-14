using Fram3.UI.Core;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    public class ElementTests
    {
        [Test]
        public void CanUpdate_SameTypeNoKey_ReturnsTrue()
        {
            var old = new TestLeafElement("a");
            var next = new TestLeafElement("b");

            Assert.That(Element.CanUpdate(old, next), Is.True);
        }

        [Test]
        public void CanUpdate_SameTypeSameKey_ReturnsTrue()
        {
            var key1 = new ValueKey<int>(1);
            var key2 = new ValueKey<int>(1);
            var old = new TestLeafElement("a", key1);
            var next = new TestLeafElement("b", key2);

            Assert.That(Element.CanUpdate(old, next), Is.True);
        }

        [Test]
        public void CanUpdate_SameTypeDifferentKey_ReturnsFalse()
        {
            var old = new TestLeafElement("a", new ValueKey<int>(1));
            var next = new TestLeafElement("b", new ValueKey<int>(2));

            Assert.That(Element.CanUpdate(old, next), Is.False);
        }

        [Test]
        public void CanUpdate_DifferentType_ReturnsFalse()
        {
            var old = new TestLeafElement("a");
            var next = new TestSingleChildElement { Child = new TestLeafElement("b") };

            Assert.That(Element.CanUpdate(old, next), Is.False);
        }

        [Test]
        public void CanUpdate_OneHasKeyOtherDoesNot_ReturnsFalse()
        {
            var old = new TestLeafElement("a", new ValueKey<int>(1));
            var next = new TestLeafElement("b");

            Assert.That(Element.CanUpdate(old, next), Is.False);
        }

        [Test]
        public void CanUpdate_NullOldElement_ReturnsFalse()
        {
            var next = new TestLeafElement("b");

            Assert.That(Element.CanUpdate(null, next), Is.False);
        }

        [Test]
        public void CanUpdate_NullNewElement_ReturnsFalse()
        {
            var old = new TestLeafElement("a");

            Assert.That(Element.CanUpdate(old, null), Is.False);
        }

        [Test]
        public void CanUpdate_BothNull_ReturnsFalse()
        {
            Assert.That(Element.CanUpdate(null, null), Is.False);
        }

        [Test]
        public void GetChildren_LeafElement_ReturnsEmpty()
        {
            var element = new TestLeafElement("leaf");

            Assert.That(element.GetChildren(), Is.Empty);
        }

        [Test]
        public void Key_WhenProvided_IsAccessible()
        {
            var key = new ValueKey<string>("test");
            var element = new TestLeafElement("a", key);

            Assert.That(element.Key, Is.EqualTo(key));
        }

        [Test]
        public void Key_WhenNotProvided_IsNull()
        {
            var element = new TestLeafElement("a");

            Assert.That(element.Key, Is.Null);
        }
    }
}
