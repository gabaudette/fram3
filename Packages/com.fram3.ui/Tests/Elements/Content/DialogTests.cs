#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Content;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class DialogTests
    {
        [Test]
        public void Constructor_StoresTitle()
        {
            var dialog = new Dialog("Confirm Action");

            Assert.That(dialog.Title, Is.EqualTo("Confirm Action"));
        }

        [Test]
        public void Constructor_NullTitle_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new Dialog(null!));
        }

        [Test]
        public void Constructor_DefaultContent_IsNull()
        {
            var dialog = new Dialog("Title");

            Assert.That(dialog.Content, Is.Null);
        }

        [Test]
        public void Constructor_StoresContent()
        {
            var body = new TestLeafElement("body");
            var dialog = new Dialog("Title", content: body);

            Assert.That(dialog.Content, Is.SameAs(body));
        }

        [Test]
        public void Constructor_DefaultActions_IsEmpty()
        {
            var dialog = new Dialog("Title");

            Assert.That(dialog.Actions, Is.Empty);
        }

        [Test]
        public void Constructor_StoresActions()
        {
            var actions = new List<(string Label, Action OnPressed)>
            {
                ("Cancel", () => { }),
                ("Confirm", () => { })
            };
            var dialog = new Dialog("Title", actions: actions);

            Assert.That(dialog.Actions, Has.Count.EqualTo(2));
        }

        [Test]
        public void Constructor_DefaultOnDismiss_IsNull()
        {
            var dialog = new Dialog("Title");

            Assert.That(dialog.OnDismiss, Is.Null);
        }

        [Test]
        public void Constructor_StoresOnDismiss()
        {
            Action callback = () => { };
            var dialog = new Dialog("Title", onDismiss: callback);

            Assert.That(dialog.OnDismiss, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultBarrierDismissible_IsTrue()
        {
            var dialog = new Dialog("Title");

            Assert.That(dialog.BarrierDismissible, Is.True);
        }

        [Test]
        public void Constructor_StoresBarrierDismissible_False()
        {
            var dialog = new Dialog("Title", barrierDismissible: false);

            Assert.That(dialog.BarrierDismissible, Is.False);
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("dialog");
            var dialog = new Dialog("Title", key: key);

            Assert.That(dialog.Key, Is.EqualTo(key));
        }

        [Test]
        public void GetChildren_ReturnsNoDirectChildren()
        {
            var dialog = new Dialog("Title");

            Assert.That(dialog.GetChildren(), Has.Count.EqualTo(0));
        }

        [Test]
        public void Mounts_WithinTree_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var dialog = new Dialog("Title");

            Assert.DoesNotThrow(() => expander.Mount(dialog, null));
        }

        [Test]
        public void Mounts_WithContent_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var dialog = new Dialog("Title", content: new TestLeafElement("body"));

            Assert.DoesNotThrow(() => expander.Mount(dialog, null));
        }

        [Test]
        public void Mounts_WithActions_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var dialog = new Dialog(
                "Delete Character?",
                actions: new List<(string, Action)>
                {
                    ("Cancel", () => { }),
                    ("Delete", () => { })
                }
            );

            Assert.DoesNotThrow(() => expander.Mount(dialog, null));
        }

        [Test]
        public void Mounts_NonDismissible_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var dialog = new Dialog("Title", barrierDismissible: false, onDismiss: () => { });

            Assert.DoesNotThrow(() => expander.Mount(dialog, null));
        }
    }
}
