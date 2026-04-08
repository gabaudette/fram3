using NUnit.Framework;
using Fram3.UI.Core;
using Fram3.UI.Tests.Mocks;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    internal sealed class FRebuildSchedulerTests
    {
        private FRebuildScheduler _scheduler;
        private FNodeExpander _expander;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new FRebuildScheduler();
            _expander = new FNodeExpander(_scheduler);
        }

        [Test]
        public void HasDirtyNodes_InitiallyFalse()
        {
            Assert.That(_scheduler.HasDirtyNodes, Is.False);
        }

        [Test]
        public void Schedule_AddsDirtyNode()
        {
            var node = new FNode(new TestLeafElement("a"), null, _scheduler);
            _scheduler.Schedule(node);

            Assert.That(_scheduler.HasDirtyNodes, Is.True);
        }

        [Test]
        public void Flush_RebuildsDirtyNode()
        {
            var firstElement = new TestLeafElement("first");
            var secondElement = new TestLeafElement("second");
            int buildCount = 0;

            TestState state = null;
            var statefulElement = new TestStatefulElement(() =>
            {
                state = new TestState(_ => buildCount++ == 0 ? firstElement : secondElement);
                return state;
            });

            FNode node = _expander.Mount(statefulElement, null);
            Assert.That(node.Children[0].Element, Is.SameAs(firstElement));

            node.MarkDirty();
            Assert.That(_scheduler.HasDirtyNodes, Is.True);

            _scheduler.Flush(_expander);

            Assert.That(node.Children[0].Element, Is.SameAs(secondElement));
            Assert.That(_scheduler.HasDirtyNodes, Is.False);
            Assert.That(node.IsDirty, Is.False);
        }

        [Test]
        public void Flush_ProcessesAncestorBeforeDescendant()
        {
            var rebuiltOrder = new System.Collections.Generic.List<int>();

            TestState parentState = null;
            TestState childState = null;

            var childElement = new TestStatefulElement(() =>
            {
                childState = new TestState(_ =>
                {
                    rebuiltOrder.Add(2);
                    return new TestLeafElement("child-built");
                });
                return childState;
            });

            var parentElement = new TestStatefulElement(() =>
            {
                parentState = new TestState(_ =>
                {
                    rebuiltOrder.Add(1);
                    return childElement;
                });
                return parentState;
            });

            FNode parentNode = _expander.Mount(parentElement, null);

            FNode childNode = parentNode.Children[0];
            childNode.MarkDirty();
            parentNode.MarkDirty();

            rebuiltOrder.Clear();
            _scheduler.Flush(_expander);

            Assert.That(rebuiltOrder[0], Is.EqualTo(1));
        }

        [Test]
        public void Flush_SkipsNodesAlreadyCleanedByAncestorRebuild()
        {
            int childBuildCount = 0;

            var childElement = new TestStatefulElement(() =>
                new TestState(_ =>
                {
                    childBuildCount++;
                    return new TestLeafElement("x");
                }));

            TestState parentState = null;
            var parentElement = new TestStatefulElement(() =>
            {
                parentState = new TestState(_ => childElement);
                return parentState;
            });

            FNode parentNode = _expander.Mount(parentElement, null);
            FNode childNode = parentNode.Children[0];

            int initialChildBuilds = childBuildCount;

            childNode.MarkDirty();
            parentNode.MarkDirty();

            _scheduler.Flush(_expander);

            Assert.That(childBuildCount, Is.EqualTo(initialChildBuilds + 1));
        }

        [Test]
        public void Flush_WhenNotDirty_DoesNotRebuild()
        {
            int buildCount = 0;
            var element = new TestStatefulElement(() =>
                new TestState(_ =>
                {
                    buildCount++;
                    return new TestLeafElement("x");
                }));

            _expander.Mount(element, null);
            int buildsAfterMount = buildCount;

            _scheduler.Flush(_expander);

            Assert.That(buildCount, Is.EqualTo(buildsAfterMount));
        }
    }
}
