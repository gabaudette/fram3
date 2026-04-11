#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FConsumerTests
    {
        private FRebuildScheduler _scheduler = null!;
        private FNodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new FRebuildScheduler();
            _expander = new FNodeExpander(_scheduler);
        }

        [Test]
        public void Constructor_NullBuilder_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FConsumer<int>(null!));
        }

        [Test]
        public void Build_ReceivesValueFromNearestProvider()
        {
            int? capturedValue = null;
            var tree = new FProvider<int>(
                42,
                new FConsumer<int>((_, v) =>
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
            FBuildContext? capturedCtx = null;
            var tree = new FProvider<int>(
                1,
                new FConsumer<int>((ctx, _) =>
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
            var consumer = new FConsumer<int>((_, _) => new TestLeafElement("leaf"));

            Assert.Throws<InvalidOperationException>(() => _expander.Mount(consumer, null));
        }

        [Test]
        public void ValueChange_RebuildPassesNewValueToBuilder()
        {
            var lastValue = -1;

            var providerNode = _expander.Mount(
                new FProvider<int>(
                    1,
                    new FConsumer<int>((_, v) =>
                    {
                        lastValue = v;
                        return new TestLeafElement("leaf");
                    })),
                null);

            _expander.UpdateElement(providerNode, new FProvider<int>(
                99,
                new FConsumer<int>((_, v) =>
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
            var tree = new FProvider<int>(
                10,
                new FProvider<int>(
                    20,
                    new FConsumer<int>((_, v) =>
                    {
                        capturedValue = v;
                        return new TestLeafElement("leaf");
                    })));

            _expander.Mount(tree, null);

            Assert.That(capturedValue, Is.EqualTo(20));
        }
    }
}
