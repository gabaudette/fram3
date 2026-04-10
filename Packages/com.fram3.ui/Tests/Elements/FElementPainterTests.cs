#nullable enable
using System;
using Fram3.UI.Elements;
using Fram3.UI.Rendering.Internal;
using Fram3.UI.Styling;
using NUnit.Framework;
using UnityEngine.UIElements;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FElementPainterTests
    {
        [Test]
        public void CreateNative_FText_ReturnsLabel()
        {
            var element = new FText("Hello");

            var native = FElementPainter.CreateNative(element);

            Assert.That(native, Is.InstanceOf<Label>());
        }

        [Test]
        public void CreateNative_FText_SetsLabelText()
        {
            var element = new FText("World");

            var native = (Label)FElementPainter.CreateNative(element);

            Assert.That(native.text, Is.EqualTo("World"));
        }

        [Test]
        public void CreateNative_FText_WithFontSize_SetsFontSize()
        {
            var element = new FText("x", new FTextStyle(FontSize: 24f));

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.fontSize, Is.EqualTo(24f));
        }

        [Test]
        public void CreateNative_FButton_ReturnsButton()
        {
            var element = new FButton("Click");

            var native = FElementPainter.CreateNative(element);

            Assert.That(native, Is.InstanceOf<Button>());
        }

        [Test]
        public void CreateNative_FButton_SetsLabel()
        {
            var element = new FButton("Submit");

            var native = (Button)FElementPainter.CreateNative(element);

            Assert.That(native.text, Is.EqualTo("Submit"));
        }

        [Test]
        public void CreateNative_FButton_StoresClickAction()
        {
            var pressed = false;
            var element = new FButton("Go", () => { pressed = true; });

            var native = (Button)FElementPainter.CreateNative(element);
            native.clickedAction?.Invoke();

            Assert.That(pressed, Is.True);
        }

        [Test]
        public void CreateNative_FColumn_ReturnsVisualElement()
        {
            var element = new FColumn();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FColumn_SetsColumnFlexDirection()
        {
            var element = new FColumn();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.flexDirection, Is.EqualTo(FlexDirection.Column));
        }

        [Test]
        public void CreateNative_FRow_SetsRowFlexDirection()
        {
            var element = new FRow();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.flexDirection, Is.EqualTo(FlexDirection.Row));
        }

        [Test]
        public void CreateNative_FColumn_CenterMainAxis_SetsJustifyCenter()
        {
            var element = new FColumn(mainAxisAlignment: FMainAxisAlignment.Center);

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.justifyContent, Is.EqualTo(Justify.Center));
        }

        [Test]
        public void CreateNative_FColumn_StretchCrossAxis_SetsAlignItemsStretch()
        {
            var element = new FColumn(crossAxisAlignment: FCrossAxisAlignment.Stretch);

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.alignItems, Is.EqualTo(Align.Stretch));
        }

        [Test]
        public void CreateNative_FPadding_SetsPaddingOnAllEdges()
        {
            var element = new FPadding(new FEdgeInsets(4f, 8f, 12f, 16f));

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.paddingTop, Is.EqualTo(4f));
            Assert.That(native.style.paddingRight, Is.EqualTo(8f));
            Assert.That(native.style.paddingBottom, Is.EqualTo(12f));
            Assert.That(native.style.paddingLeft, Is.EqualTo(16f));
        }

        [Test]
        public void CreateNative_FSizedBoxExpand_SetsFlexGrow()
        {
            var element = FSizedBox.Expand();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.flexGrow, Is.EqualTo(1f));
        }

        [Test]
        public void CreateNative_FSizedBoxFromSize_SetsWidthAndHeight()
        {
            var element = FSizedBox.FromSize(width: 100f, height: 50f);

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.width, Is.EqualTo(100f));
            Assert.That(native.style.height, Is.EqualTo(50f));
        }

        [Test]
        public void CreateNative_FCenter_SetsAlignItemsCenter()
        {
            var element = new FCenter();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.alignItems, Is.EqualTo(Align.Center));
            Assert.That(native.style.justifyContent, Is.EqualTo(Justify.Center));
        }

        [Test]
        public void CreateNative_FContainer_SetsWidthAndHeight()
        {
            var element = new FContainer(width: 200f, height: 100f);

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.width, Is.EqualTo(200f));
            Assert.That(native.style.height, Is.EqualTo(100f));
        }

        [Test]
        public void CreateNative_FContainer_WithDecoration_SetsBackgroundColor()
        {
            var element = new FContainer(
                decoration: new FBoxDecoration(Color: new FColor(1f, 0f, 0f)));

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.backgroundColor.HasValue, Is.True);
            var bg = native.style.backgroundColor!.Value;
            Assert.That(bg.r, Is.EqualTo(1f).Within(0.001f));
            Assert.That(bg.g, Is.EqualTo(0f).Within(0.001f));
            Assert.That(bg.b, Is.EqualTo(0f).Within(0.001f));
        }

        [Test]
        public void Paint_FText_UpdatesLabelText()
        {
            var original = new FText("Before");
            var label = (Label)FElementPainter.CreateNative(original);

            var updated = new FText("After");
            FElementPainter.Paint(updated, label);

            Assert.That(label.text, Is.EqualTo("After"));
        }

        [Test]
        public void Paint_FButton_UpdatesButtonText()
        {
            var original = new FButton("Old");
            var button = (Button)FElementPainter.CreateNative(original);

            var updated = new FButton("New");
            FElementPainter.Paint(updated, button);

            Assert.That(button.text, Is.EqualTo("New"));
        }
    }
}
