#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class FIconTests
    {
        [Test]
        public void Constructor_DefaultSource_IsNull()
        {
            var element = new FIcon();

            Assert.That(element.Source, Is.Null);
        }

        [Test]
        public void Constructor_DefaultDimensions_AreNull()
        {
            var element = new FIcon();

            Assert.That(element.Width, Is.Null);
            Assert.That(element.Height, Is.Null);
        }

        [Test]
        public void Constructor_StoresDimensions()
        {
            var element = new FIcon(width: 24f, height: 24f);

            Assert.That(element.Width, Is.EqualTo(24f));
            Assert.That(element.Height, Is.EqualTo(24f));
        }

        [Test]
        public void Constructor_StoresSource()
        {
            var source = new object();
            var element = new FIcon(source: source);

            Assert.That(element.Source, Is.SameAs(source));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new FValueKey<string>("icon");
            var element = new FIcon(key: key);

            Assert.That(element.Key, Is.EqualTo(key));
        }

        [Test]
        public void Constructor_DefaultSvgPath_IsNull()
        {
            var element = new FIcon();

            Assert.That(element.SvgPath, Is.Null);
        }

        [Test]
        public void Constructor_StoresSvgPath()
        {
            var element = new FIcon(svgPath: "Assets/Icons/arrow.svg");

            Assert.That(element.SvgPath, Is.EqualTo("Assets/Icons/arrow.svg"));
        }
    }
}
