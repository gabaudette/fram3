#nullable enable
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Layout
{
    [TestFixture]
    internal sealed class ContainerTests
    {
        [Test]
        public void Constructor_Defaults_AreNull()
        {
            var element = new Container();

            Assert.That(element.Decoration, Is.Null);
            Assert.That(element.Width, Is.Null);
            Assert.That(element.Height, Is.Null);
            Assert.That(element.Padding, Is.Null);
        }

        [Test]
        public void Constructor_StoresDecoration()
        {
            var decoration = new BoxDecoration(Color: FrameColor.Red);
            var element = new Container(decoration: decoration);

            Assert.That(element.Decoration, Is.EqualTo(decoration));
        }

        [Test]
        public void Constructor_StoresDimensions()
        {
            var element = new Container(width: 200f, height: 100f);

            Assert.That(element.Width, Is.EqualTo(200f));
            Assert.That(element.Height, Is.EqualTo(100f));
        }

        [Test]
        public void Constructor_StoresPadding()
        {
            var padding = EdgeInsets.All(12f);
            var element = new Container(padding: padding);

            Assert.That(element.Padding, Is.EqualTo(padding));
        }
    }
}