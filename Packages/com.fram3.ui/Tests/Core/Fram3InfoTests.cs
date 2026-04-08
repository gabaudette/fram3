using NUnit.Framework;

namespace Fram3.UI.Tests
{
    [TestFixture]
    public class Fram3InfoTests
    {
        [Test]
        public void Version_ReturnsExpectedVersion()
        {
            Assert.That(Fram3Info.Version, Is.EqualTo("0.1.0"));
        }
    }
}
