#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FCenterTests
    {
        [Test]
        public void Constructor_NoArguments_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new FCenter());
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new FValueKey<string>("c");
            var element = new FCenter(key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}