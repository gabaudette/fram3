#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class FSnackbarTests
    {
        [Test]
        public void Constructor_SetsMessage()
        {
            var element = new FSnackbar("Saved successfully");

            Assert.That(element.Message, Is.EqualTo("Saved successfully"));
        }

        [Test]
        public void Constructor_NullMessage_Throws()
        {
            Assert.That(
                () => new FSnackbar(null!),
                Throws.ArgumentNullException
            );
        }

        [Test]
        public void Constructor_DefaultActionLabel_IsNull()
        {
            var element = new FSnackbar("msg");

            Assert.That(element.ActionLabel, Is.Null);
        }

        [Test]
        public void Constructor_SetsActionLabel()
        {
            var element = new FSnackbar("msg", actionLabel: "Undo");

            Assert.That(element.ActionLabel, Is.EqualTo("Undo"));
        }

        [Test]
        public void Constructor_SetsOnAction()
        {
            Action callback = () => { };

            var element = new FSnackbar("msg", onAction: callback);

            Assert.That(element.OnAction, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnAction_IsNull()
        {
            var element = new FSnackbar("msg");

            Assert.That(element.OnAction, Is.Null);
        }

        [Test]
        public void Constructor_DefaultDuration_IsFourSeconds()
        {
            var element = new FSnackbar("msg");

            Assert.That(element.Duration, Is.EqualTo(4f));
        }

        [Test]
        public void Constructor_SetsDuration()
        {
            var element = new FSnackbar("msg", duration: 8f);

            Assert.That(element.Duration, Is.EqualTo(8f));
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new FValueKey<string>("sb");
            var element = new FSnackbar("msg", key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new FSnackbar("msg");

            Assert.That(element.GetChildren(), Is.Empty);
        }
    }
}
