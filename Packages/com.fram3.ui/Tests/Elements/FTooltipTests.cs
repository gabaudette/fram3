#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FTooltipTests
    {
        [Test]
        public void Constructor_StoresMessage()
        {
            var element = new FTooltip("Hover info") { Child = new FText("x") };

            Assert.That(element.Message, Is.EqualTo("Hover info"));
        }

        [Test]
        public void Constructor_NullMessage_StoredAsEmpty()
        {
            var element = new FTooltip(null!) { Child = new FText("x") };

            Assert.That(element.Message, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new FValueKey<string>("tip");
            var element = new FTooltip("tip text", key: key) { Child = new FText("x") };

            Assert.That(element.Key, Is.EqualTo(key));
        }

        [Test]
        public void GetChildren_ReturnsChild()
        {
            var child = new FText("content");
            var element = new FTooltip("hint") { Child = child };

            var children = element.GetChildren();

            Assert.That(children.Count, Is.EqualTo(1));
            Assert.That(children[0], Is.SameAs(child));
        }
    }
}
