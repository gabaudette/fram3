#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;
using Fram3.UI.Tests.Mocks;
using StylingTheme = Fram3.UI.Styling.Theme;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Theme
{
    [TestFixture]
    internal sealed class ThemeConsumerTests
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
        public void Of_WithProvider_ReturnsProviderTheme()
        {
            var customTheme = StylingTheme.Default with { FontSize = 18f };
            StylingTheme? capturedTheme = null;

            var tree = new ThemeProvider(
                customTheme,
                new TestStatelessElement(ctx =>
                {
                    capturedTheme = ThemeConsumer.Of(ctx);
                    return new TestLeafElement("leaf");
                }));

            _expander.Mount(tree, null);

            Assert.That(capturedTheme, Is.Not.Null);
            Assert.That(capturedTheme!.FontSize, Is.EqualTo(18f).Within(0.0001f));
        }

        [Test]
        public void Of_WithoutProvider_ReturnsFThemeDefault()
        {
            StylingTheme? capturedTheme = null;

            var element = new TestStatelessElement(ctx =>
            {
                capturedTheme = ThemeConsumer.Of(ctx);
                return new TestLeafElement("leaf");
            });

            _expander.Mount(element, null);

            Assert.That(capturedTheme, Is.Not.Null);
            Assert.That(capturedTheme, Is.EqualTo(StylingTheme.Default));
        }

        [Test]
        public void Of_NestedProviders_ReturnsNearestTheme()
        {
            var outerTheme = StylingTheme.Default with { FontSize = 12f };
            var innerTheme = StylingTheme.Default with { FontSize = 24f };
            StylingTheme? capturedTheme = null;

            var tree = new ThemeProvider(
                outerTheme,
                new ThemeProvider(
                    innerTheme,
                    new TestStatelessElement(ctx =>
                    {
                        capturedTheme = ThemeConsumer.Of(ctx);
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
                ThemeConsumer.Of(ctx);
                return new TestLeafElement("leaf");
            });

            Assert.DoesNotThrow(() => _expander.Mount(element, null));
        }
    }
}
