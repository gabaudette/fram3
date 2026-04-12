#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Integration.Helpers;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Integration
{
    [TestFixture]
    internal sealed class ReconcilerIntegrationTests
    {
        private TrackingAdapter _adapter = null!;
        private FRebuildScheduler _scheduler = null!;
        private FNodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            _adapter = new TrackingAdapter();
            (_scheduler, _expander) = TreeBuilder.MakePipeline(_adapter);
        }

        [Test]
        public void Mount_SingleLeaf_ProducesOneMountedEvent()
        {
            var leaf = new TestLeafElement("a");

            TreeBuilder.Mount(leaf, _expander);

            Assert.That(_adapter.EventsOfKind(TrackingAdapter.EventKind.Mounted).Count, Is.EqualTo(1));
        }

        [Test]
        public void Mount_MultiChildTree_ProducesMountedEventForEachNode()
        {
            var root = new TestMultiChildElement
            {
                Children = new FElement[]
                {
                    new TestLeafElement("a"),
                    new TestLeafElement("b"),
                    new TestLeafElement("c"),
                }
            };

            TreeBuilder.Mount(root, _expander);

            // root + 3 children = 4 mounted events
            Assert.That(_adapter.EventsOfKind(TrackingAdapter.EventKind.Mounted).Count, Is.EqualTo(4));
        }

        [Test]
        public void Unmount_RootWithChildren_ProducesUnmountingEventForEachNode()
        {
            var root = new TestMultiChildElement
            {
                Children = new FElement[]
                {
                    new TestLeafElement("a"),
                    new TestLeafElement("b"),
                }
            };
            var rootNode = TreeBuilder.Mount(root, _expander);
            _adapter.Clear();

            _expander.Unmount(rootNode);

            Assert.That(_adapter.EventsOfKind(TrackingAdapter.EventKind.Unmounting).Count, Is.EqualTo(3));
        }

        [Test]
        public void Rebuild_InsertChild_AdapterReceivesMountedForNewChild()
        {
            var mutableChildren = new MutableChildList(1);
            var stateful = new TestStatefulElement(
                () => new MutableChildListState(mutableChildren)
            );
            TreeBuilder.Mount(stateful, _expander);
            _adapter.Clear();

            mutableChildren.Count = 2;
            mutableChildren.State!.SetState(null);
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(_adapter.EventsOfKind(TrackingAdapter.EventKind.Mounted).Count, Is.EqualTo(1));
        }

        [Test]
        public void Rebuild_RemoveChild_AdapterReceivesUnmountingForRemovedChild()
        {
            var mutableChildren = new MutableChildList(2);
            var stateful = new TestStatefulElement(
                () => new MutableChildListState(mutableChildren)
            );
            TreeBuilder.Mount(stateful, _expander);
            _adapter.Clear();

            mutableChildren.Count = 1;
            mutableChildren.State!.SetState(null);
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(_adapter.EventsOfKind(TrackingAdapter.EventKind.Unmounting).Count, Is.EqualTo(1));
        }

        [Test]
        public void Rebuild_SwapChildType_OldUnmounted_NewSubtreeMounted()
        {
            // Initial: leaf (1 node). After swap: stateless -> leaf (2 nodes).
            // Expected: 1 unmounting (the old leaf), 2 mountings (stateless + its inner leaf).
            var toggle = new ChildTypeToggle(useLeaf: true);
            var stateful = new TestStatefulElement(
                () => new ChildTypeToggleState(toggle)
            );
            TreeBuilder.Mount(stateful, _expander);
            _adapter.Clear();

            toggle.UseLeaf = false;
            toggle.State!.SetState(null);
            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(_adapter.EventsOfKind(TrackingAdapter.EventKind.Unmounting).Count, Is.EqualTo(1));
            Assert.That(_adapter.EventsOfKind(TrackingAdapter.EventKind.Mounted).Count, Is.EqualTo(2));
        }

        [Test]
        public void Rebuild_KeyedChildren_Reversed_LogicalOrderUpdatedInNode()
        {
            var keyedToggle = new KeyedOrderToggle(reversed: false);
            var stateful = new TestStatefulElement(
                () => new KeyedOrderState(keyedToggle)
            );
            var rootNode = TreeBuilder.Mount(stateful, _expander);

            // rootNode -> stateful build child (MultiChild) -> two leaf children
            var builtNode = rootNode.Children[0];
            var firstLabel = ((TestLeafElement)builtNode.Children[0].Element).Label;
            Assert.That(firstLabel, Is.EqualTo("a"));

            keyedToggle.Reversed = true;
            keyedToggle.State!.SetState(null);
            TreeBuilder.Flush(_scheduler, _expander);

            var newFirstLabel = ((TestLeafElement)builtNode.Children[0].Element).Label;
            Assert.That(newFirstLabel, Is.EqualTo("b"));
        }

        // ---- Shared mutable state carriers ----------------------------------------------------

        private sealed class MutableChildList
        {
            public int Count;
            public MutableChildListState? State;

            public MutableChildList(int initialCount)
            {
                Count = initialCount;
            }
        }

        private sealed class MutableChildListState : FState
        {
            private readonly MutableChildList _data;

            public MutableChildListState(MutableChildList data)
            {
                _data = data;
            }

            public override void InitState()
            {
                _data.State = this;
            }

            public new void SetState(Action? action)
            {
                base.SetState(action);
            }

            public override FElement Build(FBuildContext context)
            {
                var children = new FElement[_data.Count];
                for (var i = 0; i < _data.Count; i++)
                {
                    children[i] = new TestLeafElement(i.ToString());
                }

                return new TestMultiChildElement { Children = children };
            }
        }

        private sealed class ChildTypeToggle
        {
            public bool UseLeaf;
            public ChildTypeToggleState? State;

            public ChildTypeToggle(bool useLeaf)
            {
                UseLeaf = useLeaf;
            }
        }

        private sealed class ChildTypeToggleState : FState
        {
            private readonly ChildTypeToggle _toggle;

            public ChildTypeToggleState(ChildTypeToggle toggle)
            {
                _toggle = toggle;
            }

            public override void InitState()
            {
                _toggle.State = this;
            }

            public new void SetState(Action? action)
            {
                base.SetState(action);
            }

            public override FElement Build(FBuildContext context)
            {
                if (_toggle.UseLeaf)
                {
                    return new TestLeafElement("leaf");
                }

                return new TestStatelessElement(_ => new TestLeafElement("stateless-leaf"));
            }
        }

        private sealed class KeyedOrderToggle
        {
            public bool Reversed;
            public KeyedOrderState? State;

            public KeyedOrderToggle(bool reversed)
            {
                Reversed = reversed;
            }
        }

        private sealed class KeyedOrderState : FState
        {
            private readonly KeyedOrderToggle _toggle;

            public KeyedOrderState(KeyedOrderToggle toggle)
            {
                _toggle = toggle;
            }

            public override void InitState()
            {
                _toggle.State = this;
            }

            public new void SetState(Action? action)
            {
                base.SetState(action);
            }

            public override FElement Build(FBuildContext context)
            {
                FElement[] children;
                if (_toggle.Reversed)
                {
                    children = new FElement[]
                    {
                        new TestLeafElement("b", new FValueKey<string>("b")),
                        new TestLeafElement("a", new FValueKey<string>("a")),
                    };
                }
                else
                {
                    children = new FElement[]
                    {
                        new TestLeafElement("a", new FValueKey<string>("a")),
                        new TestLeafElement("b", new FValueKey<string>("b")),
                    };
                }

                return new TestMultiChildElement { Children = children };
            }
        }
    }
}
