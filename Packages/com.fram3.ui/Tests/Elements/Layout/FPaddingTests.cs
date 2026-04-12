#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Layout
{
    [TestFixture]
    internal sealed class FPaddingTests
    {
        [Test]
        public void Constructor_StoresPadding()
        {
            var insets = FEdgeInsets.All(8f);
            var element = new FPadding(insets);

            Assert.That(element.Padding, Is.EqualTo(insets));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new FValueKey<string>("p");
            var element = new FPadding(FEdgeInsets.Zero, key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}