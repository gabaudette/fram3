#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FButtonTests
    {
        [Test]
        public void Constructor_StoresLabel()
        {
            var element = new FButton("Click me");

            Assert.That(element.Label, Is.EqualTo("Click me"));
        }

        [Test]
        public void Constructor_NullLabel_StoredAsEmpty()
        {
            var element = new FButton(null!);

            Assert.That(element.Label, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Constructor_StoresOnPressed()
        {
            Action handler = () => { };
            var element = new FButton("OK", handler);

            Assert.That(element.OnPressed, Is.SameAs(handler));
        }

        [Test]
        public void Constructor_NullOnPressed_IsNull()
        {
            var element = new FButton("Cancel");

            Assert.That(element.OnPressed, Is.Null);
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new FValueKey<string>("btn");
            var element = new FButton("Go", key: key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}