#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Content;
using Fram3.UI.Styling;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class CardTests
    {
        // --- Constructor: content ---

        [Test]
        public void Constructor_NullContent_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Card(null!));
        }

        [Test]
        public void Constructor_StoresContent()
        {
            var content = new TestLeafElement("body");
            var card = new Card(content);

            Assert.That(card.Content, Is.SameAs(content));
        }

        // --- Constructor: header ---

        [Test]
        public void Constructor_DefaultHeader_IsNull()
        {
            var card = new Card(new TestLeafElement("body"));

            Assert.That(card.Header, Is.Null);
        }

        [Test]
        public void Constructor_StoresHeader()
        {
            var header = new TestLeafElement("header");
            var card = new Card(new TestLeafElement("body"), header: header);

            Assert.That(card.Header, Is.SameAs(header));
        }

        // --- Constructor: footer ---

        [Test]
        public void Constructor_DefaultFooter_IsNull()
        {
            var card = new Card(new TestLeafElement("body"));

            Assert.That(card.Footer, Is.Null);
        }

        [Test]
        public void Constructor_StoresFooter()
        {
            var footer = new TestLeafElement("footer");
            var card = new Card(new TestLeafElement("body"), footer: footer);

            Assert.That(card.Footer, Is.SameAs(footer));
        }

        // --- Constructor: outlined ---

        [Test]
        public void Constructor_DefaultOutlined_IsFalse()
        {
            var card = new Card(new TestLeafElement("body"));

            Assert.That(card.Outlined, Is.False);
        }

        [Test]
        public void Constructor_StoresOutlined()
        {
            var card = new Card(new TestLeafElement("body"), outlined: true);

            Assert.That(card.Outlined, Is.True);
        }

        // --- GetChildren ---

        [Test]
        public void GetChildren_IsEmpty()
        {
            var card = new Card(new TestLeafElement("body"));

            Assert.That(card.GetChildren(), Is.Empty);
        }

        // --- Mount / Build ---

        [Test]
        public void Mount_ContentOnly_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var card = new Card(new TestLeafElement("body"));

            Assert.DoesNotThrow(() => expander.Mount(card, null));
        }

        [Test]
        public void Build_ContentOnly_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var card = new Card(new TestLeafElement("body"));
            var node = expander.Mount(card, null);

            var built = card.Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_WithHeader_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var card = new Card(
                new TestLeafElement("body"),
                header: new TestLeafElement("header")
            );

            Assert.DoesNotThrow(() => expander.Mount(card, null));
        }

        [Test]
        public void Build_WithHeader_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var card = new Card(
                new TestLeafElement("body"),
                header: new TestLeafElement("header")
            );
            var node = expander.Mount(card, null);

            var built = card.Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_WithFooter_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var card = new Card(
                new TestLeafElement("body"),
                footer: new TestLeafElement("footer")
            );

            Assert.DoesNotThrow(() => expander.Mount(card, null));
        }

        [Test]
        public void Build_WithFooter_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var card = new Card(
                new TestLeafElement("body"),
                footer: new TestLeafElement("footer")
            );
            var node = expander.Mount(card, null);

            var built = card.Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_WithHeaderAndFooter_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var card = new Card(
                new TestLeafElement("body"),
                header: new TestLeafElement("header"),
                footer: new TestLeafElement("footer")
            );

            Assert.DoesNotThrow(() => expander.Mount(card, null));
        }

        [Test]
        public void Build_WithHeaderAndFooter_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var card = new Card(
                new TestLeafElement("body"),
                header: new TestLeafElement("header"),
                footer: new TestLeafElement("footer")
            );
            var node = expander.Mount(card, null);

            var built = card.Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_Outlined_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var card = new Card(new TestLeafElement("body"), outlined: true);

            Assert.DoesNotThrow(() => expander.Mount(card, null));
        }

        [Test]
        public void Build_Outlined_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var card = new Card(new TestLeafElement("body"), outlined: true);
            var node = expander.Mount(card, null);

            var built = card.Build(node.Context);

            Assert.That(built, Is.Not.Null);
        }

        // --- DidUpdateElement / Rebuild ---

        [Test]
        public void DidUpdateElement_ContentOnly_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var card = new Card(new TestLeafElement("body"));
            var node = expander.Mount(card, null);
            var updated = new Card(new TestLeafElement("body2"));

            expander.UpdateElement(node, updated);

            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        [Test]
        public void DidUpdateElement_ToggleOutlined_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var card = new Card(new TestLeafElement("body"));
            var node = expander.Mount(card, null);
            var updated = new Card(new TestLeafElement("body"), outlined: true);

            expander.UpdateElement(node, updated);

            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        [Test]
        public void DidUpdateElement_AddHeader_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var card = new Card(new TestLeafElement("body"));
            var node = expander.Mount(card, null);
            var updated = new Card(
                new TestLeafElement("body"),
                header: new TestLeafElement("header")
            );

            expander.UpdateElement(node, updated);

            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        // --- Unmount ---

        [Test]
        public void Unmount_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var card = new Card(new TestLeafElement("body"));
            var node = expander.Mount(card, null);

            Assert.DoesNotThrow(() => expander.Unmount(node));
        }
    }
}
