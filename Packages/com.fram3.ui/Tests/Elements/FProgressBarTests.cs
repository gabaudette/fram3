#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FProgressBarTests
    {
        [Test]
        public void Constructor_StoresValue()
        {
            var element = new FProgressBar(value: 50f);

            Assert.That(element.Value, Is.EqualTo(50f));
        }

        [Test]
        public void Constructor_DefaultMin_IsZero()
        {
            var element = new FProgressBar(value: 0f);

            Assert.That(element.Min, Is.EqualTo(0f));
        }

        [Test]
        public void Constructor_DefaultMax_IsOneHundred()
        {
            var element = new FProgressBar(value: 0f);

            Assert.That(element.Max, Is.EqualTo(100f));
        }

        [Test]
        public void Constructor_StoresMinAndMax()
        {
            var element = new FProgressBar(value: 0.5f, min: 0f, max: 1f);

            Assert.That(element.Min, Is.EqualTo(0f));
            Assert.That(element.Max, Is.EqualTo(1f));
        }

        [Test]
        public void Constructor_DefaultTitle_IsNull()
        {
            var element = new FProgressBar(value: 0f);

            Assert.That(element.Title, Is.Null);
        }

        [Test]
        public void Constructor_StoresTitle()
        {
            var element = new FProgressBar(value: 50f, title: "Loading...");

            Assert.That(element.Title, Is.EqualTo("Loading..."));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new FValueKey<string>("pb");
            var element = new FProgressBar(value: 0f, key: key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}
