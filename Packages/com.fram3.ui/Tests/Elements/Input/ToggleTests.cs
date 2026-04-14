#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements.Input;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Input
{
    [TestFixture]
    internal sealed class ToggleTests
    {
        [Test]
        public void Constructor_DefaultValue_IsFalse()
        {
            var element = new FrameToggle();

            Assert.That(element.Value, Is.False);
        }

        [Test]
        public void Constructor_SetsValue()
        {
            var element = new FrameToggle(value: true);

            Assert.That(element.Value, Is.True);
        }

        [Test]
        public void Constructor_SetsLabel()
        {
            var element = new FrameToggle(label: "Enable feature");

            Assert.That(element.Label, Is.EqualTo("Enable feature"));
        }

        [Test]
        public void Constructor_DefaultLabel_IsNull()
        {
            var element = new FrameToggle();

            Assert.That(element.Label, Is.Null);
        }

        [Test]
        public void Constructor_SetsOnChanged()
        {
            Action<bool> callback = _ => { };

            var element = new FrameToggle(onChanged: callback);

            Assert.That(element.OnChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnChanged_IsNull()
        {
            var element = new FrameToggle();

            Assert.That(element.OnChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new ValueKey<string>("tgl");
            var element = new FrameToggle(key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new FrameToggle();

            Assert.That(element.GetChildren(), Is.Empty);
        }
    }
}
