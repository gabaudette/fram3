#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FRadioGroupTests
    {
        [Test]
        public void Constructor_SetsOptions()
        {
            var options = new List<string> { "A", "B", "C" };

            var element = new FRadioGroup(options);

            Assert.That(element.Options, Is.EqualTo(options));
        }

        [Test]
        public void Constructor_NullOptions_Throws()
        {
            Assert.That(
                () => new FRadioGroup(null!),
                Throws.ArgumentNullException
            );
        }

        [Test]
        public void Constructor_DefaultSelectedValue_IsNull()
        {
            var element = new FRadioGroup(new List<string> { "A" });

            Assert.That(element.SelectedValue, Is.Null);
        }

        [Test]
        public void Constructor_SetsSelectedValue()
        {
            var element = new FRadioGroup(new List<string> { "A", "B" }, selectedValue: "B");

            Assert.That(element.SelectedValue, Is.EqualTo("B"));
        }

        [Test]
        public void Constructor_SetsOnChanged()
        {
            Action<string> callback = _ => { };

            var element = new FRadioGroup(new List<string> { "A" }, onChanged: callback);

            Assert.That(element.OnChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnChanged_IsNull()
        {
            var element = new FRadioGroup(new List<string> { "A" });

            Assert.That(element.OnChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new FValueKey<string>("rg");
            var element = new FRadioGroup(new List<string> { "A" }, key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new FRadioGroup(new List<string> { "A" });

            Assert.That(element.GetChildren(), Is.Empty);
        }
    }
}
