using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Styling
{
    [TestFixture]
    public class FBoxDecorationTests
    {
        [Test]
        public void DefaultConstructor_AllPropertiesAreNull()
        {
            var decoration = new FBoxDecoration();

            Assert.That(decoration.Color, Is.Null);
            Assert.That(decoration.Border, Is.Null);
            Assert.That(decoration.BorderRadius, Is.Null);
            Assert.That(decoration.Shadow, Is.Null);
        }

        [Test]
        public void Constructor_SetsProperties()
        {
            var color = FColor.White;
            var border = new FBorder(FColor.Black, 1f);
            var radius = FBorderRadius.All(4f);
            var shadow = FShadow.Ambient(FColor.Black, 8f);

            var decoration = new FBoxDecoration(
                Color: color,
                Border: border,
                BorderRadius: radius,
                Shadow: shadow
            );

            Assert.That(decoration.Color, Is.EqualTo(color));
            Assert.That(decoration.Border, Is.EqualTo(border));
            Assert.That(decoration.BorderRadius, Is.EqualTo(radius));
            Assert.That(decoration.Shadow, Is.EqualTo(shadow));
        }

        [Test]
        public void None_AllPropertiesAreNull()
        {
            var none = FBoxDecoration.None;

            Assert.That(none.Color, Is.Null);
            Assert.That(none.Border, Is.Null);
            Assert.That(none.BorderRadius, Is.Null);
            Assert.That(none.Shadow, Is.Null);
        }

        [Test]
        public void RecordEquality_SameValues_AreEqual()
        {
            var a = new FBoxDecoration(Color: FColor.Red);
            var b = new FBoxDecoration(Color: FColor.Red);

            Assert.That(a, Is.EqualTo(b));
        }

        [Test]
        public void RecordEquality_DifferentValues_AreNotEqual()
        {
            var a = new FBoxDecoration(Color: FColor.Red);
            var b = new FBoxDecoration(Color: FColor.Blue);

            Assert.That(a, Is.Not.EqualTo(b));
        }
    }
}
