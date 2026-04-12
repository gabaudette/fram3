#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class FListViewTests
    {
        [Test]
        public void Constructor_NullItems_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FListView<string>(null!, _ => new FText("x")));
        }

        [Test]
        public void Constructor_NullItemBuilder_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FListView<string>(new string[] { "a" }, null!));
        }

        [Test]
        public void Constructor_SetsItems()
        {
            var items = new string[] { "a", "b", "c" };
            var element = new FListView<string>(items, _ => new FText("x"));

            Assert.That(element.Items, Is.SameAs(items));
        }

        [Test]
        public void Constructor_SetsItemBuilder()
        {
            Func<string, FElement> builder = _ => new FText("x");
            var element = new FListView<string>(new string[] { "a" }, builder);

            Assert.That(element.ItemBuilder, Is.SameAs(builder));
        }

        [Test]
        public void Constructor_DefaultItemHeight_Is32()
        {
            var element = new FListView<string>(new string[] { "a" }, _ => new FText("x"));

            Assert.That(element.ItemHeight, Is.EqualTo(32f).Within(0.0001f));
        }

        [Test]
        public void Constructor_SetsItemHeight()
        {
            var element = new FListView<string>(new string[] { "a" }, _ => new FText("x"), itemHeight: 48f);

            Assert.That(element.ItemHeight, Is.EqualTo(48f).Within(0.0001f));
        }

        [Test]
        public void Constructor_DefaultSelectionMode_IsNone()
        {
            var element = new FListView<string>(new string[] { "a" }, _ => new FText("x"));

            Assert.That(element.SelectionMode, Is.EqualTo(FListSelectionMode.None));
        }

        [Test]
        public void Constructor_SetsSelectionMode()
        {
            var element = new FListView<string>(
                new string[] { "a" },
                _ => new FText("x"),
                selectionMode: FListSelectionMode.Multiple);

            Assert.That(element.SelectionMode, Is.EqualTo(FListSelectionMode.Multiple));
        }

        [Test]
        public void Constructor_DefaultOnSelectionChanged_IsNull()
        {
            var element = new FListView<string>(new string[] { "a" }, _ => new FText("x"));

            Assert.That(element.OnSelectionChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsOnSelectionChanged()
        {
            Action<IReadOnlyList<string>> callback = _ => { };
            var element = new FListView<string>(
                new string[] { "a" },
                _ => new FText("x"),
                onSelectionChanged: callback);

            Assert.That(element.OnSelectionChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new FValueKey<string>("list");
            var element = new FListView<string>(new string[] { "a" }, _ => new FText("x"), key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new FListView<string>(new string[] { "a" }, _ => new FText("x"));

            Assert.That(element.GetChildren(), Is.Empty);
        }

        [Test]
        public void ItemBuilder_ProducesElementForEachItem()
        {
            var items = new string[] { "hello", "world" };
            var element = new FListView<string>(items, s => new FText(s));

            var first = (FText)element.ItemBuilder(items[0]);
            var second = (FText)element.ItemBuilder(items[1]);

            Assert.That(first.Text, Is.EqualTo("hello"));
            Assert.That(second.Text, Is.EqualTo("world"));
        }
    }
}
