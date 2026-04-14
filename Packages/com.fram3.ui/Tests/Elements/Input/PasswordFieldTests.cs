#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements.Input;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Input
{
    [TestFixture]
    internal sealed class PasswordFieldTests
    {
        [Test]
        public void Constructor_DefaultValue_IsEmptyString()
        {
            var element = new PasswordField();

            Assert.That(element.Value, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Constructor_NullValue_TreatedAsEmpty()
        {
            var element = new PasswordField(value: null);

            Assert.That(element.Value, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Constructor_SetsValue()
        {
            var element = new PasswordField(value: "secret");

            Assert.That(element.Value, Is.EqualTo("secret"));
        }

        [Test]
        public void Constructor_SetsPlaceholder()
        {
            var element = new PasswordField(placeholder: "Enter password");

            Assert.That(element.Placeholder, Is.EqualTo("Enter password"));
        }

        [Test]
        public void Constructor_DefaultPlaceholder_IsNull()
        {
            var element = new PasswordField();

            Assert.That(element.Placeholder, Is.Null);
        }

        [Test]
        public void Constructor_SetsReadOnly()
        {
            var element = new PasswordField(readOnly: true);

            Assert.That(element.ReadOnly, Is.True);
        }

        [Test]
        public void Constructor_DefaultReadOnly_IsFalse()
        {
            var element = new PasswordField();

            Assert.That(element.ReadOnly, Is.False);
        }

        [Test]
        public void Constructor_SetsOnChanged()
        {
            Action<string> callback = _ => { };

            var element = new PasswordField(onChanged: callback);

            Assert.That(element.OnChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnChanged_IsNull()
        {
            var element = new PasswordField();

            Assert.That(element.OnChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new ValueKey<string>("pw");
            var element = new PasswordField(key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new PasswordField();

            Assert.That(element.GetChildren(), Is.Empty);
        }
    }
}
