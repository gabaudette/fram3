#nullable enable
using System;
using Fram3.UI.Animation;
using NUnit.Framework;

namespace Fram3.UI.Tests.Animation
{
    [TestFixture]
    internal sealed class FAnimatedValueTests
    {
        [Test]
        public void Constructor_NullKey_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FAnimatedValue<float>(null!, 1f, FLerp.Float));
        }

        [Test]
        public void Constructor_NullLerp_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FAnimatedValue<float>("x", 1f, null!));
        }

        [Test]
        public void Key_ReturnsConstructorValue()
        {
            var value = new FAnimatedValue<float>("opacity", 1f, FLerp.Float);
            Assert.That(value.Key, Is.EqualTo("opacity"));
        }

        [Test]
        public void Target_ReturnsConstructorValue()
        {
            var value = new FAnimatedValue<float>("opacity", 0.5f, FLerp.Float);
            Assert.That(value.Target, Is.EqualTo(0.5f));
        }

        [Test]
        public void HasChangedFrom_SameTarget_ReturnsFalse()
        {
            var a = new FAnimatedValue<float>("x", 1f, FLerp.Float);
            var b = new FAnimatedValue<float>("x", 1f, FLerp.Float);
            Assert.That(a.HasChangedFrom(b), Is.False);
        }

        [Test]
        public void HasChangedFrom_DifferentTarget_ReturnsTrue()
        {
            var a = new FAnimatedValue<float>("x", 0f, FLerp.Float);
            var b = new FAnimatedValue<float>("x", 1f, FLerp.Float);
            Assert.That(a.HasChangedFrom(b), Is.True);
        }

        [Test]
        public void HasChangedFrom_DifferentType_ReturnsTrue()
        {
            var a = new FAnimatedValue<float>("x", 1f, FLerp.Float);
            var b = new FAnimatedValue<int>("x", 1, (x, y, t) => (int)(x + (y - x) * t));
            Assert.That(a.HasChangedFrom(b), Is.True);
        }

        [Test]
        public void Interpolate_AtZero_ReturnsFromValue()
        {
            var previous = new FAnimatedValue<float>("x", 0f, FLerp.Float);
            var current = new FAnimatedValue<float>("x", 1f, FLerp.Float);

            var result = (float)current.Interpolate(previous, 0f);

            Assert.That(result, Is.EqualTo(0f));
        }

        [Test]
        public void Interpolate_AtOne_ReturnsToValue()
        {
            var previous = new FAnimatedValue<float>("x", 0f, FLerp.Float);
            var current = new FAnimatedValue<float>("x", 1f, FLerp.Float);

            var result = (float)current.Interpolate(previous, 1f);

            Assert.That(result, Is.EqualTo(1f));
        }

        [Test]
        public void Interpolate_AtMidpoint_ReturnsMidValue()
        {
            var previous = new FAnimatedValue<float>("x", 0f, FLerp.Float);
            var current = new FAnimatedValue<float>("x", 2f, FLerp.Float);

            var result = (float)current.Interpolate(previous, 0.5f);

            Assert.That(result, Is.EqualTo(1f).Within(0.0001f));
        }

        [Test]
        public void Interpolate_PreviousTypeMismatch_UsesCurrentTargetAsFrom()
        {
            var previous = new FAnimatedValue<int>("x", 0, (a, b, t) => a);
            var current = new FAnimatedValue<float>("x", 5f, FLerp.Float);

            var result = (float)current.Interpolate(previous, 0.5f);

            Assert.That(result, Is.EqualTo(5f).Within(0.0001f));
        }

        [Test]
        public void WithTarget_ReturnsNewValueWithSameKeyAndLerp()
        {
            var original = new FAnimatedValue<float>("x", 0f, FLerp.Float);
            var updated = (FAnimatedValue<float>)original.WithTarget(1f);

            Assert.That(updated.Key, Is.EqualTo("x"));
            Assert.That(updated.Target, Is.EqualTo(1f));
        }

        [Test]
        public void WithTarget_NewValueInterpolatesCorrectly()
        {
            var base_ = new FAnimatedValue<float>("x", 0f, FLerp.Float);
            var frozen = (FAnimatedValue<float>)base_.WithTarget(10f);
            var next = new FAnimatedValue<float>("x", 20f, FLerp.Float);

            var result = (float)next.Interpolate(frozen, 0.5f);

            Assert.That(result, Is.EqualTo(15f).Within(0.0001f));
        }
    }
}
