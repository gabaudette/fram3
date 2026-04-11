#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FCheckboxTests
    {
        [Test]
        public void Constructor_DefaultValue_IsFalse()
        {
            var element = new FCheckbox();

            Assert.That(element.Value, Is.False);
        }

        [Test]
        public void Constructor_SetsValue()
        {
            var element = new FCheckbox(value: true);

            Assert.That(element.Value, Is.True);
        }

        [Test]
        public void Constructor_SetsLabel()
        {
            var element = new FCheckbox(label: "Accept terms");

            Assert.That(element.Label, Is.EqualTo("Accept terms"));
        }

        [Test]
        public void Constructor_DefaultLabel_IsNull()
        {
            var element = new FCheckbox();

            Assert.That(element.Label, Is.Null);
        }

        [Test]
        public void Constructor_SetsOnChanged()
        {
            Action<bool> callback = _ => { };

            var element = new FCheckbox(onChanged: callback);

            Assert.That(element.OnChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnChanged_IsNull()
        {
            var element = new FCheckbox();

            Assert.That(element.OnChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new FValueKey<string>("chk");
            var element = new FCheckbox(key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new FCheckbox();

            Assert.That(element.GetChildren(), Is.Empty);
        }
    }
}
