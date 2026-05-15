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
    internal sealed class AlertTests
    {
        [Test]
        public void Constructor_StoresTitle()
        {
            var alert = new Alert("System error");

            Assert.That(alert.Title, Is.EqualTo("System error"));
        }

        [Test]
        public void Constructor_NullTitle_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new Alert(null!));
        }

        [Test]
        public void Constructor_DefaultMessage_IsNull()
        {
            var alert = new Alert("Info");

            Assert.That(alert.Message, Is.Null);
        }

        [Test]
        public void Constructor_StoresMessage()
        {
            var alert = new Alert("Info", message: "Details here.");

            Assert.That(alert.Message, Is.EqualTo("Details here."));
        }

        [Test]
        public void Constructor_DefaultSeverity_IsInfo()
        {
            var alert = new Alert("Info");

            Assert.That(alert.Severity, Is.EqualTo(AlertSeverity.Info));
        }

        [Test]
        public void Constructor_StoresSeverity()
        {
            var alert = new Alert("Error", severity: AlertSeverity.Error);

            Assert.That(alert.Severity, Is.EqualTo(AlertSeverity.Error));
        }

        [Test]
        public void Constructor_DefaultActions_IsEmpty()
        {
            var alert = new Alert("Info");

            Assert.That(alert.Actions, Is.Empty);
        }

        [Test]
        public void Constructor_StoresActions()
        {
            var actions = new List<(string Label, Action OnPressed)>
            {
                ("OK", () => { })
            };
            var alert = new Alert("Info", actions: actions);

            Assert.That(alert.Actions, Has.Count.EqualTo(1));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("alert");
            var alert = new Alert("Info", key: key);

            Assert.That(alert.Key, Is.EqualTo(key));
        }

        [Test]
        public void GetChildren_ReturnsNoDirectChildren()
        {
            var alert = new Alert("Info");

            Assert.That(alert.GetChildren(), Has.Count.EqualTo(0));
        }

        [Test]
        public void Mounts_WithinTree_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var alert = new Alert("Info");

            Assert.DoesNotThrow(() => expander.Mount(alert, null));
        }

        [Test]
        public void Mounts_WithMessage_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var alert = new Alert("Warning", message: "Something needs attention.", severity: AlertSeverity.Warning);

            Assert.DoesNotThrow(() => expander.Mount(alert, null));
        }

        [Test]
        public void Mounts_WithActions_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var alert = new Alert(
                "Confirm",
                severity: AlertSeverity.Error,
                actions: new List<(string, Action)> { ("Dismiss", () => { }) }
            );

            Assert.DoesNotThrow(() => expander.Mount(alert, null));
        }

        [Test]
        public void Mounts_AllSeverities_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);

            foreach (AlertSeverity severity in Enum.GetValues(typeof(AlertSeverity)))
            {
                var alert = new Alert("Test", severity: severity);
                Assert.DoesNotThrow(() => expander.Mount(alert, null), $"Severity {severity} threw");
            }
        }
    }
}
