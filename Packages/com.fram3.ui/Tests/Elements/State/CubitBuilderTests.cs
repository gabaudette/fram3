#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.State;
using Fram3.UI.GlobalState;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.State
{
    [TestFixture]
    internal sealed class CubitBuilderTests
    {
        private RebuildScheduler _scheduler = null!;
        private NodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new RebuildScheduler();
            _expander = new NodeExpander(_scheduler);
        }

        private sealed class CounterCubit : Cubit<int>
        {
            public CounterCubit(int initial = 0) : base(initial) { }

            public void Increment() => Emit(State + 1);
            public void SetTo(int v) => Emit(v);
        }

        [Test]
        public void Constructor_NullBuilder_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CubitBuilder<CounterCubit, int>(null!));
        }

        [Test]
        public void Mount_BuildsSubtreeWithInitialState()
        {
            var cubit = new CounterCubit(7);
            int? captured = null;

            var tree = new Provider<CounterCubit>(
                cubit,
                new CubitBuilder<CounterCubit, int>((_, state) =>
                {
                    captured = state;
                    return new TestLeafElement("leaf");
                }));

            _expander.Mount(tree, null);

            Assert.That(captured, Is.EqualTo(7));
        }

        [Test]
        public void StateChange_SchedulesRebuild()
        {
            var cubit = new CounterCubit(0);

            var tree = new Provider<CounterCubit>(
                cubit,
                new CubitBuilder<CounterCubit, int>((_, _) => new TestLeafElement("leaf")));

            var providerNode = _expander.Mount(tree, null);
            var builderNode = providerNode.Children[0];
            builderNode.IsDirty = false;

            cubit.Increment();

            Assert.That(builderNode.IsDirty, Is.True);
        }

        [Test]
        public void StateChange_RebuildPassesUpdatedStateToBuilder()
        {
            var cubit = new CounterCubit(0);
            var lastState = -1;

            var tree = new Provider<CounterCubit>(
                cubit,
                new CubitBuilder<CounterCubit, int>((_, state) =>
                {
                    lastState = state;
                    return new TestLeafElement("leaf");
                }));

            _expander.Mount(tree, null);
            lastState = -1;

            cubit.SetTo(42);
            _scheduler.Flush(_expander);

            Assert.That(lastState, Is.EqualTo(42));
        }

        [Test]
        public void SameState_DoesNotScheduleRebuild()
        {
            var cubit = new CounterCubit(5);

            var tree = new Provider<CounterCubit>(
                cubit,
                new CubitBuilder<CounterCubit, int>((_, _) => new TestLeafElement("leaf")));

            var providerNode = _expander.Mount(tree, null);
            var builderNode = providerNode.Children[0];
            builderNode.IsDirty = false;

            cubit.SetTo(5);

            Assert.That(builderNode.IsDirty, Is.False);
        }

        [Test]
        public void Unmount_RemovesListenerFromCubit()
        {
            var cubit = new CounterCubit(0);

            var tree = new Provider<CounterCubit>(
                cubit,
                new CubitBuilder<CounterCubit, int>((_, _) => new TestLeafElement("leaf")));

            var providerNode = _expander.Mount(tree, null);
            var builderNode = providerNode.Children[0];
            _expander.Unmount(builderNode);
            builderNode.IsDirty = false;

            cubit.Increment();

            Assert.That(builderNode.IsDirty, Is.False);
        }
    }
}
