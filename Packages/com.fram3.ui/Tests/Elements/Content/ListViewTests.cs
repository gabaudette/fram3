#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class ListViewTests
    {
        [Test]
        public void Constructor_NullItems_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ListView<string>(null!, _ => new Text("x")));
        }

        [Test]
        public void Constructor_NullItemBuilder_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ListView<string>(new string[] { "a" }, null!));
        }

        [Test]
        public void Constructor_SetsItems()
        {
            var items = new string[] { "a", "b", "c" };
            var element = new ListView<string>(items, _ => new Text("x"));

            Assert.That(element.Items, Is.SameAs(items));
        }

        [Test]
        public void Constructor_SetsItemBuilder()
        {
            Func<string, Element> builder = _ => new Text("x");
            var element = new ListView<string>(new string[] { "a" }, builder);

            Assert.That(element.ItemBuilder, Is.SameAs(builder));
        }

        [Test]
        public void Constructor_DefaultItemHeight_Is32()
        {
            var element = new ListView<string>(new string[] { "a" }, _ => new Text("x"));

            Assert.That(element.ItemHeight, Is.EqualTo(32f).Within(0.0001f));
        }

        [Test]
        public void Constructor_SetsItemHeight()
        {
            var element = new ListView<string>(new string[] { "a" }, _ => new Text("x"), itemHeight: 48f);

            Assert.That(element.ItemHeight, Is.EqualTo(48f).Within(0.0001f));
        }

        [Test]
        public void Constructor_DefaultSelectionMode_IsNone()
        {
            var element = new ListView<string>(new string[] { "a" }, _ => new Text("x"));

            Assert.That(element.SelectionMode, Is.EqualTo(ListSelectionMode.None));
        }

        [Test]
        public void Constructor_SetsSelectionMode()
        {
            var element = new ListView<string>(
                new string[] { "a" },
                _ => new Text("x"),
                selectionMode: ListSelectionMode.Multiple);

            Assert.That(element.SelectionMode, Is.EqualTo(ListSelectionMode.Multiple));
        }

        [Test]
        public void Constructor_DefaultOnSelectionChanged_IsNull()
        {
            var element = new ListView<string>(new string[] { "a" }, _ => new Text("x"));

            Assert.That(element.OnSelectionChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsOnSelectionChanged()
        {
            Action<IReadOnlyList<string>> callback = _ => { };
            var element = new ListView<string>(
                new string[] { "a" },
                _ => new Text("x"),
                onSelectionChanged: callback);

            Assert.That(element.OnSelectionChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new ValueKey<string>("list");
            var element = new ListView<string>(new string[] { "a" }, _ => new Text("x"), key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new ListView<string>(new string[] { "a" }, _ => new Text("x"));

            Assert.That(element.GetChildren(), Is.Empty);
        }

        [Test]
        public void ItemBuilder_ProducesElementForEachItem()
        {
            var items = new string[] { "hello", "world" };
            var element = new ListView<string>(items, s => new Text(s));

            var first = (Text)element.ItemBuilder(items[0]);
            var second = (Text)element.ItemBuilder(items[1]);

            Assert.That(first.Content, Is.EqualTo("hello"));
            Assert.That(second.Content, Is.EqualTo("world"));
        }
    }
}
