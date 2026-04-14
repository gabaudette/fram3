#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Layout
{
    [TestFixture]
    internal sealed class PaddingTests
    {
        [Test]
        public void Constructor_StoresPadding()
        {
            var insets = EdgeInsets.All(8f);
            var element = new Padding(insets);

            Assert.That(element.Insets, Is.EqualTo(insets));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("p");
            var element = new Padding(EdgeInsets.Zero, key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}