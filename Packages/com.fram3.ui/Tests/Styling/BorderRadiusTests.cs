using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Styling
{
    [TestFixture]
    public class BorderRadiusTests
    {
        [Test]
        public void Constructor_SetsAllCorners()
        {
            var radius = new BorderRadius(1f, 2f, 3f, 4f);

            Assert.That(radius.TopLeft, Is.EqualTo(1f));
            Assert.That(radius.TopRight, Is.EqualTo(2f));
            Assert.That(radius.BottomRight, Is.EqualTo(3f));
            Assert.That(radius.BottomLeft, Is.EqualTo(4f));
        }

        [Test]
        public void All_SetsAllCornersToSameValue()
        {
            var radius = BorderRadius.All(8f);

            Assert.That(radius.TopLeft, Is.EqualTo(8f));
            Assert.That(radius.TopRight, Is.EqualTo(8f));
            Assert.That(radius.BottomRight, Is.EqualTo(8f));
            Assert.That(radius.BottomLeft, Is.EqualTo(8f));
        }

        [Test]
        public void OnlyTop_SetsTopCornersOnly()
        {
            var radius = BorderRadius.OnlyTop(6f);

            Assert.That(radius.TopLeft, Is.EqualTo(6f));
            Assert.That(radius.TopRight, Is.EqualTo(6f));
            Assert.That(radius.BottomRight, Is.EqualTo(0f));
            Assert.That(radius.BottomLeft, Is.EqualTo(0f));
        }

        [Test]
        public void OnlyBottom_SetsBottomCornersOnly()
        {
            var radius = BorderRadius.OnlyBottom(6f);

            Assert.That(radius.TopLeft, Is.EqualTo(0f));
            Assert.That(radius.TopRight, Is.EqualTo(0f));
            Assert.That(radius.BottomRight, Is.EqualTo(6f));
            Assert.That(radius.BottomLeft, Is.EqualTo(6f));
        }

        [Test]
        public void Zero_AllCornersAreZero()
        {
            Assert.That(BorderRadius.Zero, Is.EqualTo(new BorderRadius(0f, 0f, 0f, 0f)));
        }

        [Test]
        public void Equals_SameValues_ReturnsTrue()
        {
            Assert.That(BorderRadius.All(4f), Is.EqualTo(BorderRadius.All(4f)));
        }

        [Test]
        public void Equals_DifferentValues_ReturnsFalse()
        {
            Assert.That(BorderRadius.All(4f), Is.Not.EqualTo(BorderRadius.All(8f)));
        }

        [Test]
        public void Only_NoArguments_ProducesZeroRadius()
        {
            var radius = BorderRadius.Only();

            Assert.That(radius, Is.EqualTo(BorderRadius.Zero));
        }

        [Test]
        public void Only_TopLeftOnly_SetsTopLeftCorner()
        {
            var radius = BorderRadius.Only(topLeft: 10f);

            Assert.That(radius.TopLeft, Is.EqualTo(10f));
            Assert.That(radius.TopRight, Is.EqualTo(0f));
            Assert.That(radius.BottomRight, Is.EqualTo(0f));
            Assert.That(radius.BottomLeft, Is.EqualTo(0f));
        }

        [Test]
        public void Only_BottomRightOnly_SetsBottomRightCorner()
        {
            var radius = BorderRadius.Only(bottomRight: 5f);

            Assert.That(radius.TopLeft, Is.EqualTo(0f));
            Assert.That(radius.TopRight, Is.EqualTo(0f));
            Assert.That(radius.BottomRight, Is.EqualTo(5f));
            Assert.That(radius.BottomLeft, Is.EqualTo(0f));
        }

        [Test]
        public void Only_AllCornersSpecified_SetsAllCorners()
        {
            var radius = BorderRadius.Only(topLeft: 1f, topRight: 2f, bottomRight: 3f, bottomLeft: 4f);

            Assert.That(radius.TopLeft, Is.EqualTo(1f));
            Assert.That(radius.TopRight, Is.EqualTo(2f));
            Assert.That(radius.BottomRight, Is.EqualTo(3f));
            Assert.That(radius.BottomLeft, Is.EqualTo(4f));
        }

        [Test]
        public void Horizontal_SetsLeftAndRightSideCornersIndependently()
        {
            var radius = BorderRadius.Horizontal(left: 8f, right: 4f);

            Assert.That(radius.TopLeft, Is.EqualTo(8f));
            Assert.That(radius.BottomLeft, Is.EqualTo(8f));
            Assert.That(radius.TopRight, Is.EqualTo(4f));
            Assert.That(radius.BottomRight, Is.EqualTo(4f));
        }

        [Test]
        public void Horizontal_DefaultsToZero()
        {
            var radius = BorderRadius.Horizontal();

            Assert.That(radius, Is.EqualTo(BorderRadius.Zero));
        }

        [Test]
        public void Vertical_SetsTopAndBottomCornersIndependently()
        {
            var radius = BorderRadius.Vertical(top: 12f, bottom: 6f);

            Assert.That(radius.TopLeft, Is.EqualTo(12f));
            Assert.That(radius.TopRight, Is.EqualTo(12f));
            Assert.That(radius.BottomLeft, Is.EqualTo(6f));
            Assert.That(radius.BottomRight, Is.EqualTo(6f));
        }

        [Test]
        public void Vertical_DefaultsToZero()
        {
            var radius = BorderRadius.Vertical();

            Assert.That(radius, Is.EqualTo(BorderRadius.Zero));
        }
    }
}
