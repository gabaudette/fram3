#nullable enable
using Fram3.UI.Animation;
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Animation
{
    [TestFixture]
    internal sealed class LerpTests
    {
        private const float Tolerance = 0.0001f;

        [Test]
        public void Float_AtZero_ReturnsA()
        {
            Assert.That(Lerp.Float(0f, 10f, 0f), Is.EqualTo(0f).Within(Tolerance));
        }

        [Test]
        public void Float_AtOne_ReturnsB()
        {
            Assert.That(Lerp.Float(0f, 10f, 1f), Is.EqualTo(10f).Within(Tolerance));
        }

        [Test]
        public void Float_AtHalf_ReturnsMidpoint()
        {
            Assert.That(Lerp.Float(0f, 10f, 0.5f), Is.EqualTo(5f).Within(Tolerance));
        }

        [Test]
        public void NullableFloat_BothNull_ReturnsNull()
        {
            Assert.That(Lerp.NullableFloat(null, null, 0.5f), Is.Null);
        }

        [Test]
        public void NullableFloat_OnlyANull_UsesB()
        {
            Assert.That(Lerp.NullableFloat(null, 10f, 1f), Is.EqualTo(10f).Within(Tolerance));
        }

        [Test]
        public void NullableFloat_OnlyBNull_UsesA()
        {
            Assert.That(Lerp.NullableFloat(10f, null, 0f), Is.EqualTo(10f).Within(Tolerance));
        }

        [Test]
        public void Color_BlendsMidpoint()
        {
            var a = new FrameColor(0f, 0f, 0f, 1f);
            var b = new FrameColor(1f, 1f, 1f, 1f);

            var result = Lerp.Color(a, b, 0.5f);

            Assert.That(result.R, Is.EqualTo(0.5f).Within(Tolerance));
            Assert.That(result.G, Is.EqualTo(0.5f).Within(Tolerance));
            Assert.That(result.B, Is.EqualTo(0.5f).Within(Tolerance));
        }

        [Test]
        public void NullableColor_BothNull_ReturnsNull()
        {
            Assert.That(Lerp.NullableColor(null, null, 0.5f), Is.Null);
        }

        [Test]
        public void Color_AtZero_ReturnsA()
        {
            var a = FrameColor.Red;
            var b = FrameColor.Blue;

            Assert.That(Lerp.Color(a, b, 0f), Is.EqualTo(a));
        }

        [Test]
        public void Color_AtOne_ReturnsB()
        {
            var a = FrameColor.Red;
            var b = FrameColor.Blue;

            Assert.That(Lerp.Color(a, b, 1f), Is.EqualTo(b));
        }

        [Test]
        public void EdgeInsets_BlendsMidpoint()
        {
            var a = EdgeInsets.All(0f);
            var b = EdgeInsets.All(20f);

            var result = Lerp.EdgeInsets(a, b, 0.5f);

            Assert.That(result.Top, Is.EqualTo(10f).Within(Tolerance));
            Assert.That(result.Right, Is.EqualTo(10f).Within(Tolerance));
            Assert.That(result.Bottom, Is.EqualTo(10f).Within(Tolerance));
            Assert.That(result.Left, Is.EqualTo(10f).Within(Tolerance));
        }

        [Test]
        public void Alignment_BlendsMidpoint()
        {
            var a = Alignment.TopLeft;
            var b = Alignment.BottomRight;

            var result = Lerp.Alignment(a, b, 0.5f);

            Assert.That(result.X, Is.EqualTo(0f).Within(Tolerance));
            Assert.That(result.Y, Is.EqualTo(0f).Within(Tolerance));
        }

        [Test]
        public void BorderRadius_BlendsMidpoint()
        {
            var a = BorderRadius.Zero;
            var b = BorderRadius.All(20f);

            var result = Lerp.BorderRadius(a, b, 0.5f);

            Assert.That(result.TopLeft, Is.EqualTo(10f).Within(Tolerance));
            Assert.That(result.TopRight, Is.EqualTo(10f).Within(Tolerance));
            Assert.That(result.BottomRight, Is.EqualTo(10f).Within(Tolerance));
            Assert.That(result.BottomLeft, Is.EqualTo(10f).Within(Tolerance));
        }

        [Test]
        public void NullableBorderRadius_BothNull_ReturnsNull()
        {
            Assert.That(Lerp.NullableBorderRadius(null, null, 0.5f), Is.Null);
        }

        [Test]
        public void Border_BlendsMidpoint()
        {
            var a = new Border(FrameColor.Black, 0f);
            var b = new Border(FrameColor.White, 10f);

            var result = Lerp.Border(a, b, 0.5f);

            Assert.That(result.Width, Is.EqualTo(5f).Within(Tolerance));
            Assert.That(result.Color.R, Is.EqualTo(0.5f).Within(Tolerance));
        }

        [Test]
        public void NullableBorder_BothNull_ReturnsNull()
        {
            Assert.That(Lerp.NullableBorder(null, null, 0.5f), Is.Null);
        }

        [Test]
        public void Shadow_BlendsMidpoint()
        {
            var a = new Shadow(FrameColor.Black, 0f, 0f, 0f);
            var b = new Shadow(FrameColor.White, 10f, 10f, 10f);

            var result = Lerp.Shadow(a, b, 0.5f);

            Assert.That(result.OffsetX, Is.EqualTo(5f).Within(Tolerance));
            Assert.That(result.OffsetY, Is.EqualTo(5f).Within(Tolerance));
            Assert.That(result.BlurRadius, Is.EqualTo(5f).Within(Tolerance));
        }

        [Test]
        public void NullableShadow_BothNull_ReturnsNull()
        {
            Assert.That(Lerp.NullableShadow(null, null, 0.5f), Is.Null);
        }

        [Test]
        public void BoxDecoration_BlendsAllComponents()
        {
            var a = new BoxDecoration(
                FrameColor.Black,
                new Border(FrameColor.Black, 0f),
                BorderRadius.Zero,
                new Shadow(FrameColor.Black, 0f, 0f, 0f)
            );
            var b = new BoxDecoration(
                FrameColor.White,
                new Border(FrameColor.White, 10f),
                BorderRadius.All(20f),
                new Shadow(FrameColor.White, 10f, 10f, 10f)
            );

            var result = Lerp.BoxDecoration(a, b, 0.5f);

            Assert.That(result.Color!.Value.R, Is.EqualTo(0.5f).Within(Tolerance));
            Assert.That(result.Border!.Width, Is.EqualTo(5f).Within(Tolerance));
            Assert.That(result.BorderRadius!.Value.TopLeft, Is.EqualTo(10f).Within(Tolerance));
            Assert.That(result.Shadow!.OffsetX, Is.EqualTo(5f).Within(Tolerance));
        }

        [Test]
        public void BoxDecoration_NullComponents_ArePreservedAsNull()
        {
            var a = BoxDecoration.None;
            var b = BoxDecoration.None;

            var result = Lerp.BoxDecoration(a, b, 0.5f);

            Assert.That(result.Color, Is.Null);
            Assert.That(result.Border, Is.Null);
            Assert.That(result.BorderRadius, Is.Null);
            Assert.That(result.Shadow, Is.Null);
        }

        [Test]
        public void TextStyle_BlendsNumericProperties()
        {
            var a = new TextStyle(FontSize: 10f, Color: FrameColor.Black, LetterSpacing: 0f);
            var b = new TextStyle(FontSize: 20f, Color: FrameColor.White, LetterSpacing: 4f);

            var result = Lerp.TextStyle(a, b, 0.5f);

            Assert.That(result.FontSize, Is.EqualTo(15f).Within(Tolerance));
            Assert.That(result.Color!.Value.R, Is.EqualTo(0.5f).Within(Tolerance));
            Assert.That(result.LetterSpacing, Is.EqualTo(2f).Within(Tolerance));
        }

        [Test]
        public void TextStyle_BooleanProperties_SnapAtHalf()
        {
            var a = new TextStyle(Bold: false);
            var b = new TextStyle(Bold: true);

            var resultBeforeHalf = Lerp.TextStyle(a, b, 0.4f);
            var resultAtHalf = Lerp.TextStyle(a, b, 0.5f);

            Assert.That(resultBeforeHalf.Bold, Is.False);
            Assert.That(resultAtHalf.Bold, Is.True);
        }
    }
}
