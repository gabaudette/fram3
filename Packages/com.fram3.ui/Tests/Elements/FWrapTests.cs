#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements;
using Fram3.UI.Rendering.Internal;
using NUnit.Framework;
using UnityEngine.UIElements;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FWrapTests
    {
        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new FValueKey<string>("wrap");
            var element = new FWrap(key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_WithNoChildren_Throws()
        {
            var element = new FWrap();

            Assert.Throws<System.InvalidOperationException>(() => element.GetChildren());
        }

        [Test]
        public void Children_CanBeSet()
        {
            var child = new FText("hello");
            var element = new FWrap { Children = new FElement[] { child } };

            Assert.That(element.GetChildren(), Has.Count.EqualTo(1));
        }

        // -----------------------------------------------------------------------
        // Painter -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FWrap_ReturnsVisualElement()
        {
            var element = new FWrap();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FWrap_DoesNotThrow()
        {
            var element = new FWrap();

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        [Test]
        public void Paint_FWrap_DoesNotThrow()
        {
            var original = new FWrap();
            var native = FElementPainter.CreateNative(original);

            Assert.DoesNotThrow(() => FElementPainter.Paint(new FWrap(), native));
        }

#if FRAM3_PURE_TESTS
        [Test]
        public void CreateNative_FWrap_SetsFlexDirectionRow()
        {
            var element = new FWrap();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.flexDirection, Is.EqualTo(FlexDirection.Row));
        }

        [Test]
        public void CreateNative_FWrap_SetsFlexWrapWrap()
        {
            var element = new FWrap();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.flexWrap, Is.EqualTo(Wrap.Wrap));
        }
#endif
    }
}
