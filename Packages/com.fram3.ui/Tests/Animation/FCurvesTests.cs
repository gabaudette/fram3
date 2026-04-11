#nullable enable
using Fram3.UI.Animation;
using NUnit.Framework;

namespace Fram3.UI.Tests.Animation
{
    [TestFixture]
    internal sealed class FCurvesTests
    {
        [Test]
        public void Linear_AtZero_ReturnsZero()
        {
            Assert.That(FCurves.Linear(0f), Is.EqualTo(0f).Within(0.0001f));
        }

        [Test]
        public void Linear_AtOne_ReturnsOne()
        {
            Assert.That(FCurves.Linear(1f), Is.EqualTo(1f).Within(0.0001f));
        }

        [Test]
        public void Linear_AtHalf_ReturnsHalf()
        {
            Assert.That(FCurves.Linear(0.5f), Is.EqualTo(0.5f).Within(0.0001f));
        }

        [Test]
        public void EaseIn_AtZero_ReturnsZero()
        {
            Assert.That(FCurves.EaseIn(0f), Is.EqualTo(0f).Within(0.0001f));
        }

        [Test]
        public void EaseIn_AtOne_ReturnsOne()
        {
            Assert.That(FCurves.EaseIn(1f), Is.EqualTo(1f).Within(0.0001f));
        }

        [Test]
        public void EaseIn_AtHalf_IsLessThanHalf()
        {
            Assert.That(FCurves.EaseIn(0.5f), Is.LessThan(0.5f));
        }

        [Test]
        public void EaseOut_AtZero_ReturnsZero()
        {
            Assert.That(FCurves.EaseOut(0f), Is.EqualTo(0f).Within(0.0001f));
        }

        [Test]
        public void EaseOut_AtOne_ReturnsOne()
        {
            Assert.That(FCurves.EaseOut(1f), Is.EqualTo(1f).Within(0.0001f));
        }

        [Test]
        public void EaseOut_AtHalf_IsGreaterThanHalf()
        {
            Assert.That(FCurves.EaseOut(0.5f), Is.GreaterThan(0.5f));
        }

        [Test]
        public void EaseInOut_AtZero_ReturnsZero()
        {
            Assert.That(FCurves.EaseInOut(0f), Is.EqualTo(0f).Within(0.0001f));
        }

        [Test]
        public void EaseInOut_AtOne_ReturnsOne()
        {
            Assert.That(FCurves.EaseInOut(1f), Is.EqualTo(1f).Within(0.0001f));
        }

        [Test]
        public void EaseInOut_AtHalf_ReturnsHalf()
        {
            Assert.That(FCurves.EaseInOut(0.5f), Is.EqualTo(0.5f).Within(0.0001f));
        }

        [Test]
        public void BounceOut_AtZero_ReturnsZero()
        {
            Assert.That(FCurves.BounceOut(0f), Is.EqualTo(0f).Within(0.0001f));
        }

        [Test]
        public void BounceOut_AtOne_ReturnsOne()
        {
            Assert.That(FCurves.BounceOut(1f), Is.EqualTo(1f).Within(0.0001f));
        }

        [Test]
        public void ElasticOut_AtZero_ReturnsZero()
        {
            Assert.That(FCurves.ElasticOut(0f), Is.EqualTo(0f).Within(0.0001f));
        }

        [Test]
        public void ElasticOut_AtOne_ReturnsOne()
        {
            Assert.That(FCurves.ElasticOut(1f), Is.EqualTo(1f).Within(0.0001f));
        }
    }
}
