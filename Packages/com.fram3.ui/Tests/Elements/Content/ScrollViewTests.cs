#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class ScrollViewTests
    {
        [Test]
        public void Constructor_DefaultScrollDirection_IsVertical()
        {
            var element = new ScrollView();

            Assert.That(element.ScrollDirection, Is.EqualTo(ScrollDirection.Vertical));
        }

        [Test]
        public void Constructor_StoresScrollDirection()
        {
            var element = new ScrollView(ScrollDirection.Horizontal);

            Assert.That(element.ScrollDirection, Is.EqualTo(ScrollDirection.Horizontal));
        }

        [Test]
        public void Constructor_BothScrollDirection_IsStored()
        {
            var element = new ScrollView(ScrollDirection.Both);

            Assert.That(element.ScrollDirection, Is.EqualTo(ScrollDirection.Both));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("sv");
            var element = new ScrollView(key: key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}
