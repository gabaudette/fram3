using NUnit.Framework;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Mocks;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    internal sealed class FNodeExpanderTests
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
        public void Mount_LeafElement_CreatesNodeWithNoChildren()
        {
            var element = new TestLeafElement("x");

            var node = _expander.Mount(element, null);

            Assert.That(node.Element, Is.SameAs(element));
            Assert.That(node.Children, Is.Empty);
            Assert.That(node.State, Is.Null);
        }

        [Test]
        public void Mount_SingleChildElement_CreatesNodeWithOneChild()
        {
            var child = new TestLeafElement("child");
            var element = new TestSingleChildElement { Child = child };

            var node = _expander.Mount(element, null);

            Assert.That(node.Children.Count, Is.EqualTo(1));
            Assert.That(node.Children[0].Element, Is.SameAs(child));
        }

        [Test]
        public void Mount_MultiChildElement_CreatesCorrectNumberOfChildren()
        {
            var element = new TestMultiChildElement
            {
                Children = new FElement[]
                {
                    new TestLeafElement("a"),
                    new TestLeafElement("b"),
                    new TestLeafElement("c")
                }
            };

            var node = _expander.Mount(element, null);

            Assert.That(node.Children.Count, Is.EqualTo(3));
        }

        [Test]
        public void Mount_StatefulElement_MountsStateAndCallsInitState()
        {
            TestState? capturedState = null;
            var element = new TestStatefulElement(() =>
            {
                capturedState = new TestState(_ => new TestLeafElement("built"));
                return capturedState;
            });

            var node = _expander.Mount(element, null);

            Assert.That(node.State, Is.Not.Null);
            Assert.That(capturedState!.InitStateCalled, Is.True);
            Assert.That(capturedState.Mounted, Is.True);        }

        [Test]
        public void Mount_StatefulElement_BuildsChildFromState()
        {
            var builtElement = new TestLeafElement("built");
            var element = new TestStatefulElement(() => new TestState(_ => builtElement));

            var node = _expander.Mount(element, null);

            Assert.That(node.Children.Count, Is.EqualTo(1));
            Assert.That(node.Children[0].Element, Is.SameAs(builtElement));
        }

        [Test]
        public void Mount_StatelessElement_BuildsChildFromBuildMethod()
        {
            var builtElement = new TestLeafElement("built");
            var element = new TestStatelessElement(_ => builtElement);

            var node = _expander.Mount(element, null);

            Assert.That(node.Children.Count, Is.EqualTo(1));
            Assert.That(node.Children[0].Element, Is.SameAs(builtElement));
        }

        [Test]
        public void Mount_SetsDepthCorrectlyForNestedNodes()
        {
            var root = new TestSingleChildElement
            {
                Child = new TestSingleChildElement
                {
                    Child = new TestLeafElement("leaf")
                }
            };

            var rootNode = _expander.Mount(root, null);
            var childNode = rootNode.Children[0];
            var grandchildNode = childNode.Children[0];

            Assert.That(rootNode.Depth, Is.EqualTo(0));
            Assert.That(childNode.Depth, Is.EqualTo(1));
            Assert.That(grandchildNode.Depth, Is.EqualTo(2));
        }

        [Test]
        public void Unmount_StatefulNode_CallsDisposeAndUnmountsState()
        {
            TestState? state = null;
            var element = new TestStatefulElement(() =>
            {
                state = new TestState(_ => new TestLeafElement("x"));
                return state;
            });

            var node = _expander.Mount(element, null);

            _expander.Unmount(node);

            Assert.That(state != null && state.DisposeCalled, Is.True);
            Assert.That(state != null && state.Mounted, Is.False);
            Assert.That(node.State, Is.Null);
        }

        [Test]
        public void Unmount_RecursivelyUnmountsChildren()
        {
            TestState? innerState = null;
            var innerStateful = new TestStatefulElement(() =>
            {
                innerState = new TestState(_ => new TestLeafElement("leaf"));
                return innerState;
            });

            var root = new TestSingleChildElement { Child = innerStateful };

            var rootNode = _expander.Mount(root, null);

            _expander.Unmount(rootNode);

            Assert.That(innerState != null && innerState.DisposeCalled, Is.True);
            Assert.That(rootNode.Children, Is.Empty);
        }

        [Test]
        public void UpdateElement_StatefulNode_CallsDidUpdateElement()
        {
            var original = new TestStatefulElement(
                () => new TestState(_ => new TestLeafElement("x")),
                config: "v1");

            var node = _expander.Mount(original, null);
            var state = (TestState)node.State!;

            var updated = new TestStatefulElement(
                () => new TestState(_ => new TestLeafElement("x")),
                config: "v2"
            );

            _expander.UpdateElement(node, updated);

            Assert.That(state?.LastOldElement, Is.SameAs(original));
            Assert.That(node.Element, Is.SameAs(updated));
        }

        [Test]
        public void Rebuild_StatefulNode_RebuildsChildren()
        {
            var firstBuilt = new TestLeafElement("first");
            var secondBuilt = new TestLeafElement("second");
            var buildCount = 0;

            TestState? state;
            var element = new TestStatefulElement(() =>
            {
                state = new TestState(_ => buildCount++ == 0 ? firstBuilt : secondBuilt);
                return state;
            });

            var node = _expander.Mount(element, null);
            Assert.That(node.Children[0].Element, Is.SameAs(firstBuilt));

            node.MarkDirty();

            _expander.Rebuild(node);

            Assert.That(node.Children[0].Element, Is.SameAs(secondBuilt));
            Assert.That(node.IsDirty, Is.False);
        }
    }
}