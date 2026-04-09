using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Styling
{
    [TestFixture]
    public class FAlignmentTests
    {
        [Test]
        public void Constructor_SetsXAndY()
        {
            var alignment = new FAlignment(0.5f, -0.5f);

            Assert.That(alignment.X, Is.EqualTo(0.5f));
            Assert.That(alignment.Y, Is.EqualTo(-0.5f));
        }

        [Test]
        public void StaticPresets_HaveExpectedValues()
        {
            Assert.That(FAlignment.TopLeft, Is.EqualTo(new FAlignment(-1f, -1f)));
            Assert.That(FAlignment.TopCenter, Is.EqualTo(new FAlignment(0f, -1f)));
            Assert.That(FAlignment.TopRight, Is.EqualTo(new FAlignment(1f, -1f)));
            Assert.That(FAlignment.CenterLeft, Is.EqualTo(new FAlignment(-1f, 0f)));
            Assert.That(FAlignment.Center, Is.EqualTo(new FAlignment(0f, 0f)));
            Assert.That(FAlignment.CenterRight, Is.EqualTo(new FAlignment(1f, 0f)));
            Assert.That(FAlignment.BottomLeft, Is.EqualTo(new FAlignment(-1f, 1f)));
            Assert.That(FAlignment.BottomCenter, Is.EqualTo(new FAlignment(0f, 1f)));
            Assert.That(FAlignment.BottomRight, Is.EqualTo(new FAlignment(1f, 1f)));
        }

        [Test]
        public void Equals_SameValues_ReturnsTrue()
        {
            Assert.That(FAlignment.Center, Is.EqualTo(new FAlignment(0f, 0f)));
        }

        [Test]
        public void Equals_DifferentValues_ReturnsFalse()
        {
            Assert.That(FAlignment.TopLeft, Is.Not.EqualTo(FAlignment.BottomRight));
        }
    }
}
