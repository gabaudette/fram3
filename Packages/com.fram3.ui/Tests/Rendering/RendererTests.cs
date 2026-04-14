#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Rendering;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;
using UnityEngine.UIElements;

namespace Fram3.UI.Tests.Rendering
{
    [TestFixture]
    internal sealed class RendererTests
    {
        private VisualElement _container = null!;
        private Renderer _renderer = null!;

        [SetUp]
        public void SetUp()
        {
            _container = new VisualElement();
            _renderer = new Renderer();
        }

        [TearDown]
        public void TearDown()
        {
            _renderer.Dispose();
        }

        [Test]
        public void Mount_NullRoot_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _renderer.Mount(null!, _container));
        }

        [Test]
        public void Mount_NullContainer_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _renderer.Mount(new TestRenderLeaf("x"), null!));
        }

        [Test]
        public void Mount_WhenAlreadyMounted_ThrowsInvalidOperationException()
        {
            _renderer.Mount(new TestRenderLeaf("a"), _container);

            Assert.Throws<InvalidOperationException>(() =>
                _renderer.Mount(new TestRenderLeaf("b"), _container));
        }

        [Test]
        public void Mount_LeafElement_ProducesNativeChildInContainer()
        {
            _renderer.Mount(new TestRenderLeaf("a"), _container);

            Assert.That(_container.childCount, Is.GreaterThan(0));
        }

        [Test]
        public void Mount_StatelessElement_ExpandsIntoNativeChildren()
        {
            var element = new TestStatelessElement(_ => new TestRenderLeaf("child"));

            _renderer.Mount(element, _container);

            Assert.That(_container.childCount, Is.GreaterThan(0));
        }

        [Test]
        public void Dispose_UnmountsTree_RemovesNativeChildrenFromContainer()
        {
            _renderer.Mount(new TestRenderLeaf("a"), _container);

            Assert.That(_container.childCount, Is.GreaterThan(0));

            _renderer.Dispose();

            Assert.That(_container.childCount, Is.EqualTo(0));
        }

        [Test]
        public void Dispose_CalledTwice_DoesNotThrow()
        {
            _renderer.Mount(new TestRenderLeaf("a"), _container);
            _renderer.Dispose();

            Assert.DoesNotThrow(() => _renderer.Dispose());
        }

        [Test]
        public void Tick_WhenNoDirtyNodes_DoesNotThrow()
        {
            _renderer.Mount(new TestRenderLeaf("a"), _container);

            Assert.DoesNotThrow(() => _renderer.Tick(0f));
        }

        [Test]
        public void Tick_AfterSetState_RebuildsDirtySubtree()
        {
            var toggleState = new ToggleState();
            var element = new TestStatefulElement(() => toggleState);

            _renderer.Mount(element, _container);

            int childCountBefore = _container.childCount;

            toggleState.Toggle();
            _renderer.Tick(0f);

            Assert.That(toggleState.BuildCount, Is.GreaterThanOrEqualTo(2));
        }

        /// <summary>
        /// A stateful test helper that counts builds and triggers SetState on demand.
        /// </summary>
        private sealed class ToggleState : State<TestStatefulElement>
        {
            private bool _flag;
            public int BuildCount { get; private set; }

            public void Toggle()
            {
                SetState(() => _flag = !_flag);
            }

            public override Element Build(BuildContext context)
            {
                BuildCount++;
                return new TestRenderLeaf(_flag ? "on" : "off");
            }
        }
    }
}
