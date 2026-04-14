#nullable enable
using System;
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
    internal sealed class ThemeProviderTests
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
        public void Constructor_NullTheme_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ThemeProvider(null!, new TestLeafElement("child")));
        }

        [Test]
        public void Constructor_NullChild_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ThemeProvider(StylingTheme.Default, null!));
        }

        [Test]
        public void Theme_IsAccessibleFromDescendant()
        {
            var customTheme = StylingTheme.Default with { FontSize = 20f };
            StylingTheme? capturedTheme = null;

            var provider = new ThemeProvider(
                customTheme,
                new TestStatelessElement(ctx =>
                {
                    capturedTheme = ctx.GetInherited<ThemeProvider>().Theme;
                    return new TestLeafElement("leaf");
                }));

            _expander.Mount(provider, null);

            Assert.That(capturedTheme, Is.Not.Null);
            Assert.That(capturedTheme!.FontSize, Is.EqualTo(20f).Within(0.0001f));
        }

        [Test]
        public void UpdateShouldNotify_DifferentTheme_ReturnsTrue()
        {
            var old = new ThemeProvider(StylingTheme.Default, new TestLeafElement("a"));
            var current = new ThemeProvider(StylingTheme.Default with { FontSize = 99f }, new TestLeafElement("b"));

            Assert.That(current.UpdateShouldNotify(old), Is.True);
        }

        [Test]
        public void UpdateShouldNotify_SameTheme_ReturnsFalse()
        {
            var theme = StylingTheme.Default;
            var old = new ThemeProvider(theme, new TestLeafElement("a"));
            var current = new ThemeProvider(theme, new TestLeafElement("b"));

            Assert.That(current.UpdateShouldNotify(old), Is.False);
        }

        [Test]
        public void UpdateShouldNotify_WrongType_ReturnsTrue()
        {
            var old = new TestInheritedElement(new TestLeafElement("a"));
            var current = new ThemeProvider(StylingTheme.Default, new TestLeafElement("b"));

            Assert.That(current.UpdateShouldNotify(old), Is.True);
        }

        [Test]
        public void ThemeChange_NotifiesDependent()
        {
            var buildCount = 0;
            var provider = new ThemeProvider(
                StylingTheme.Default,
                new TestStatelessElement(ctx =>
                {
                    ctx.GetInherited<ThemeProvider>();
                    buildCount++;
                    return new TestLeafElement("leaf");
                }));

            var providerNode = _expander.Mount(provider, null);
            buildCount = 0;

            var newProvider = new ThemeProvider(
                StylingTheme.Default with { FontSize = 22f },
                new TestStatelessElement(ctx =>
                {
                    ctx.GetInherited<ThemeProvider>();
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
            var provider = new ThemeProvider(
                StylingTheme.Default,
                new TestStatelessElement(ctx =>
                {
                    ctx.GetInherited<ThemeProvider>();
                    buildCount++;
                    return new TestLeafElement("leaf");
                }));

            var providerNode = _expander.Mount(provider, null);
            buildCount = 0;

            var newProvider = new ThemeProvider(
                StylingTheme.Default,
                new TestStatelessElement(ctx =>
                {
                    ctx.GetInherited<ThemeProvider>();
                    buildCount++;
                    return new TestLeafElement("leaf");
                }));
            _expander.UpdateElement(providerNode, newProvider);
            _scheduler.Flush(_expander);

            Assert.That(buildCount, Is.EqualTo(0));
        }

        private sealed class TestInheritedElement : InheritedElement
        {
            public TestInheritedElement(Element child) : base(null)
            {
                Child = child;
            }

            public override bool UpdateShouldNotify(InheritedElement oldElement) => true;
        }
    }
}
