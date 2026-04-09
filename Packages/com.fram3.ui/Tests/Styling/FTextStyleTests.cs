using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Styling
{
    [TestFixture]
    public class FTextStyleTests
    {
        [Test]
        public void DefaultConstructor_AllPropertiesAreDefault()
        {
            var style = new FTextStyle();

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
            var style = new FTextStyle(
                FontSize: 16f,
                Color: FColor.White,
                Bold: true,
                Italic: true,
                Underline: true,
                LetterSpacing: 1.5f,
                LineHeight: 1.2f
            );

            Assert.That(style.FontSize, Is.EqualTo(16f));
            Assert.That(style.Color, Is.EqualTo(FColor.White));
            Assert.That(style.Bold, Is.True);
            Assert.That(style.Italic, Is.True);
            Assert.That(style.Underline, Is.True);
            Assert.That(style.LetterSpacing, Is.EqualTo(1.5f));
            Assert.That(style.LineHeight, Is.EqualTo(1.2f));
        }

        [Test]
        public void Inherit_AllPropertiesAreNull()
        {
            var style = FTextStyle.Inherit;

            Assert.That(style.FontSize, Is.Null);
            Assert.That(style.Color, Is.Null);
        }

        [Test]
        public void RecordEquality_SameValues_AreEqual()
        {
            var a = new FTextStyle(FontSize: 14f, Color: FColor.Black);
            var b = new FTextStyle(FontSize: 14f, Color: FColor.Black);

            Assert.That(a, Is.EqualTo(b));
        }

        [Test]
        public void RecordEquality_DifferentValues_AreNotEqual()
        {
            var a = new FTextStyle(FontSize: 14f);
            var b = new FTextStyle(FontSize: 16f);

            Assert.That(a, Is.Not.EqualTo(b));
        }
    }
}
