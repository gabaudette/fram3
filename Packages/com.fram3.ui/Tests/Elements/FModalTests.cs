#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FModalTests
    {
        [Test]
        public void Constructor_SetsChild()
        {
            var child = new FText("content");

            var element = new FModal(child);

            Assert.That(element.Child, Is.SameAs(child));
        }

        [Test]
        public void Constructor_NullChild_Throws()
        {
            Assert.That(
                () => new FModal(null!),
                Throws.ArgumentNullException
            );
        }

        [Test]
        public void Constructor_DefaultBarrierDismissible_IsTrue()
        {
            var element = new FModal(new FText("x"));

            Assert.That(element.BarrierDismissible, Is.True);
        }

        [Test]
        public void Constructor_SetsBarrierDismissible_False()
        {
            var element = new FModal(new FText("x"), barrierDismissible: false);

            Assert.That(element.BarrierDismissible, Is.False);
        }

        [Test]
        public void Constructor_SetsOnDismiss()
        {
            Action callback = () => { };

            var element = new FModal(new FText("x"), onDismiss: callback);

            Assert.That(element.OnDismiss, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnDismiss_IsNull()
        {
            var element = new FModal(new FText("x"));

            Assert.That(element.OnDismiss, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new FValueKey<string>("modal");
            var element = new FModal(new FText("x"), key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsSingleChild()
        {
            var child = new FText("content");

            var element = new FModal(child);

            Assert.That(element.GetChildren(), Is.EqualTo(new[] { child }));
        }
    }
}
