#nullable enable
using Fram3.UI.Elements;
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FContainerTests
    {
        [Test]
        public void Constructor_Defaults_AreNull()
        {
            var element = new FContainer();

            Assert.That(element.Decoration, Is.Null);
            Assert.That(element.Width, Is.Null);
            Assert.That(element.Height, Is.Null);
            Assert.That(element.Padding, Is.Null);
        }

        [Test]
        public void Constructor_StoresDecoration()
        {
            var decoration = new FBoxDecoration(Color: FColor.Red);
            var element = new FContainer(decoration: decoration);

            Assert.That(element.Decoration, Is.EqualTo(decoration));
        }

        [Test]
        public void Constructor_StoresDimensions()
        {
            var element = new FContainer(width: 200f, height: 100f);

            Assert.That(element.Width, Is.EqualTo(200f));
            Assert.That(element.Height, Is.EqualTo(100f));
        }

        [Test]
        public void Constructor_StoresPadding()
        {
            var padding = FEdgeInsets.All(12f);
            var element = new FContainer(padding: padding);

            Assert.That(element.Padding, Is.EqualTo(padding));
        }
    }
}
