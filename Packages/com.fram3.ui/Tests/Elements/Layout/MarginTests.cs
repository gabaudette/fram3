#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Rendering.Internal;
using Fram3.UI.Styling;
using NUnit.Framework;
using UnityEngine.UIElements;

namespace Fram3.UI.Tests.Elements.Layout
{
    [TestFixture]
    internal sealed class MarginTests
    {
        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------

        [Test]
        public void Constructor_StoresMargin()
        {
            var insets = EdgeInsets.All(8f);
            var element = new Margin(insets, new Text("x"));

            Assert.That(element.Insets, Is.EqualTo(insets));
        }

        [Test]
        public void Constructor_StoresChild()
        {
            var child = new Text("hello");
            var element = new Margin(EdgeInsets.Zero, child);

            Assert.That(element.Child, Is.SameAs(child));
        }

        [Test]
        public void Constructor_NullChild_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _ = new Margin(EdgeInsets.Zero, null!));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("m");
            var element = new Margin(EdgeInsets.Zero, new Text("x"), key);

            Assert.That(element.Key, Is.EqualTo(key));
        }

        [Test]
        public void GetChildren_ReturnsSingleChild()
        {
            var child = new Text("x");
            var element = new Margin(EdgeInsets.All(4f), child);

            Assert.That(element.GetChildren(), Has.Count.EqualTo(1));
        }

        [Test]
        public void GetChildren_ContainsCorrectChild()
        {
            var child = new Text("x");
            var element = new Margin(EdgeInsets.All(4f), child);

            Assert.That(element.GetChildren()[0], Is.SameAs(child));
        }

        // -----------------------------------------------------------------------
        // Painter -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_ReturnsVisualElement()
        {
            var element = new Margin(EdgeInsets.All(8f), new Text("x"));

            var native = ElementPainter.CreateNative(element);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_DoesNotThrow()
        {
            var element = new Margin(EdgeInsets.All(8f), new Text("x"));

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element));
        }

        [Test]
        public void Paint_DoesNotThrow()
        {
            var original = new Margin(EdgeInsets.All(8f), new Text("x"));
            var native = ElementPainter.CreateNative(original);
            var updated = new Margin(EdgeInsets.All(16f), new Text("x"));

            Assert.DoesNotThrow(() => ElementPainter.Paint(updated, native));
        }

#if FRAM3_PURE_TESTS
        [Test]
        public void CreateNative_AppliesAllMarginEdges()
        {
            var insets = new EdgeInsets(top: 4f, right: 8f, bottom: 12f, left: 16f);
            var element = new Margin(insets, new Text("x"));

            var native = ElementPainter.CreateNative(element);

            Assert.That(native.style.marginTop, Is.EqualTo(4f).Within(0.0001f));
            Assert.That(native.style.marginRight, Is.EqualTo(8f).Within(0.0001f));
            Assert.That(native.style.marginBottom, Is.EqualTo(12f).Within(0.0001f));
            Assert.That(native.style.marginLeft, Is.EqualTo(16f).Within(0.0001f));
        }

        [Test]
        public void Paint_UpdatesMarginEdges()
        {
            var original = new Margin(EdgeInsets.All(2f), new Text("x"));
            var native = ElementPainter.CreateNative(original);

            var updated = new Margin(new EdgeInsets(top: 10f, right: 20f, bottom: 30f, left: 40f), new Text("x"));
            ElementPainter.Paint(updated, native);

            Assert.That(native.style.marginTop, Is.EqualTo(10f).Within(0.0001f));
            Assert.That(native.style.marginRight, Is.EqualTo(20f).Within(0.0001f));
            Assert.That(native.style.marginBottom, Is.EqualTo(30f).Within(0.0001f));
            Assert.That(native.style.marginLeft, Is.EqualTo(40f).Within(0.0001f));
        }

        [Test]
        public void CreateNative_Symmetric_AppliesCorrectEdges()
        {
            var element = new Margin(EdgeInsets.Symmetric(vertical: 6f, horizontal: 12f), new Text("x"));

            var native = ElementPainter.CreateNative(element);

            Assert.That(native.style.marginTop, Is.EqualTo(6f).Within(0.0001f));
            Assert.That(native.style.marginBottom, Is.EqualTo(6f).Within(0.0001f));
            Assert.That(native.style.marginLeft, Is.EqualTo(12f).Within(0.0001f));
            Assert.That(native.style.marginRight, Is.EqualTo(12f).Within(0.0001f));
        }
#endif
    }
}
