#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Rendering.Internal;
using NUnit.Framework;
using UnityEngine.UIElements;
using UiWrap = UnityEngine.UIElements.Wrap;
using Wrap = Fram3.UI.Elements.Layout.Wrap;

namespace Fram3.UI.Tests.Elements.Layout
{
    [TestFixture]
    internal sealed class WrapTests
    {
        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new ValueKey<string>("wrap");
            var element = new Wrap(key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_WithNoChildren_Throws()
        {
            var element = new Wrap();

            Assert.Throws<System.InvalidOperationException>(() => element.GetChildren());
        }

        [Test]
        public void Children_CanBeSet()
        {
            var child = new Text("hello");
            var element = new Wrap { Children = new Element[] { child } };

            Assert.That(element.GetChildren(), Has.Count.EqualTo(1));
        }

        // -----------------------------------------------------------------------
        // Painter -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FWrap_ReturnsVisualElement()
        {
            var element = new Wrap();

            var native = ElementPainter.CreateNative(element);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FWrap_DoesNotThrow()
        {
            var element = new Wrap();

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element));
        }

        [Test]
        public void Paint_FWrap_DoesNotThrow()
        {
            var original = new Wrap();
            var native = ElementPainter.CreateNative(original);

            Assert.DoesNotThrow(() => ElementPainter.Paint(new Wrap(), native));
        }

#if FRAM3_PURE_TESTS
        [Test]
        public void CreateNative_FWrap_SetsFlexDirectionRow()
        {
            var element = new Wrap();

            var native = ElementPainter.CreateNative(element);

            Assert.That(native.style.flexDirection, Is.EqualTo(FlexDirection.Row));
        }

        [Test]
        public void CreateNative_FWrap_SetsFlexWrapWrap()
        {
            var element = new Wrap();

            var native = ElementPainter.CreateNative(element);

            Assert.That(native.style.flexWrap, Is.EqualTo(UiWrap.Wrap));
        }
#endif
    }
}
