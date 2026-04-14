#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class TextTests
    {
        [Test]
        public void Constructor_StoresText()
        {
            var element = new Text("Hello");

            Assert.That(element.Content, Is.EqualTo("Hello"));
        }

        [Test]
        public void Constructor_NullText_StoredAsEmpty()
        {
            var element = new Text(null!);

            Assert.That(element.Content, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Constructor_StoresStyle()
        {
            var style = new TextStyle(FontSize: 16f);
            var element = new Text("Hi", style);

            Assert.That(element.Style, Is.EqualTo(style));
        }

        [Test]
        public void Constructor_NullStyle_StyleIsNull()
        {
            var element = new Text("Hi");

            Assert.That(element.Style, Is.Null);
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("k");
            var element = new Text("x", key: key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}