#nullable enable
using Fram3.UI.Animation;
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Animation
{
    [TestFixture]
    internal sealed class FLerpTests
    {
        private const float Tolerance = 0.0001f;

        [Test]
        public void Float_AtZero_ReturnsA()
        {
            Assert.That(FLerp.Float(0f, 10f, 0f), Is.EqualTo(0f).Within(Tolerance));
        }

        [Test]
        public void Float_AtOne_ReturnsB()
        {
            Assert.That(FLerp.Float(0f, 10f, 1f), Is.EqualTo(10f).Within(Tolerance));
        }

        [Test]
        public void Float_AtHalf_ReturnsMidpoint()
        {
            Assert.That(FLerp.Float(0f, 10f, 0.5f), Is.EqualTo(5f).Within(Tolerance));
        }

        [Test]
        public void NullableFloat_BothNull_ReturnsNull()
        {
            Assert.That(FLerp.NullableFloat(null, null, 0.5f), Is.Null);
        }

        [Test]
        public void NullableFloat_OnlyANull_UsesB()
        {
            Assert.That(FLerp.NullableFloat(null, 10f, 1f), Is.EqualTo(10f).Within(Tolerance));
        }

        [Test]
        public void NullableFloat_OnlyBNull_UsesA()
        {
            Assert.That(FLerp.NullableFloat(10f, null, 0f), Is.EqualTo(10f).Within(Tolerance));
        }

        [Test]
        public void Color_BlendsMidpoint()
        {
            var a = new FColor(0f, 0f, 0f, 1f);
            var b = new FColor(1f, 1f, 1f, 1f);

            var result = FLerp.Color(a, b, 0.5f);

            Assert.That(result.R, Is.EqualTo(0.5f).Within(Tolerance));
            Assert.That(result.G, Is.EqualTo(0.5f).Within(Tolerance));
            Assert.That(result.B, Is.EqualTo(0.5f).Within(Tolerance));
        }

        [Test]
        public void NullableColor_BothNull_ReturnsNull()
        {
            Assert.That(FLerp.NullableColor(null, null, 0.5f), Is.Null);
        }

        [Test]
        public void Color_AtZero_ReturnsA()
        {
            var a = FColor.Red;
            var b = FColor.Blue;

            Assert.That(FLerp.Color(a, b, 0f), Is.EqualTo(a));
        }

        [Test]
        public void Color_AtOne_ReturnsB()
        {
            var a = FColor.Red;
            var b = FColor.Blue;

            Assert.That(FLerp.Color(a, b, 1f), Is.EqualTo(b));
        }

        [Test]
        public void EdgeInsets_BlendsMidpoint()
        {
            var a = FEdgeInsets.All(0f);
            var b = FEdgeInsets.All(20f);

            var result = FLerp.EdgeInsets(a, b, 0.5f);

            Assert.That(result.Top, Is.EqualTo(10f).Within(Tolerance));
            Assert.That(result.Right, Is.EqualTo(10f).Within(Tolerance));
            Assert.That(result.Bottom, Is.EqualTo(10f).Within(Tolerance));
            Assert.That(result.Left, Is.EqualTo(10f).Within(Tolerance));
        }

        [Test]
        public void Alignment_BlendsMidpoint()
        {
            var a = FAlignment.TopLeft;
            var b = FAlignment.BottomRight;

            var result = FLerp.Alignment(a, b, 0.5f);

            Assert.That(result.X, Is.EqualTo(0f).Within(Tolerance));
            Assert.That(result.Y, Is.EqualTo(0f).Within(Tolerance));
        }

        [Test]
        public void BorderRadius_BlendsMidpoint()
        {
            var a = FBorderRadius.Zero;
            var b = FBorderRadius.All(20f);

            var result = FLerp.BorderRadius(a, b, 0.5f);

            Assert.That(result.TopLeft, Is.EqualTo(10f).Within(Tolerance));
            Assert.That(result.TopRight, Is.EqualTo(10f).Within(Tolerance));
            Assert.That(result.BottomRight, Is.EqualTo(10f).Within(Tolerance));
            Assert.That(result.BottomLeft, Is.EqualTo(10f).Within(Tolerance));
        }

        [Test]
        public void NullableBorderRadius_BothNull_ReturnsNull()
        {
            Assert.That(FLerp.NullableBorderRadius(null, null, 0.5f), Is.Null);
        }

        [Test]
        public void Border_BlendsMidpoint()
        {
            var a = new FBorder(FColor.Black, 0f);
            var b = new FBorder(FColor.White, 10f);

            var result = FLerp.Border(a, b, 0.5f);

            Assert.That(result.Width, Is.EqualTo(5f).Within(Tolerance));
            Assert.That(result.Color.R, Is.EqualTo(0.5f).Within(Tolerance));
        }

        [Test]
        public void NullableBorder_BothNull_ReturnsNull()
        {
            Assert.That(FLerp.NullableBorder(null, null, 0.5f), Is.Null);
        }

        [Test]
        public void Shadow_BlendsMidpoint()
        {
            var a = new FShadow(FColor.Black, 0f, 0f, 0f);
            var b = new FShadow(FColor.White, 10f, 10f, 10f);

            var result = FLerp.Shadow(a, b, 0.5f);

            Assert.That(result.OffsetX, Is.EqualTo(5f).Within(Tolerance));
            Assert.That(result.OffsetY, Is.EqualTo(5f).Within(Tolerance));
            Assert.That(result.BlurRadius, Is.EqualTo(5f).Within(Tolerance));
        }

        [Test]
        public void NullableShadow_BothNull_ReturnsNull()
        {
            Assert.That(FLerp.NullableShadow(null, null, 0.5f), Is.Null);
        }

        [Test]
        public void BoxDecoration_BlendsAllComponents()
        {
            var a = new FBoxDecoration(
                FColor.Black,
                new FBorder(FColor.Black, 0f),
                FBorderRadius.Zero,
                new FShadow(FColor.Black, 0f, 0f, 0f)
            );
            var b = new FBoxDecoration(
                FColor.White,
                new FBorder(FColor.White, 10f),
                FBorderRadius.All(20f),
                new FShadow(FColor.White, 10f, 10f, 10f)
            );

            var result = FLerp.BoxDecoration(a, b, 0.5f);

            Assert.That(result.Color!.Value.R, Is.EqualTo(0.5f).Within(Tolerance));
            Assert.That(result.Border!.Width, Is.EqualTo(5f).Within(Tolerance));
            Assert.That(result.BorderRadius!.Value.TopLeft, Is.EqualTo(10f).Within(Tolerance));
            Assert.That(result.Shadow!.OffsetX, Is.EqualTo(5f).Within(Tolerance));
        }

        [Test]
        public void BoxDecoration_NullComponents_ArePreservedAsNull()
        {
            var a = FBoxDecoration.None;
            var b = FBoxDecoration.None;

            var result = FLerp.BoxDecoration(a, b, 0.5f);

            Assert.That(result.Color, Is.Null);
            Assert.That(result.Border, Is.Null);
            Assert.That(result.BorderRadius, Is.Null);
            Assert.That(result.Shadow, Is.Null);
        }

        [Test]
        public void TextStyle_BlendsNumericProperties()
        {
            var a = new FTextStyle(FontSize: 10f, Color: FColor.Black, LetterSpacing: 0f);
            var b = new FTextStyle(FontSize: 20f, Color: FColor.White, LetterSpacing: 4f);

            var result = FLerp.TextStyle(a, b, 0.5f);

            Assert.That(result.FontSize, Is.EqualTo(15f).Within(Tolerance));
            Assert.That(result.Color!.Value.R, Is.EqualTo(0.5f).Within(Tolerance));
            Assert.That(result.LetterSpacing, Is.EqualTo(2f).Within(Tolerance));
        }

        [Test]
        public void TextStyle_BooleanProperties_SnapAtHalf()
        {
            var a = new FTextStyle(Bold: false);
            var b = new FTextStyle(Bold: true);

            var resultBeforeHalf = FLerp.TextStyle(a, b, 0.4f);
            var resultAtHalf = FLerp.TextStyle(a, b, 0.5f);

            Assert.That(resultBeforeHalf.Bold, Is.False);
            Assert.That(resultAtHalf.Bold, Is.True);
        }
    }
}
