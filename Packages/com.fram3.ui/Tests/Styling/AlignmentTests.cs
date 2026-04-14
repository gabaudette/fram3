using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Styling
{
    [TestFixture]
    public class AlignmentTests
    {
        [Test]
        public void Constructor_SetsXAndY()
        {
            var alignment = new Alignment(0.5f, -0.5f);

            Assert.That(alignment.X, Is.EqualTo(0.5f));
            Assert.That(alignment.Y, Is.EqualTo(-0.5f));
        }

        [Test]
        public void StaticPresets_HaveExpectedValues()
        {
            Assert.That(Alignment.TopLeft, Is.EqualTo(new Alignment(-1f, -1f)));
            Assert.That(Alignment.TopCenter, Is.EqualTo(new Alignment(0f, -1f)));
            Assert.That(Alignment.TopRight, Is.EqualTo(new Alignment(1f, -1f)));
            Assert.That(Alignment.CenterLeft, Is.EqualTo(new Alignment(-1f, 0f)));
            Assert.That(Alignment.Center, Is.EqualTo(new Alignment(0f, 0f)));
            Assert.That(Alignment.CenterRight, Is.EqualTo(new Alignment(1f, 0f)));
            Assert.That(Alignment.BottomLeft, Is.EqualTo(new Alignment(-1f, 1f)));
            Assert.That(Alignment.BottomCenter, Is.EqualTo(new Alignment(0f, 1f)));
            Assert.That(Alignment.BottomRight, Is.EqualTo(new Alignment(1f, 1f)));
        }

        [Test]
        public void Equals_SameValues_ReturnsTrue()
        {
            Assert.That(Alignment.Center, Is.EqualTo(new Alignment(0f, 0f)));
        }

        [Test]
        public void Equals_DifferentValues_ReturnsFalse()
        {
            Assert.That(Alignment.TopLeft, Is.Not.EqualTo(Alignment.BottomRight));
        }
    }
}
