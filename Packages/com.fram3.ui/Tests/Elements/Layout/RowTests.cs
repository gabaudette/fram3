#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Layout
{
    [TestFixture]
    internal sealed class RowTests
    {
        [Test]
        public void Constructor_Defaults_AreStart()
        {
            var element = new Row();

            Assert.That(element.MainAxisAlignment, Is.EqualTo(MainAxisAlignment.Start));
            Assert.That(element.CrossAxisAlignment, Is.EqualTo(CrossAxisAlignment.Start));
        }

        [Test]
        public void Constructor_StoresAlignments()
        {
            var element = new Row(
                mainAxisAlignment: MainAxisAlignment.SpaceBetween,
                crossAxisAlignment: CrossAxisAlignment.End);

            Assert.That(element.MainAxisAlignment, Is.EqualTo(MainAxisAlignment.SpaceBetween));
            Assert.That(element.CrossAxisAlignment, Is.EqualTo(CrossAxisAlignment.End));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("row");
            var element = new Row(key: key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}