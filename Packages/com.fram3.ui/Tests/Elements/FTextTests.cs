#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements;
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FTextTests
    {
        [Test]
        public void Constructor_StoresText()
        {
            var element = new FText("Hello");

            Assert.That(element.Text, Is.EqualTo("Hello"));
        }

        [Test]
        public void Constructor_NullText_StoredAsEmpty()
        {
            var element = new FText(null!);

            Assert.That(element.Text, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Constructor_StoresStyle()
        {
            var style = new FTextStyle(FontSize: 16f);
            var element = new FText("Hi", style);

            Assert.That(element.Style, Is.EqualTo(style));
        }

        [Test]
        public void Constructor_NullStyle_StyleIsNull()
        {
            var element = new FText("Hi");

            Assert.That(element.Style, Is.Null);
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new FValueKey<string>("k");
            var element = new FText("x", key: key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}
