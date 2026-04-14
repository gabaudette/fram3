using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Styling
{
    [TestFixture]
    public class BoxDecorationTests
    {
        [Test]
        public void DefaultConstructor_AllPropertiesAreNull()
        {
            var decoration = new BoxDecoration();

            Assert.That(decoration.Color, Is.Null);
            Assert.That(decoration.Border, Is.Null);
            Assert.That(decoration.BorderRadius, Is.Null);
            Assert.That(decoration.Shadow, Is.Null);
        }

        [Test]
        public void Constructor_SetsProperties()
        {
            var color = FrameColor.White;
            var border = new Border(FrameColor.Black, 1f);
            var radius = BorderRadius.All(4f);
            var shadow = Shadow.Ambient(FrameColor.Black, 8f);

            var decoration = new BoxDecoration(
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
            var none = BoxDecoration.None;

            Assert.That(none.Color, Is.Null);
            Assert.That(none.Border, Is.Null);
            Assert.That(none.BorderRadius, Is.Null);
            Assert.That(none.Shadow, Is.Null);
        }

        [Test]
        public void RecordEquality_SameValues_AreEqual()
        {
            var a = new BoxDecoration(Color: FrameColor.Red);
            var b = new BoxDecoration(Color: FrameColor.Red);

            Assert.That(a, Is.EqualTo(b));
        }

        [Test]
        public void RecordEquality_DifferentValues_AreNotEqual()
        {
            var a = new BoxDecoration(Color: FrameColor.Red);
            var b = new BoxDecoration(Color: FrameColor.Blue);

            Assert.That(a, Is.Not.EqualTo(b));
        }
    }
}
