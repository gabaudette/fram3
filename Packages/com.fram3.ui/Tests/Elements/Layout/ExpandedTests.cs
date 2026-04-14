#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Layout
{
    [TestFixture]
    internal sealed class ExpandedTests
    {
        [Test]
        public void Constructor_DefaultFlex_IsOne()
        {
            var element = new Expanded { Child = new Text("x") };

            Assert.That(element.Flex, Is.EqualTo(1f));
        }

        [Test]
        public void Constructor_StoresFlex()
        {
            var element = new Expanded(flex: 2f) { Child = new Text("x") };

            Assert.That(element.Flex, Is.EqualTo(2f));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("exp");
            var element = new Expanded(key: key) { Child = new Text("x") };

            Assert.That(element.Key, Is.EqualTo(key));
        }

        [Test]
        public void GetChildren_ReturnsChild()
        {
            var child = new Text("hello");
            var element = new Expanded { Child = child };

            var children = element.GetChildren();

            Assert.That(children.Count, Is.EqualTo(1));
            Assert.That(children[0], Is.SameAs(child));
        }
    }
}
