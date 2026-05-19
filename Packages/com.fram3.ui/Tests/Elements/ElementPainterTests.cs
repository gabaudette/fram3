#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Rendering.Internal;
using Fram3.UI.Styling;
using StylingTheme = Fram3.UI.Styling.Theme;
using NUnit.Framework;
using UnityEngine.UIElements;
using Button = Fram3.UI.Elements.Input.Button;
using ProgressBar = Fram3.UI.Elements.Content.ProgressBar;
using ScrollView = Fram3.UI.Elements.Content.ScrollView;
using Column = Fram3.UI.Elements.Layout.Column;
using Row = Fram3.UI.Elements.Layout.Row;
using UIProgressBar = UnityEngine.UIElements.ProgressBar;
using UIScrollView = UnityEngine.UIElements.ScrollView;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class ElementPainterTests
    {
        [Test]
        public void CreateNative_FText_ReturnsLabel()
        {
            var element = new Text("Hello");

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native, Is.InstanceOf<Label>());
        }

        [Test]
        public void CreateNative_FText_SetsLabelText()
        {
            var element = new Text("World");

            var native = (Label)ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.text, Is.EqualTo("World"));
        }

        [Test]
        public void CreateNative_FButton_WithNullOnPressed_DoesNotThrow()
        {
            var element = new Button("Cancel");

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element, StylingTheme.Default));
        }

        [Test]
        public void CreateNative_FColumn_ReturnsVisualElement()
        {
            var element = new Column();

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FRow_ReturnsVisualElement()
        {
            var element = new Row();

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FPadding_ReturnsVisualElement()
        {
            var element = new Padding(EdgeInsets.All(8f));

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FSizedBox_ReturnsVisualElement()
        {
            var element = SizedBox.FromSize(width: 100f, height: 50f);

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FCenter_ReturnsVisualElement()
        {
            var element = new Center();

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FContainer_ReturnsVisualElement()
        {
            var element = new Container(width: 200f, height: 100f);

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FContainer_WithDecoration_DoesNotThrow()
        {
            var element = new Container(
                decoration: new BoxDecoration(Color: new FrameColor(1f, 0f, 0f)));

            Assert.DoesNotThrow(() => ElementPainter.CreateNative(element, StylingTheme.Default));
        }

        [Test]
        public void Paint_FText_UpdatesLabelText()
        {
            var original = new Text("Before");
            var label = (Label)ElementPainter.CreateNative(original, StylingTheme.Default);

            var updated = new Text("After");
            ElementPainter.Paint(updated, label, StylingTheme.Default);

            Assert.That(label.text, Is.EqualTo("After"));
        }

        [Test]
        public void CreateNative_FProgressBar_ReturnsProgressBar()
        {
            var element = new ProgressBar(value: 50f);

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native, Is.InstanceOf<UIProgressBar>());
        }

        [Test]
        public void CreateNative_FScrollView_ReturnsScrollView()
        {
            var element = new ScrollView { Child = new Text("x") };

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native, Is.InstanceOf<UIScrollView>());
        }

        [Test]
        public void CreateNative_FImage_ReturnsImage()
        {
            var element = new FrameImage();

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native, Is.InstanceOf<Image>());
        }

        [Test]
        public void CreateNative_FIcon_ReturnsImage()
        {
            var element = new Icon();

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native, Is.InstanceOf<Image>());
        }

        [Test]
        public void CreateNative_FStack_ReturnsVisualElement()
        {
            var element = new Stack { Children = new Element[] { new Text("a") } };

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FExpanded_ReturnsVisualElement()
        {
            var element = new Expanded { Child = new Text("x") };

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FDivider_ReturnsVisualElement()
        {
            var element = new Divider();

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void CreateNative_FTooltip_ReturnsVisualElement()
        {
            var element = new Tooltip("tip") { Child = new Text("x") };

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.GetType(), Is.EqualTo(typeof(VisualElement)));
        }

        [Test]
        public void Paint_FProgressBar_UpdatesValue()
        {
            var original = new ProgressBar(value: 10f);
            var native = (UIProgressBar)ElementPainter.CreateNative(original, StylingTheme.Default);

            var updated = new ProgressBar(value: 90f);
            ElementPainter.Paint(updated, native, StylingTheme.Default);

            Assert.That(native.value, Is.EqualTo(90f));
        }

        [Test]
        public void Paint_FScrollView_UpdatesMode()
        {
            var original = new ScrollView { Child = new Text("x") };
            var native = (UIScrollView)ElementPainter.CreateNative(original, StylingTheme.Default);

            var updated = new ScrollView(ScrollDirection.Horizontal) { Child = new Text("x") };
            ElementPainter.Paint(updated, native, StylingTheme.Default);

            Assert.That(native.mode, Is.EqualTo(ScrollViewMode.Horizontal));
        }

        [Test]
        public void CreateNative_FSpinner_ReturnsVisualElement()
        {
            var element = new Spinner();

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native, Is.InstanceOf<VisualElement>());
        }

#if FRAM3_PURE_TESTS
        [Test]
        public void CreateNative_FText_WithFontSize_SetsFontSize()
        {
            var element = new Text("x", new TextStyle(FontSize: 24f));

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.fontSize, Is.EqualTo(24f));
        }

        [Test]
        public void CreateNative_FColumn_SetsColumnFlexDirection()
        {
            var element = new Column();

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.flexDirection, Is.EqualTo(FlexDirection.Column));
        }

        [Test]
        public void CreateNative_FRow_SetsRowFlexDirection()
        {
            var element = new Row();

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.flexDirection, Is.EqualTo(FlexDirection.Row));
        }

        [Test]
        public void CreateNative_FColumn_CenterMainAxis_SetsJustifyCenter()
        {
            var element = new Column(mainAxisAlignment: MainAxisAlignment.Center);

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.justifyContent, Is.EqualTo(Justify.Center));
        }

        [Test]
        public void CreateNative_FColumn_StretchCrossAxis_SetsAlignItemsStretch()
        {
            var element = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch);

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.alignItems, Is.EqualTo(Align.Stretch));
        }

        [Test]
        public void CreateNative_FPadding_SetsPaddingOnAllEdges()
        {
            var element = new Padding(new EdgeInsets(4f, 8f, 12f, 16f));

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.paddingTop, Is.EqualTo(4f));
            Assert.That(native.style.paddingRight, Is.EqualTo(8f));
            Assert.That(native.style.paddingBottom, Is.EqualTo(12f));
            Assert.That(native.style.paddingLeft, Is.EqualTo(16f));
        }

        [Test]
        public void CreateNative_FSizedBoxExpand_SetsFlexGrow()
        {
            var element = SizedBox.Expand();

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.flexGrow, Is.EqualTo(1f));
        }

        [Test]
        public void CreateNative_FSizedBoxFromSize_SetsWidthAndHeight()
        {
            var element = SizedBox.FromSize(width: 100f, height: 50f);

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.width, Is.EqualTo(100f));
            Assert.That(native.style.height, Is.EqualTo(50f));
        }

        [Test]
        public void CreateNative_FCenter_SetsAlignItemsCenter()
        {
            var element = new Center();

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.alignItems, Is.EqualTo(Align.Center));
            Assert.That(native.style.justifyContent, Is.EqualTo(Justify.Center));
        }

        [Test]
        public void CreateNative_FContainer_SetsWidthAndHeight()
        {
            var element = new Container(width: 200f, height: 100f);

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.width, Is.EqualTo(200f));
            Assert.That(native.style.height, Is.EqualTo(100f));
        }

        [Test]
        public void CreateNative_FContainer_WithDecoration_SetsBackgroundColor()
        {
            var element = new Container(
                decoration: new BoxDecoration(Color: new FrameColor(1f, 0f, 0f)));

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.backgroundColor.HasValue, Is.True);
            var bg = native.style.backgroundColor!.Value;
            Assert.That(bg.r, Is.EqualTo(1f).Within(0.001f));
            Assert.That(bg.g, Is.EqualTo(0f).Within(0.001f));
            Assert.That(bg.b, Is.EqualTo(0f).Within(0.001f));
        }

        [Test]
        public void CreateNative_FProgressBar_SetsValueAndRange()
        {
            var element = new ProgressBar(value: 75f, min: 0f, max: 100f);

            var native = (UIProgressBar)ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.value, Is.EqualTo(75f));
            Assert.That(native.lowValue, Is.EqualTo(0f));
            Assert.That(native.highValue, Is.EqualTo(100f));
        }

        [Test]
        public void CreateNative_FProgressBar_WithTitle_SetsTitle()
        {
            var element = new ProgressBar(value: 50f, title: "Loading");

            var native = (UIProgressBar)ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.title, Is.EqualTo("Loading"));
        }

        [Test]
        public void CreateNative_FScrollView_DefaultMode_IsVertical()
        {
            var element = new ScrollView { Child = new Text("x") };

            var native = (UIScrollView)ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.mode, Is.EqualTo(ScrollViewMode.Vertical));
        }

        [Test]
        public void CreateNative_FScrollView_Horizontal_SetsHorizontalMode()
        {
            var element = new ScrollView(ScrollDirection.Horizontal) { Child = new Text("x") };

            var native = (UIScrollView)ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.mode, Is.EqualTo(ScrollViewMode.Horizontal));
        }

        [Test]
        public void CreateNative_FScrollView_Both_SetsVerticalAndHorizontalMode()
        {
            var element = new ScrollView(ScrollDirection.Both) { Child = new Text("x") };

            var native = (UIScrollView)ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.mode, Is.EqualTo(ScrollViewMode.VerticalAndHorizontal));
        }

        [Test]
        public void CreateNative_FImage_WithDimensions_SetsWidthAndHeight()
        {
            var element = new FrameImage(width: 64f, height: 32f);

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.width, Is.EqualTo(64f));
            Assert.That(native.style.height, Is.EqualTo(32f));
        }

        [Test]
        public void CreateNative_FIcon_WithDimensions_SetsWidthAndHeight()
        {
            var element = new Icon(width: 24f, height: 24f);

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.width, Is.EqualTo(24f));
            Assert.That(native.style.height, Is.EqualTo(24f));
        }

        [Test]
        public void CreateNative_FExpanded_SetsFlexGrow()
        {
            var element = new Expanded(flex: 2f) { Child = new Text("x") };

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.flexGrow, Is.EqualTo(2f));
        }

        [Test]
        public void CreateNative_FExpanded_DefaultFlex_SetsFlexGrowToOne()
        {
            var element = new Expanded { Child = new Text("x") };

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.flexGrow, Is.EqualTo(1f));
        }

        [Test]
        public void CreateNative_FDivider_Horizontal_SetsHeight()
        {
            var element = new Divider(thickness: 2f);

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.height, Is.EqualTo(2f));
        }

        [Test]
        public void CreateNative_FDivider_Vertical_SetsWidth()
        {
            var element = new Divider(axis: DividerAxis.Vertical, thickness: 2f);

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.width, Is.EqualTo(2f));
        }

        [Test]
        public void CreateNative_FDivider_WithColor_SetsBackgroundColor()
        {
            var element = new Divider(color: new FrameColor(0f, 0f, 0f));

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.backgroundColor.HasValue, Is.True);
        }

        [Test]
        public void CreateNative_FDivider_WithIndent_SetsMargins()
        {
            var element = new Divider(indent: 8f);

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.style.marginLeft, Is.EqualTo(8f));
            Assert.That(native.style.marginRight, Is.EqualTo(8f));
        }

        [Test]
        public void CreateNative_FTooltip_SetsTooltip()
        {
            var element = new Tooltip("Some tip") { Child = new Text("x") };

            var native = ElementPainter.CreateNative(element, StylingTheme.Default);

            Assert.That(native.tooltip, Is.EqualTo("Some tip"));
        }

        [Test]
        public void ApplyAsStackChild_SetsPositionAbsolute()
        {
            var native = new VisualElement();

            ElementPainter.ApplyAsStackChild(native);

            Assert.That(native.style.position, Is.EqualTo(Position.Absolute));
        }

        // -----------------------------------------------------------------------
        // PatchNativeGrid
        // -----------------------------------------------------------------------

        private static Grid<int> MakeGrid(int columnCount, int itemCount,
            float colSpacing = 0f, float rowSpacing = 0f)
        {
            var items = new int[itemCount];
            for (var i = 0; i < itemCount; i++) items[i] = i;
            return new Grid<int>(
                columnCount: columnCount,
                items: items,
                itemBuilder: i => new Text(i.ToString()),
                columnSpacing: colSpacing,
                rowSpacing: rowSpacing
            );
        }

        [Test]
        public void CreateNative_Grid_StoresGridStateInUserData()
        {
            var grid = MakeGrid(columnCount: 2, itemCount: 4);

            var native = ElementPainter.CreateNative(grid, StylingTheme.Default);

            Assert.That(native.userData, Is.Not.Null);
        }

        [Test]
        public void CreateNative_Grid_CorrectRowCount()
        {
            // 4 items, 2 columns -> 2 rows
            var grid = MakeGrid(columnCount: 2, itemCount: 4);

            var native = ElementPainter.CreateNative(grid, StylingTheme.Default);

            Assert.That(native.childCount, Is.EqualTo(2));
        }

        [Test]
        public void Paint_Grid_SameStructure_DoesNotClearContainer()
        {
            var grid = MakeGrid(columnCount: 2, itemCount: 4);
            var native = ElementPainter.CreateNative(grid, StylingTheme.Default);
            var firstRow = native[0];

            // Paint with same column count and item count
            ElementPainter.Paint(grid, native, StylingTheme.Default);

            Assert.That(native[0], Is.SameAs(firstRow), "Row VisualElement should be reused");
        }

        [Test]
        public void Paint_Grid_ItemCountIncreases_AppendsRows()
        {
            var grid2 = MakeGrid(columnCount: 2, itemCount: 2);
            var native = ElementPainter.CreateNative(grid2, StylingTheme.Default);
            Assert.That(native.childCount, Is.EqualTo(1));

            var grid4 = MakeGrid(columnCount: 2, itemCount: 4);
            ElementPainter.Paint(grid4, native, StylingTheme.Default);

            Assert.That(native.childCount, Is.EqualTo(2));
        }

        [Test]
        public void Paint_Grid_ItemCountDecreases_RemovesRows()
        {
            var grid4 = MakeGrid(columnCount: 2, itemCount: 4);
            var native = ElementPainter.CreateNative(grid4, StylingTheme.Default);
            Assert.That(native.childCount, Is.EqualTo(2));

            var grid2 = MakeGrid(columnCount: 2, itemCount: 2);
            ElementPainter.Paint(grid2, native, StylingTheme.Default);

            Assert.That(native.childCount, Is.EqualTo(1));
        }

        [Test]
        public void Paint_Grid_ColumnCountChanges_RebuildsContainer()
        {
            var grid2 = MakeGrid(columnCount: 2, itemCount: 4);
            var native = ElementPainter.CreateNative(grid2, StylingTheme.Default);
            var firstRow = native[0];

            var grid4 = MakeGrid(columnCount: 4, itemCount: 4);
            ElementPainter.Paint(grid4, native, StylingTheme.Default);

            Assert.That(native[0], Is.Not.SameAs(firstRow), "Column change should trigger full rebuild");
        }

        [Test]
        public void Paint_Grid_RowSpacingChanges_RebuildsContainer()
        {
            var gridNoSpacing = MakeGrid(columnCount: 2, itemCount: 4);
            var native = ElementPainter.CreateNative(gridNoSpacing, StylingTheme.Default);
            var firstRow = native[0];

            var gridSpacing = MakeGrid(columnCount: 2, itemCount: 4, rowSpacing: 8f);
            ElementPainter.Paint(gridSpacing, native, StylingTheme.Default);

            Assert.That(native[0], Is.Not.SameAs(firstRow), "RowSpacing change should trigger full rebuild");
        }

        [Test]
        public void Paint_Grid_ZeroItems_EmptyContainer()
        {
            var grid2 = MakeGrid(columnCount: 2, itemCount: 2);
            var native = ElementPainter.CreateNative(grid2, StylingTheme.Default);

            var grid0 = MakeGrid(columnCount: 2, itemCount: 0);
            ElementPainter.Paint(grid0, native, StylingTheme.Default);

            Assert.That(native.childCount, Is.EqualTo(0));
        }

        [Test]
        public void Paint_Grid_ZeroToNonZero_AddsRows()
        {
            var grid0 = MakeGrid(columnCount: 2, itemCount: 0);
            var native = ElementPainter.CreateNative(grid0, StylingTheme.Default);
            Assert.That(native.childCount, Is.EqualTo(0));

            var grid4 = MakeGrid(columnCount: 2, itemCount: 4);
            ElementPainter.Paint(grid4, native, StylingTheme.Default);

            Assert.That(native.childCount, Is.EqualTo(2));
        }

        [Test]
        public void Paint_Grid_WithRowSpacing_ItemCountIncreases_AddsRowAndSpacer()
        {
            // 2 items, 2 cols, 1 row -> childCount = 1
            var grid2 = MakeGrid(columnCount: 2, itemCount: 2, rowSpacing: 4f);
            var native = ElementPainter.CreateNative(grid2, StylingTheme.Default);
            Assert.That(native.childCount, Is.EqualTo(1));

            // 4 items, 2 cols, 2 rows -> childCount = 3 (row + spacer + row)
            var grid4 = MakeGrid(columnCount: 2, itemCount: 4, rowSpacing: 4f);
            ElementPainter.Paint(grid4, native, StylingTheme.Default);

            Assert.That(native.childCount, Is.EqualTo(3));
        }

        [Test]
        public void Paint_Grid_WithRowSpacing_ItemCountDecreases_RemovesRowAndSpacer()
        {
            // 4 items, 2 cols, rowSpacing -> childCount = 3
            var grid4 = MakeGrid(columnCount: 2, itemCount: 4, rowSpacing: 4f);
            var native = ElementPainter.CreateNative(grid4, StylingTheme.Default);
            Assert.That(native.childCount, Is.EqualTo(3));

            // 2 items -> childCount = 1
            var grid2 = MakeGrid(columnCount: 2, itemCount: 2, rowSpacing: 4f);
            ElementPainter.Paint(grid2, native, StylingTheme.Default);

            Assert.That(native.childCount, Is.EqualTo(1));
        }
#endif
    }
}