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
    internal sealed class FProviderTests
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
        public void Constructor_NullChild_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FProvider<int>(42, null!));
        }

        [Test]
        public void Value_IsAccessibleFromDescendantViaGetInherited()
        {
            int? capturedValue = null;
            var provider = new FProvider<int>(
                99,
                new TestStatelessElement(ctx =>
                {
                    capturedValue = ctx.GetInherited<FProvider<int>>().Value;
                    return new TestLeafElement("leaf");
                }));

            _expander.Mount(provider, null);

            Assert.That(capturedValue, Is.EqualTo(99));
        }

        [Test]
        public void UpdateShouldNotify_DifferentValue_ReturnsTrue()
        {
            var old = new FProvider<int>(1, new TestLeafElement("a"));
            var current = new FProvider<int>(2, new TestLeafElement("b"));

            Assert.That(current.UpdateShouldNotify(old), Is.True);
        }

        [Test]
        public void UpdateShouldNotify_SameValue_ReturnsFalse()
        {
            var old = new FProvider<int>(5, new TestLeafElement("a"));
            var current = new FProvider<int>(5, new TestLeafElement("b"));

            Assert.That(current.UpdateShouldNotify(old), Is.False);
        }

        [Test]
        public void UpdateShouldNotify_WrongType_ReturnsTrue()
        {
            var old = new FProvider<string>("hello", new TestLeafElement("a"));
            var current = new FProvider<int>(5, new TestLeafElement("b"));

            Assert.That(current.UpdateShouldNotify(old), Is.True);
        }

        [Test]
        public void ValueChange_NotifiesDependent()
        {
            var buildCount = 0;
            var provider = new FProvider<int>(
                1,
                new TestStatelessElement(ctx =>
                {
                    ctx.GetInherited<FProvider<int>>();
                    buildCount++;
                    return new TestLeafElement("leaf");
                }));

            var providerNode = _expander.Mount(provider, null);
            buildCount = 0;

            var newProvider = new FProvider<int>(2, new TestStatelessElement(ctx =>
            {
                ctx.GetInherited<FProvider<int>>();
                buildCount++;
                return new TestLeafElement("leaf");
            }));
            _expander.UpdateElement(providerNode, newProvider);
            _scheduler.Flush(_expander);

            Assert.That(buildCount, Is.GreaterThan(0));
        }

        [Test]
        public void SameValue_DoesNotNotifyDependent()
        {
            var buildCount = 0;
            var provider = new FProvider<int>(
                5,
                new TestStatelessElement(ctx =>
                {
                    ctx.GetInherited<FProvider<int>>();
                    buildCount++;
                    return new TestLeafElement("leaf");
                }));

            var providerNode = _expander.Mount(provider, null);
            buildCount = 0;

            var newProvider = new FProvider<int>(5, new TestStatelessElement(ctx =>
            {
                ctx.GetInherited<FProvider<int>>();
                buildCount++;
                return new TestLeafElement("leaf");
            }));
            _expander.UpdateElement(providerNode, newProvider);
            _scheduler.Flush(_expander);

            Assert.That(buildCount, Is.EqualTo(0));
        }

        [Test]
        public void FindInherited_ReturnsNullWhenNoProvider()
        {
            FProvider<int>? found = null;
            var element = new TestStatelessElement(ctx =>
            {
                found = ctx.FindInherited<FProvider<int>>();
                return new TestLeafElement("leaf");
            });

            _expander.Mount(element, null);

            Assert.That(found, Is.Null);
        }
    }
}
