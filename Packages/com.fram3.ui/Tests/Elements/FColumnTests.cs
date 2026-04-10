#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements;
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FColumnTests
    {
        [Test]
        public void Constructor_Defaults_AreStart()
        {
            var element = new FColumn();

            Assert.That(element.MainAxisAlignment, Is.EqualTo(FMainAxisAlignment.Start));
            Assert.That(element.CrossAxisAlignment, Is.EqualTo(FCrossAxisAlignment.Start));
        }

        [Test]
        public void Constructor_StoresAlignments()
        {
            var element = new FColumn(
                mainAxisAlignment: FMainAxisAlignment.Center,
                crossAxisAlignment: FCrossAxisAlignment.Stretch);

            Assert.That(element.MainAxisAlignment, Is.EqualTo(FMainAxisAlignment.Center));
            Assert.That(element.CrossAxisAlignment, Is.EqualTo(FCrossAxisAlignment.Stretch));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new FValueKey<string>("col");
            var element = new FColumn(key: key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}
