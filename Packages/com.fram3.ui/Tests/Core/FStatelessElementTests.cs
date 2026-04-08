using System;
using Fram3.UI.Core;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    public class FStatelessElementTests
    {
        [Test]
        public void Build_ReturnsElementFromBuilder()
        {
            var expected = new TestLeafElement("result");
            var element = new TestStatelessElement(ctx => expected);
            var node = new FNode(element, null);

            var result = element.Build(node.Context);

            Assert.That(result, Is.SameAs(expected));
        }

        [Test]
        public void Build_ReceivesBuildContext()
        {
            FBuildContext receivedContext = null;
            var element = new TestStatelessElement(ctx =>
            {
                receivedContext = ctx;
                return new TestLeafElement("result");
            });
            var node = new FNode(element, null);

            element.Build(node.Context);

            Assert.That(receivedContext, Is.Not.Null);
            Assert.That(receivedContext, Is.SameAs(node.Context));
        }

        [Test]
        public void Key_IsPassedThrough()
        {
            var key = new FValueKey<string>("my-key");
            var element = new TestStatelessElement(ctx => new TestLeafElement("r"), key);

            Assert.That(element.Key, Is.EqualTo(key));
        }
    }
}
