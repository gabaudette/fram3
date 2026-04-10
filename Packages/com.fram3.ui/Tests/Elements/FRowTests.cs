#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements;
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FRowTests
    {
        [Test]
        public void Constructor_Defaults_AreStart()
        {
            var element = new FRow();

            Assert.That(element.MainAxisAlignment, Is.EqualTo(FMainAxisAlignment.Start));
            Assert.That(element.CrossAxisAlignment, Is.EqualTo(FCrossAxisAlignment.Start));
        }

        [Test]
        public void Constructor_StoresAlignments()
        {
            var element = new FRow(
                mainAxisAlignment: FMainAxisAlignment.SpaceBetween,
                crossAxisAlignment: FCrossAxisAlignment.End);

            Assert.That(element.MainAxisAlignment, Is.EqualTo(FMainAxisAlignment.SpaceBetween));
            Assert.That(element.CrossAxisAlignment, Is.EqualTo(FCrossAxisAlignment.End));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new FValueKey<string>("row");
            var element = new FRow(key: key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}