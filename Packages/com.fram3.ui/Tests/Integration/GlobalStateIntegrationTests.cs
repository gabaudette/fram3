#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.State;
using Fram3.UI.GlobalState;
using Fram3.UI.Tests.Integration.Helpers;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Integration
{
    /// <summary>
    /// Verifies FCubitBuilder, FSelector, and FConsumer wired to a live element tree via
    /// FProvider. Tests that state changes cause the correct subtrees to rebuild.
    /// </summary>
    [TestFixture]
    internal sealed class GlobalStateIntegrationTests
    {
        private FRebuildScheduler _scheduler = null!;
        private FNodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            (_scheduler, _expander) = TreeBuilder.MakePipeline();
        }

        [Test]
        public void FCubitBuilder_InitialBuild_ReceivesCurrentState()
        {
            var cubit = new CounterCubit(7);
            int? capturedState = null;

            var builder = new FCubitBuilder<CounterCubit, int>((ctx, state) =>
            {
                capturedState = state;
                return new TestLeafElement(state.ToString());
            });

            var root = new FProvider<CounterCubit>(cubit, builder);
            TreeBuilder.Mount(root, _expander);

            Assert.That(capturedState, Is.EqualTo(7));
            cubit.Dispose();
        }

        [Test]
        public void FCubitBuilder_StateChange_TriggersRebuild()
        {
            var cubit = new CounterCubit(0);
            var buildCount = 0;

            var builder = new FCubitBuilder<CounterCubit, int>((ctx, state) =>
            {
                buildCount++;
                return new TestLeafElement(state.ToString());
            });

            var root = new FProvider<CounterCubit>(cubit, builder);
            TreeBuilder.Mount(root, _expander);
            Assert.That(buildCount, Is.EqualTo(1));

            cubit.Increment();
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(buildCount, Is.EqualTo(2));
            cubit.Dispose();
        }

        [Test]
        public void FCubitBuilder_DisposedOnUnmount_ListenerRemoved()
        {
            var cubit = new CounterCubit(0);
            var buildCount = 0;

            var builder = new FCubitBuilder<CounterCubit, int>((ctx, state) =>
            {
                buildCount++;
                return new TestLeafElement(state.ToString());
            });

            var root = new FProvider<CounterCubit>(cubit, builder);
            var rootNode = TreeBuilder.Mount(root, _expander);
            _expander.Unmount(rootNode);

            // After unmount the cubit builder state is disposed -- no more rebuilds
            cubit.Increment();
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(buildCount, Is.EqualTo(1));
            cubit.Dispose();
        }

        [Test]
        public void FSelector_OnlyRebuilds_WhenSelectedValueChanges()
        {
            var cubit = new CounterCubit(0);
            var buildCount = 0;

            // Select only the parity (even/odd) of the counter
            var selector = new FSelector<CounterCubit, int, bool>(
                state => state % 2 == 0,
                (ctx, isEven) =>
                {
                    buildCount++;
                    return new TestLeafElement(isEven.ToString());
                }
            );

            var root = new FProvider<CounterCubit>(cubit, selector);
            TreeBuilder.Mount(root, _expander);
            Assert.That(buildCount, Is.EqualTo(1));

            // 0 -> 2: parity unchanged (still even) -- should NOT rebuild
            cubit.IncrementBy(2);
            TreeBuilder.Flush(_scheduler, _expander);
            Assert.That(buildCount, Is.EqualTo(1));

            // 2 -> 3: parity changes (even -> odd) -- SHOULD rebuild
            cubit.Increment();
            TreeBuilder.Flush(_scheduler, _expander);
            Assert.That(buildCount, Is.EqualTo(2));

            cubit.Dispose();
        }

        [Test]
        public void FSelector_DisposedOnUnmount_NoFurtherRebuilds()
        {
            var cubit = new CounterCubit(0);
            var buildCount = 0;

            var selector = new FSelector<CounterCubit, int, int>(
                state => state,
                (ctx, val) =>
                {
                    buildCount++;
                    return new TestLeafElement(val.ToString());
                }
            );

            var root = new FProvider<CounterCubit>(cubit, selector);
            var rootNode = TreeBuilder.Mount(root, _expander);
            _expander.Unmount(rootNode);

            cubit.Increment();
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(buildCount, Is.EqualTo(1));
            cubit.Dispose();
        }

        [Test]
        public void FConsumer_RebuildsWhenProviderValueChanges()
        {
            var providerState = new SimpleProviderState<string>("initial");
            var host = new TestStatefulElement(() => providerState);

            var buildCount = 0;
            var consumer = new FConsumer<string>((ctx, val) =>
            {
                buildCount++;
                return new TestLeafElement(val);
            });

            providerState.SetConsumer(consumer);
            TreeBuilder.Mount(host, _expander);
            Assert.That(buildCount, Is.EqualTo(1));

            providerState.Transition("updated");
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(buildCount, Is.EqualTo(2));
        }

        [Test]
        public void FConsumer_ExpandedOncePerHostRebuild_WhenValueUnchanged()
        {
            // FConsumer is a stateless element in the provider's expansion path.
            // When the host rebuilds with the same value, UpdateShouldNotify returns false
            // (no extra dependent notification), but the consumer is still re-expanded
            // once via the normal tree-expansion cascade triggered by the host rebuild.
            var providerState = new SimpleProviderState<string>("value");
            var host = new TestStatefulElement(() => providerState);

            var buildCount = 0;
            var consumer = new FConsumer<string>((ctx, val) =>
            {
                buildCount++;
                return new TestLeafElement(val);
            });

            providerState.SetConsumer(consumer);
            TreeBuilder.Mount(host, _expander);
            Assert.That(buildCount, Is.EqualTo(1));

            providerState.Transition("value"); // same value
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(buildCount, Is.EqualTo(2));
        }

        // ---- Test cubit -----------------------------------------------------------------------

        private sealed class CounterCubit : FCubit<int>
        {
            public CounterCubit(int initial) : base(initial) { }
            public void Increment() => Emit(State + 1);
            public void IncrementBy(int n) => Emit(State + n);
        }

        // ---- Helper state for FConsumer tests -------------------------------------------------

        private sealed class SimpleProviderState<T> : FState
        {
            private T _value;
            private FElement? _consumer;

            public SimpleProviderState(T initial) { _value = initial; }

            public void SetConsumer(FElement consumer) { _consumer = consumer; }

            public void Transition(T newValue)
            {
                SetState(() => _value = newValue);
            }

            public override FElement Build(FBuildContext context)
            {
                return new FProvider<T>(_value, _consumer!);
            }
        }
    }
}
