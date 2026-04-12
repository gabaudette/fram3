#nullable enable
using System;
using Fram3.UI.Animation;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Animation;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Animation
{
    [TestFixture]
    internal sealed class FAnimationBuilderTests
    {
        private FRebuildScheduler _scheduler = null!;
        private FNodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new FRebuildScheduler();
            _expander = new FNodeExpander(_scheduler);
        }

        [TearDown]
        public void TearDown()
        {
            FAnimationSystem.Reset();
        }

        [Test]
        public void Constructor_NullBuilder_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FAnimationBuilder(1f, null!));
        }

        [Test]
        public void Constructor_ZeroDuration_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new FAnimationBuilder(0f, (_, __) => new TestLeafElement("x")));
        }

        [Test]
        public void Mount_PassesControllerToBuilder()
        {
            FAnimationController? captured = null;

            var element = new FAnimationBuilder(1f, (_, ctrl) =>
            {
                captured = ctrl;
                return new TestLeafElement("child");
            });

            _expander.Mount(element, null);

            Assert.That(captured, Is.Not.Null);
        }

        [Test]
        public void Mount_ControllerStartsIdle()
        {
            FAnimationController? captured = null;

            var element = new FAnimationBuilder(1f, (_, ctrl) =>
            {
                captured = ctrl;
                return new TestLeafElement("child");
            });

            _expander.Mount(element, null);

            Assert.That(captured!.Status, Is.EqualTo(FAnimationStatus.Idle));
        }

        [Test]
        public void Tick_WhileRunning_SchedulesRebuild()
        {
            FAnimationController? captured = null;

            var element = new FAnimationBuilder(1f, (_, ctrl) =>
            {
                captured = ctrl;
                return new TestLeafElement("child");
            });

            var node = _expander.Mount(element, null);
            node.IsDirty = false;

            captured!.Forward();
            FAnimationSystem.Tick(0.1f);

            Assert.That(node.IsDirty, Is.True);
        }

        [Test]
        public void Tick_WhenIdle_DoesNotScheduleRebuild()
        {
            var element = new FAnimationBuilder(1f, (_, __) => new TestLeafElement("child"));

            var node = _expander.Mount(element, null);
            node.IsDirty = false;

            FAnimationSystem.Tick(0.1f);

            Assert.That(node.IsDirty, Is.False);
        }

        [Test]
        public void Unmount_DisposesController()
        {
            FAnimationController? captured = null;

            var element = new FAnimationBuilder(1f, (_, ctrl) =>
            {
                captured = ctrl;
                return new TestLeafElement("child");
            });

            var node = _expander.Mount(element, null);
            _expander.Unmount(node);

            Assert.That(FAnimationSystem.RegisteredCount, Is.EqualTo(0));
        }

        [Test]
        public void Unmount_AfterUnmount_TickDoesNotScheduleRebuild()
        {
            FAnimationController? captured = null;

            var element = new FAnimationBuilder(1f, (_, ctrl) =>
            {
                captured = ctrl;
                return new TestLeafElement("child");
            });

            var node = _expander.Mount(element, null);
            captured!.Forward();
            _expander.Unmount(node);
            node.IsDirty = false;

            FAnimationSystem.Tick(0.1f);

            Assert.That(node.IsDirty, Is.False);
        }
    }
}
