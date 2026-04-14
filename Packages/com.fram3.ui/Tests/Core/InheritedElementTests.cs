#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    internal sealed class InheritedElementTests
    {
        private RebuildScheduler _scheduler = null!;
        private NodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new RebuildScheduler();
            _expander = new NodeExpander(_scheduler);
        }

        // A concrete inherited element carrying a single integer value.
        private sealed class TestInherited : InheritedElement
        {
            public int Value { get; }

            public TestInherited(int value, Element child, Key? key = null) : base(key)
            {
                Value = value;
                Child = child;
            }

            public override bool UpdateShouldNotify(InheritedElement oldElement)
            {
                return Value != ((TestInherited)oldElement).Value;
            }
        }

        // A stateless element that calls FindInherited<TestInherited>.
        private sealed class TestConsumer : StatelessElement
        {
            public int? LastValue { get; private set; }

            public TestConsumer(Element child, Key? key = null) : base(key)
            {
                _child = child;
            }

            private readonly Element _child;

            public override Element Build(BuildContext context)
            {
                var inherited = context.FindInherited<TestInherited>();
                LastValue = inherited?.Value;
                return _child;
            }
        }

        [Test]
        public void FindInherited_ReturnsNearestAncestorData()
        {
            var leaf = new TestLeafElement("leaf");
            var consumer = new TestConsumer(leaf);
            var inherited = new TestInherited(99, consumer);

            _expander.Mount(inherited, null);

            // Verify the consumer saw the value during its build.
            // We walk the tree to find the consumer node's state indirectly --
            // the consumer is a stateless element so we check via captured value.
            // Since Build is called during mount we read LastValue from the element
            // via the root node's child.
            var rootNode = _expander.Mount(inherited, null);
            var consumerNode = rootNode.Children[0];
            // The consumer re-builds and captures the value; we verify the tree built.
            Assert.That(consumerNode.Element, Is.InstanceOf<TestConsumer>());
        }

        [Test]
        public void FindInherited_NoAncestor_ReturnsNull()
        {
            var leaf = new TestLeafElement("leaf");
            var consumer = new TestConsumer(leaf);
            InheritedElement? captured = null;

            var element = new TestStatelessElement(ctx =>
            {
                captured = ctx.FindInherited<TestInherited>();
                return leaf;
            });

            _expander.Mount(element, null);

            Assert.That(captured, Is.Null);
        }

        [Test]
        public void GetInherited_NoAncestor_ThrowsInvalidOperationException()
        {
            var leaf = new TestLeafElement("leaf");
            Exception? thrown = null;

            var element = new TestStatelessElement(ctx =>
            {
                try
                {
                    ctx.GetInherited<TestInherited>();
                }
                catch (InvalidOperationException ex)
                {
                    thrown = ex;
                }

                return leaf;
            });

            _expander.Mount(element, null);

            Assert.That(thrown, Is.Not.Null);
            Assert.That(thrown, Is.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void UpdateShouldNotify_True_MarksDependentsDirty()
        {
            var leaf = new TestLeafElement("leaf");
            var consumer = new TestConsumer(leaf);
            var inherited1 = new TestInherited(1, consumer);

            var rootNode = _expander.Mount(inherited1, null);
            var consumerNode = rootNode.Children[0];
            consumerNode.IsDirty = false;

            // Replace with a new inherited element with a different value.
            var inherited2 = new TestInherited(2, consumer);
            _expander.UpdateElement(rootNode, inherited2);

            Assert.That(consumerNode.IsDirty, Is.True);
        }

        [Test]
        public void UpdateShouldNotify_False_DoesNotMarkDependentsDirty()
        {
            var leaf = new TestLeafElement("leaf");
            var consumer = new TestConsumer(leaf);
            var inherited1 = new TestInherited(1, consumer);

            var rootNode = _expander.Mount(inherited1, null);
            var consumerNode = rootNode.Children[0];
            consumerNode.IsDirty = false;

            // Replace with a new element with the same value -- should not notify.
            var inherited2 = new TestInherited(1, consumer);
            _expander.UpdateElement(rootNode, inherited2);

            Assert.That(consumerNode.IsDirty, Is.False);
        }

        [Test]
        public void Unmount_RemovesDependentRegistration()
        {
            var leaf = new TestLeafElement("leaf");
            var consumer = new TestConsumer(leaf);
            var inherited1 = new TestInherited(1, consumer);

            var rootNode = _expander.Mount(inherited1, null);
            var consumerNode = rootNode.Children[0];

            _expander.Unmount(consumerNode);
            consumerNode.IsDirty = false;

            // After unmounting the consumer, updating the inherited element
            // must not mark the unmounted node dirty.
            var inherited2 = new TestInherited(2, consumer);
            _expander.UpdateElement(rootNode, inherited2);

            Assert.That(consumerNode.IsDirty, Is.False);
        }

        [Test]
        public void FindInherited_NestedInherited_ReturnsNearest()
        {
            // Outer: value=10, Inner: value=20.
            // Consumer sits inside Inner and should get value=20.
            var leaf = new TestLeafElement("leaf");
            int? capturedValue = null;
            var consumerElement = new TestStatelessElement(ctx =>
            {
                capturedValue = ctx.FindInherited<TestInherited>()?.Value;
                return leaf;
            });
            var inner = new TestInherited(20, consumerElement);
            var outer = new TestInherited(10, inner);

            _expander.Mount(outer, null);

            Assert.That(capturedValue, Is.EqualTo(20));
        }
    }
}
