#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class FScrollViewTests
    {
        [Test]
        public void Constructor_DefaultScrollDirection_IsVertical()
        {
            var element = new FScrollView();

            Assert.That(element.ScrollDirection, Is.EqualTo(FScrollDirection.Vertical));
        }

        [Test]
        public void Constructor_StoresScrollDirection()
        {
            var element = new FScrollView(FScrollDirection.Horizontal);

            Assert.That(element.ScrollDirection, Is.EqualTo(FScrollDirection.Horizontal));
        }

        [Test]
        public void Constructor_BothScrollDirection_IsStored()
        {
            var element = new FScrollView(FScrollDirection.Both);

            Assert.That(element.ScrollDirection, Is.EqualTo(FScrollDirection.Both));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new FValueKey<string>("sv");
            var element = new FScrollView(key: key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}
