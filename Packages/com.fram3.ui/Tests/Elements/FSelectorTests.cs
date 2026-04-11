#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements;
using Fram3.UI.GlobalState;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FSelectorTests
    {
        private FRebuildScheduler _scheduler = null!;
        private FNodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new FRebuildScheduler();
            _expander = new FNodeExpander(_scheduler);
        }

        private sealed class CounterCubit : FCubit<int>
        {
            public CounterCubit(int initial = 0) : base(initial) { }

            public void Increment() => Emit(State + 1);
            public void SetTo(int v) => Emit(v);
        }

        [Test]
        public void Constructor_NullSelector_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FSelector<CounterCubit, int, bool>(null!, (_, _) => new TestLeafElement("x")));
        }

        [Test]
        public void Constructor_NullBuilder_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FSelector<CounterCubit, int, bool>(state => state > 0, null!));
        }

        [Test]
        public void Mount_BuildsSubtreeWithInitialDerivedValue()
        {
            var cubit = new CounterCubit(10);
            bool? captured = null;

            var tree = new FProvider<CounterCubit>(
                cubit,
                new FSelector<CounterCubit, int, bool>(
                    state => state > 5,
                    (_, isAbove) =>
                    {
                        captured = isAbove;
                        return new TestLeafElement("leaf");
                    }));

            _expander.Mount(tree, null);

            Assert.That(captured, Is.True);
        }

        [Test]
        public void StateChange_DerivedValueChanges_SchedulesRebuild()
        {
            var cubit = new CounterCubit(0);

            var tree = new FProvider<CounterCubit>(
                cubit,
                new FSelector<CounterCubit, int, bool>(
                    state => state > 5,
                    (_, _) => new TestLeafElement("leaf")));

            var providerNode = _expander.Mount(tree, null);
            var selectorNode = providerNode.Children[0];
            selectorNode.IsDirty = false;

            cubit.SetTo(6);

            Assert.That(selectorNode.IsDirty, Is.True);
        }

        [Test]
        public void StateChange_DerivedValueUnchanged_DoesNotScheduleRebuild()
        {
            var cubit = new CounterCubit(10);

            var tree = new FProvider<CounterCubit>(
                cubit,
                new FSelector<CounterCubit, int, bool>(
                    state => state > 5,
                    (_, _) => new TestLeafElement("leaf")));

            var providerNode = _expander.Mount(tree, null);
            var selectorNode = providerNode.Children[0];
            selectorNode.IsDirty = false;

            cubit.SetTo(20);

            Assert.That(selectorNode.IsDirty, Is.False);
        }

        [Test]
        public void StateChange_DerivedValueChanges_RebuildPassesNewValueToBuilder()
        {
            var cubit = new CounterCubit(0);
            bool? lastValue = null;

            var tree = new FProvider<CounterCubit>(
                cubit,
                new FSelector<CounterCubit, int, bool>(
                    state => state > 5,
                    (_, isAbove) =>
                    {
                        lastValue = isAbove;
                        return new TestLeafElement("leaf");
                    }));

            _expander.Mount(tree, null);
            lastValue = null;

            cubit.SetTo(6);
            _scheduler.Flush(_expander);

            Assert.That(lastValue, Is.True);
        }

        [Test]
        public void Unmount_RemovesListenerFromCubit()
        {
            var cubit = new CounterCubit(0);

            var tree = new FProvider<CounterCubit>(
                cubit,
                new FSelector<CounterCubit, int, bool>(
                    state => state > 5,
                    (_, _) => new TestLeafElement("leaf")));

            var providerNode = _expander.Mount(tree, null);
            var selectorNode = providerNode.Children[0];
            _expander.Unmount(selectorNode);
            selectorNode.IsDirty = false;

            cubit.SetTo(10);

            Assert.That(selectorNode.IsDirty, Is.False);
        }

        [Test]
        public void MultipleStateChanges_OnlyRebuildsWhenDerivedValueFlips()
        {
            var cubit = new CounterCubit(0);
            var buildCount = 0;

            var tree = new FProvider<CounterCubit>(
                cubit,
                new FSelector<CounterCubit, int, bool>(
                    state => state > 5,
                    (_, _) =>
                    {
                        buildCount++;
                        return new TestLeafElement("leaf");
                    }));

            _expander.Mount(tree, null);
            buildCount = 0;

            // Stays false -- no rebuild.
            cubit.SetTo(1);
            cubit.SetTo(2);
            cubit.SetTo(3);
            _scheduler.Flush(_expander);
            Assert.That(buildCount, Is.EqualTo(0));

            // Flips to true -- one rebuild.
            cubit.SetTo(6);
            _scheduler.Flush(_expander);
            Assert.That(buildCount, Is.EqualTo(1));

            // Stays true -- no additional rebuild.
            buildCount = 0;
            cubit.SetTo(7);
            cubit.SetTo(8);
            _scheduler.Flush(_expander);
            Assert.That(buildCount, Is.EqualTo(0));
        }
    }
}
