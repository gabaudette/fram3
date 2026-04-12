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
    internal sealed class FFloatFieldTests
    {
        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------

        [Test]
        public void Constructor_DefaultValue_IsZero()
        {
            var element = new FFloatField();

            Assert.That(element.Value, Is.EqualTo(0f));
        }

        [Test]
        public void Constructor_SetsValue()
        {
            var element = new FFloatField(value: 3.14f);

            Assert.That(element.Value, Is.EqualTo(3.14f).Within(0.0001f));
        }

        [Test]
        public void Constructor_SetsLabel()
        {
            var element = new FFloatField(label: "Scale");

            Assert.That(element.Label, Is.EqualTo("Scale"));
        }

        [Test]
        public void Constructor_DefaultLabel_IsNull()
        {
            var element = new FFloatField();

            Assert.That(element.Label, Is.Null);
        }

        [Test]
        public void Constructor_SetsOnChanged()
        {
            Action<float> callback = _ => { };

            var element = new FFloatField(onChanged: callback);

            Assert.That(element.OnChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnChanged_IsNull()
        {
            var element = new FFloatField();

            Assert.That(element.OnChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new FValueKey<string>("ff");
            var element = new FFloatField(key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new FFloatField();

            Assert.That(element.GetChildren(), Is.Empty);
        }

        // -----------------------------------------------------------------------
        // Painter -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FFloatField_ReturnsFloatField()
        {
            var element = new FFloatField();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native, Is.InstanceOf<FloatField>());
        }

        [Test]
        public void CreateNative_FFloatField_WithNullOnChanged_DoesNotThrow()
        {
            var element = new FFloatField(value: 1.5f);

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        [Test]
        public void CreateNative_FFloatField_WithOnChanged_DoesNotThrow()
        {
            var element = new FFloatField(value: 1.5f, onChanged: _ => { });

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        [Test]
        public void Paint_FFloatField_DoesNotThrow()
        {
            var original = new FFloatField(value: 1f);
            var native = (FloatField)FElementPainter.CreateNative(original);

            var updated = new FFloatField(value: 2f);

            Assert.DoesNotThrow(() => FElementPainter.Paint(updated, native));
        }

#if FRAM3_PURE_TESTS
        [Test]
        public void CreateNative_FFloatField_SetsValue()
        {
            var element = new FFloatField(value: 0.5f);

            var native = (FloatField)FElementPainter.CreateNative(element);

            Assert.That(native.value, Is.EqualTo(0.5f).Within(0.0001f));
        }

        [Test]
        public void CreateNative_FFloatField_SetsLabel()
        {
            var element = new FFloatField(label: "Speed");

            var native = (FloatField)FElementPainter.CreateNative(element);

            Assert.That(native.label, Is.EqualTo("Speed"));
        }

        [Test]
        public void CreateNative_FFloatField_NullLabel_LeavesLabelNull()
        {
            var element = new FFloatField();

            var native = (FloatField)FElementPainter.CreateNative(element);

            Assert.That(native.label, Is.Null);
        }

        [Test]
        public void CreateNative_FFloatField_WithOnChanged_InvokesCallbackOnValueChange()
        {
            float? received = null;
            var element = new FFloatField(onChanged: v => received = v);
            var native = (FloatField)FElementPainter.CreateNative(element);

            native.SimulateValueChanged(2.5f);

            Assert.That(received, Is.EqualTo(2.5f).Within(0.0001f));
        }

        [Test]
        public void Paint_FFloatField_UpdatesValue()
        {
            var original = new FFloatField(value: 1f);
            var native = (FloatField)FElementPainter.CreateNative(original);

            var updated = new FFloatField(value: 9.9f);
            FElementPainter.Paint(updated, native);

            Assert.That(native.value, Is.EqualTo(9.9f).Within(0.0001f));
        }

        [Test]
        public void Paint_FFloatField_UpdatesLabel()
        {
            var original = new FFloatField(label: "old");
            var native = (FloatField)FElementPainter.CreateNative(original);

            var updated = new FFloatField(label: "new");
            FElementPainter.Paint(updated, native);

            Assert.That(native.label, Is.EqualTo("new"));
        }
#endif
    }
}
