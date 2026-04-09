using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Styling
{
    [TestFixture]
    public class FBorderTests
    {
        [Test]
        public void Constructor_SetsColorAndWidth()
        {
            var border = new FBorder(FColor.Black, 2f);

            Assert.That(border.Color, Is.EqualTo(FColor.Black));
            Assert.That(border.Width, Is.EqualTo(2f));
        }

        [Test]
        public void None_HasZeroWidthAndTransparentColor()
        {
            var none = FBorder.None;

            Assert.That(none.Width, Is.EqualTo(0f));
            Assert.That(none.Color, Is.EqualTo(FColor.Transparent));
        }

        [Test]
        public void RecordEquality_SameValues_AreEqual()
        {
            var a = new FBorder(FColor.Red, 1f);
            var b = new FBorder(FColor.Red, 1f);

            Assert.That(a, Is.EqualTo(b));
        }

        [Test]
        public void RecordEquality_DifferentValues_AreNotEqual()
        {
            var a = new FBorder(FColor.Red, 1f);
            var b = new FBorder(FColor.Blue, 1f);

            Assert.That(a, Is.Not.EqualTo(b));
        }
    }
}
