#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Layout
{
    [TestFixture]
    internal sealed class DividerTests
    {
        [Test]
        public void Constructor_Defaults_AreCorrect()
        {
            var element = new Divider();

            Assert.That(element.Axis, Is.EqualTo(DividerAxis.Horizontal));
            Assert.That(element.Thickness, Is.EqualTo(1f));
            Assert.That(element.Color, Is.Null);
            Assert.That(element.Indent, Is.EqualTo(0f));
        }

        [Test]
        public void Constructor_StoresAxis()
        {
            var element = new Divider(axis: DividerAxis.Vertical);

            Assert.That(element.Axis, Is.EqualTo(DividerAxis.Vertical));
        }

        [Test]
        public void Constructor_StoresThickness()
        {
            var element = new Divider(thickness: 3f);

            Assert.That(element.Thickness, Is.EqualTo(3f));
        }

        [Test]
        public void Constructor_StoresColor()
        {
            var color = FrameColor.Red;
            var element = new Divider(color: color);

            Assert.That(element.Color, Is.EqualTo(color));
        }

        [Test]
        public void Constructor_StoresIndent()
        {
            var element = new Divider(indent: 16f);

            Assert.That(element.Indent, Is.EqualTo(16f));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("div");
            var element = new Divider(key: key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}
