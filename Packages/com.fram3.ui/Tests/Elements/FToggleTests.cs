#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FToggleTests
    {
        [Test]
        public void Constructor_DefaultValue_IsFalse()
        {
            var element = new FToggle();

            Assert.That(element.Value, Is.False);
        }

        [Test]
        public void Constructor_SetsValue()
        {
            var element = new FToggle(value: true);

            Assert.That(element.Value, Is.True);
        }

        [Test]
        public void Constructor_SetsLabel()
        {
            var element = new FToggle(label: "Enable feature");

            Assert.That(element.Label, Is.EqualTo("Enable feature"));
        }

        [Test]
        public void Constructor_DefaultLabel_IsNull()
        {
            var element = new FToggle();

            Assert.That(element.Label, Is.Null);
        }

        [Test]
        public void Constructor_SetsOnChanged()
        {
            Action<bool> callback = _ => { };

            var element = new FToggle(onChanged: callback);

            Assert.That(element.OnChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnChanged_IsNull()
        {
            var element = new FToggle();

            Assert.That(element.OnChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new FValueKey<string>("tgl");
            var element = new FToggle(key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new FToggle();

            Assert.That(element.GetChildren(), Is.Empty);
        }
    }
}
