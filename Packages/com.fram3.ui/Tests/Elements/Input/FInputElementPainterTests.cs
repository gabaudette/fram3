#nullable enable
using System.Collections.Generic;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Elements.Input;
using Fram3.UI.Rendering.Internal;
using NUnit.Framework;
using UnityEngine.UIElements;

namespace Fram3.UI.Tests.Elements.Input
{
    [TestFixture]
    internal sealed class FInputElementPainterTests
    {
        // -----------------------------------------------------------------------
        // FTextField -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FTextField_ReturnsTextField()
        {
            var element = new FTextField();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native, Is.InstanceOf<TextField>());
        }

        [Test]
        public void CreateNative_FTextField_WithNullOnChanged_DoesNotThrow()
        {
            var element = new FTextField(value: "hello");

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        [Test]
        public void CreateNative_FTextField_WithOnChanged_DoesNotThrow()
        {
            var element = new FTextField(value: "hello", onChanged: _ => { });

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        // -----------------------------------------------------------------------
        // FToggle -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FToggle_ReturnsToggle()
        {
            var element = new FToggle();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native, Is.InstanceOf<Toggle>());
        }

        [Test]
        public void CreateNative_FToggle_WithNullOnChanged_DoesNotThrow()
        {
            var element = new FToggle(value: true);

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        [Test]
        public void CreateNative_FToggle_WithOnChanged_DoesNotThrow()
        {
            var element = new FToggle(value: false, onChanged: _ => { });

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        // -----------------------------------------------------------------------
        // FSlider -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FSlider_ReturnsSlider()
        {
            var element = new FSlider();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native, Is.InstanceOf<Slider>());
        }

        [Test]
        public void CreateNative_FSlider_WithNullOnChanged_DoesNotThrow()
        {
            var element = new FSlider(value: 0.5f, min: 0f, max: 1f);

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        [Test]
        public void CreateNative_FSlider_WithOnChanged_DoesNotThrow()
        {
            var element = new FSlider(value: 0.5f, min: 0f, max: 1f, onChanged: _ => { });

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        // -----------------------------------------------------------------------
        // FDropdown -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FDropdown_ReturnsDropdownField()
        {
            var element = new FDropdown(new string[] { "A", "B" });

            var native = FElementPainter.CreateNative(element);

            Assert.That(native, Is.InstanceOf<DropdownField>());
        }

        [Test]
        public void CreateNative_FDropdown_WithNullOnChanged_DoesNotThrow()
        {
            var element = new FDropdown(new string[] { "X", "Y" }, selectedIndex: 0);

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        [Test]
        public void CreateNative_FDropdown_WithOnChanged_DoesNotThrow()
        {
            var element = new FDropdown(new string[] { "X", "Y" }, selectedIndex: 0, onChanged: _ => { });

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        // -----------------------------------------------------------------------
        // FGestureDetector -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FGestureDetector_ReturnsVisualElement()
        {
            var element = new FGestureDetector(new FText("tap me"));

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FGestureDetector_WithNullCallbacks_DoesNotThrow()
        {
            var element = new FGestureDetector(new FText("tap me"));

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        [Test]
        public void CreateNative_FGestureDetector_WithAllCallbacks_DoesNotThrow()
        {
            var element = new FGestureDetector(
                new FText("tap me"),
                onTap: () => { },
                onPointerEnter: () => { },
                onPointerExit: () => { });

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        // -----------------------------------------------------------------------
        // Paint -- FTextField
        // -----------------------------------------------------------------------

        [Test]
        public void Paint_FTextField_DoesNotThrow()
        {
            var original = new FTextField(value: "initial");
            var native = (TextField)FElementPainter.CreateNative(original);

            var updated = new FTextField(value: "updated");

            Assert.DoesNotThrow(() => FElementPainter.Paint(updated, native));
        }

        // -----------------------------------------------------------------------
        // Paint -- FToggle
        // -----------------------------------------------------------------------

        [Test]
        public void Paint_FToggle_DoesNotThrow()
        {
            var original = new FToggle(value: false);
            var native = (Toggle)FElementPainter.CreateNative(original);

            var updated = new FToggle(value: true);

            Assert.DoesNotThrow(() => FElementPainter.Paint(updated, native));
        }

        // -----------------------------------------------------------------------
        // Paint -- FSlider
        // -----------------------------------------------------------------------

        [Test]
        public void Paint_FSlider_DoesNotThrow()
        {
            var original = new FSlider(value: 0f, min: 0f, max: 10f);
            var native = (Slider)FElementPainter.CreateNative(original);

            var updated = new FSlider(value: 5f, min: 0f, max: 10f);

            Assert.DoesNotThrow(() => FElementPainter.Paint(updated, native));
        }

        // -----------------------------------------------------------------------
        // Paint -- FDropdown
        // -----------------------------------------------------------------------

        [Test]
        public void Paint_FDropdown_DoesNotThrow()
        {
            var original = new FDropdown(new string[] { "A", "B" }, selectedIndex: 0);
            var native = (DropdownField)FElementPainter.CreateNative(original);

            var updated = new FDropdown(new string[] { "A", "B", "C" }, selectedIndex: 1);

            Assert.DoesNotThrow(() => FElementPainter.Paint(updated, native));
        }

#if FRAM3_PURE_TESTS
        // -----------------------------------------------------------------------
        // FTextField -- stub-only property assertions
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FTextField_SetsValue()
        {
            var element = new FTextField(value: "hello");

            var native = (TextField)FElementPainter.CreateNative(element);

            Assert.That(native.value, Is.EqualTo("hello"));
        }

        [Test]
        public void CreateNative_FTextField_SetsReadOnly()
        {
            var element = new FTextField(readOnly: true);

            var native = (TextField)FElementPainter.CreateNative(element);

            Assert.That(native.isReadOnly, Is.True);
        }

        [Test]
        public void CreateNative_FTextField_SetsMultiline()
        {
            var element = new FTextField(multiline: true);

            var native = (TextField)FElementPainter.CreateNative(element);

            Assert.That(native.multiline, Is.True);
        }

        [Test]
        public void CreateNative_FTextField_SetsPlaceholder()
        {
            var element = new FTextField(placeholder: "Type here...");

            var native = (TextField)FElementPainter.CreateNative(element);

            Assert.That(native.textEdition.placeholder, Is.EqualTo("Type here..."));
        }

        [Test]
        public void CreateNative_FTextField_NullPlaceholder_LeavesPlaceholderNull()
        {
            var element = new FTextField();

            var native = (TextField)FElementPainter.CreateNative(element);

            Assert.That(native.textEdition.placeholder, Is.Null);
        }

        [Test]
        public void CreateNative_FTextField_WithOnChanged_InvokesCallbackOnValueChange()
        {
            string? received = null;
            var element = new FTextField(onChanged: v => received = v);
            var native = (TextField)FElementPainter.CreateNative(element);

            native.SimulateValueChanged("world");

            Assert.That(received, Is.EqualTo("world"));
        }

        [Test]
        public void Paint_FTextField_UpdatesValue()
        {
            var original = new FTextField(value: "initial");
            var native = (TextField)FElementPainter.CreateNative(original);

            var updated = new FTextField(value: "updated");
            FElementPainter.Paint(updated, native);

            Assert.That(native.value, Is.EqualTo("updated"));
        }

        [Test]
        public void Paint_FTextField_UpdatesReadOnly()
        {
            var original = new FTextField(readOnly: false);
            var native = (TextField)FElementPainter.CreateNative(original);

            var updated = new FTextField(readOnly: true);
            FElementPainter.Paint(updated, native);

            Assert.That(native.isReadOnly, Is.True);
        }

        [Test]
        public void Paint_FTextField_UpdatesPlaceholder()
        {
            var original = new FTextField(placeholder: "old");
            var native = (TextField)FElementPainter.CreateNative(original);

            var updated = new FTextField(placeholder: "new");
            FElementPainter.Paint(updated, native);

            Assert.That(native.textEdition.placeholder, Is.EqualTo("new"));
        }

        // -----------------------------------------------------------------------
        // FToggle -- stub-only property assertions
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FToggle_SetsValue()
        {
            var element = new FToggle(value: true);

            var native = (Toggle)FElementPainter.CreateNative(element);

            Assert.That(native.value, Is.True);
        }

        [Test]
        public void CreateNative_FToggle_SetsLabel()
        {
            var element = new FToggle(label: "Enable feature");

            var native = (Toggle)FElementPainter.CreateNative(element);

            Assert.That(native.label, Is.EqualTo("Enable feature"));
        }

        [Test]
        public void CreateNative_FToggle_NullLabel_LeavesLabelNull()
        {
            var element = new FToggle();

            var native = (Toggle)FElementPainter.CreateNative(element);

            Assert.That(native.label, Is.Null);
        }

        [Test]
        public void CreateNative_FToggle_WithOnChanged_InvokesCallbackOnValueChange()
        {
            bool? received = null;
            var element = new FToggle(onChanged: v => received = v);
            var native = (Toggle)FElementPainter.CreateNative(element);

            native.SimulateValueChanged(true);

            Assert.That(received, Is.True);
        }

        [Test]
        public void Paint_FToggle_UpdatesValue()
        {
            var original = new FToggle(value: false);
            var native = (Toggle)FElementPainter.CreateNative(original);

            var updated = new FToggle(value: true);
            FElementPainter.Paint(updated, native);

            Assert.That(native.value, Is.True);
        }

        [Test]
        public void Paint_FToggle_UpdatesLabel()
        {
            var original = new FToggle(label: "old label");
            var native = (Toggle)FElementPainter.CreateNative(original);

            var updated = new FToggle(label: "new label");
            FElementPainter.Paint(updated, native);

            Assert.That(native.label, Is.EqualTo("new label"));
        }

        // -----------------------------------------------------------------------
        // FSlider -- stub-only property assertions
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FSlider_SetsValue()
        {
            var element = new FSlider(value: 0.75f, min: 0f, max: 1f);

            var native = (Slider)FElementPainter.CreateNative(element);

            Assert.That(native.value, Is.EqualTo(0.75f).Within(0.0001f));
        }

        [Test]
        public void CreateNative_FSlider_SetsLowAndHighValue()
        {
            var element = new FSlider(min: 10f, max: 100f);

            var native = (Slider)FElementPainter.CreateNative(element);

            Assert.That(native.lowValue, Is.EqualTo(10f).Within(0.0001f));
            Assert.That(native.highValue, Is.EqualTo(100f).Within(0.0001f));
        }

        [Test]
        public void CreateNative_FSlider_SetsLabel()
        {
            var element = new FSlider(label: "Volume");

            var native = (Slider)FElementPainter.CreateNative(element);

            Assert.That(native.label, Is.EqualTo("Volume"));
        }

        [Test]
        public void CreateNative_FSlider_NullLabel_LeavesLabelNull()
        {
            var element = new FSlider();

            var native = (Slider)FElementPainter.CreateNative(element);

            Assert.That(native.label, Is.Null);
        }

        [Test]
        public void CreateNative_FSlider_WithOnChanged_InvokesCallbackOnValueChange()
        {
            float? received = null;
            var element = new FSlider(onChanged: v => received = v);
            var native = (Slider)FElementPainter.CreateNative(element);

            native.SimulateValueChanged(0.5f);

            Assert.That(received, Is.EqualTo(0.5f).Within(0.0001f));
        }

        [Test]
        public void Paint_FSlider_UpdatesValue()
        {
            var original = new FSlider(value: 0f, min: 0f, max: 10f);
            var native = (Slider)FElementPainter.CreateNative(original);

            var updated = new FSlider(value: 7f, min: 0f, max: 10f);
            FElementPainter.Paint(updated, native);

            Assert.That(native.value, Is.EqualTo(7f).Within(0.0001f));
        }

        [Test]
        public void Paint_FSlider_UpdatesRange()
        {
            var original = new FSlider(min: 0f, max: 10f);
            var native = (Slider)FElementPainter.CreateNative(original);

            var updated = new FSlider(min: 5f, max: 50f);
            FElementPainter.Paint(updated, native);

            Assert.That(native.lowValue, Is.EqualTo(5f).Within(0.0001f));
            Assert.That(native.highValue, Is.EqualTo(50f).Within(0.0001f));
        }

        [Test]
        public void Paint_FSlider_UpdatesLabel()
        {
            var original = new FSlider(label: "old");
            var native = (Slider)FElementPainter.CreateNative(original);

            var updated = new FSlider(label: "new");
            FElementPainter.Paint(updated, native);

            Assert.That(native.label, Is.EqualTo("new"));
        }

        // -----------------------------------------------------------------------
        // FDropdown -- stub-only property assertions
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FDropdown_SetsChoices()
        {
            var element = new FDropdown(new string[] { "Red", "Green", "Blue" });

            var native = (DropdownField)FElementPainter.CreateNative(element);

            Assert.That(native.choices, Is.EqualTo(new List<string> { "Red", "Green", "Blue" }));
        }

        [Test]
        public void CreateNative_FDropdown_SetsSelectedIndex()
        {
            var element = new FDropdown(new string[] { "A", "B", "C" }, selectedIndex: 1);

            var native = (DropdownField)FElementPainter.CreateNative(element);

            Assert.That(native.index, Is.EqualTo(1));
        }

        [Test]
        public void CreateNative_FDropdown_SetsLabel()
        {
            var element = new FDropdown(new string[] { "A" }, label: "Pick one");

            var native = (DropdownField)FElementPainter.CreateNative(element);

            Assert.That(native.label, Is.EqualTo("Pick one"));
        }

        [Test]
        public void CreateNative_FDropdown_NullLabel_LeavesLabelNull()
        {
            var element = new FDropdown(new string[] { "A" });

            var native = (DropdownField)FElementPainter.CreateNative(element);

            Assert.That(native.label, Is.Null);
        }

        [Test]
        public void CreateNative_FDropdown_WithOnChanged_InvokesCallbackOnIndexChange()
        {
            int? received = null;
            var element = new FDropdown(
                new string[] { "X", "Y", "Z" },
                selectedIndex: 0,
                onChanged: i => received = i);
            var native = (DropdownField)FElementPainter.CreateNative(element);

            native.SimulateIndexChanged(2);

            Assert.That(received, Is.EqualTo(2));
        }

        [Test]
        public void Paint_FDropdown_UpdatesChoices()
        {
            var original = new FDropdown(new string[] { "A", "B" });
            var native = (DropdownField)FElementPainter.CreateNative(original);

            var updated = new FDropdown(new string[] { "C", "D", "E" });
            FElementPainter.Paint(updated, native);

            Assert.That(native.choices, Is.EqualTo(new List<string> { "C", "D", "E" }));
        }

        [Test]
        public void Paint_FDropdown_UpdatesSelectedIndex()
        {
            var original = new FDropdown(new string[] { "A", "B", "C" }, selectedIndex: 0);
            var native = (DropdownField)FElementPainter.CreateNative(original);

            var updated = new FDropdown(new string[] { "A", "B", "C" }, selectedIndex: 2);
            FElementPainter.Paint(updated, native);

            Assert.That(native.index, Is.EqualTo(2));
        }

        [Test]
        public void Paint_FDropdown_UpdatesLabel()
        {
            var original = new FDropdown(new string[] { "A" }, label: "old");
            var native = (DropdownField)FElementPainter.CreateNative(original);

            var updated = new FDropdown(new string[] { "A" }, label: "new");
            FElementPainter.Paint(updated, native);

            Assert.That(native.label, Is.EqualTo("new"));
        }

        // -----------------------------------------------------------------------
        // FGestureDetector -- callback firing
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FGestureDetector_OnTap_InvokesCallbackOnClick()
        {
            var tapped = false;
            var element = new FGestureDetector(new FText("x"), onTap: () => tapped = true);
            var native = FElementPainter.CreateNative(element);

            native.SimulateEvent(new ClickEvent());

            Assert.That(tapped, Is.True);
        }

        [Test]
        public void CreateNative_FGestureDetector_OnPointerEnter_InvokesCallbackOnEnter()
        {
            var entered = false;
            var element = new FGestureDetector(new FText("x"), onPointerEnter: () => entered = true);
            var native = FElementPainter.CreateNative(element);

            native.SimulateEvent(new PointerEnterEvent());

            Assert.That(entered, Is.True);
        }

        [Test]
        public void CreateNative_FGestureDetector_OnPointerExit_InvokesCallbackOnLeave()
        {
            var exited = false;
            var element = new FGestureDetector(new FText("x"), onPointerExit: () => exited = true);
            var native = FElementPainter.CreateNative(element);

            native.SimulateEvent(new PointerLeaveEvent());

            Assert.That(exited, Is.True);
        }

        [Test]
        public void CreateNative_FGestureDetector_NullOnTap_DoesNotFireOnClick()
        {
            var element = new FGestureDetector(new FText("x"), onTap: null);
            var native = FElementPainter.CreateNative(element);

            Assert.DoesNotThrow(() => native.SimulateEvent(new ClickEvent()));
        }
#endif
    }
}
