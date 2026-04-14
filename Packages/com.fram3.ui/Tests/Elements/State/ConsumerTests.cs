#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.State;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.State
{
    [TestFixture]
    internal sealed class ConsumerTests
    {
        private RebuildScheduler _scheduler = null!;
        private NodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new RebuildScheduler();
            _expander = new NodeExpander(_scheduler);
        }

        [Test]
        public void Constructor_NullBuilder_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Consumer<int>(null!));
        }

        [Test]
        public void Build_ReceivesValueFromNearestProvider()
        {
            int? capturedValue = null;
            var tree = new Provider<int>(
                42,
                new Consumer<int>((_, v) =>
                {
                    capturedValue = v;
                    return new TestLeafElement("leaf");
                }));

            _expander.Mount(tree, null);

            Assert.That(capturedValue, Is.EqualTo(42));
        }

        [Test]
        public void Build_ReceivesBuildContext()
        {
            BuildContext? capturedCtx = null;
            var tree = new Provider<int>(
                1,
                new Consumer<int>((ctx, _) =>
                {
                    capturedCtx = ctx;
                    return new TestLeafElement("leaf");
                }));

            _expander.Mount(tree, null);

            Assert.That(capturedCtx, Is.Not.Null);
        }

        [Test]
        public void Build_NoProvider_ThrowsInvalidOperationException()
        {
            var consumer = new Consumer<int>((_, _) => new TestLeafElement("leaf"));

            Assert.Throws<InvalidOperationException>(() => _expander.Mount(consumer, null));
        }

        [Test]
        public void ValueChange_RebuildPassesNewValueToBuilder()
        {
            var lastValue = -1;

            var providerNode = _expander.Mount(
                new Provider<int>(
                    1,
                    new Consumer<int>((_, v) =>
                    {
                        lastValue = v;
                        return new TestLeafElement("leaf");
                    })),
                null);

            _expander.UpdateElement(providerNode, new Provider<int>(
                99,
                new Consumer<int>((_, v) =>
                {
                    lastValue = v;
                    return new TestLeafElement("leaf");
                })));
            _scheduler.Flush(_expander);

            Assert.That(lastValue, Is.EqualTo(99));
        }

        [Test]
        public void NestingProviders_InnerProviderShadowsOuter()
        {
            int? capturedValue = null;
            var tree = new Provider<int>(
                10,
                new Provider<int>(
                    20,
                    new Consumer<int>((_, v) =>
                    {
                        capturedValue = v;
                        return new TestLeafElement("leaf");
                    })));

            _expander.Mount(tree, null);

            Assert.That(capturedValue, Is.EqualTo(20));
        }
    }
}
