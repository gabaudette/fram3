#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Content;
using Fram3.UI.Styling;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class BadgeTests
    {
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
        public void GetChildren_ReturnsChild()
        {
            var child = new TestLeafElement("child");
            var badge = new Badge(child);

            Assert.That(badge.GetChildren(), Is.EqualTo(new[] { child }));
        }

        [Test]
        public void GetChildren_ReturnsSingleChild()
        {
            var badge = new Badge(new TestLeafElement("child"), count: 3);

            Assert.That(badge.GetChildren(), Has.Count.EqualTo(1));
        }

        [Test]
        public void Mounts_WithinTree_WithoutThrowing()
        {
            var scheduler = new RebuildScheduler();
            var expander = new NodeExpander(scheduler);
            var child = new TestLeafElement("icon");
            var badge = new Badge(child, count: 5);

            Assert.DoesNotThrow(() => expander.Mount(badge, null));
        }
    }
}
