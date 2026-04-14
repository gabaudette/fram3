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
    internal sealed class IntFieldTests
    {
        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------

        [Test]
        public void Constructor_DefaultValue_IsZero()
        {
            var element = new IntField();

            Assert.That(element.Value, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_SetsValue()
        {
            var element = new IntField(value: 42);

            Assert.That(element.Value, Is.EqualTo(42));
        }

        [Test]
        public void Constructor_SetsLabel()
        {
            var element = new IntField(label: "Count");

            Assert.That(element.Label, Is.EqualTo("Count"));
        }

        [Test]
        public void Constructor_DefaultLabel_IsNull()
        {
            var element = new IntField();

            Assert.That(element.Label, Is.Null);
        }

        [Test]
        public void Constructor_SetsOnChanged()
        {
            Action<int> callback = _ => { };

            var element = new IntField(onChanged: callback);

            Assert.That(element.OnChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnChanged_IsNull()
        {
            var element = new IntField();

            Assert.That(element.OnChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new ValueKey<string>("intf");
            var element = new IntField(key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new IntField();

            Assert.That(element.GetChildren(), Is.Empty);
        }

        // -----------------------------------------------------------------------
        // Painter -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FIntField_ReturnsIntegerField()
        {
            var element = new IntField();

            var native = ElementPainter.CreateNative(element);

            Assert.That(native, Is.InstanceOf<IntegerField>());
        }

        [Test]
        public void CreateNative_FIntField_WithNullOnChanged_DoesNotThrow()
        {
            var element = new IntField(value: 5);

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element));
        }

        [Test]
        public void CreateNative_FIntField_WithOnChanged_DoesNotThrow()
        {
            var element = new IntField(value: 5, onChanged: _ => { });

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element));
        }

        [Test]
        public void Paint_FIntField_DoesNotThrow()
        {
            var original = new IntField(value: 1);
            var native = (IntegerField)ElementPainter.CreateNative(original);

            var updated = new IntField(value: 2);

            Assert.DoesNotThrow(() => ElementPainter.Paint(updated, native));
        }

#if FRAM3_PURE_TESTS
        [Test]
        public void CreateNative_FIntField_SetsValue()
        {
            var element = new IntField(value: 99);

            var native = (IntegerField)ElementPainter.CreateNative(element);

            Assert.That(native.value, Is.EqualTo(99));
        }

        [Test]
        public void CreateNative_FIntField_SetsLabel()
        {
            var element = new IntField(label: "Amount");

            var native = (IntegerField)ElementPainter.CreateNative(element);

            Assert.That(native.label, Is.EqualTo("Amount"));
        }

        [Test]
        public void CreateNative_FIntField_NullLabel_LeavesLabelNull()
        {
            var element = new IntField();

            var native = (IntegerField)ElementPainter.CreateNative(element);

            Assert.That(native.label, Is.Null);
        }

        [Test]
        public void CreateNative_FIntField_WithOnChanged_InvokesCallbackOnValueChange()
        {
            int? received = null;
            var element = new IntField(onChanged: v => received = v);
            var native = (IntegerField)ElementPainter.CreateNative(element);

            native.SimulateValueChanged(7);

            Assert.That(received, Is.EqualTo(7));
        }

        [Test]
        public void Paint_FIntField_UpdatesValue()
        {
            var original = new IntField(value: 1);
            var native = (IntegerField)ElementPainter.CreateNative(original);

            var updated = new IntField(value: 10);
            ElementPainter.Paint(updated, native);

            Assert.That(native.value, Is.EqualTo(10));
        }

        [Test]
        public void Paint_FIntField_UpdatesLabel()
        {
            var original = new IntField(label: "old");
            var native = (IntegerField)ElementPainter.CreateNative(original);

            var updated = new IntField(label: "new");
            ElementPainter.Paint(updated, native);

            Assert.That(native.label, Is.EqualTo("new"));
        }
#endif
    }
}
