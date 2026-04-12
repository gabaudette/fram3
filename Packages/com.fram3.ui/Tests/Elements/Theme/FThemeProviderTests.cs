#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Theme
{
    [TestFixture]
    internal sealed class FThemeProviderTests
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
        public void Constructor_NullTheme_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FThemeProvider(null!, new TestLeafElement("child")));
        }

        [Test]
        public void Constructor_NullChild_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FThemeProvider(FTheme.Default, null!));
        }

        [Test]
        public void Theme_IsAccessibleFromDescendant()
        {
            var customTheme = FTheme.Default with { FontSize = 20f };
            FTheme? capturedTheme = null;

            var provider = new FThemeProvider(
                customTheme,
                new TestStatelessElement(ctx =>
                {
                    capturedTheme = ctx.GetInherited<FThemeProvider>().Theme;
                    return new TestLeafElement("leaf");
                }));

            _expander.Mount(provider, null);

            Assert.That(capturedTheme, Is.Not.Null);
            Assert.That(capturedTheme!.FontSize, Is.EqualTo(20f).Within(0.0001f));
        }

        [Test]
        public void UpdateShouldNotify_DifferentTheme_ReturnsTrue()
        {
            var old = new FThemeProvider(FTheme.Default, new TestLeafElement("a"));
            var current = new FThemeProvider(FTheme.Default with { FontSize = 99f }, new TestLeafElement("b"));

            Assert.That(current.UpdateShouldNotify(old), Is.True);
        }

        [Test]
        public void UpdateShouldNotify_SameTheme_ReturnsFalse()
        {
            var theme = FTheme.Default;
            var old = new FThemeProvider(theme, new TestLeafElement("a"));
            var current = new FThemeProvider(theme, new TestLeafElement("b"));

            Assert.That(current.UpdateShouldNotify(old), Is.False);
        }

        [Test]
        public void UpdateShouldNotify_WrongType_ReturnsTrue()
        {
            var old = new TestInheritedElement(new TestLeafElement("a"));
            var current = new FThemeProvider(FTheme.Default, new TestLeafElement("b"));

            Assert.That(current.UpdateShouldNotify(old), Is.True);
        }

        [Test]
        public void ThemeChange_NotifiesDependent()
        {
            var buildCount = 0;
            var provider = new FThemeProvider(
                FTheme.Default,
                new TestStatelessElement(ctx =>
                {
                    ctx.GetInherited<FThemeProvider>();
                    buildCount++;
                    return new TestLeafElement("leaf");
                }));

            var providerNode = _expander.Mount(provider, null);
            buildCount = 0;

            var newProvider = new FThemeProvider(
                FTheme.Default with { FontSize = 22f },
                new TestStatelessElement(ctx =>
                {
                    ctx.GetInherited<FThemeProvider>();
                    buildCount++;
                    return new TestLeafElement("leaf");
                }));
            _expander.UpdateElement(providerNode, newProvider);
            _scheduler.Flush(_expander);

            Assert.That(buildCount, Is.GreaterThan(0));
        }

        [Test]
        public void SameTheme_DoesNotNotifyDependent()
        {
            var buildCount = 0;
            var provider = new FThemeProvider(
                FTheme.Default,
                new TestStatelessElement(ctx =>
                {
                    ctx.GetInherited<FThemeProvider>();
                    buildCount++;
                    return new TestLeafElement("leaf");
                }));

            var providerNode = _expander.Mount(provider, null);
            buildCount = 0;

            var newProvider = new FThemeProvider(
                FTheme.Default,
                new TestStatelessElement(ctx =>
                {
                    ctx.GetInherited<FThemeProvider>();
                    buildCount++;
                    return new TestLeafElement("leaf");
                }));
            _expander.UpdateElement(providerNode, newProvider);
            _scheduler.Flush(_expander);

            Assert.That(buildCount, Is.EqualTo(0));
        }

        private sealed class TestInheritedElement : FInheritedElement
        {
            public TestInheritedElement(FElement child) : base(null)
            {
                Child = child;
            }

            public override bool UpdateShouldNotify(FInheritedElement oldElement) => true;
        }
    }
}
