using System;
using Fram3.UI.Core;
using NUnit.Framework;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    public class FKeyTests
    {
        [Test]
        public void ValueKeys_WithSameValue_AreEqual()
        {
            var key1 = new FValueKey<int>(42);
            var key2 = new FValueKey<int>(42);

            Assert.That(key1, Is.EqualTo(key2));
        }

        [Test]
        public void ValueKeys_WithDifferentValues_AreNotEqual()
        {
            var key1 = new FValueKey<int>(1);
            var key2 = new FValueKey<int>(2);

            Assert.That(key1, Is.Not.EqualTo(key2));
        }

        [Test]
        public void ValueKeys_WithDifferentTypes_AreNotEqual()
        {
            var key1 = new FValueKey<int>(1);
            var key2 = new FValueKey<string>("1");

            Assert.That(key1.Equals(key2), Is.False);
        }

        [Test]
        public void ValueKeys_WithSameStringValue_AreEqual()
        {
            var key1 = new FValueKey<string>("hello");
            var key2 = new FValueKey<string>("hello");

            Assert.That(key1, Is.EqualTo(key2));
        }

        [Test]
        public void ValueKeys_WithNullValues_AreEqual()
        {
            var key1 = new FValueKey<string?>(null);
            var key2 = new FValueKey<string?>(null);

            Assert.That(key1, Is.EqualTo(key2));
        }

        [Test]
        public void ValueKeys_OneNullOneNot_AreNotEqual()
        {
            var key1 = new FValueKey<string?>(null);
            var key2 = new FValueKey<string>("hello");

            Assert.That(key1, Is.Not.EqualTo(key2));
        }

        [Test]
        public void ValueKeys_SameValue_ProduceSameHashCode()
        {
            var key1 = new FValueKey<int>(42);
            var key2 = new FValueKey<int>(42);

            Assert.That(key1.GetHashCode(), Is.EqualTo(key2.GetHashCode()));
        }

        [Test]
        public void ObjectKey_SameInstance_AreEqual()
        {
            var obj = new object();
            var key1 = new FObjectKey(obj);
            var key2 = new FObjectKey(obj);

            Assert.That(key1, Is.EqualTo(key2));
        }

        [Test]
        public void ObjectKey_DifferentInstances_AreNotEqual()
        {
            var key1 = new FObjectKey(new object());
            var key2 = new FObjectKey(new object());

            Assert.That(key1, Is.Not.EqualTo(key2));
        }

        [Test]
        public void ObjectKey_NullValue_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new FObjectKey(null));
        }

        [Test]
        public void UniqueKey_TwoInstances_AreNeverEqual()
        {
            var key1 = new FUniqueKey();
            var key2 = new FUniqueKey();

            Assert.That(key1, Is.Not.EqualTo(key2));
        }

        [Test]
        public void UniqueKey_SameInstance_IsEqualToItself()
        {
            var key = new FUniqueKey();

            Assert.That(key, Is.EqualTo(key));
        }

        [Test]
        public void OperatorEquals_BothNull_ReturnsTrue()
        {
            FKey? key1 = null;
            FKey? key2 = null;

            Assert.That(key1 == key2, Is.True);
        }

        [Test]
        public void OperatorEquals_OneNull_ReturnsFalse()
        {
            FKey key1 = new FValueKey<int>(1);
            FKey? key2 = null;

            Assert.That(key1 == key2, Is.False);
            Assert.That(key2 == key1, Is.False);
        }

        [Test]
        public void OperatorNotEquals_DifferentValues_ReturnsTrue()
        {
            FKey key1 = new FValueKey<int>(1);
            FKey key2 = new FValueKey<int>(2);

            Assert.That(key1 != key2, Is.True);
        }
    }
}
