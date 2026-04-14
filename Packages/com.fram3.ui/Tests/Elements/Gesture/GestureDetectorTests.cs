#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Gesture;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Gesture
{
    [TestFixture]
    internal sealed class GestureDetectorTests
    {
        [Test]
        public void Constructor_SetsChild()
        {
            var child = new Text("x");
            var element = new GestureDetector(child);

            Assert.That(element.Child, Is.SameAs(child));
        }

        [Test]
        public void Constructor_NullChild_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new GestureDetector(null!));
        }

        [Test]
        public void Constructor_DefaultCallbacks_AreNull()
        {
            var element = new GestureDetector(new Text("x"));

            Assert.That(element.OnTap, Is.Null);
            Assert.That(element.OnPointerEnter, Is.Null);
            Assert.That(element.OnPointerExit, Is.Null);
        }

        [Test]
        public void Constructor_SetsOnTap()
        {
            Action callback = () => { };
            var element = new GestureDetector(new Text("x"), onTap: callback);

            Assert.That(element.OnTap, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_SetsOnPointerEnter()
        {
            Action callback = () => { };
            var element = new GestureDetector(new Text("x"), onPointerEnter: callback);

            Assert.That(element.OnPointerEnter, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_SetsOnPointerExit()
        {
            Action callback = () => { };
            var element = new GestureDetector(new Text("x"), onPointerExit: callback);

            Assert.That(element.OnPointerExit, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new ValueKey<string>("gest");
            var element = new GestureDetector(new Text("x"), key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsSingleChild()
        {
            var child = new Text("x");
            var element = new GestureDetector(child);

            var children = element.GetChildren();

            Assert.That(children.Count, Is.EqualTo(1));
            Assert.That(children[0], Is.SameAs(child));
        }
    }
}
