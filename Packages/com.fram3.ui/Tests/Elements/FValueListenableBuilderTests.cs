#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FValueListenableBuilderTests
    {
        private FRebuildScheduler _scheduler = null!;
        private FNodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new FRebuildScheduler();
            _expander = new FNodeExpander(_scheduler);
        }

        [Test]
        public void Constructor_NullNotifier_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FValueListenableBuilder<int>(null!, (_, _) => new TestLeafElement("x")));
        }

        [Test]
        public void Constructor_NullBuilder_ThrowsArgumentNullException()
        {
            var notifier = new FValueNotifier<int>(0);

            Assert.Throws<ArgumentNullException>(() =>
                new FValueListenableBuilder<int>(notifier, null!));
        }

        [Test]
        public void Mount_BuildsSubtreeWithCurrentValue()
        {
            var notifier = new FValueNotifier<int>(42);
            var capturedValue = -1;
            var element = new FValueListenableBuilder<int>(
                notifier,
                (_, v) =>
                {
                    capturedValue = v;
                    return new TestLeafElement("x");
                });

            _expander.Mount(element, null);

            Assert.That(capturedValue, Is.EqualTo(42));
        }

        [Test]
        public void ValueChange_SchedulesRebuild()
        {
            var notifier = new FValueNotifier<int>(0);
            var element = new FValueListenableBuilder<int>(
                notifier,
                (_, v) => new TestLeafElement(v.ToString()));

            var node = _expander.Mount(element, null);
            Assert.That(node.IsDirty, Is.False);

            notifier.Value = 1;

            Assert.That(node.IsDirty, Is.True);
        }

        [Test]
        public void ValueChange_ToSameValue_DoesNotScheduleRebuild()
        {
            var notifier = new FValueNotifier<int>(5);
            var element = new FValueListenableBuilder<int>(
                notifier,
                (_, v) => new TestLeafElement(v.ToString()));

            var node = _expander.Mount(element, null);

            notifier.Value = 5;

            Assert.That(node.IsDirty, Is.False);
        }

        [Test]
        public void Unmount_RemovesListenerFromNotifier()
        {
            var notifier = new FValueNotifier<int>(0);
            var element = new FValueListenableBuilder<int>(
                notifier,
                (_, v) => new TestLeafElement(v.ToString()));

            var node = _expander.Mount(element, null);
            _expander.Unmount(node);

            // After unmount the listener is gone -- dirty flag must not be set.
            notifier.Value = 99;
            Assert.That(node.IsDirty, Is.False);
        }

        [Test]
        public void DidUpdateElement_NewNotifier_SwapsListeners()
        {
            var notifier1 = new FValueNotifier<int>(0);
            var notifier2 = new FValueNotifier<int>(10);
            var element1 = new FValueListenableBuilder<int>(
                notifier1,
                (_, v) => new TestLeafElement(v.ToString()));

            var node = _expander.Mount(element1, null);

            var element2 = new FValueListenableBuilder<int>(
                notifier2,
                (_, v) => new TestLeafElement(v.ToString()));
            _expander.UpdateElement(node, element2);
            _expander.Rebuild(node);
            node.IsDirty = false;

            // Old notifier must not trigger rebuild.
            notifier1.Value = 99;
            Assert.That(node.IsDirty, Is.False);

            // New notifier must trigger rebuild.
            notifier2.Value = 99;
            Assert.That(node.IsDirty, Is.True);
        }

        [Test]
        public void DidUpdateElement_SameNotifier_DoesNotDoubleSubscribe()
        {
            var notifier = new FValueNotifier<int>(0);
            var buildCount = 0;
            var element1 = new FValueListenableBuilder<int>(
                notifier,
                (_, v) => { buildCount++; return new TestLeafElement(v.ToString()); });

            var node = _expander.Mount(element1, null);
            buildCount = 0;

            // Update to a new element sharing the same notifier.
            var element2 = new FValueListenableBuilder<int>(
                notifier,
                (_, v) => { buildCount++; return new TestLeafElement(v.ToString()); });
            _expander.UpdateElement(node, element2);

            // Change the value once -- should mark dirty once, not twice.
            notifier.Value = 1;
            Assert.That(node.IsDirty, Is.True);

            // Confirm only one rebuild is scheduled (scheduler deduplicates).
            // Flush via the expander; HasDirtyNodes should be false after one flush.
            _scheduler.Flush(_expander);
            Assert.That(_scheduler.HasDirtyNodes, Is.False);
        }

        [Test]
        public void Builder_ReceivesBuildContext()
        {
            var notifier = new FValueNotifier<int>(0);
            FBuildContext? capturedContext = null;
            var element = new FValueListenableBuilder<int>(
                notifier,
                (ctx, _) =>
                {
                    capturedContext = ctx;
                    return new TestLeafElement("x");
                });

            _expander.Mount(element, null);

            Assert.That(capturedContext, Is.Not.Null);
        }
    }
}
