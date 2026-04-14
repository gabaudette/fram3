using System;
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Styling
{
    [TestFixture]
    public class ColorTests
    {
        [Test]
        public void Constructor_SetsChannels()
        {
            var color = new FrameColor(0.1f, 0.2f, 0.3f, 0.4f);

            Assert.That(color.R, Is.EqualTo(0.1f));
            Assert.That(color.G, Is.EqualTo(0.2f));
            Assert.That(color.B, Is.EqualTo(0.3f));
            Assert.That(color.A, Is.EqualTo(0.4f));
        }

        [Test]
        public void Constructor_DefaultAlpha_IsOne()
        {
            var color = new FrameColor(1f, 0f, 0f);

            Assert.That(color.A, Is.EqualTo(1f));
        }

        [Test]
        public void FromRgba255_NormalizesChannels()
        {
            var color = FrameColor.FromRgba255(255, 128, 0, 255);

            Assert.That(color.R, Is.EqualTo(1f));
            Assert.That(color.G, Is.EqualTo(128 / 255f));
            Assert.That(color.B, Is.EqualTo(0f));
            Assert.That(color.A, Is.EqualTo(1f));
        }

        [Test]
        public void FromHex_SixDigit_ParsesCorrectly()
        {
            var color = FrameColor.FromHex("#FF8000");

            Assert.That(color.R, Is.EqualTo(1f));
            Assert.That(color.G, Is.EqualTo(128 / 255f));
            Assert.That(color.B, Is.EqualTo(0f));
            Assert.That(color.A, Is.EqualTo(1f));
        }

        [Test]
        public void FromHex_EightDigit_ParsesAlpha()
        {
            var color = FrameColor.FromHex("FF000080");

            Assert.That(color.R, Is.EqualTo(1f));
            Assert.That(color.G, Is.EqualTo(0f));
            Assert.That(color.B, Is.EqualTo(0f));
            Assert.That(color.A, Is.EqualTo(128 / 255f));
        }

        [Test]
        public void FromHex_InvalidLength_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => FrameColor.FromHex("#FFF"));
        }

        [Test]
        public void FromHex_InvalidCharacters_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => FrameColor.FromHex("#GGGGGG"));
        }

        [Test]
        public void FromHex_NullInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => FrameColor.FromHex(null!));
        }

        [Test]
        public void WithAlpha_ReturnsNewColorWithUpdatedAlpha()
        {
            var color = FrameColor.Red.WithAlpha(0.5f);

            Assert.That(color.R, Is.EqualTo(1f));
            Assert.That(color.G, Is.EqualTo(0f));
            Assert.That(color.B, Is.EqualTo(0f));
            Assert.That(color.A, Is.EqualTo(0.5f));
        }

        [Test]
        public void Equals_SameValues_ReturnsTrue()
        {
            var a = new FrameColor(0.1f, 0.2f, 0.3f, 0.4f);
            var b = new FrameColor(0.1f, 0.2f, 0.3f, 0.4f);

            Assert.That(a, Is.EqualTo(b));
        }

        [Test]
        public void Equals_DifferentValues_ReturnsFalse()
        {
            var a = FrameColor.Red;
            var b = FrameColor.Blue;

            Assert.That(a, Is.Not.EqualTo(b));
        }

        [Test]
        public void EqualityOperator_SameValues_ReturnsTrue()
        {
            Assert.That(FrameColor.White == new FrameColor(1f, 1f, 1f, 1f), Is.True);
        }

        [Test]
        public void InequalityOperator_DifferentValues_ReturnsTrue()
        {
            Assert.That(FrameColor.Black != FrameColor.White, Is.True);
        }

        [Test]
        public void StaticColors_HaveExpectedValues()
        {
            Assert.That(FrameColor.Transparent, Is.EqualTo(new FrameColor(0f, 0f, 0f, 0f)));
            Assert.That(FrameColor.White, Is.EqualTo(new FrameColor(1f, 1f, 1f, 1f)));
            Assert.That(FrameColor.Black, Is.EqualTo(new FrameColor(0f, 0f, 0f, 1f)));
            Assert.That(FrameColor.Red, Is.EqualTo(new FrameColor(1f, 0f, 0f, 1f)));
            Assert.That(FrameColor.Green, Is.EqualTo(new FrameColor(0f, 1f, 0f, 1f)));
            Assert.That(FrameColor.Blue, Is.EqualTo(new FrameColor(0f, 0f, 1f, 1f)));
        }
    }
}
