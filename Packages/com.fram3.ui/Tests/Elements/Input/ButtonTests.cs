#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements.Input;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Input
{
    [TestFixture]
    internal sealed class ButtonTests
    {
        [Test]
        public void Constructor_StoresLabel()
        {
            var element = new Button("Click me");

            Assert.That(element.Label, Is.EqualTo("Click me"));
        }

        [Test]
        public void Constructor_NullLabel_StoredAsEmpty()
        {
            var element = new Button(null!);

            Assert.That(element.Label, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Constructor_StoresOnPressed()
        {
            Action handler = () => { };
            var element = new Button("OK", handler);

            Assert.That(element.OnPressed, Is.SameAs(handler));
        }

        [Test]
        public void Constructor_NullOnPressed_IsNull()
        {
            var element = new Button("Cancel");

            Assert.That(element.OnPressed, Is.Null);
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("btn");
            var element = new Button("Go", key: key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}