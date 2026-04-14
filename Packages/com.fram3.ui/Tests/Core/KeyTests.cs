#nullable enable
using System;
using Fram3.UI.Core;
using NUnit.Framework;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    public class KeyTests
    {
        [Test]
        public void ValueKeys_WithSameValue_AreEqual()
        {
            var key1 = new ValueKey<int>(42);
            var key2 = new ValueKey<int>(42);

            Assert.That(key1, Is.EqualTo(key2));
        }

        [Test]
        public void ValueKeys_WithDifferentValues_AreNotEqual()
        {
            var key1 = new ValueKey<int>(1);
            var key2 = new ValueKey<int>(2);

            Assert.That(key1, Is.Not.EqualTo(key2));
        }

        [Test]
        public void ValueKeys_WithDifferentTypes_AreNotEqual()
        {
            var key1 = new ValueKey<int>(1);
            var key2 = new ValueKey<string>("1");

            Assert.That(key1.Equals(key2), Is.False);
        }

        [Test]
        public void ValueKeys_WithSameStringValue_AreEqual()
        {
            var key1 = new ValueKey<string>("hello");
            var key2 = new ValueKey<string>("hello");

            Assert.That(key1, Is.EqualTo(key2));
        }

        [Test]
        public void ValueKeys_WithNullValues_AreEqual()
        {
            var key1 = new ValueKey<string?>(null);
            var key2 = new ValueKey<string?>(null);

            Assert.That(key1, Is.EqualTo(key2));
        }

        [Test]
        public void ValueKeys_OneNullOneNot_AreNotEqual()
        {
            var key1 = new ValueKey<string?>(null);
            var key2 = new ValueKey<string>("hello");

            Assert.That(key1, Is.Not.EqualTo(key2));
        }

        [Test]
        public void ValueKeys_SameValue_ProduceSameHashCode()
        {
            var key1 = new ValueKey<int>(42);
            var key2 = new ValueKey<int>(42);

            Assert.That(key1.GetHashCode(), Is.EqualTo(key2.GetHashCode()));
        }

        [Test]
        public void ObjectKey_SameInstance_AreEqual()
        {
            var obj = new object();
            var key1 = new ObjectKey(obj);
            var key2 = new ObjectKey(obj);

            Assert.That(key1, Is.EqualTo(key2));
        }

        [Test]
        public void ObjectKey_DifferentInstances_AreNotEqual()
        {
            var key1 = new ObjectKey(new object());
            var key2 = new ObjectKey(new object());

            Assert.That(key1, Is.Not.EqualTo(key2));
        }

        [Test]
        public void ObjectKey_NullValue_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObjectKey(null));
        }

        [Test]
        public void UniqueKey_TwoInstances_AreNeverEqual()
        {
            var key1 = new UniqueKey();
            var key2 = new UniqueKey();

            Assert.That(key1, Is.Not.EqualTo(key2));
        }

        [Test]
        public void UniqueKey_SameInstance_IsEqualToItself()
        {
            var key = new UniqueKey();

            Assert.That(key, Is.EqualTo(key));
        }

        [Test]
        public void OperatorEquals_BothNull_ReturnsTrue()
        {
            Key? key1 = null;
            Key? key2 = null;

            Assert.That(key1 == key2, Is.True);
        }

        [Test]
        public void OperatorEquals_OneNull_ReturnsFalse()
        {
            Key key1 = new ValueKey<int>(1);
            Key? key2 = null;

            Assert.That(key1 == key2, Is.False);
            Assert.That(key2 == key1, Is.False);
        }

        [Test]
        public void OperatorNotEquals_DifferentValues_ReturnsTrue()
        {
            Key key1 = new ValueKey<int>(1);
            Key key2 = new ValueKey<int>(2);

            Assert.That(key1 != key2, Is.True);
        }
    }
}
