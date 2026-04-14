using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Styling
{
    [TestFixture]
    public class TextStyleTests
    {
        [Test]
        public void DefaultConstructor_AllPropertiesAreDefault()
        {
            var style = new TextStyle();

            Assert.That(style.FontSize, Is.Null);
            Assert.That(style.Color, Is.Null);
            Assert.That(style.Bold, Is.False);
            Assert.That(style.Italic, Is.False);
            Assert.That(style.Underline, Is.False);
            Assert.That(style.LetterSpacing, Is.EqualTo(0f));
            Assert.That(style.LineHeight, Is.Null);
        }

        [Test]
        public void Constructor_SetsProperties()
        {
            var style = new TextStyle(
                FontSize: 16f,
                Color: FrameColor.White,
                Bold: true,
                Italic: true,
                Underline: true,
                LetterSpacing: 1.5f,
                LineHeight: 1.2f
            );

            Assert.That(style.FontSize, Is.EqualTo(16f));
            Assert.That(style.Color, Is.EqualTo(FrameColor.White));
            Assert.That(style.Bold, Is.True);
            Assert.That(style.Italic, Is.True);
            Assert.That(style.Underline, Is.True);
            Assert.That(style.LetterSpacing, Is.EqualTo(1.5f));
            Assert.That(style.LineHeight, Is.EqualTo(1.2f));
        }

        [Test]
        public void Inherit_AllPropertiesAreNull()
        {
            var style = TextStyle.Inherit;

            Assert.That(style.FontSize, Is.Null);
            Assert.That(style.Color, Is.Null);
        }

        [Test]
        public void RecordEquality_SameValues_AreEqual()
        {
            var a = new TextStyle(FontSize: 14f, Color: FrameColor.Black);
            var b = new TextStyle(FontSize: 14f, Color: FrameColor.Black);

            Assert.That(a, Is.EqualTo(b));
        }

        [Test]
        public void RecordEquality_DifferentValues_AreNotEqual()
        {
            var a = new TextStyle(FontSize: 14f);
            var b = new TextStyle(FontSize: 16f);

            Assert.That(a, Is.Not.EqualTo(b));
        }

        [Test]
        public void AsBold_ReturnsCopyWithBoldTrue()
        {
            var original = new TextStyle();
            var bold = original.AsBold();

            Assert.That(bold.Bold, Is.True);
            Assert.That(original.Bold, Is.False);
        }

        [Test]
        public void AsBold_PreservesOtherProperties()
        {
            var original = new TextStyle(FontSize: 14f, Color: FrameColor.Black);
            var bold = original.AsBold();

            Assert.That(bold.FontSize, Is.EqualTo(14f));
            Assert.That(bold.Color, Is.EqualTo(FrameColor.Black));
        }

        [Test]
        public void AsItalic_ReturnsCopyWithItalicTrue()
        {
            var original = new TextStyle();
            var italic = original.AsItalic();

            Assert.That(italic.Italic, Is.True);
            Assert.That(original.Italic, Is.False);
        }

        [Test]
        public void AsItalic_PreservesOtherProperties()
        {
            var original = new TextStyle(FontSize: 18f);
            var italic = original.AsItalic();

            Assert.That(italic.FontSize, Is.EqualTo(18f));
        }

        [Test]
        public void AsUnderlined_ReturnsCopyWithUnderlineTrue()
        {
            var original = new TextStyle();
            var underlined = original.AsUnderlined();

            Assert.That(underlined.Underline, Is.True);
            Assert.That(original.Underline, Is.False);
        }

        [Test]
        public void WithSize_ReturnsCopyWithGivenFontSize()
        {
            var original = new TextStyle();
            var sized = original.WithSize(20f);

            Assert.That(sized.FontSize, Is.EqualTo(20f));
            Assert.That(original.FontSize, Is.Null);
        }

        [Test]
        public void WithColor_ReturnsCopyWithGivenColor()
        {
            var original = new TextStyle();
            var colored = original.WithColor(FrameColor.White);

            Assert.That(colored.Color, Is.EqualTo(FrameColor.White));
            Assert.That(original.Color, Is.Null);
        }

        [Test]
        public void WithLetterSpacing_ReturnsCopyWithGivenSpacing()
        {
            var original = new TextStyle();
            var spaced = original.WithLetterSpacing(2.5f);

            Assert.That(spaced.LetterSpacing, Is.EqualTo(2.5f));
            Assert.That(original.LetterSpacing, Is.EqualTo(0f));
        }

        [Test]
        public void WithLineHeight_ReturnsCopyWithGivenLineHeight()
        {
            var original = new TextStyle();
            var lined = original.WithLineHeight(1.5f);

            Assert.That(lined.LineHeight, Is.EqualTo(1.5f));
            Assert.That(original.LineHeight, Is.Null);
        }

        [Test]
        public void FluentChaining_ProducesCorrectStyle()
        {
            var style = TextStyle.Inherit
                .WithSize(16f)
                .WithColor(FrameColor.Black)
                .AsBold()
                .AsItalic()
                .AsUnderlined()
                .WithLetterSpacing(1f)
                .WithLineHeight(1.4f);

            Assert.That(style.FontSize, Is.EqualTo(16f));
            Assert.That(style.Color, Is.EqualTo(FrameColor.Black));
            Assert.That(style.Bold, Is.True);
            Assert.That(style.Italic, Is.True);
            Assert.That(style.Underline, Is.True);
            Assert.That(style.LetterSpacing, Is.EqualTo(1f));
            Assert.That(style.LineHeight, Is.EqualTo(1.4f));
        }
    }
}
