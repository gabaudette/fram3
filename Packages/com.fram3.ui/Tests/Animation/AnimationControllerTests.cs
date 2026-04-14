#nullable enable
using System;
using Fram3.UI.Animation;
using NUnit.Framework;

namespace Fram3.UI.Tests.Animation
{
    [TestFixture]
    internal sealed class AnimationControllerTests
    {
        [TearDown]
        public void TearDown()
        {
            AnimationSystem.Reset();
        }

        [Test]
        public void Constructor_ZeroDuration_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new AnimationController(0f));
        }

        [Test]
        public void Constructor_NegativeDuration_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new AnimationController(-1f));
        }

        [Test]
        public void Constructor_ValidDuration_SetsIdleStatus()
        {
            using var ctrl = new AnimationController(1f);

            Assert.That(ctrl.Status, Is.EqualTo(AnimationStatus.Idle));
        }

        [Test]
        public void Constructor_ValidDuration_ValueIsZero()
        {
            using var ctrl = new AnimationController(1f);

            Assert.That(ctrl.Value, Is.EqualTo(0f));
        }

        [Test]
        public void Forward_SetsStatusToForward()
        {
            using var ctrl = new AnimationController(1f);

            ctrl.Forward();

            Assert.That(ctrl.Status, Is.EqualTo(AnimationStatus.Forward));
        }

        [Test]
        public void Reverse_SetsStatusToReverse()
        {
            using var ctrl = new AnimationController(1f);
            ctrl.Forward();
            AnimationSystem.Tick(0.5f);

            ctrl.Reverse();

            Assert.That(ctrl.Status, Is.EqualTo(AnimationStatus.Reverse));
        }

        [Test]
        public void Reset_SetsStatusToIdle_AndValueToZero()
        {
            using var ctrl = new AnimationController(1f);
            ctrl.Forward();
            AnimationSystem.Tick(0.5f);

            ctrl.Reset();

            Assert.That(ctrl.Status, Is.EqualTo(AnimationStatus.Idle));
            Assert.That(ctrl.Value, Is.EqualTo(0f));
        }

        [Test]
        public void Tick_AdvancesValue()
        {
            using var ctrl = new AnimationController(1f);
            ctrl.Forward();

            AnimationSystem.Tick(0.5f);

            Assert.That(ctrl.Value, Is.EqualTo(0.5f).Within(0.0001f));
        }

        [Test]
        public void Tick_CompletesForwardAnimation()
        {
            using var ctrl = new AnimationController(1f);
            ctrl.Forward();

            AnimationSystem.Tick(1f);

            Assert.That(ctrl.Status, Is.EqualTo(AnimationStatus.Completed));
            Assert.That(ctrl.Value, Is.EqualTo(1f).Within(0.0001f));
        }

        [Test]
        public void Tick_CompletesReverseAnimation()
        {
            using var ctrl = new AnimationController(1f);
            ctrl.Forward();
            AnimationSystem.Tick(1f);
            ctrl.Reverse();

            AnimationSystem.Tick(1f);

            Assert.That(ctrl.Status, Is.EqualTo(AnimationStatus.Completed));
            Assert.That(ctrl.Value, Is.EqualTo(0f).Within(0.0001f));
        }

        [Test]
        public void Tick_IdleController_DoesNotChangeValue()
        {
            using var ctrl = new AnimationController(1f);

            AnimationSystem.Tick(0.5f);

            Assert.That(ctrl.Value, Is.EqualTo(0f));
        }

        [Test]
        public void Forward_WhenAlreadyAtOne_RestartsFromZero()
        {
            using var ctrl = new AnimationController(1f);
            ctrl.Forward();
            AnimationSystem.Tick(1f);

            ctrl.Forward();
            AnimationSystem.Tick(0.25f);

            Assert.That(ctrl.Value, Is.EqualTo(0.25f).Within(0.0001f));
        }

        [Test]
        public void Reverse_WhenAlreadyAtZero_RestartsFromOne()
        {
            using var ctrl = new AnimationController(1f);

            ctrl.Reverse();
            AnimationSystem.Tick(0.25f);

            Assert.That(ctrl.Value, Is.EqualTo(0.75f).Within(0.0001f));
        }

        [Test]
        public void AddListener_Null_ThrowsArgumentNullException()
        {
            using var ctrl = new AnimationController(1f);

            Assert.Throws<ArgumentNullException>(() => ctrl.AddListener(null!));
        }

        [Test]
        public void RemoveListener_Null_ThrowsArgumentNullException()
        {
            using var ctrl = new AnimationController(1f);

            Assert.Throws<ArgumentNullException>(() => ctrl.RemoveListener(null!));
        }

        [Test]
        public void AddListener_NotifiedOnTick()
        {
            using var ctrl = new AnimationController(1f);
            float? received = null;
            ctrl.AddListener(v => received = v);
            ctrl.Forward();

            AnimationSystem.Tick(0.5f);

            Assert.That(received, Is.EqualTo(0.5f).Within(0.0001f));
        }

        [Test]
        public void RemoveListener_StopsNotification()
        {
            using var ctrl = new AnimationController(1f);
            var callCount = 0;
            void Listener(float _) => callCount++;
            ctrl.AddListener(Listener);
            ctrl.Forward();
            AnimationSystem.Tick(0.25f);

            ctrl.RemoveListener(Listener);
            AnimationSystem.Tick(0.25f);

            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void Dispose_UnregistersFromSystem()
        {
            var ctrl = new AnimationController(1f);
            var countBefore = AnimationSystem.RegisteredCount;

            ctrl.Dispose();

            Assert.That(AnimationSystem.RegisteredCount, Is.EqualTo(countBefore - 1));
        }

        [Test]
        public void Dispose_CalledTwice_DoesNotThrow()
        {
            var ctrl = new AnimationController(1f);
            ctrl.Dispose();

            Assert.DoesNotThrow(() => ctrl.Dispose());
        }

        [Test]
        public void Forward_AfterDispose_ThrowsObjectDisposedException()
        {
            var ctrl = new AnimationController(1f);
            ctrl.Dispose();

            Assert.Throws<ObjectDisposedException>(() => ctrl.Forward());
        }

        [Test]
        public void Curve_IsAppliedToValue()
        {
            Curve squareCurve = t => t * t;
            using var ctrl = new AnimationController(1f, squareCurve);
            ctrl.Forward();

            AnimationSystem.Tick(0.5f);

            Assert.That(ctrl.Value, Is.EqualTo(0.25f).Within(0.0001f));
        }
    }
}
