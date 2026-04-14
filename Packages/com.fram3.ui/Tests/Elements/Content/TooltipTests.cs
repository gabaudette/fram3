#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class TooltipTests
    {
        [Test]
        public void Constructor_StoresMessage()
        {
            var element = new Tooltip("Hover info") { Child = new Text("x") };

            Assert.That(element.Message, Is.EqualTo("Hover info"));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("tip");
            var element = new Tooltip("tip text", key: key) { Child = new Text("x") };

            Assert.That(element.Key, Is.EqualTo(key));
        }

        [Test]
        public void GetChildren_ReturnsChild()
        {
            var child = new Text("content");
            var element = new Tooltip("hint") { Child = child };

            var children = element.GetChildren();

            Assert.That(children.Count, Is.EqualTo(1));
            Assert.That(children[0], Is.SameAs(child));
        }
    }
}
