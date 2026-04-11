#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements;
using Fram3.UI.Rendering.Internal;
using NUnit.Framework;
using UnityEngine.UIElements;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FIntFieldTests
    {
        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------

        [Test]
        public void Constructor_DefaultValue_IsZero()
        {
            var element = new FIntField();

            Assert.That(element.Value, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_SetsValue()
        {
            var element = new FIntField(value: 42);

            Assert.That(element.Value, Is.EqualTo(42));
        }

        [Test]
        public void Constructor_SetsLabel()
        {
            var element = new FIntField(label: "Count");

            Assert.That(element.Label, Is.EqualTo("Count"));
        }

        [Test]
        public void Constructor_DefaultLabel_IsNull()
        {
            var element = new FIntField();

            Assert.That(element.Label, Is.Null);
        }

        [Test]
        public void Constructor_SetsOnChanged()
        {
            Action<int> callback = _ => { };

            var element = new FIntField(onChanged: callback);

            Assert.That(element.OnChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnChanged_IsNull()
        {
            var element = new FIntField();

            Assert.That(element.OnChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new FValueKey<string>("intf");
            var element = new FIntField(key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new FIntField();

            Assert.That(element.GetChildren(), Is.Empty);
        }

        // -----------------------------------------------------------------------
        // Painter -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FIntField_ReturnsIntegerField()
        {
            var element = new FIntField();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native, Is.InstanceOf<IntegerField>());
        }

        [Test]
        public void CreateNative_FIntField_WithNullOnChanged_DoesNotThrow()
        {
            var element = new FIntField(value: 5);

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        [Test]
        public void CreateNative_FIntField_WithOnChanged_DoesNotThrow()
        {
            var element = new FIntField(value: 5, onChanged: _ => { });

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        [Test]
        public void Paint_FIntField_DoesNotThrow()
        {
            var original = new FIntField(value: 1);
            var native = (IntegerField)FElementPainter.CreateNative(original);

            var updated = new FIntField(value: 2);

            Assert.DoesNotThrow(() => FElementPainter.Paint(updated, native));
        }

#if FRAM3_PURE_TESTS
        [Test]
        public void CreateNative_FIntField_SetsValue()
        {
            var element = new FIntField(value: 99);

            var native = (IntegerField)FElementPainter.CreateNative(element);

            Assert.That(native.value, Is.EqualTo(99));
        }

        [Test]
        public void CreateNative_FIntField_SetsLabel()
        {
            var element = new FIntField(label: "Amount");

            var native = (IntegerField)FElementPainter.CreateNative(element);

            Assert.That(native.label, Is.EqualTo("Amount"));
        }

        [Test]
        public void CreateNative_FIntField_NullLabel_LeavesLabelNull()
        {
            var element = new FIntField();

            var native = (IntegerField)FElementPainter.CreateNative(element);

            Assert.That(native.label, Is.Null);
        }

        [Test]
        public void CreateNative_FIntField_WithOnChanged_InvokesCallbackOnValueChange()
        {
            int? received = null;
            var element = new FIntField(onChanged: v => received = v);
            var native = (IntegerField)FElementPainter.CreateNative(element);

            native.SimulateValueChanged(7);

            Assert.That(received, Is.EqualTo(7));
        }

        [Test]
        public void Paint_FIntField_UpdatesValue()
        {
            var original = new FIntField(value: 1);
            var native = (IntegerField)FElementPainter.CreateNative(original);

            var updated = new FIntField(value: 10);
            FElementPainter.Paint(updated, native);

            Assert.That(native.value, Is.EqualTo(10));
        }

        [Test]
        public void Paint_FIntField_UpdatesLabel()
        {
            var original = new FIntField(label: "old");
            var native = (IntegerField)FElementPainter.CreateNative(original);

            var updated = new FIntField(label: "new");
            FElementPainter.Paint(updated, native);

            Assert.That(native.label, Is.EqualTo("new"));
        }
#endif
    }
}
