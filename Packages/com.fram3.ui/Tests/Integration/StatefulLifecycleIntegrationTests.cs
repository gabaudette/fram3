#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Integration.Helpers;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Integration
{
    /// <summary>
    /// Verifies that State lifecycle hooks fire in the correct order and with correct
    /// arguments across mount, rebuild, and unmount phases of a real element tree.
    /// </summary>
    [TestFixture]
    internal sealed class StatefulLifecycleIntegrationTests
    {
        private RebuildScheduler _scheduler = null!;
        private NodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            (_scheduler, _expander) = TreeBuilder.MakePipeline();
        }

        [Test]
        public void InitState_CalledExactlyOnce_OnMount()
        {
            var state = new TestState(_ => new TestLeafElement("x"));
            var stateful = new TestStatefulElement(() => state);

            TreeBuilder.Mount(stateful, _expander);

            Assert.That(state.InitStateCalled, Is.True);
        }

        [Test]
        public void Build_CalledOnce_DuringMount()
        {
            var state = new TestState(_ => new TestLeafElement("x"));
            var stateful = new TestStatefulElement(() => state);

            TreeBuilder.Mount(stateful, _expander);

            Assert.That(state.BuildCount, Is.EqualTo(1));
        }

        [Test]
        public void SetState_TriggersRebuild_BuildCountIncreases()
        {
            var state = new TestState(_ => new TestLeafElement("x"));
            var stateful = new TestStatefulElement(() => state);
            TreeBuilder.Mount(stateful, _expander);

            state.SetState(null);
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(state.BuildCount, Is.EqualTo(2));
        }

        [Test]
        public void Dispose_CalledOnUnmount_DisposeFlagSet()
        {
            var state = new TestState(_ => new TestLeafElement("x"));
            var stateful = new TestStatefulElement(() => state);
            var rootNode = TreeBuilder.Mount(stateful, _expander);

            _expander.Unmount(rootNode);

            Assert.That(state.DisposeCalled, Is.True);
        }

        [Test]
        public void Mounted_FalseAfterUnmount()
        {
            var state = new TestState(_ => new TestLeafElement("x"));
            var stateful = new TestStatefulElement(() => state);
            var rootNode = TreeBuilder.Mount(stateful, _expander);

            Assert.That(state.Mounted, Is.True);
            _expander.Unmount(rootNode);
            Assert.That(state.Mounted, Is.False);
        }

        [Test]
        public void SetState_AfterUnmount_ThrowsInvalidOperationException()
        {
            var state = new TestState(_ => new TestLeafElement("x"));
            var stateful = new TestStatefulElement(() => state);
            var rootNode = TreeBuilder.Mount(stateful, _expander);
            _expander.Unmount(rootNode);

            Assert.Throws<InvalidOperationException>(() => state.SetState(null));
        }

        [Test]
        public void DidUpdateElement_CalledWithPreviousElement_WhenParentRebuilds()
        {
            var parentConfig = new ParentConfig("v1");
            var childState = new TestState(_ => new TestLeafElement("leaf"));
            var parentState = new ParentRebuildState(parentConfig, childState);
            var parentStateful = new TestStatefulElement(() => parentState);

            TreeBuilder.Mount(parentStateful, _expander);

            // Trigger parent rebuild which swaps the child element's Config
            parentConfig.Value = "v2";
            parentState.SetState(null);
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(childState.LastOldElement, Is.Not.Null);
            Assert.That(((TestStatefulElement)childState.LastOldElement!).Config, Is.EqualTo("v1"));
        }

        [Test]
        public void ChildOnMounted_FiresBeforeParentOnMounted_KnownOrdering()
        {
            // Documents the known behavior: NodeExpander.Mount calls ExpandChildren
            // before calling adapter.OnMounted, so children are fully mounted first.
            // This test captures that ordering as a contract.
            var adapter = new TrackingAdapter();
            var (_, expander) = TreeBuilder.MakePipeline(adapter);

            var root = new TestMultiChildElement
            {
                Children = new Element[]
                {
                    new TestLeafElement("child"),
                }
            };

            TreeBuilder.Mount(root, expander);

            var events = adapter.EventsOfKind(TrackingAdapter.EventKind.Mounted);
            // Child comes before parent (child is mounted first in ExpandChildren loop,
            // then parent adapter.OnMounted is fired after all children).
            Assert.That(events[0].Node.Element, Is.InstanceOf<TestLeafElement>());
            Assert.That(events[1].Node.Element, Is.InstanceOf<TestMultiChildElement>());
        }

        [Test]
        public void MultipleSetState_BeforeFlush_ProducesOnlyOneRebuild()
        {
            var state = new TestState(_ => new TestLeafElement("x"));
            var stateful = new TestStatefulElement(() => state);
            TreeBuilder.Mount(stateful, _expander);

            state.SetState(null);
            state.SetState(null);
            state.SetState(null);
            TreeBuilder.Flush(_scheduler, _expander);

            // Three dirty marks on the same node -- scheduler deduplicates via SortedSet
            Assert.That(state.BuildCount, Is.EqualTo(2)); // 1 from mount + 1 from flush
        }

        // ---- Helpers --------------------------------------------------------------------------

        private sealed class ParentConfig
        {
            public string Value;
            public ParentConfig(string value) { Value = value; }
        }

        private sealed class ParentRebuildState : State
        {
            private readonly ParentConfig _config;
            private readonly TestState _childState;

            public ParentRebuildState(ParentConfig config, TestState childState)
            {
                _config = config;
                _childState = childState;
            }

            public new void SetState(Action? action)
            {
                base.SetState(action);
            }

            public override Element Build(BuildContext context)
            {
                return new TestStatefulElement(() => _childState, _config.Value);
            }
        }
    }
}
