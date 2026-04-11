#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.GlobalState;
using NUnit.Framework;

namespace Fram3.UI.Tests.GlobalState
{
    [TestFixture]
    internal sealed class FEquatableTests
    {
        private sealed class TestState : FEquatable
        {
            public int Value { get; }

            public TestState(int value)
            {
                Value = value;
            }

            public override bool Equals(FEquatable? other)
            {
                return other is TestState s && s.Value == Value;
            }

            public override int GetHashCode() => Value.GetHashCode();
        }

        [Test]
        public void Equals_SameValue_ReturnsTrue()
        {
            var a = new TestState(42);
            var b = new TestState(42);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void Equals_DifferentValue_ReturnsFalse()
        {
            var a = new TestState(1);
            var b = new TestState(2);

            Assert.That(a.Equals(b), Is.False);
        }

        [Test]
        public void Equals_Null_ReturnsFalse()
        {
            var a = new TestState(1);

            Assert.That(a.Equals(null), Is.False);
        }

        [Test]
        public void ObjectEquals_SameValue_ReturnsTrue()
        {
            object a = new TestState(5);
            object b = new TestState(5);

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void ObjectEquals_DifferentValue_ReturnsFalse()
        {
            object a = new TestState(5);
            object b = new TestState(6);

            Assert.That(a.Equals(b), Is.False);
        }
    }
}
