#nullable enable
using System;
using Fram3.UI.Core;
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
        public void CreateNative_FButton_WithNullOnPressed_DoesNotThrow()
        {
            var element = new FButton("Cancel");

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
        }

        [Test]
        public void CreateNative_FColumn_ReturnsVisualElement()
        {
            var element = new FColumn();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FRow_ReturnsVisualElement()
        {
            var element = new FRow();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FPadding_ReturnsVisualElement()
        {
            var element = new FPadding(FEdgeInsets.All(8f));

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FSizedBox_ReturnsVisualElement()
        {
            var element = FSizedBox.FromSize(width: 100f, height: 50f);

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FCenter_ReturnsVisualElement()
        {
            var element = new FCenter();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FContainer_ReturnsVisualElement()
        {
            var element = new FContainer(width: 200f, height: 100f);

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FContainer_WithDecoration_DoesNotThrow()
        {
            var element = new FContainer(
                decoration: new FBoxDecoration(Color: new FColor(1f, 0f, 0f)));

            Assert.DoesNotThrow(() => FElementPainter.CreateNative(element));
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

        [Test]
        public void CreateNative_FProgressBar_ReturnsProgressBar()
        {
            var element = new FProgressBar(value: 50f);

            var native = FElementPainter.CreateNative(element);

            Assert.That(native, Is.InstanceOf<ProgressBar>());
        }

        [Test]
        public void CreateNative_FScrollView_ReturnsScrollView()
        {
            var element = new FScrollView { Child = new FText("x") };

            var native = FElementPainter.CreateNative(element);

            Assert.That(native, Is.InstanceOf<ScrollView>());
        }

        [Test]
        public void CreateNative_FImage_ReturnsImage()
        {
            var element = new FImage();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native, Is.InstanceOf<Image>());
        }

        [Test]
        public void CreateNative_FIcon_ReturnsImage()
        {
            var element = new FIcon();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native, Is.InstanceOf<Image>());
        }

        [Test]
        public void CreateNative_FStack_ReturnsVisualElement()
        {
            var element = new FStack { Children = new FElement[] { new FText("a") } };

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FExpanded_ReturnsVisualElement()
        {
            var element = new FExpanded { Child = new FText("x") };

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FDivider_ReturnsVisualElement()
        {
            var element = new FDivider();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FTooltip_ReturnsVisualElement()
        {
            var element = new FTooltip("tip") { Child = new FText("x") };

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void Paint_FProgressBar_UpdatesValue()
        {
            var original = new FProgressBar(value: 10f);
            var native = (ProgressBar)FElementPainter.CreateNative(original);

            var updated = new FProgressBar(value: 90f);
            FElementPainter.Paint(updated, native);

            Assert.That(native.value, Is.EqualTo(90f));
        }

        [Test]
        public void Paint_FScrollView_UpdatesMode()
        {
            var original = new FScrollView(FScrollDirection.Vertical) { Child = new FText("x") };
            var native = (ScrollView)FElementPainter.CreateNative(original);

            var updated = new FScrollView(FScrollDirection.Horizontal) { Child = new FText("x") };
            FElementPainter.Paint(updated, native);

            Assert.That(native.mode, Is.EqualTo(ScrollViewMode.Horizontal));
        }

        [Test]
        public void CreateNative_FSpinner_ReturnsVisualElement()
        {
            var element = new FSpinner();

            var native = FElementPainter.CreateNative(element);

            Assert.That(native, Is.InstanceOf<VisualElement>());
        }

#if FRAM3_PURE_TESTS
        [Test]
        public void CreateNative_FText_WithFontSize_SetsFontSize()
        {
            var element = new FText("x", new FTextStyle(FontSize: 24f));

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.fontSize, Is.EqualTo(24f));
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
        public void CreateNative_FProgressBar_SetsValueAndRange()
        {
            var element = new FProgressBar(value: 75f, min: 0f, max: 100f);

            var native = (ProgressBar)FElementPainter.CreateNative(element);

            Assert.That(native.value, Is.EqualTo(75f));
            Assert.That(native.lowValue, Is.EqualTo(0f));
            Assert.That(native.highValue, Is.EqualTo(100f));
        }

        [Test]
        public void CreateNative_FProgressBar_WithTitle_SetsTitle()
        {
            var element = new FProgressBar(value: 50f, title: "Loading");

            var native = (ProgressBar)FElementPainter.CreateNative(element);

            Assert.That(native.title, Is.EqualTo("Loading"));
        }

        [Test]
        public void CreateNative_FScrollView_DefaultMode_IsVertical()
        {
            var element = new FScrollView { Child = new FText("x") };

            var native = (ScrollView)FElementPainter.CreateNative(element);

            Assert.That(native.mode, Is.EqualTo(ScrollViewMode.Vertical));
        }

        [Test]
        public void CreateNative_FScrollView_Horizontal_SetsHorizontalMode()
        {
            var element = new FScrollView(FScrollDirection.Horizontal) { Child = new FText("x") };

            var native = (ScrollView)FElementPainter.CreateNative(element);

            Assert.That(native.mode, Is.EqualTo(ScrollViewMode.Horizontal));
        }

        [Test]
        public void CreateNative_FScrollView_Both_SetsVerticalAndHorizontalMode()
        {
            var element = new FScrollView(FScrollDirection.Both) { Child = new FText("x") };

            var native = (ScrollView)FElementPainter.CreateNative(element);

            Assert.That(native.mode, Is.EqualTo(ScrollViewMode.VerticalAndHorizontal));
        }

        [Test]
        public void CreateNative_FImage_WithDimensions_SetsWidthAndHeight()
        {
            var element = new FImage(width: 64f, height: 32f);

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.width, Is.EqualTo(64f));
            Assert.That(native.style.height, Is.EqualTo(32f));
        }

        [Test]
        public void CreateNative_FIcon_WithDimensions_SetsWidthAndHeight()
        {
            var element = new FIcon(width: 24f, height: 24f);

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.width, Is.EqualTo(24f));
            Assert.That(native.style.height, Is.EqualTo(24f));
        }

        [Test]
        public void CreateNative_FExpanded_SetsFlexGrow()
        {
            var element = new FExpanded(flex: 2f) { Child = new FText("x") };

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.flexGrow, Is.EqualTo(2f));
        }

        [Test]
        public void CreateNative_FExpanded_DefaultFlex_SetsFlexGrowToOne()
        {
            var element = new FExpanded { Child = new FText("x") };

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.flexGrow, Is.EqualTo(1f));
        }

        [Test]
        public void CreateNative_FDivider_Horizontal_SetsHeight()
        {
            var element = new FDivider(thickness: 2f);

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.height, Is.EqualTo(2f));
        }

        [Test]
        public void CreateNative_FDivider_Vertical_SetsWidth()
        {
            var element = new FDivider(axis: FDividerAxis.Vertical, thickness: 2f);

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.width, Is.EqualTo(2f));
        }

        [Test]
        public void CreateNative_FDivider_WithColor_SetsBackgroundColor()
        {
            var element = new FDivider(color: new FColor(0f, 0f, 0f));

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.backgroundColor.HasValue, Is.True);
        }

        [Test]
        public void CreateNative_FDivider_WithIndent_SetsMargins()
        {
            var element = new FDivider(indent: 8f);

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.style.marginLeft, Is.EqualTo(8f));
            Assert.That(native.style.marginRight, Is.EqualTo(8f));
        }

        [Test]
        public void CreateNative_FTooltip_SetsTooltip()
        {
            var element = new FTooltip("Some tip") { Child = new FText("x") };

            var native = FElementPainter.CreateNative(element);

            Assert.That(native.tooltip, Is.EqualTo("Some tip"));
        }

        [Test]
        public void ApplyAsStackChild_SetsPositionAbsolute()
        {
            var native = new VisualElement();

            FElementPainter.ApplyAsStackChild(native);

            Assert.That(native.style.position, Is.EqualTo(Position.Absolute));
        }
#endif
    }
}
