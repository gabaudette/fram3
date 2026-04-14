using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Styling
{
    [TestFixture]
    public class EdgeInsetsTests
    {
        [Test]
        public void Constructor_SetsAllEdges()
        {
            var insets = new EdgeInsets(1f, 2f, 3f, 4f);

            Assert.That(insets.Top, Is.EqualTo(1f));
            Assert.That(insets.Right, Is.EqualTo(2f));
            Assert.That(insets.Bottom, Is.EqualTo(3f));
            Assert.That(insets.Left, Is.EqualTo(4f));
        }

        [Test]
        public void All_SetsAllEdgesToSameValue()
        {
            var insets = EdgeInsets.All(8f);

            Assert.That(insets.Top, Is.EqualTo(8f));
            Assert.That(insets.Right, Is.EqualTo(8f));
            Assert.That(insets.Bottom, Is.EqualTo(8f));
            Assert.That(insets.Left, Is.EqualTo(8f));
        }

        [Test]
        public void Symmetric_SetsVerticalAndHorizontal()
        {
            var insets = EdgeInsets.Symmetric(vertical: 4f, horizontal: 8f);

            Assert.That(insets.Top, Is.EqualTo(4f));
            Assert.That(insets.Bottom, Is.EqualTo(4f));
            Assert.That(insets.Left, Is.EqualTo(8f));
            Assert.That(insets.Right, Is.EqualTo(8f));
        }

        [Test]
        public void OnlyTop_SetsOnlyTopEdge()
        {
            var insets = EdgeInsets.OnlyTop(5f);

            Assert.That(insets.Top, Is.EqualTo(5f));
            Assert.That(insets.Right, Is.EqualTo(0f));
            Assert.That(insets.Bottom, Is.EqualTo(0f));
            Assert.That(insets.Left, Is.EqualTo(0f));
        }

        [Test]
        public void OnlyRight_SetsOnlyRightEdge()
        {
            var insets = EdgeInsets.OnlyRight(5f);

            Assert.That(insets.Top, Is.EqualTo(0f));
            Assert.That(insets.Right, Is.EqualTo(5f));
            Assert.That(insets.Bottom, Is.EqualTo(0f));
            Assert.That(insets.Left, Is.EqualTo(0f));
        }

        [Test]
        public void OnlyBottom_SetsOnlyBottomEdge()
        {
            var insets = EdgeInsets.OnlyBottom(5f);

            Assert.That(insets.Top, Is.EqualTo(0f));
            Assert.That(insets.Right, Is.EqualTo(0f));
            Assert.That(insets.Bottom, Is.EqualTo(5f));
            Assert.That(insets.Left, Is.EqualTo(0f));
        }

        [Test]
        public void OnlyLeft_SetsOnlyLeftEdge()
        {
            var insets = EdgeInsets.OnlyLeft(5f);

            Assert.That(insets.Top, Is.EqualTo(0f));
            Assert.That(insets.Right, Is.EqualTo(0f));
            Assert.That(insets.Bottom, Is.EqualTo(0f));
            Assert.That(insets.Left, Is.EqualTo(5f));
        }

        [Test]
        public void Zero_AllEdgesAreZero()
        {
            Assert.That(EdgeInsets.Zero, Is.EqualTo(new EdgeInsets(0f, 0f, 0f, 0f)));
        }

        [Test]
        public void Horizontal_ReturnsSumOfLeftAndRight()
        {
            var insets = new EdgeInsets(0f, 3f, 0f, 5f);

            Assert.That(insets.Horizontal, Is.EqualTo(8f));
        }

        [Test]
        public void Vertical_ReturnsSumOfTopAndBottom()
        {
            var insets = new EdgeInsets(3f, 0f, 5f, 0f);

            Assert.That(insets.Vertical, Is.EqualTo(8f));
        }

        [Test]
        public void Equals_SameValues_ReturnsTrue()
        {
            var a = EdgeInsets.All(4f);
            var b = EdgeInsets.All(4f);

            Assert.That(a, Is.EqualTo(b));
        }

        [Test]
        public void Equals_DifferentValues_ReturnsFalse()
        {
            Assert.That(EdgeInsets.All(4f), Is.Not.EqualTo(EdgeInsets.All(8f)));
        }
    }
}
