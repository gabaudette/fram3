#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements.Input;
using Fram3.UI.Rendering.Internal;
using Fram3.UI.Styling;
using NUnit.Framework;
using UnityEngine.UIElements;
using FloatField = Fram3.UI.Elements.Input.FloatField;
using UiFloatField = UnityEngine.UIElements.FloatField;

namespace Fram3.UI.Tests.Elements.Input
{
    [TestFixture]
    internal sealed class FloatFieldTests
    {
        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------

        [Test]
        public void Constructor_DefaultValue_IsZero()
        {
            var element = new FloatField();

            Assert.That(element.Value, Is.EqualTo(0f));
        }

        [Test]
        public void Constructor_SetsValue()
        {
            var element = new FloatField(value: 3.14f);

            Assert.That(element.Value, Is.EqualTo(3.14f).Within(0.0001f));
        }

        [Test]
        public void Constructor_SetsLabel()
        {
            var element = new FloatField(label: "Scale");

            Assert.That(element.Label, Is.EqualTo("Scale"));
        }

        [Test]
        public void Constructor_DefaultLabel_IsNull()
        {
            var element = new FloatField();

            Assert.That(element.Label, Is.Null);
        }

        [Test]
        public void Constructor_SetsOnChanged()
        {
            Action<float> callback = _ => { };

            var element = new FloatField(onChanged: callback);

            Assert.That(element.OnChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnChanged_IsNull()
        {
            var element = new FloatField();

            Assert.That(element.OnChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new ValueKey<string>("ff");
            var element = new FloatField(key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new FloatField();

            Assert.That(element.GetChildren(), Is.Empty);
        }

        // -----------------------------------------------------------------------
        // Painter -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FFloatField_ReturnsFloatField()
        {
            var element = new FloatField();

            var native = ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native, Is.InstanceOf<UiFloatField>());
        }

        [Test]
        public void CreateNative_FFloatField_WithNullOnChanged_DoesNotThrow()
        {
            var element = new FloatField(value: 1.5f);

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element, Theme.Default));
        }

        [Test]
        public void CreateNative_FFloatField_WithOnChanged_DoesNotThrow()
        {
            var element = new FloatField(value: 1.5f, onChanged: _ => { });

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element, Theme.Default));
        }

        [Test]
        public void Paint_FFloatField_DoesNotThrow()
        {
            var original = new FloatField(value: 1f);
            var native = (UiFloatField)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new FloatField(value: 2f);

            Assert.DoesNotThrow(() => ElementPainter.Paint(updated, native, Theme.Default));
        }

#if FRAM3_PURE_TESTS
        [Test]
        public void CreateNative_FFloatField_SetsValue()
        {
            var element = new FloatField(value: 0.5f);

            var native = (UiFloatField)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.value, Is.EqualTo(0.5f).Within(0.0001f));
        }

        [Test]
        public void CreateNative_FFloatField_SetsLabel()
        {
            var element = new FloatField(label: "Speed");

            var native = (UiFloatField)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.label, Is.EqualTo("Speed"));
        }

        [Test]
        public void CreateNative_FFloatField_NullLabel_LeavesLabelNull()
        {
            var element = new FloatField();

            var native = (UiFloatField)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.label, Is.Null);
        }

        [Test]
        public void CreateNative_FFloatField_WithOnChanged_InvokesCallbackOnValueChange()
        {
            float? received = null;
            var element = new FloatField(onChanged: v => received = v);
            var native = (UiFloatField)ElementPainter.CreateNative(element, Theme.Default);

            native.SimulateValueChanged(2.5f);

            Assert.That(received, Is.EqualTo(2.5f).Within(0.0001f));
        }

        [Test]
        public void Paint_FFloatField_UpdatesValue()
        {
            var original = new FloatField(value: 1f);
            var native = (UiFloatField)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new FloatField(value: 9.9f);
            ElementPainter.Paint(updated, native, Theme.Default);

            Assert.That(native.value, Is.EqualTo(9.9f).Within(0.0001f));
        }

        [Test]
        public void Paint_FFloatField_UpdatesLabel()
        {
            var original = new FloatField(label: "old");
            var native = (UiFloatField)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new FloatField(label: "new");
            ElementPainter.Paint(updated, native, Theme.Default);

            Assert.That(native.label, Is.EqualTo("new"));
        }
#endif
    }
}
