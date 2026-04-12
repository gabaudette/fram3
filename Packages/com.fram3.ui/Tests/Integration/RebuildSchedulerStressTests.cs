#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Integration.Helpers;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Integration
{
    /// <summary>
    /// Stress-tests the FRebuildScheduler inside a real tree. Validates depth-ordered
    /// processing, coalescing of duplicate dirty marks, and re-entrant scheduling
    /// (a rebuild that marks another node dirty within the same flush).
    /// </summary>
    [TestFixture]
    internal sealed class RebuildSchedulerStressTests
    {
        private FRebuildScheduler _scheduler = null!;
        private FNodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            (_scheduler, _expander) = TreeBuilder.MakePipeline();
        }

        [Test]
        public void Flush_ProcessesNodesDepthFirst_ShallowBeforeDeep()
        {
            var order = new List<int>();

            var deepState = new OrderRecordingState(2, order);
            var shallowState = new OrderRecordingState(1, order);

            // Build tree: shallowStateful -> deepStateful -> leaf
            var deepStateful = new TestStatefulElement(() => deepState, "deep");
            shallowState.SetChild(deepStateful);
            var shallowStateful = new TestStatefulElement(() => shallowState, "shallow");

            TreeBuilder.Mount(shallowStateful, _expander);
            order.Clear();

            // Schedule deep before shallow to prove ordering is by depth, not insertion order
            deepState.ScheduleRebuild();
            shallowState.ScheduleRebuild();

            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(order[0], Is.EqualTo(1), "Shallow node should rebuild first");
            Assert.That(order[1], Is.EqualTo(2), "Deep node should rebuild second");
        }

        [Test]
        public void Flush_DuplicateDirtyMarks_OnlyOneRebuildOccurs()
        {
            var state = new CountingState();
            var stateful = new TestStatefulElement(() => state);
            TreeBuilder.Mount(stateful, _expander);

            var countBeforeFlush = state.BuildCount;

            // Mark the same node dirty three times
            state.MarkDirty();
            state.MarkDirty();
            state.MarkDirty();

            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(state.BuildCount - countBeforeFlush, Is.EqualTo(1));
        }

        [Test]
        public void Flush_ReentrantSchedule_NewDirtyNodeProcessedInSameFlushing_WhenDeeperDepth()
        {
            // A parent rebuild will mark a deeper child dirty mid-flush.
            // Because the child is deeper and we haven't processed it yet, it should
            // be included in the current flush pass.
            var childState = new CountingState();
            var childStateful = new TestStatefulElement(() => childState, "child");

            var parentState = new ReentrantParentState(childState, childStateful);
            var parentStateful = new TestStatefulElement(() => parentState);

            TreeBuilder.Mount(parentStateful, _expander);

            var parentCountBefore = parentState.BuildCount;
            var childCountBefore = childState.BuildCount;

            parentState.MarkDirty();
            TreeBuilder.Flush(_scheduler, _expander);

            // Parent rebuilt once; its rebuild also schedules child -- child is deeper so it
            // runs in the same pass
            Assert.That(parentState.BuildCount - parentCountBefore, Is.EqualTo(1));
            Assert.That(childState.BuildCount - childCountBefore, Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void Flush_AncestorDirtied_DescendantRebuildSkipped()
        {
            // When parent and child are both dirty, rebuilding parent re-expands the child
            // via FTreePatcher, clearing the child's dirty flag. The scheduler should skip
            // the now-clean child entry.
            var childState = new CountingState();
            var childStateful = new TestStatefulElement(() => childState, "child");

            var parentState = new StaticChildState(childStateful);
            var parentStateful = new TestStatefulElement(() => parentState);

            TreeBuilder.Mount(parentStateful, _expander);

            var parentCountBefore = parentState.BuildCount;
            var childCountBefore = childState.BuildCount;

            // Both dirty -- ancestor should subsume child rebuild
            parentState.MarkDirty();
            childState.MarkDirty();

            TreeBuilder.Flush(_scheduler, _expander);

            // Parent rebuilds once. Child is re-expanded through parent's rebuild (one
            // extra Build call), and the scheduler skips its own entry because IsDirty
            // was cleared by the parent's expansion.
            Assert.That(parentState.BuildCount - parentCountBefore, Is.EqualTo(1));
            Assert.That(childState.BuildCount - childCountBefore, Is.EqualTo(1));
        }

        [Test]
        public void Flush_ManyDirtyNodes_AllRebuilt()
        {
            const int count = 20;
            var states = new CountingState[count];

            // Build a flat tree: one parent with many stateful children
            var children = new FElement[count];
            for (var i = 0; i < count; i++)
            {
                var idx = i;
                states[i] = new CountingState();
                children[i] = new TestStatefulElement(() => states[idx], idx.ToString());
            }

            var root = new TestMultiChildElement { Children = children };
            TreeBuilder.Mount(root, _expander);

            var countsBefore = new int[count];
            for (var i = 0; i < count; i++) { countsBefore[i] = states[i].BuildCount; }

            // Mark all dirty
            foreach (var s in states) { s.MarkDirty(); }

            TreeBuilder.Flush(_scheduler, _expander);

            for (var i = 0; i < count; i++)
            {
                Assert.That(states[i].BuildCount - countsBefore[i], Is.EqualTo(1),
                    $"State[{i}] should rebuild exactly once");
            }
        }

        // ---- Helpers --------------------------------------------------------------------------

        private sealed class OrderRecordingState : FState
        {
            private readonly int _depth;
            private readonly List<int> _order;
            private FElement _child = null!;

            public OrderRecordingState(int depth, List<int> order)
            {
                _depth = depth;
                _order = order;
            }

            public void SetChild(FElement child) { _child = child; }

            public void ScheduleRebuild()
            {
                SetState(null);
            }

            public override FElement Build(FBuildContext context)
            {
                _order.Add(_depth);
                return _child ?? (FElement)new TestLeafElement($"leaf-{_depth}");
            }
        }

        private sealed class CountingState : FState
        {
            public int BuildCount { get; private set; }

            public void MarkDirty()
            {
                SetState(null);
            }

            public override FElement Build(FBuildContext context)
            {
                BuildCount++;
                return new TestLeafElement("leaf");
            }
        }

        private sealed class ReentrantParentState : FState
        {
            private readonly CountingState _childState;
            private readonly FElement _childElement;
            private bool _initialBuildDone;

            public int BuildCount { get; private set; }

            public ReentrantParentState(CountingState childState, FElement childElement)
            {
                _childState = childState;
                _childElement = childElement;
            }

            public void MarkDirty()
            {
                SetState(null);
            }

            public override FElement Build(FBuildContext context)
            {
                BuildCount++;
                if (_initialBuildDone)
                {
                    // Schedule child dirty during rebuild (re-entrant scheduling).
                    // Only safe after the initial mount because the child must be
                    // mounted before SetState can be called on it.
                    _childState.MarkDirty();
                }

                _initialBuildDone = true;
                return _childElement;
            }
        }

        private sealed class StaticChildState : FState
        {
            private readonly FElement _child;

            public int BuildCount { get; private set; }

            public StaticChildState(FElement child)
            {
                _child = child;
            }

            public void MarkDirty()
            {
                SetState(null);
            }

            public override FElement Build(FBuildContext context)
            {
                BuildCount++;
                return _child;
            }
        }
    }
}
