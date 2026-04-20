#nullable enable
using System.Collections.Generic;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Elements.Input;
using Fram3.UI.Rendering.Internal;
using Fram3.UI.Styling;
using NUnit.Framework;
using UnityEngine.UIElements;
using TextField = Fram3.UI.Elements.Input.TextField;
using UiTextField = UnityEngine.UIElements.TextField;

namespace Fram3.UI.Tests.Elements.Input
{
    [TestFixture]
    internal sealed class InputElementPainterTests
    {
        // -----------------------------------------------------------------------
        // TextField -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FTextField_ReturnsTextField()
        {
            var element = new TextField();

            var native = ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native, Is.InstanceOf<UiTextField>());
        }

        [Test]
        public void CreateNative_FTextField_WithNullOnChanged_DoesNotThrow()
        {
            var element = new TextField(value: "hello");

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element, Theme.Default));
        }

        [Test]
        public void CreateNative_FTextField_WithOnChanged_DoesNotThrow()
        {
            var element = new TextField(value: "hello", onChanged: _ => { });

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element, Theme.Default));
        }

        // -----------------------------------------------------------------------
        // FrameToggle -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FToggle_ReturnsToggle()
        {
            var element = new FrameToggle();

            var native = ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native, Is.InstanceOf<Toggle>());
        }

        [Test]
        public void CreateNative_FToggle_WithNullOnChanged_DoesNotThrow()
        {
            var element = new FrameToggle(value: true);

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element, Theme.Default));
        }

        [Test]
        public void CreateNative_FToggle_WithOnChanged_DoesNotThrow()
        {
            var element = new FrameToggle(value: false, onChanged: _ => { });

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element, Theme.Default));
        }

        // -----------------------------------------------------------------------
        // FrameSlider -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FSlider_ReturnsSlider()
        {
            var element = new FrameSlider();

            var native = ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native, Is.InstanceOf<Slider>());
        }

        [Test]
        public void CreateNative_FSlider_WithNullOnChanged_DoesNotThrow()
        {
            var element = new FrameSlider(value: 0.5f, min: 0f, max: 1f);

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element, Theme.Default));
        }

        [Test]
        public void CreateNative_FSlider_WithOnChanged_DoesNotThrow()
        {
            var element = new FrameSlider(value: 0.5f, min: 0f, max: 1f, onChanged: _ => { });

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element, Theme.Default));
        }

        // -----------------------------------------------------------------------
        // Dropdown -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FDropdown_ReturnsDropdownField()
        {
            var element = new Dropdown(new string[] { "A", "B" });

            var native = ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native, Is.InstanceOf<DropdownField>());
        }

        [Test]
        public void CreateNative_FDropdown_WithNullOnChanged_DoesNotThrow()
        {
            var element = new Dropdown(new string[] { "X", "Y" }, selectedIndex: 0);

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element, Theme.Default));
        }

        [Test]
        public void CreateNative_FDropdown_WithOnChanged_DoesNotThrow()
        {
            var element = new Dropdown(new string[] { "X", "Y" }, selectedIndex: 0, onChanged: _ => { });

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element, Theme.Default));
        }

        // -----------------------------------------------------------------------
        // GestureDetector -- CreateNative
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FGestureDetector_ReturnsVisualElement()
        {
            var element = new GestureDetector(new Text("tap me"));

            var native = ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FGestureDetector_WithNullCallbacks_DoesNotThrow()
        {
            var element = new GestureDetector(new Text("tap me"));

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element, Theme.Default));
        }

        [Test]
        public void CreateNative_FGestureDetector_WithAllCallbacks_DoesNotThrow()
        {
            var element = new GestureDetector(
                new Text("tap me"),
                onTap: () => { },
                onPointerEnter: () => { },
                onPointerExit: () => { });

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element, Theme.Default));
        }

        // -----------------------------------------------------------------------
        // Paint -- TextField
        // -----------------------------------------------------------------------

        [Test]
        public void Paint_FTextField_DoesNotThrow()
        {
            var original = new TextField(value: "initial");
            var native = (UiTextField)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new TextField(value: "updated");

            Assert.DoesNotThrow(() => ElementPainter.Paint(updated, native, Theme.Default));
        }

        // -----------------------------------------------------------------------
        // Paint -- FrameToggle
        // -----------------------------------------------------------------------

        [Test]
        public void Paint_FToggle_DoesNotThrow()
        {
            var original = new FrameToggle(value: false);
            var native = (Toggle)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new FrameToggle(value: true);

            Assert.DoesNotThrow(() => ElementPainter.Paint(updated, native, Theme.Default));
        }

        // -----------------------------------------------------------------------
        // Paint -- FrameSlider
        // -----------------------------------------------------------------------

        [Test]
        public void Paint_FSlider_DoesNotThrow()
        {
            var original = new FrameSlider(value: 0f, min: 0f, max: 10f);
            var native = (Slider)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new FrameSlider(value: 5f, min: 0f, max: 10f);

            Assert.DoesNotThrow(() => ElementPainter.Paint(updated, native, Theme.Default));
        }

        // -----------------------------------------------------------------------
        // Paint -- Dropdown
        // -----------------------------------------------------------------------

        [Test]
        public void Paint_FDropdown_DoesNotThrow()
        {
            var original = new Dropdown(new string[] { "A", "B" }, selectedIndex: 0);
            var native = (DropdownField)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new Dropdown(new string[] { "A", "B", "C" }, selectedIndex: 1);

            Assert.DoesNotThrow(() => ElementPainter.Paint(updated, native, Theme.Default));
        }

#if FRAM3_PURE_TESTS
        // -----------------------------------------------------------------------
        // TextField -- stub-only property assertions
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FTextField_SetsValue()
        {
            var element = new TextField(value: "hello");

            var native = (UiTextField)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.value, Is.EqualTo("hello"));
        }

        [Test]
        public void CreateNative_FTextField_SetsReadOnly()
        {
            var element = new TextField(readOnly: true);

            var native = (UiTextField)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.isReadOnly, Is.True);
        }

        [Test]
        public void CreateNative_FTextField_SetsMultiline()
        {
            var element = new TextField(multiline: true);

            var native = (UiTextField)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.multiline, Is.True);
        }

        [Test]
        public void CreateNative_FTextField_SetsPlaceholder()
        {
            var element = new TextField(placeholder: "Type here...");

            var native = (UiTextField)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.textEdition.placeholder, Is.EqualTo("Type here..."));
        }

        [Test]
        public void CreateNative_FTextField_NullPlaceholder_LeavesPlaceholderNull()
        {
            var element = new TextField();

            var native = (UiTextField)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.textEdition.placeholder, Is.Null);
        }

        [Test]
        public void CreateNative_FTextField_WithOnChanged_InvokesCallbackOnValueChange()
        {
            string? received = null;
            var element = new TextField(onChanged: v => received = v);
            var native = (UiTextField)ElementPainter.CreateNative(element, Theme.Default);

            native.SimulateValueChanged("world");

            Assert.That(received, Is.EqualTo("world"));
        }

        [Test]
        public void Paint_FTextField_UpdatesValue()
        {
            var original = new TextField(value: "initial");
            var native = (UiTextField)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new TextField(value: "updated");
            ElementPainter.Paint(updated, native, Theme.Default);

            Assert.That(native.value, Is.EqualTo("updated"));
        }

        [Test]
        public void Paint_FTextField_UpdatesReadOnly()
        {
            var original = new TextField(readOnly: false);
            var native = (UiTextField)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new TextField(readOnly: true);
            ElementPainter.Paint(updated, native, Theme.Default);

            Assert.That(native.isReadOnly, Is.True);
        }

        [Test]
        public void Paint_FTextField_UpdatesPlaceholder()
        {
            var original = new TextField(placeholder: "old");
            var native = (UiTextField)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new TextField(placeholder: "new");
            ElementPainter.Paint(updated, native, Theme.Default);

            Assert.That(native.textEdition.placeholder, Is.EqualTo("new"));
        }

        // -----------------------------------------------------------------------
        // FrameToggle -- stub-only property assertions
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FToggle_SetsValue()
        {
            var element = new FrameToggle(value: true);

            var native = (Toggle)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.value, Is.True);
        }

        [Test]
        public void CreateNative_FToggle_SetsLabel()
        {
            var element = new FrameToggle(label: "Enable feature");

            var native = (Toggle)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.label, Is.EqualTo("Enable feature"));
        }

        [Test]
        public void CreateNative_FToggle_NullLabel_LeavesLabelNull()
        {
            var element = new FrameToggle();

            var native = (Toggle)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.label, Is.Null);
        }

        [Test]
        public void CreateNative_FToggle_WithOnChanged_InvokesCallbackOnValueChange()
        {
            bool? received = null;
            var element = new FrameToggle(onChanged: v => received = v);
            var native = (Toggle)ElementPainter.CreateNative(element, Theme.Default);

            native.SimulateValueChanged(true);

            Assert.That(received, Is.True);
        }

        [Test]
        public void Paint_FToggle_UpdatesValue()
        {
            var original = new FrameToggle(value: false);
            var native = (Toggle)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new FrameToggle(value: true);
            ElementPainter.Paint(updated, native, Theme.Default);

            Assert.That(native.value, Is.True);
        }

        [Test]
        public void Paint_FToggle_UpdatesLabel()
        {
            var original = new FrameToggle(label: "old label");
            var native = (Toggle)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new FrameToggle(label: "new label");
            ElementPainter.Paint(updated, native, Theme.Default);

            Assert.That(native.label, Is.EqualTo("new label"));
        }

        // -----------------------------------------------------------------------
        // FrameSlider -- stub-only property assertions
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FSlider_SetsValue()
        {
            var element = new FrameSlider(value: 0.75f, min: 0f, max: 1f);

            var native = (Slider)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.value, Is.EqualTo(0.75f).Within(0.0001f));
        }

        [Test]
        public void CreateNative_FSlider_SetsLowAndHighValue()
        {
            var element = new FrameSlider(min: 10f, max: 100f);

            var native = (Slider)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.lowValue, Is.EqualTo(10f).Within(0.0001f));
            Assert.That(native.highValue, Is.EqualTo(100f).Within(0.0001f));
        }

        [Test]
        public void CreateNative_FSlider_SetsLabel()
        {
            var element = new FrameSlider(label: "Volume");

            var native = (Slider)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.label, Is.EqualTo("Volume"));
        }

        [Test]
        public void CreateNative_FSlider_NullLabel_LeavesLabelNull()
        {
            var element = new FrameSlider();

            var native = (Slider)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.label, Is.Null);
        }

        [Test]
        public void CreateNative_FSlider_WithOnChanged_InvokesCallbackOnValueChange()
        {
            float? received = null;
            var element = new FrameSlider(onChanged: v => received = v);
            var native = (Slider)ElementPainter.CreateNative(element, Theme.Default);

            native.SimulateValueChanged(0.5f);

            Assert.That(received, Is.EqualTo(0.5f).Within(0.0001f));
        }

        [Test]
        public void Paint_FSlider_UpdatesValue()
        {
            var original = new FrameSlider(value: 0f, min: 0f, max: 10f);
            var native = (Slider)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new FrameSlider(value: 7f, min: 0f, max: 10f);
            ElementPainter.Paint(updated, native, Theme.Default);

            Assert.That(native.value, Is.EqualTo(7f).Within(0.0001f));
        }

        [Test]
        public void Paint_FSlider_UpdatesRange()
        {
            var original = new FrameSlider(min: 0f, max: 10f);
            var native = (Slider)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new FrameSlider(min: 5f, max: 50f);
            ElementPainter.Paint(updated, native, Theme.Default);

            Assert.That(native.lowValue, Is.EqualTo(5f).Within(0.0001f));
            Assert.That(native.highValue, Is.EqualTo(50f).Within(0.0001f));
        }

        [Test]
        public void Paint_FSlider_UpdatesLabel()
        {
            var original = new FrameSlider(label: "old");
            var native = (Slider)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new FrameSlider(label: "new");
            ElementPainter.Paint(updated, native, Theme.Default);

            Assert.That(native.label, Is.EqualTo("new"));
        }

        // -----------------------------------------------------------------------
        // Dropdown -- stub-only property assertions
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FDropdown_SetsChoices()
        {
            var element = new Dropdown(new string[] { "Red", "Green", "Blue" });

            var native = (DropdownField)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.choices, Is.EqualTo(new List<string> { "Red", "Green", "Blue" }));
        }

        [Test]
        public void CreateNative_FDropdown_SetsSelectedIndex()
        {
            var element = new Dropdown(new string[] { "A", "B", "C" }, selectedIndex: 1);

            var native = (DropdownField)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.index, Is.EqualTo(1));
        }

        [Test]
        public void CreateNative_FDropdown_SetsLabel()
        {
            var element = new Dropdown(new string[] { "A" }, label: "Pick one");

            var native = (DropdownField)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.label, Is.EqualTo("Pick one"));
        }

        [Test]
        public void CreateNative_FDropdown_NullLabel_LeavesLabelNull()
        {
            var element = new Dropdown(new string[] { "A" });

            var native = (DropdownField)ElementPainter.CreateNative(element, Theme.Default);

            Assert.That(native.label, Is.Null);
        }

        [Test]
        public void CreateNative_FDropdown_WithOnChanged_InvokesCallbackOnIndexChange()
        {
            int? received = null;
            var element = new Dropdown(
                new string[] { "X", "Y", "Z" },
                selectedIndex: 0,
                onChanged: i => received = i);
            var native = (DropdownField)ElementPainter.CreateNative(element, Theme.Default);

            native.SimulateIndexChanged(2);

            Assert.That(received, Is.EqualTo(2));
        }

        [Test]
        public void Paint_FDropdown_UpdatesChoices()
        {
            var original = new Dropdown(new string[] { "A", "B" });
            var native = (DropdownField)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new Dropdown(new string[] { "C", "D", "E" });
            ElementPainter.Paint(updated, native, Theme.Default);

            Assert.That(native.choices, Is.EqualTo(new List<string> { "C", "D", "E" }));
        }

        [Test]
        public void Paint_FDropdown_UpdatesSelectedIndex()
        {
            var original = new Dropdown(new string[] { "A", "B", "C" }, selectedIndex: 0);
            var native = (DropdownField)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new Dropdown(new string[] { "A", "B", "C" }, selectedIndex: 2);
            ElementPainter.Paint(updated, native, Theme.Default);

            Assert.That(native.index, Is.EqualTo(2));
        }

        [Test]
        public void Paint_FDropdown_UpdatesLabel()
        {
            var original = new Dropdown(new string[] { "A" }, label: "old");
            var native = (DropdownField)ElementPainter.CreateNative(original, Theme.Default);

            var updated = new Dropdown(new string[] { "A" }, label: "new");
            ElementPainter.Paint(updated, native, Theme.Default);

            Assert.That(native.label, Is.EqualTo("new"));
        }

        // -----------------------------------------------------------------------
        // GestureDetector -- callback firing
        // -----------------------------------------------------------------------

        [Test]
        public void CreateNative_FGestureDetector_OnTap_InvokesCallbackOnClick()
        {
            var tapped = false;
            var element = new GestureDetector(new Text("x"), onTap: () => tapped = true);
            var native = ElementPainter.CreateNative(element, Theme.Default);

            native.SimulateEvent(new ClickEvent());

            Assert.That(tapped, Is.True);
        }

        [Test]
        public void CreateNative_FGestureDetector_OnPointerEnter_InvokesCallbackOnEnter()
        {
            var entered = false;
            var element = new GestureDetector(new Text("x"), onPointerEnter: () => entered = true);
            var native = ElementPainter.CreateNative(element, Theme.Default);

            native.SimulateEvent(new PointerEnterEvent());

            Assert.That(entered, Is.True);
        }

        [Test]
        public void CreateNative_FGestureDetector_OnPointerExit_InvokesCallbackOnLeave()
        {
            var exited = false;
            var element = new GestureDetector(new Text("x"), onPointerExit: () => exited = true);
            var native = ElementPainter.CreateNative(element, Theme.Default);

            native.SimulateEvent(new PointerLeaveEvent());

            Assert.That(exited, Is.True);
        }

        [Test]
        public void CreateNative_FGestureDetector_NullOnTap_DoesNotFireOnClick()
        {
            var element = new GestureDetector(new Text("x"), onTap: null);
            var native = ElementPainter.CreateNative(element, Theme.Default);

            Assert.DoesNotThrow(() => native.SimulateEvent(new ClickEvent()));
        }
#endif
    }
}
