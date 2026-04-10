#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FGestureDetectorTests
    {
        [Test]
        public void Constructor_SetsChild()
        {
            var child = new FText("x");
            var element = new FGestureDetector(child);

            Assert.That(element.Child, Is.SameAs(child));
        }

        [Test]
        public void Constructor_NullChild_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new FGestureDetector(null!));
        }

        [Test]
        public void Constructor_DefaultCallbacks_AreNull()
        {
            var element = new FGestureDetector(new FText("x"));

            Assert.That(element.OnTap, Is.Null);
            Assert.That(element.OnPointerEnter, Is.Null);
            Assert.That(element.OnPointerExit, Is.Null);
        }

        [Test]
        public void Constructor_SetsOnTap()
        {
            Action callback = () => { };
            var element = new FGestureDetector(new FText("x"), onTap: callback);

            Assert.That(element.OnTap, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_SetsOnPointerEnter()
        {
            Action callback = () => { };
            var element = new FGestureDetector(new FText("x"), onPointerEnter: callback);

            Assert.That(element.OnPointerEnter, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_SetsOnPointerExit()
        {
            Action callback = () => { };
            var element = new FGestureDetector(new FText("x"), onPointerExit: callback);

            Assert.That(element.OnPointerExit, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new FValueKey<string>("gest");
            var element = new FGestureDetector(new FText("x"), key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsSingleChild()
        {
            var child = new FText("x");
            var element = new FGestureDetector(child);

            var children = element.GetChildren();

            Assert.That(children.Count, Is.EqualTo(1));
            Assert.That(children[0], Is.SameAs(child));
        }
    }
}
