#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Layout;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Layout
{
    [TestFixture]
    internal sealed class CenterTests
    {
        [Test]
        public void Constructor_NoArguments_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new Center());
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("c");
            var element = new Center(key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}