#nullable enable
using NUnit.Framework;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Mocks;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    internal sealed class RebuildSchedulerTests
    {
        private RebuildScheduler? _scheduler;
        private NodeExpander? _expander;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new RebuildScheduler();
            _expander = new NodeExpander(_scheduler);
        }

        [Test]
        public void HasDirtyNodes_InitiallyFalse()
        {
            Assert.That(_scheduler != null && _scheduler.HasDirtyNodes, Is.False);
        }

        [Test]
        public void Schedule_AddsDirtyNode()
        {
            var node = new Node(new TestLeafElement("a"), null, _scheduler);
            _scheduler?.Schedule(node);

            Assert.That(_scheduler != null && _scheduler.HasDirtyNodes, Is.True);
        }

        [Test]
        public void Flush_RebuildsDirtyNode()
        {
            var firstElement = new TestLeafElement("first");
            var secondElement = new TestLeafElement("second");
            var buildCount = 0;

            TestState? state;
            var statefulElement = new TestStatefulElement(() =>
            {
                state = new TestState(_ => buildCount++ == 0 ? firstElement : secondElement);
                return state;
            });

            var node = _expander?.Mount(statefulElement, null);
            Assert.That(node?.Children[0].Element, Is.SameAs(firstElement));

            node?.MarkDirty();
            Assert.That(_scheduler?.HasDirtyNodes, Is.True);

            if (_expander != null)
            {
                _scheduler?.Flush(_expander);
            }

            Assert.That(node?.Children[0].Element, Is.SameAs(secondElement));
            Assert.That(_scheduler != null && _scheduler.HasDirtyNodes, Is.False);
            Assert.That(node != null && node.IsDirty, Is.False);
        }

        [Test]
        public void Flush_ProcessesAncestorBeforeDescendant()
        {
            var rebuiltOrder = new System.Collections.Generic.List<int>();

            TestState? parentState;
            TestState? childState;

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

            var parentNode = _expander?.Mount(parentElement, null);

            var childNode = parentNode?.Children[0];
            childNode?.MarkDirty();
            parentNode?.MarkDirty();

            rebuiltOrder.Clear();
            if (_expander != null)
            {
                _scheduler?.Flush(_expander);
            }

            Assert.That(rebuiltOrder[0], Is.EqualTo(1));
        }

        [Test]
        public void Flush_SkipsNodesAlreadyCleanedByAncestorRebuild()
        {
            var childBuildCount = 0;

            var childElement = new TestStatefulElement(() =>
                new TestState(_ =>
                {
                    childBuildCount++;
                    return new TestLeafElement("x");
                }));

            TestState? parentState;
            var parentElement = new TestStatefulElement(() =>
            {
                parentState = new TestState(_ => childElement);
                return parentState;
            });

            var parentNode = _expander?.Mount(parentElement, null);
            var childNode = parentNode?.Children[0];

            var initialChildBuilds = childBuildCount;

            childNode?.MarkDirty();
            parentNode?.MarkDirty();

            if (_expander != null)
            {
                _scheduler?.Flush(_expander);
            }

            Assert.That(childBuildCount, Is.EqualTo(initialChildBuilds + 1));
        }

        [Test]
        public void Flush_WhenNotDirty_DoesNotRebuild()
        {
            var buildCount = 0;
            var element = new TestStatefulElement(() =>
                new TestState(_ =>
                {
                    buildCount++;
                    return new TestLeafElement("x");
                }));

            _expander?.Mount(element, null);
            var buildsAfterMount = buildCount;

            if (_expander != null)
            {
                _scheduler?.Flush(_expander);
            }

            Assert.That(buildCount, Is.EqualTo(buildsAfterMount));
        }
    }
}