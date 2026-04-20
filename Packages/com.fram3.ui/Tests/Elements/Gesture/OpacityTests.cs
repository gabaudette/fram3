#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Rendering.Internal;
using Fram3.UI.Styling;
using NUnit.Framework;
using UnityEngine.UIElements;

namespace Fram3.UI.Tests.Elements.Gesture
{
    [TestFixture]
    internal sealed class OpacityTests
    {
        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------

        [Test]
        public void Constructor_SetsValue()
        {
            var element = new Opacity(value: 0.5f, child: new Text("x"));

            Assert.That(element.Value, Is.EqualTo(0.5f).Within(0.0001f));
        }

        [Test]
        public void Constructor_ClampsValueAboveOne()
        {
            var element = new Opacity(value: 1.5f, child: new Text("x"));

            Assert.That(element.Value, Is.EqualTo(1f).Within(0.0001f));
        }

        [Test]
        public void Constructor_ClampsValueBelowZero()
        {
            var element = new Opacity(value: -0.5f, child: new Text("x"));

            Assert.That(element.Value, Is.EqualTo(0f).Within(0.0001f));
        }

        [Test]
        public void Constructor_SetsChild()
        {
            var child = new Text("hello");
            var element = new Opacity(value: 1f, child: child);

            Assert.That(element.Child, Is.SameAs(child));
        }

        [Test]
        public void Constructor_NullChild_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _ = new Opacity(value: 1f, child: null!));
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new ValueKey<string>("op");
            var element = new Opacity(value: 0.8f, child: new Text("x"), key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsSingleChild()
        {
            var child = new Text("x");
            var element = new Opacity(value: 0.5f, child: child);

            Assert.That(element.GetChildren(), Has.Count.EqualTo(1));
        }

        // -----------------------------------------------------------------------
        // Painter -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FOpacity_ReturnsVisualElement()
        {
            var element = new Opacity(value: 0.5f, child: new Text("x"));

            var native = ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FOpacity_DoesNotThrow()
        {
            var element = new Opacity(value: 0.7f, child: new Text("x"));

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element, Theme.Default));
        }

        [Test]
        public void Paint_FOpacity_DoesNotThrow()
        {
            var original = new Opacity(value: 0.5f, child: new Text("x"));
            var native = ElementPainter.CreateNative(original, Theme.Default);

            var updated = new Opacity(value: 0.9f, child: new Text("x"));

            Assert.DoesNotThrow(() => ElementPainter.Paint(updated, native, Theme.Default));
        }

#if FRAM3_PURE_TESTS
        [Test]
        public void CreateNative_FOpacity_SetsOpacityStyle()
        {
            var element = new Opacity(value: 0.4f, child: new Text("x"));

            var native = ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.style.opacity, Is.EqualTo(0.4f).Within(0.0001f));
        }

        [Test]
        public void Paint_FOpacity_UpdatesOpacityStyle()
        {
            var original = new Opacity(value: 0.2f, child: new Text("x"));
            var native = ElementPainter.CreateNative(original, Theme.Default);

            var updated = new Opacity(value: 0.8f, child: new Text("x"));
            ElementPainter.Paint(updated, native, Theme.Default);

            Assert.That(native.style.opacity, Is.EqualTo(0.8f).Within(0.0001f));
        }
#endif
    }
}
