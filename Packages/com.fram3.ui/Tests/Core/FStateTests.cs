using System;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    public class FStateTests
    {
        private TestState CreateMountedState()
        {
            var state = new TestState(ctx => new TestLeafElement("built"));
            var element = new TestStatefulElement(() => state);
            var node = new FNode(element, null);
            state.Mount(node);
            return state;
        }

        [Test]
        public void CreateState_ReturnsStateInstance()
        {
            TestState capturedState = null;
            var element = new TestStatefulElement(() =>
            {
                capturedState = new TestState(ctx => new TestLeafElement("x"));
                return capturedState;
            });

            var result = element.CreateState();

            Assert.That(result, Is.SameAs(capturedState));
        }

        [Test]
        public void Mount_MakesStateAvailable()
        {
            var state = CreateMountedState();

            Assert.That(state.Mounted, Is.True);
            Assert.That(state.Context, Is.Not.Null);
            Assert.That(state.Element, Is.Not.Null);
        }

        [Test]
        public void Unmount_ClearsState()
        {
            var state = CreateMountedState();

            state.Unmount();

            Assert.That(state.Mounted, Is.False);
            Assert.That(state.Context, Is.Null);
            Assert.That(state.Element, Is.Null);
        }

        [Test]
        public void InitState_IsCalledWhenInvoked()
        {
            var state = CreateMountedState();
            state.InitState();

            Assert.That(state.InitStateCalled, Is.True);
        }

        [Test]
        public void Build_IncrementsCount()
        {
            var state = CreateMountedState();

            state.Build(state.Context);
            state.Build(state.Context);

            Assert.That(state.BuildCount, Is.EqualTo(2));
        }

        [Test]
        public void DidUpdateElement_ReceivesOldElement()
        {
            var state = CreateMountedState();
            var oldElement = new TestStatefulElement(
                () => new TestState(ctx => new TestLeafElement("x")),
                config: "old"
            );

            state.DidUpdateElement(oldElement);

            Assert.That(state.LastOldElement, Is.SameAs(oldElement));
        }

        [Test]
        public void Dispose_MarksAsDisposed()
        {
            var state = CreateMountedState();

            state.Dispose();

            Assert.That(state.DisposeCalled, Is.True);
        }

        [Test]
        public void SetState_WhenMounted_ExecutesAction()
        {
            var state = CreateMountedState();
            bool actionCalled = false;

            state.SetState(() => { actionCalled = true; });

            Assert.That(actionCalled, Is.True);
        }

        [Test]
        public void SetState_WhenUnmounted_ThrowsInvalidOperationException()
        {
            var state = CreateMountedState();
            state.Unmount();

            Assert.Throws<InvalidOperationException>(() =>
            {
                state.SetState(() => { });
            });
        }

        [Test]
        public void SetState_MarksNodeDirty()
        {
            var state = CreateMountedState();

            state.SetState(() => { });

            Assert.That(state.Context.Node.IsDirty, Is.True);
        }

        [Test]
        public void SetState_WithNullAction_DoesNotThrow()
        {
            var state = CreateMountedState();

            Assert.DoesNotThrow(() => state.SetState(null));
        }

        [Test]
        public void TypedState_ProvidesTypedElementAccess()
        {
            var state = new TestState(ctx => new TestLeafElement("x"));
            var element = new TestStatefulElement(() => state, config: "typed");
            var node = new FNode(element, null);
            state.Mount(node);

            Assert.That(state.Element, Is.InstanceOf<TestStatefulElement>());
            Assert.That(state.Element.Config, Is.EqualTo("typed"));
        }
    }
}
