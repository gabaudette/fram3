#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class FTabViewTests
    {
        private static FTab MakeTab(string label = "Tab")
        {
            return new FTab(label, new FText("content"));
        }

        [Test]
        public void Constructor_SetsTabs()
        {
            var tabs = new List<FTab> { MakeTab("A"), MakeTab("B") };

            var element = new FTabView(tabs);

            Assert.That(element.Tabs, Is.SameAs(tabs));
        }

        [Test]
        public void Constructor_NullTabs_Throws()
        {
            Assert.That(
                () => new FTabView(null!),
                Throws.ArgumentNullException
            );
        }

        [Test]
        public void Constructor_EmptyTabs_Throws()
        {
            Assert.That(
                () => new FTabView(new List<FTab>()),
                Throws.ArgumentException
            );
        }

        [Test]
        public void Constructor_DefaultInitialIndex_IsZero()
        {
            var element = new FTabView(new List<FTab> { MakeTab() });

            Assert.That(element.InitialIndex, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_SetsInitialIndex()
        {
            var element = new FTabView(new List<FTab> { MakeTab("A"), MakeTab("B") }, initialIndex: 1);

            Assert.That(element.InitialIndex, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_SetsOnTabChanged()
        {
            Action<int> callback = _ => { };

            var element = new FTabView(new List<FTab> { MakeTab() }, onTabChanged: callback);

            Assert.That(element.OnTabChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnTabChanged_IsNull()
        {
            var element = new FTabView(new List<FTab> { MakeTab() });

            Assert.That(element.OnTabChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new FValueKey<string>("tv");
            var element = new FTabView(new List<FTab> { MakeTab() }, key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void CreateState_ReturnsNonNullState()
        {
            var element = new FTabView(new List<FTab> { MakeTab() });

            var state = element.CreateState();

            Assert.That(state, Is.Not.Null);
        }

        [Test]
        public void FTab_Constructor_SetsLabel()
        {
            var tab = new FTab("Settings", new FText("x"));

            Assert.That(tab.Label, Is.EqualTo("Settings"));
        }

        [Test]
        public void FTab_Constructor_SetsContent()
        {
            var content = new FText("hello");
            var tab = new FTab("X", content);

            Assert.That(tab.Content, Is.SameAs(content));
        }

        [Test]
        public void FTab_Constructor_NullLabel_Throws()
        {
            Assert.That(
                () => new FTab(null!, new FText("x")),
                Throws.ArgumentNullException
            );
        }

        [Test]
        public void FTab_Constructor_NullContent_Throws()
        {
            Assert.That(
                () => new FTab("X", null!),
                Throws.ArgumentNullException
            );
        }
    }
}
