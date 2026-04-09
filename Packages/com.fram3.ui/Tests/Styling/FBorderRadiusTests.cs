using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Styling
{
    [TestFixture]
    public class FBorderRadiusTests
    {
        [Test]
        public void Constructor_SetsAllCorners()
        {
            var radius = new FBorderRadius(1f, 2f, 3f, 4f);

            Assert.That(radius.TopLeft, Is.EqualTo(1f));
            Assert.That(radius.TopRight, Is.EqualTo(2f));
            Assert.That(radius.BottomRight, Is.EqualTo(3f));
            Assert.That(radius.BottomLeft, Is.EqualTo(4f));
        }

        [Test]
        public void All_SetsAllCornersToSameValue()
        {
            var radius = FBorderRadius.All(8f);

            Assert.That(radius.TopLeft, Is.EqualTo(8f));
            Assert.That(radius.TopRight, Is.EqualTo(8f));
            Assert.That(radius.BottomRight, Is.EqualTo(8f));
            Assert.That(radius.BottomLeft, Is.EqualTo(8f));
        }

        [Test]
        public void OnlyTop_SetsTopCornersOnly()
        {
            var radius = FBorderRadius.OnlyTop(6f);

            Assert.That(radius.TopLeft, Is.EqualTo(6f));
            Assert.That(radius.TopRight, Is.EqualTo(6f));
            Assert.That(radius.BottomRight, Is.EqualTo(0f));
            Assert.That(radius.BottomLeft, Is.EqualTo(0f));
        }

        [Test]
        public void OnlyBottom_SetsBottomCornersOnly()
        {
            var radius = FBorderRadius.OnlyBottom(6f);

            Assert.That(radius.TopLeft, Is.EqualTo(0f));
            Assert.That(radius.TopRight, Is.EqualTo(0f));
            Assert.That(radius.BottomRight, Is.EqualTo(6f));
            Assert.That(radius.BottomLeft, Is.EqualTo(6f));
        }

        [Test]
        public void Zero_AllCornersAreZero()
        {
            Assert.That(FBorderRadius.Zero, Is.EqualTo(new FBorderRadius(0f, 0f, 0f, 0f)));
        }

        [Test]
        public void Equals_SameValues_ReturnsTrue()
        {
            Assert.That(FBorderRadius.All(4f), Is.EqualTo(FBorderRadius.All(4f)));
        }

        [Test]
        public void Equals_DifferentValues_ReturnsFalse()
        {
            Assert.That(FBorderRadius.All(4f), Is.Not.EqualTo(FBorderRadius.All(8f)));
        }
    }
}
