#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements.Input;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Input
{
    [TestFixture]
    internal sealed class CheckboxTests
    {
        [Test]
        public void Constructor_DefaultValue_IsFalse()
        {
            var element = new Checkbox();

            Assert.That(element.Value, Is.False);
        }

        [Test]
        public void Constructor_SetsValue()
        {
            var element = new Checkbox(value: true);

            Assert.That(element.Value, Is.True);
        }

        [Test]
        public void Constructor_SetsLabel()
        {
            var element = new Checkbox(label: "Accept terms");

            Assert.That(element.Label, Is.EqualTo("Accept terms"));
        }

        [Test]
        public void Constructor_DefaultLabel_IsNull()
        {
            var element = new Checkbox();

            Assert.That(element.Label, Is.Null);
        }

        [Test]
        public void Constructor_SetsOnChanged()
        {
            Action<bool> callback = _ => { };

            var element = new Checkbox(onChanged: callback);

            Assert.That(element.OnChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnChanged_IsNull()
        {
            var element = new Checkbox();

            Assert.That(element.OnChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new ValueKey<string>("chk");
            var element = new Checkbox(key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new Checkbox();

            Assert.That(element.GetChildren(), Is.Empty);
        }
    }
}
