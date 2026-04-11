#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FExpandedTests
    {
        [Test]
        public void Constructor_DefaultFlex_IsOne()
        {
            var element = new FExpanded { Child = new FText("x") };

            Assert.That(element.Flex, Is.EqualTo(1f));
        }

        [Test]
        public void Constructor_StoresFlex()
        {
            var element = new FExpanded(flex: 2f) { Child = new FText("x") };

            Assert.That(element.Flex, Is.EqualTo(2f));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new FValueKey<string>("exp");
            var element = new FExpanded(key: key) { Child = new FText("x") };

            Assert.That(element.Key, Is.EqualTo(key));
        }

        [Test]
        public void GetChildren_ReturnsChild()
        {
            var child = new FText("hello");
            var element = new FExpanded { Child = child };

            var children = element.GetChildren();

            Assert.That(children.Count, Is.EqualTo(1));
            Assert.That(children[0], Is.SameAs(child));
        }
    }
}
