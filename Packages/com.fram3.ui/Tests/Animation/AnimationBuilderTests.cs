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
    internal sealed class AnimationBuilderTests
    {
        private RebuildScheduler _scheduler = null!;
        private NodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new RebuildScheduler();
            _expander = new NodeExpander(_scheduler);
        }

        [TearDown]
        public void TearDown()
        {
            AnimationSystem.Reset();
        }

        [Test]
        public void Constructor_NullBuilder_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new AnimationBuilder(1f, null!));
        }

        [Test]
        public void Constructor_ZeroDuration_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new AnimationBuilder(0f, (_, __) => new TestLeafElement("x")));
        }

        [Test]
        public void Mount_PassesControllerToBuilder()
        {
            AnimationController? captured = null;

            var element = new AnimationBuilder(1f, (_, ctrl) =>
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
            AnimationController? captured = null;

            var element = new AnimationBuilder(1f, (_, ctrl) =>
            {
                captured = ctrl;
                return new TestLeafElement("child");
            });

            _expander.Mount(element, null);

            Assert.That(captured!.Status, Is.EqualTo(AnimationStatus.Idle));
        }

        [Test]
        public void Tick_WhileRunning_SchedulesRebuild()
        {
            AnimationController? captured = null;

            var element = new AnimationBuilder(1f, (_, ctrl) =>
            {
                captured = ctrl;
                return new TestLeafElement("child");
            });

            var node = _expander.Mount(element, null);
            node.IsDirty = false;

            captured!.Forward();
            AnimationSystem.Tick(0.1f);

            Assert.That(node.IsDirty, Is.True);
        }

        [Test]
        public void Tick_WhenIdle_DoesNotScheduleRebuild()
        {
            var element = new AnimationBuilder(1f, (_, __) => new TestLeafElement("child"));

            var node = _expander.Mount(element, null);
            node.IsDirty = false;

            AnimationSystem.Tick(0.1f);

            Assert.That(node.IsDirty, Is.False);
        }

        [Test]
        public void Unmount_DisposesController()
        {
            AnimationController? captured = null;

            var element = new AnimationBuilder(1f, (_, ctrl) =>
            {
                captured = ctrl;
                return new TestLeafElement("child");
            });

            var node = _expander.Mount(element, null);
            _expander.Unmount(node);

            Assert.That(AnimationSystem.RegisteredCount, Is.EqualTo(0));
        }

        [Test]
        public void Unmount_AfterUnmount_TickDoesNotScheduleRebuild()
        {
            AnimationController? captured = null;

            var element = new AnimationBuilder(1f, (_, ctrl) =>
            {
                captured = ctrl;
                return new TestLeafElement("child");
            });

            var node = _expander.Mount(element, null);
            captured!.Forward();
            _expander.Unmount(node);
            node.IsDirty = false;

            AnimationSystem.Tick(0.1f);

            Assert.That(node.IsDirty, Is.False);
        }
    }
}
