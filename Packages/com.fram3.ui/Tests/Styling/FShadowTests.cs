using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Styling
{
    [TestFixture]
    public class FShadowTests
    {
        [Test]
        public void Constructor_SetsAllProperties()
        {
            var shadow = new FShadow(FColor.Black, 2f, 4f, 8f);

            Assert.That(shadow.Color, Is.EqualTo(FColor.Black));
            Assert.That(shadow.OffsetX, Is.EqualTo(2f));
            Assert.That(shadow.OffsetY, Is.EqualTo(4f));
            Assert.That(shadow.BlurRadius, Is.EqualTo(8f));
        }

        [Test]
        public void Ambient_HasZeroOffsets()
        {
            var shadow = FShadow.Ambient(FColor.Black, 6f);

            Assert.That(shadow.OffsetX, Is.EqualTo(0f));
            Assert.That(shadow.OffsetY, Is.EqualTo(0f));
            Assert.That(shadow.BlurRadius, Is.EqualTo(6f));
            Assert.That(shadow.Color, Is.EqualTo(FColor.Black));
        }

        [Test]
        public void RecordEquality_SameValues_AreEqual()
        {
            var a = new FShadow(FColor.Black, 1f, 2f, 3f);
            var b = new FShadow(FColor.Black, 1f, 2f, 3f);

            Assert.That(a, Is.EqualTo(b));
        }

        [Test]
        public void RecordEquality_DifferentValues_AreNotEqual()
        {
            var a = new FShadow(FColor.Black, 1f, 2f, 3f);
            var b = new FShadow(FColor.Black, 1f, 2f, 4f);

            Assert.That(a, Is.Not.EqualTo(b));
        }
    }
}
