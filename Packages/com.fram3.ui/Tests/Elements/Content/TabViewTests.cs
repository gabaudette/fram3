#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class TabViewTests
    {
        private static Tab MakeTab(string label = "Tab")
        {
            return new Tab(label, new Text("content"));
        }

        [Test]
        public void Constructor_SetsTabs()
        {
            var tabs = new List<Tab> { MakeTab("A"), MakeTab("B") };

            var element = new TabView(tabs);

            Assert.That(element.Tabs, Is.SameAs(tabs));
        }

        [Test]
        public void Constructor_NullTabs_Throws()
        {
            Assert.That(
                () => new TabView(null!),
                Throws.ArgumentNullException
            );
        }

        [Test]
        public void Constructor_EmptyTabs_Throws()
        {
            Assert.That(
                () => new TabView(new List<Tab>()),
                Throws.ArgumentException
            );
        }

        [Test]
        public void Constructor_DefaultInitialIndex_IsZero()
        {
            var element = new TabView(new List<Tab> { MakeTab() });

            Assert.That(element.InitialIndex, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_SetsInitialIndex()
        {
            var element = new TabView(new List<Tab> { MakeTab("A"), MakeTab("B") }, initialIndex: 1);

            Assert.That(element.InitialIndex, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_SetsOnTabChanged()
        {
            Action<int> callback = _ => { };

            var element = new TabView(new List<Tab> { MakeTab() }, onTabChanged: callback);

            Assert.That(element.OnTabChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnTabChanged_IsNull()
        {
            var element = new TabView(new List<Tab> { MakeTab() });

            Assert.That(element.OnTabChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new ValueKey<string>("tv");
            var element = new TabView(new List<Tab> { MakeTab() }, key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void CreateState_ReturnsNonNullState()
        {
            var element = new TabView(new List<Tab> { MakeTab() });

            var state = element.CreateState();

            Assert.That(state, Is.Not.Null);
        }

        [Test]
        public void FTab_Constructor_SetsLabel()
        {
            var tab = new Tab("Settings", new Text("x"));

            Assert.That(tab.Label, Is.EqualTo("Settings"));
        }

        [Test]
        public void FTab_Constructor_SetsContent()
        {
            var content = new Text("hello");
            var tab = new Tab("X", content);

            Assert.That(tab.Content, Is.SameAs(content));
        }

        [Test]
        public void FTab_Constructor_NullLabel_Throws()
        {
            Assert.That(
                () => new Tab(null!, new Text("x")),
                Throws.ArgumentNullException
            );
        }

        [Test]
        public void FTab_Constructor_NullContent_Throws()
        {
            Assert.That(
                () => new Tab("X", null!),
                Throws.ArgumentNullException
            );
        }
    }
}
