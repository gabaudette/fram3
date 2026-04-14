#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Input;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Input
{
    [TestFixture]
    internal sealed class DropdownTests
    {
        private static readonly IReadOnlyList<string> SomeOptions = new[] { "A", "B", "C" };

        [Test]
        public void Constructor_SetsOptions()
        {
            var element = new Dropdown(SomeOptions);

            Assert.That(element.Options, Is.SameAs(SomeOptions));
        }

        [Test]
        public void Constructor_NullOptions_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Dropdown(null!));
        }

        [Test]
        public void Constructor_DefaultSelectedIndex_IsMinusOne()
        {
            var element = new Dropdown(SomeOptions);

            Assert.That(element.SelectedIndex, Is.EqualTo(-1));
        }

        [Test]
        public void Constructor_SetsSelectedIndex()
        {
            var element = new Dropdown(SomeOptions, selectedIndex: 1);

            Assert.That(element.SelectedIndex, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_SetsLabel()
        {
            var element = new Dropdown(SomeOptions, label: "Mode");

            Assert.That(element.Label, Is.EqualTo("Mode"));
        }

        [Test]
        public void Constructor_DefaultLabel_IsNull()
        {
            var element = new Dropdown(SomeOptions);

            Assert.That(element.Label, Is.Null);
        }

        [Test]
        public void Constructor_SetsOnChanged()
        {
            Action<int> callback = _ => { };

            var element = new Dropdown(SomeOptions, onChanged: callback);

            Assert.That(element.OnChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnChanged_IsNull()
        {
            var element = new Dropdown(SomeOptions);

            Assert.That(element.OnChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new ValueKey<string>("dd");
            var element = new Dropdown(SomeOptions, key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new Dropdown(SomeOptions);

            Assert.That(element.GetChildren(), Is.Empty);
        }
    }
}
