#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements.Input;
using Fram3.UI.Rendering.Internal;
using NUnit.Framework;
using UnityEngine.UIElements;

namespace Fram3.UI.Tests.Elements.Input
{
    [TestFixture]
    internal sealed class FMinMaxSliderTests
    {
        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------

        [Test]
        public void Constructor_DefaultValues()
        {
            var element = new FMinMaxSlider();

            Assert.That(element.MinValue, Is.EqualTo(0f));
            Assert.That(element.MaxValue, Is.EqualTo(1f));
            Assert.That(element.LowLimit, Is.EqualTo(0f));
            Assert.That(element.HighLimit, Is.EqualTo(1f));
        }

        [Test]
        public void Constructor_SetsMinAndMaxValues()
        {
            var element = new FMinMaxSlider(minValue: 0.2f, maxValue: 0.8f);

            Assert.That(element.MinValue, Is.EqualTo(0.2f).Within(0.0001f));
            Assert.That(element.MaxValue, Is.EqualTo(0.8f).Within(0.0001f));
        }

        [Test]
        public void Constructor_SetsLowAndHighLimits()
        {
            var element = new FMinMaxSlider(lowLimit: -10f, highLimit: 10f);

            Assert.That(element.LowLimit, Is.EqualTo(-10f).Within(0.0001f));
            Assert.That(element.HighLimit, Is.EqualTo(10f).Within(0.0001f));
        }

        [Test]
        public void Constructor_SetsLabel()
        {
            var element = new FMinMaxSlider(label: "Range");

            Assert.That(element.Label, Is.EqualTo("Range"));
        }

        [Test]
        public void Constructor_DefaultLabel_IsNull()
        {
            var element = new FMinMaxSlider();

            Assert.That(element.Label, Is.Null);
        }

        [Test]
        public void Constructor_SetsOnChanged()
        {
            Action<float, float> callback = (_, _) => { };

            var element = new FMinMaxSlider(onChanged: callback);

            Assert.That(element.OnChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnChanged_IsNull()
        {
            var element = new FMinMaxSlider();

            Assert.That(element.OnChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new FValueKey<string>("mms");
            var element = new FMinMaxSlider(key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new FMinMaxSlider();

            Assert.That(element.GetChildren(), Is.Empty);
        }

        // -----------------------------------------------------------------------
        // Painter -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FMinMaxSlider_ReturnsMinMaxSlider()
        {
            var element = new FMinMaxSlider();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native, Is.InstanceOf<MinMaxSlider>());
        }

        [Test]
        public void CreateNative_FMinMaxSlider_WithNullOnChanged_DoesNotThrow()
        {
            var element = new FMinMaxSlider(minValue: 0.1f, maxValue: 0.9f);

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        [Test]
        public void CreateNative_FMinMaxSlider_WithOnChanged_DoesNotThrow()
        {
            var element = new FMinMaxSlider(onChanged: (_, _) => { });

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        [Test]
        public void Paint_FMinMaxSlider_DoesNotThrow()
        {
            var original = new FMinMaxSlider(minValue: 0f, maxValue: 1f);
            var native = (MinMaxSlider)FElementPainter.CreateNative(original);

            var updated = new FMinMaxSlider(minValue: 0.2f, maxValue: 0.8f);

            Assert.DoesNotThrow(() => FElementPainter.Paint(updated, native));
        }

#if FRAM3_PURE_TESTS
        [Test]
        public void CreateNative_FMinMaxSlider_SetsMinAndMaxValues()
        {
            var element = new FMinMaxSlider(minValue: 0.25f, maxValue: 0.75f);

            var native = (MinMaxSlider)FElementPainter.CreateNative(element);

            Assert.That(native.minValue, Is.EqualTo(0.25f).Within(0.0001f));
            Assert.That(native.maxValue, Is.EqualTo(0.75f).Within(0.0001f));
        }

        [Test]
        public void CreateNative_FMinMaxSlider_SetsLimits()
        {
            var element = new FMinMaxSlider(lowLimit: -5f, highLimit: 5f);

            var native = (MinMaxSlider)FElementPainter.CreateNative(element);

            Assert.That(native.lowLimit, Is.EqualTo(-5f).Within(0.0001f));
            Assert.That(native.highLimit, Is.EqualTo(5f).Within(0.0001f));
        }

        [Test]
        public void CreateNative_FMinMaxSlider_SetsLabel()
        {
            var element = new FMinMaxSlider(label: "Threshold");

            var native = (MinMaxSlider)FElementPainter.CreateNative(element);

            Assert.That(native.label, Is.EqualTo("Threshold"));
        }

        [Test]
        public void CreateNative_FMinMaxSlider_NullLabel_LeavesLabelNull()
        {
            var element = new FMinMaxSlider();

            var native = (MinMaxSlider)FElementPainter.CreateNative(element);

            Assert.That(native.label, Is.Null);
        }

        [Test]
        public void CreateNative_FMinMaxSlider_WithOnChanged_InvokesCallbackOnValueChange()
        {
            float receivedMin = -1f;
            float receivedMax = -1f;
            var element = new FMinMaxSlider(onChanged: (mn, mx) => { receivedMin = mn; receivedMax = mx; });
            var native = (MinMaxSlider)FElementPainter.CreateNative(element);

            native.SimulateValueChanged(0.3f, 0.7f);

            Assert.That(receivedMin, Is.EqualTo(0.3f).Within(0.0001f));
            Assert.That(receivedMax, Is.EqualTo(0.7f).Within(0.0001f));
        }

        [Test]
        public void Paint_FMinMaxSlider_UpdatesMinAndMaxValues()
        {
            var original = new FMinMaxSlider(minValue: 0f, maxValue: 1f);
            var native = (MinMaxSlider)FElementPainter.CreateNative(original);

            var updated = new FMinMaxSlider(minValue: 0.4f, maxValue: 0.6f);
            FElementPainter.Paint(updated, native);

            Assert.That(native.minValue, Is.EqualTo(0.4f).Within(0.0001f));
            Assert.That(native.maxValue, Is.EqualTo(0.6f).Within(0.0001f));
        }

        [Test]
        public void Paint_FMinMaxSlider_UpdatesLimits()
        {
            var original = new FMinMaxSlider(lowLimit: 0f, highLimit: 1f);
            var native = (MinMaxSlider)FElementPainter.CreateNative(original);

            var updated = new FMinMaxSlider(lowLimit: -100f, highLimit: 100f);
            FElementPainter.Paint(updated, native);

            Assert.That(native.lowLimit, Is.EqualTo(-100f).Within(0.0001f));
            Assert.That(native.highLimit, Is.EqualTo(100f).Within(0.0001f));
        }

        [Test]
        public void Paint_FMinMaxSlider_UpdatesLabel()
        {
            var original = new FMinMaxSlider(label: "old");
            var native = (MinMaxSlider)FElementPainter.CreateNative(original);

            var updated = new FMinMaxSlider(label: "new");
            FElementPainter.Paint(updated, native);

            Assert.That(native.label, Is.EqualTo("new"));
        }
#endif
    }
}
