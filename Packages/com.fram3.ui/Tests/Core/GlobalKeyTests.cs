#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Integration.Helpers;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    internal sealed class GlobalKeyTests
    {
        private NodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            (_, _expander) = TreeBuilder.MakePipeline();
        }

        [Test]
        public void CurrentContext_ReturnsContext_AfterMount()
        {
            var key = new GlobalKey();
            var element = new TestStatelessElement(_ => new TestLeafElement("child"), key);

            TreeBuilder.Mount(element, _expander);

            Assert.That(key.CurrentContext, Is.Not.Null);
            Assert.That(key.CurrentContext!.Element, Is.SameAs(element));
        }

        [Test]
        public void CurrentState_ReturnsState_AfterMount()
        {
            var key = new GlobalKey();
            var state = new TestState(_ => new TestLeafElement("x"));
            var element = new TestStatefulElement(() => state, key: key);

            TreeBuilder.Mount(element, _expander);

            Assert.That(key.CurrentState, Is.SameAs(state));
        }

        [Test]
        public void CurrentContext_ReturnsNull_AfterUnmount()
        {
            var key = new GlobalKey();
            var element = new TestStatelessElement(_ => new TestLeafElement("child"), key);

            var root = TreeBuilder.Mount(element, _expander);
            _expander.Unmount(root);

            Assert.That(key.CurrentContext, Is.Null);
        }

        [Test]
        public void TypedGlobalKey_CurrentState_ReturnsCastState()
        {
            var key = new GlobalKey<TestState>();
            var state = new TestState(_ => new TestLeafElement("x"));
            var element = new TestStatefulElement(() => state, key: key);

            TreeBuilder.Mount(element, _expander);

            Assert.That(key.CurrentState, Is.SameAs(state));
        }

        [Test]
        public void TwoDistinctGlobalKeys_AreNotEqual()
        {
            var key1 = new GlobalKey();
            var key2 = new GlobalKey();

            Assert.That(key1, Is.Not.EqualTo(key2));
        }
    }
}
