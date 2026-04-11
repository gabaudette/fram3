#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements;
using Fram3.UI.Styling;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements
{
    [TestFixture]
    internal sealed class FThemeConsumerTests
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
        public void Of_WithProvider_ReturnsProviderTheme()
        {
            var customTheme = FTheme.Default with { FontSize = 18f };
            FTheme? capturedTheme = null;

            var tree = new FThemeProvider(
                customTheme,
                new TestStatelessElement(ctx =>
                {
                    capturedTheme = FThemeConsumer.Of(ctx);
                    return new TestLeafElement("leaf");
                }));

            _expander.Mount(tree, null);

            Assert.That(capturedTheme, Is.Not.Null);
            Assert.That(capturedTheme!.FontSize, Is.EqualTo(18f).Within(0.0001f));
        }

        [Test]
        public void Of_WithoutProvider_ReturnsFThemeDefault()
        {
            FTheme? capturedTheme = null;

            var element = new TestStatelessElement(ctx =>
            {
                capturedTheme = FThemeConsumer.Of(ctx);
                return new TestLeafElement("leaf");
            });

            _expander.Mount(element, null);

            Assert.That(capturedTheme, Is.Not.Null);
            Assert.That(capturedTheme, Is.EqualTo(FTheme.Default));
        }

        [Test]
        public void Of_NestedProviders_ReturnsNearestTheme()
        {
            var outerTheme = FTheme.Default with { FontSize = 12f };
            var innerTheme = FTheme.Default with { FontSize = 24f };
            FTheme? capturedTheme = null;

            var tree = new FThemeProvider(
                outerTheme,
                new FThemeProvider(
                    innerTheme,
                    new TestStatelessElement(ctx =>
                    {
                        capturedTheme = FThemeConsumer.Of(ctx);
                        return new TestLeafElement("leaf");
                    })));

            _expander.Mount(tree, null);

            Assert.That(capturedTheme, Is.Not.Null);
            Assert.That(capturedTheme!.FontSize, Is.EqualTo(24f).Within(0.0001f));
        }

        [Test]
        public void Of_DoesNotThrow_WhenNoProviderIsPresent()
        {
            var element = new TestStatelessElement(ctx =>
            {
                FThemeConsumer.Of(ctx);
                return new TestLeafElement("leaf");
            });

            Assert.DoesNotThrow(() => _expander.Mount(element, null));
        }
    }
}
