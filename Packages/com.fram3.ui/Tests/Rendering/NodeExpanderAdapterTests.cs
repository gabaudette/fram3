#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Rendering
{
    [TestFixture]
    internal sealed class NodeExpanderAdapterTests
    {
        private RecordingAdapter _adapter = null!;
        private RebuildScheduler _scheduler = null!;
        private NodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            _adapter = new RecordingAdapter();
            _scheduler = new RebuildScheduler();
            _expander = new NodeExpander(_scheduler, _adapter);
        }

        [Test]
        public void Mount_CallsOnMountedForEachNode()
        {
            var leaf = new TestRenderLeaf("x");

            _expander.Mount(leaf, null);

            Assert.That(_adapter.MountedNodes, Has.Count.EqualTo(1));
        }

        [Test]
        public void Mount_StatelessElement_CallsOnMountedForExpandedNodes()
        {
            var element = new TestStatelessElement(_ => new TestRenderLeaf("child"));

            _expander.Mount(element, null);

            Assert.That(_adapter.MountedNodes, Has.Count.EqualTo(2));
        }

        [Test]
        public void Unmount_CallsOnUnmountingBeforeChildrenAreCleared()
        {
            var leaf = new TestRenderLeaf("y");
            var node = _expander.Mount(leaf, null);

            _adapter.MountedNodes.Clear();
            _expander.Unmount(node);

            Assert.That(_adapter.UnmountingNodes, Has.Count.EqualTo(1));
        }

        [Test]
        public void Rebuild_CallsOnRebuiltAfterPatch()
        {
            var state = new RebuildState();
            var element = new TestStatefulElement(() => state);
            var node = _expander.Mount(element, null);

            state.Trigger();
            _scheduler.Flush(_expander);

            Assert.That(_adapter.RebuiltNodes, Has.Count.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void NoAdapter_MountAndUnmount_DoNotThrow()
        {
            var expanderWithoutAdapter = new NodeExpander(_scheduler);
            var leaf = new TestRenderLeaf("z");

            Node node = null!;
            Assert.DoesNotThrow(() => node = expanderWithoutAdapter.Mount(leaf, null));
            Assert.DoesNotThrow(() => expanderWithoutAdapter.Unmount(node));
        }

        private sealed class RecordingAdapter : IRenderAdapter
        {
            public List<Node> MountedNodes { get; } = new();
            public List<Node> UnmountingNodes { get; } = new();
            public List<Node> RebuiltNodes { get; } = new();

            public void OnMounted(Node node) => MountedNodes.Add(node);
            public void OnUnmounting(Node node) => UnmountingNodes.Add(node);
            public void OnRebuilt(Node node) => RebuiltNodes.Add(node);
        }

        private sealed class RebuildState : State
        {
            public void Trigger() => SetState(null);

            public override Element Build(BuildContext context) => new TestRenderLeaf("rebuilt");
        }
    }
}
