#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FSliderTests
    {
        [Test]
        public void Constructor_DefaultValues()
        {
            var element = new FSlider();

            Assert.That(element.Value, Is.EqualTo(0f));
            Assert.That(element.Min, Is.EqualTo(0f));
            Assert.That(element.Max, Is.EqualTo(1f));
        }

        [Test]
        public void Constructor_SetsValue()
        {
            var element = new FSlider(value: 0.5f);

            Assert.That(element.Value, Is.EqualTo(0.5f));
        }

        [Test]
        public void Constructor_SetsMinMax()
        {
            var element = new FSlider(min: -10f, max: 10f);

            Assert.That(element.Min, Is.EqualTo(-10f));
            Assert.That(element.Max, Is.EqualTo(10f));
        }

        [Test]
        public void Constructor_SetsLabel()
        {
            var element = new FSlider(label: "Volume");

            Assert.That(element.Label, Is.EqualTo("Volume"));
        }

        [Test]
        public void Constructor_DefaultLabel_IsNull()
        {
            var element = new FSlider();

            Assert.That(element.Label, Is.Null);
        }

        [Test]
        public void Constructor_SetsOnChanged()
        {
            Action<float> callback = _ => { };

            var element = new FSlider(onChanged: callback);

            Assert.That(element.OnChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnChanged_IsNull()
        {
            var element = new FSlider();

            Assert.That(element.OnChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new FValueKey<string>("sld");
            var element = new FSlider(key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new FSlider();

            Assert.That(element.GetChildren(), Is.Empty);
        }
    }
}
