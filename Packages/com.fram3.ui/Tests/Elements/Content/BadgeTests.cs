#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;
using StylingTheme = Fram3.UI.Styling.Theme;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class BadgeTests
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
        public void Constructor_StoresChild()
        {
            var child = new TestLeafElement("child");
            var badge = new Badge(child);

            Assert.That(badge.Child, Is.SameAs(child));
        }

        [Test]
        public void Constructor_DefaultCount_IsNull()
        {
            var badge = new Badge(new TestLeafElement("child"));

            Assert.That(badge.Count, Is.Null);
        }

        [Test]
        public void Constructor_StoresCount()
        {
            var badge = new Badge(new TestLeafElement("child"), count: 5);

            Assert.That(badge.Count, Is.EqualTo(5));
        }

        [Test]
        public void Constructor_DefaultColor_IsNull()
        {
            var badge = new Badge(new TestLeafElement("child"));

            Assert.That(badge.Color, Is.Null);
        }

        [Test]
        public void Constructor_StoresColor()
        {
            var color = FrameColor.FromHex("#FF0000");
            var badge = new Badge(new TestLeafElement("child"), color: color);

            Assert.That(badge.Color, Is.EqualTo(color));
        }

        [Test]
        public void Constructor_StoresKey()
        {
            var key = new ValueKey<string>("badge");
            var badge = new Badge(new TestLeafElement("child"), key: key);

            Assert.That(badge.Key, Is.EqualTo(key));
        }

        [Test]
        public void Build_ReturnsStack()
        {
            var child = new TestLeafElement("child");
            var badge = new Badge(child);
            Element? built = null;

            var tree = new ThemeProvider(
                StylingTheme.Default,
                new TestStatelessElement(ctx =>
                {
                    built = badge.Build(ctx);
                    return new TestLeafElement("leaf");
                })
            );

            _expander.Mount(tree, null);

            Assert.That(built, Is.InstanceOf<Stack>());
        }

        [Test]
        public void Build_StackContainsChild()
        {
            var child = new TestLeafElement("child");
            var badge = new Badge(child);
            Stack? stack = null;

            var tree = new ThemeProvider(
                StylingTheme.Default,
                new TestStatelessElement(ctx =>
                {
                    stack = (Stack)badge.Build(ctx);
                    return new TestLeafElement("leaf");
                })
            );

            _expander.Mount(tree, null);

            Assert.That(stack, Is.Not.Null);
            Assert.That(stack!.GetChildren(), Does.Contain(child));
        }

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var badge = new Badge(new TestLeafElement("child"));

            Assert.That(badge.GetChildren(), Is.Empty);
        }
    }
}
