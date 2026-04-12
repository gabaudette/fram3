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
    internal sealed class FMarginTests
    {
        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------

        [Test]
        public void Constructor_StoresMargin()
        {
            var insets = FEdgeInsets.All(8f);
            var element = new FMargin(insets, new FText("x"));

            Assert.That(element.Margin, Is.EqualTo(insets));
        }

        [Test]
        public void Constructor_StoresChild()
        {
            var child = new FText("hello");
            var element = new FMargin(FEdgeInsets.Zero, child);

            Assert.That(element.Child, Is.SameAs(child));
        }

        [Test]
        public void Constructor_NullChild_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _ = new FMargin(FEdgeInsets.Zero, null!));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new FValueKey<string>("m");
            var element = new FMargin(FEdgeInsets.Zero, new FText("x"), key);

            Assert.That(element.Key, Is.EqualTo(key));
        }

        [Test]
        public void GetChildren_ReturnsSingleChild()
        {
            var child = new FText("x");
            var element = new FMargin(FEdgeInsets.All(4f), child);

            Assert.That(element.GetChildren(), Has.Count.EqualTo(1));
        }

        [Test]
        public void GetChildren_ContainsCorrectChild()
        {
            var child = new FText("x");
            var element = new FMargin(FEdgeInsets.All(4f), child);

            Assert.That(element.GetChildren()[0], Is.SameAs(child));
        }

        // -----------------------------------------------------------------------
        // Painter -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_ReturnsVisualElement()
        {
            var element = new FMargin(FEdgeInsets.All(8f), new FText("x"));

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_DoesNotThrow()
        {
            var element = new FMargin(FEdgeInsets.All(8f), new FText("x"));

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        [Test]
        public void Paint_DoesNotThrow()
        {
            var original = new FMargin(FEdgeInsets.All(8f), new FText("x"));
            var native = FElementPainter.CreateNative(original);
            var updated = new FMargin(FEdgeInsets.All(16f), new FText("x"));

            Assert.DoesNotThrow(() => FElementPainter.Paint(updated, native));
        }

#if FRAM3_PURE_TESTS
        [Test]
        public void CreateNative_AppliesAllMarginEdges()
        {
            var insets = new FEdgeInsets(top: 4f, right: 8f, bottom: 12f, left: 16f);
            var element = new FMargin(insets, new FText("x"));

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.marginTop, Is.EqualTo(4f).Within(0.0001f));
            Assert.That(native.style.marginRight, Is.EqualTo(8f).Within(0.0001f));
            Assert.That(native.style.marginBottom, Is.EqualTo(12f).Within(0.0001f));
            Assert.That(native.style.marginLeft, Is.EqualTo(16f).Within(0.0001f));
        }

        [Test]
        public void Paint_UpdatesMarginEdges()
        {
            var original = new FMargin(FEdgeInsets.All(2f), new FText("x"));
            var native = FElementPainter.CreateNative(original);

            var updated = new FMargin(new FEdgeInsets(top: 10f, right: 20f, bottom: 30f, left: 40f), new FText("x"));
            FElementPainter.Paint(updated, native);

            Assert.That(native.style.marginTop, Is.EqualTo(10f).Within(0.0001f));
            Assert.That(native.style.marginRight, Is.EqualTo(20f).Within(0.0001f));
            Assert.That(native.style.marginBottom, Is.EqualTo(30f).Within(0.0001f));
            Assert.That(native.style.marginLeft, Is.EqualTo(40f).Within(0.0001f));
        }

        [Test]
        public void CreateNative_Symmetric_AppliesCorrectEdges()
        {
            var element = new FMargin(FEdgeInsets.Symmetric(vertical: 6f, horizontal: 12f), new FText("x"));

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.marginTop, Is.EqualTo(6f).Within(0.0001f));
            Assert.That(native.style.marginBottom, Is.EqualTo(6f).Within(0.0001f));
            Assert.That(native.style.marginLeft, Is.EqualTo(12f).Within(0.0001f));
            Assert.That(native.style.marginRight, Is.EqualTo(12f).Within(0.0001f));
        }
#endif
    }
}
