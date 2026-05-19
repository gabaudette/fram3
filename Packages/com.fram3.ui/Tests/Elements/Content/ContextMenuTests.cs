#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Content;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class ContextMenuTests
    {
        private static IReadOnlyList<ContextMenuItem> OneItem() =>
            new[] { new ContextMenuItem("Action", () => { }) };

        // ContextMenuItem

        [Test]
        public void ContextMenuItem_StoresLabel()
        {
            var item = new ContextMenuItem("Copy", () => { });

            Assert.That(item.Label, Is.EqualTo("Copy"));
        }

        [Test]
        public void ContextMenuItem_StoresOnTap()
        {
            Action cb = () => { };
            var item = new ContextMenuItem("Copy", cb);

            Assert.That(item.OnTap, Is.SameAs(cb));
        }

        [Test]
        public void ContextMenuItem_DefaultDisabled_IsFalse()
        {
            var item = new ContextMenuItem("Copy", () => { });

            Assert.That(item.Disabled, Is.False);
        }

        [Test]
        public void ContextMenuItem_StoresDisabled_True()
        {
            var item = new ContextMenuItem("Locked", () => { }, disabled: true);

            Assert.That(item.Disabled, Is.True);
        }

        [Test]
        public void ContextMenuItem_NullLabel_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new ContextMenuItem(null!, () => { }));
        }

        [Test]
        public void ContextMenuItem_NullOnTap_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new ContextMenuItem("Copy", null!));
        }

        // ContextMenu

        [Test]
        public void Constructor_StoresItems()
        {
            var items = new[]
            {
                new ContextMenuItem("Cut", () => { }),
                new ContextMenuItem("Copy", () => { })
            };
            var menu = new ContextMenu(items, 100f, 200f, () => { });

            Assert.That(menu.Items, Has.Count.EqualTo(2));
        }

        [Test]
        public void Constructor_StoresX()
        {
            var menu = new ContextMenu(OneItem(), 42f, 0f, () => { });

            Assert.That(menu.X, Is.EqualTo(42f));
        }

        [Test]
        public void Constructor_StoresY()
        {
            var menu = new ContextMenu(OneItem(), 0f, 99f, () => { });

            Assert.That(menu.Y, Is.EqualTo(99f));
        }

        [Test]
        public void Constructor_StoresOnDismiss()
        {
            Action cb = () => { };
            var menu = new ContextMenu(OneItem(), 0f, 0f, cb);

            Assert.That(menu.OnDismiss, Is.SameAs(cb));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("ctx");
            var menu = new ContextMenu(OneItem(), 0f, 0f, () => { }, key: key);

            Assert.That(menu.Key, Is.EqualTo(key));
        }

        [Test]
        public void Constructor_NullItems_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _ = new ContextMenu(null!, 0f, 0f, () => { }));
        }

        [Test]
        public void Constructor_EmptyItems_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                _ = new ContextMenu(Array.Empty<ContextMenuItem>(), 0f, 0f, () => { }));
        }

        [Test]
        public void Constructor_NullOnDismiss_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _ = new ContextMenu(OneItem(), 0f, 0f, null!));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var menu = new ContextMenu(OneItem(), 0f, 0f, () => { });

            Assert.That(menu.GetChildren(), Is.Empty);
        }

        [Test]
        public void Mounts_WithinTree_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var menu = new ContextMenu(OneItem(), 0f, 0f, () => { });

            Assert.DoesNotThrow(() => expander.Mount(menu, null));
        }

        [Test]
        public void Mounts_WithMultipleItems_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var items = new[]
            {
                new ContextMenuItem("Attack", () => { }),
                new ContextMenuItem("Defend", () => { }),
                new ContextMenuItem("Flee", () => { }, disabled: true)
            };
            var menu = new ContextMenu(items, 300f, 150f, () => { });

            Assert.DoesNotThrow(() => expander.Mount(menu, null));
        }
    }
}
