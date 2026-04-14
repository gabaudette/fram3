using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Styling
{
    [TestFixture]
    public class BorderTests
    {
        [Test]
        public void Constructor_SetsColorAndWidth()
        {
            var border = new Border(FrameColor.Black, 2f);

            Assert.That(border.Color, Is.EqualTo(FrameColor.Black));
            Assert.That(border.Width, Is.EqualTo(2f));
        }

        [Test]
        public void None_HasZeroWidthAndTransparentColor()
        {
            var none = Border.None;

            Assert.That(none.Width, Is.EqualTo(0f));
            Assert.That(none.Color, Is.EqualTo(FrameColor.Transparent));
        }

        [Test]
        public void RecordEquality_SameValues_AreEqual()
        {
            var a = new Border(FrameColor.Red, 1f);
            var b = new Border(FrameColor.Red, 1f);

            Assert.That(a, Is.EqualTo(b));
        }

        [Test]
        public void RecordEquality_DifferentValues_AreNotEqual()
        {
            var a = new Border(FrameColor.Red, 1f);
            var b = new Border(FrameColor.Blue, 1f);

            Assert.That(a, Is.Not.EqualTo(b));
        }
    }
}
