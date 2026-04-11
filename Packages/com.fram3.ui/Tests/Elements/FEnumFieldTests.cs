#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements;
using Fram3.UI.Rendering.Internal;
using NUnit.Framework;
using UnityEngine.UIElements;

namespace Fram3.UI.Tests.Elements
{
    internal enum TestDirection { Left, Right, Up, Down }

    [TestFixture]
    internal sealed class FEnumFieldTests
    {
        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------

        [Test]
        public void Constructor_DefaultValue_IsFirstEnumMember()
        {
            var element = new FEnumField<TestDirection>();

            Assert.That(element.Value, Is.EqualTo(default(TestDirection)));
        }

        [Test]
        public void Constructor_SetsValue()
        {
            var element = new FEnumField<TestDirection>(value: TestDirection.Right);

            Assert.That(element.Value, Is.EqualTo(TestDirection.Right));
        }

        [Test]
        public void Constructor_SetsLabel()
        {
            var element = new FEnumField<TestDirection>(label: "Direction");

            Assert.That(element.Label, Is.EqualTo("Direction"));
        }

        [Test]
        public void Constructor_DefaultLabel_IsNull()
        {
            var element = new FEnumField<TestDirection>();

            Assert.That(element.Label, Is.Null);
        }

        [Test]
        public void Constructor_SetsOnChanged()
        {
            Action<TestDirection> callback = _ => { };

            var element = new FEnumField<TestDirection>(onChanged: callback);

            Assert.That(element.OnChanged, Is.SameAs(callback));
        }

        [Test]
        public void Constructor_DefaultOnChanged_IsNull()
        {
            var element = new FEnumField<TestDirection>();

            Assert.That(element.OnChanged, Is.Null);
        }

        [Test]
        public void Constructor_SetsKey()
        {
            var key = new FValueKey<string>("ef");
            var element = new FEnumField<TestDirection>(key: key);

            Assert.That(element.Key, Is.SameAs(key));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var element = new FEnumField<TestDirection>();

            Assert.That(element.GetChildren(), Is.Empty);
        }

        // -----------------------------------------------------------------------
        // Painter -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FEnumField_ReturnsEnumField()
        {
            var element = new FEnumField<TestDirection>();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native, Is.InstanceOf<EnumField>());
        }

        [Test]
        public void CreateNative_FEnumField_WithNullOnChanged_DoesNotThrow()
        {
            var element = new FEnumField<TestDirection>(value: TestDirection.Up);

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        [Test]
        public void CreateNative_FEnumField_WithOnChanged_DoesNotThrow()
        {
            var element = new FEnumField<TestDirection>(value: TestDirection.Up, onChanged: _ => { });

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        [Test]
        public void Paint_FEnumField_DoesNotThrow()
        {
            var original = new FEnumField<TestDirection>(value: TestDirection.Left);
            var native = (EnumField)FElementPainter.CreateNative(original);

            var updated = new FEnumField<TestDirection>(value: TestDirection.Right);

            Assert.DoesNotThrow(() => FElementPainter.Paint(updated, native));
        }

#if FRAM3_PURE_TESTS
        [Test]
        public void CreateNative_FEnumField_SetsValue()
        {
            var element = new FEnumField<TestDirection>(value: TestDirection.Down);

            var native = (EnumField)FElementPainter.CreateNative(element);

            Assert.That(native.value, Is.EqualTo(TestDirection.Down));
        }

        [Test]
        public void CreateNative_FEnumField_SetsLabel()
        {
            var element = new FEnumField<TestDirection>(label: "Facing");

            var native = (EnumField)FElementPainter.CreateNative(element);

            Assert.That(native.label, Is.EqualTo("Facing"));
        }

        [Test]
        public void CreateNative_FEnumField_NullLabel_LeavesLabelNull()
        {
            var element = new FEnumField<TestDirection>();

            var native = (EnumField)FElementPainter.CreateNative(element);

            Assert.That(native.label, Is.Null);
        }

        [Test]
        public void CreateNative_FEnumField_WithOnChanged_InvokesCallbackOnValueChange()
        {
            TestDirection? received = null;
            var element = new FEnumField<TestDirection>(onChanged: v => received = v);
            var native = (EnumField)FElementPainter.CreateNative(element);

            native.SimulateValueChanged(TestDirection.Up);

            Assert.That(received, Is.EqualTo(TestDirection.Up));
        }

        [Test]
        public void CreateNative_FEnumField_WithOnChanged_NullEvent_DoesNotInvokeCallback()
        {
            var invoked = false;
            var element = new FEnumField<TestDirection>(onChanged: _ => invoked = true);
            var native = (EnumField)FElementPainter.CreateNative(element);

            native.SimulateValueChanged(null);

            Assert.That(invoked, Is.False);
        }

        [Test]
        public void Paint_FEnumField_UpdatesValue()
        {
            var original = new FEnumField<TestDirection>(value: TestDirection.Left);
            var native = (EnumField)FElementPainter.CreateNative(original);

            var updated = new FEnumField<TestDirection>(value: TestDirection.Right);
            FElementPainter.Paint(updated, native);

            Assert.That(native.value, Is.EqualTo(TestDirection.Right));
        }

        [Test]
        public void Paint_FEnumField_UpdatesLabel()
        {
            var original = new FEnumField<TestDirection>(label: "old");
            var native = (EnumField)FElementPainter.CreateNative(original);

            var updated = new FEnumField<TestDirection>(label: "new");
            FElementPainter.Paint(updated, native);

            Assert.That(native.label, Is.EqualTo("new"));
        }
#endif
    }
}
