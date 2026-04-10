#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FTextFieldTests
    {
        [Test]
        public void Constructor_DefaultValue_IsEmptyString()
        {
            var element = new FTextField();

            Assert.That(element.Value, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Constructor_NullValue_TreatedAsEmpty()
        {
            var element = new FTextField(value: null);

            Assert.That(element.Value, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Constructor_SetsValue()
        {
            var element = new FTextField(value: "hello");

            Assert.That(element.Value, Is.EqualTo("hello"));
        }

        [Test]
        public void Constructor_SetsPlaceholder()
        {
            var element = new FTextField(placeholder: "Enter text...");

            Assert.That(element.Placeholder, Is.EqualTo("Enter text..."));
        }

        [Test]
        public void Constructor_DefaultPlaceholder_IsNull()
        {
            var element = new FTextField();

            Assert.That(element.Placeholder, Is.Null);
        }

        [Test]
        public void Constructor_SetsReadOnly()
        {
            var element = new FTextField(readOnly: true);

            Assert.That(element.ReadOnly, Is.True);
        }

        [Test]
        public void Constructor_DefaultReadOnly_IsFalse()
        {
            var element = new FTextField();

            Assert.That(element.ReadOnly, Is.False);
        }

        [Test]
        public void Constructor_SetsMultiline()
        {
            var element = new FTextField(multiline: true);

            Assert.That(element.Multiline, Is.True);
        }

        [Test]
        public void Constructor_DefaultMultiline_IsFalse()
        {
            var element = new FTextField();

            Assert.That(element.Multiline, Is.False);
        }

        [Test]
        public void Constructor_SetsOnChanged()
        {
            Action<string> callback = _ => { };

            var element = new FTextField(onChanged: callback);

            Assert.That(element.OnChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnChanged_IsNull()
        {
            var element = new FTextField();

            Assert.That(element.OnChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new FValueKey<string>("tf");
            var element = new FTextField(key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new FTextField();

            Assert.That(element.GetChildren(), Is.Empty);
        }
    }
}
