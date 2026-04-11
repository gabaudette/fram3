#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FImageTests
    {
        [Test]
        public void Constructor_DefaultSource_IsNull()
        {
            var element = new FImage();

            Assert.That(element.Source, Is.Null);
        }

        [Test]
        public void Constructor_DefaultDimensions_AreNull()
        {
            var element = new FImage();

            Assert.That(element.Width, Is.Null);
            Assert.That(element.Height, Is.Null);
        }

        [Test]
        public void Constructor_StoresDimensions()
        {
            var element = new FImage(width: 64f, height: 64f);

            Assert.That(element.Width, Is.EqualTo(64f));
            Assert.That(element.Height, Is.EqualTo(64f));
        }

        [Test]
        public void Constructor_StoresSource()
        {
            var source = new object();
            var element = new FImage(source: source);

            Assert.That(element.Source, Is.SameAs(source));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new FValueKey<string>("img");
            var element = new FImage(key: key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}
