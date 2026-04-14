#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Layout
{
    [TestFixture]
    internal sealed class StackTests
    {
        [Test]
        public void Constructor_NoArgs_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new Stack());
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("stack");
            var element = new Stack(key: key);

            Assert.That(element.Key, Is.EqualTo(key));
        }

        [Test]
        public void GetChildren_WithChildren_ReturnsChildren()
        {
            var child1 = new Text("A");
            var child2 = new Text("B");
            var element = new Stack { Children = new Element[] { child1, child2 } };

            var children = element.GetChildren();

            Assert.That(children.Count, Is.EqualTo(2));
            Assert.That(children[0], Is.SameAs(child1));
            Assert.That(children[1], Is.SameAs(child2));
        }
    }
}
