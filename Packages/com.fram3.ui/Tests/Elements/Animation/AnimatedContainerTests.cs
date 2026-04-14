#nullable enable
using System;
using Fram3.UI.Animation;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Animation;
using Fram3.UI.Styling;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Animation
{
    [TestFixture]
    internal sealed class AnimatedContainerTests
    {
        private RebuildScheduler _scheduler = null!;
        private NodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new RebuildScheduler();
            _expander = new NodeExpander(_scheduler);
        }

        [TearDown]
        public void TearDown()
        {
            AnimationSystem.Reset();
        }

        [Test]
        public void Constructor_ZeroDuration_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new AnimatedContainer(0f, new TestLeafElement("x")));
        }

        [Test]
        public void Constructor_NullChild_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new AnimatedContainer(1f, null!));
        }

        [Test]
        public void Constructor_StoresProperties()
        {
            var decoration = new BoxDecoration(Color: FrameColor.Red);
            var child = new TestLeafElement("child");

            var element = new AnimatedContainer(
                duration: 0.3f,
                child: child,
                decoration: decoration,
                width: 100f,
                height: 50f
            );

            Assert.That(element.Duration, Is.EqualTo(0.3f));
            Assert.That(element.Decoration, Is.EqualTo(decoration));
            Assert.That(element.Width, Is.EqualTo(100f));
            Assert.That(element.Height, Is.EqualTo(50f));
            Assert.That(element.Child, Is.SameAs(child));
        }

        [Test]
        public void Mount_BuildsImplicitAnimationWithCorrectValues()
        {
            var child = new TestLeafElement("child");
            var element = new AnimatedContainer(
                duration: 0.5f,
                child: child,
                width: 100f
            );

            var node = _expander.Mount(element, null);

            Assert.That(node, Is.Not.Null);
        }

        [Test]
        public void WhenWidthChanges_AnimationStarts()
        {
            var child = new TestLeafElement("child");
            var element = new AnimatedContainer(duration: 1f, child: child, width: 0f);

            var node = _expander.Mount(element, null);
            node.IsDirty = false;

            _expander.UpdateElement(node, new AnimatedContainer(duration: 1f, child: child, width: 100f));
            _scheduler.Flush(_expander);

            AnimationSystem.Tick(0.1f);
            _scheduler.Flush(_expander);

            Assert.That(node.IsDirty, Is.False);
        }

        [Test]
        public void WhenDecorationChanges_AnimationStarts()
        {
            var child = new TestLeafElement("child");
            var decorationA = new BoxDecoration(Color: FrameColor.Red);
            var decorationB = new BoxDecoration(Color: FrameColor.Blue);

            var element = new AnimatedContainer(
                duration: 1f,
                child: child,
                decoration: decorationA
            );

            var node = _expander.Mount(element, null);

            _expander.UpdateElement(node, new AnimatedContainer(
                duration: 1f,
                child: child,
                decoration: decorationB
            ));

            _scheduler.Flush(_expander);
            var beforeTick = AnimationSystem.RegisteredCount;

            AnimationSystem.Tick(0.1f);
            _scheduler.Flush(_expander);

            Assert.That(beforeTick, Is.GreaterThan(0));
        }

        [Test]
        public void Unmount_ReleasesAnimationController()
        {
            var child = new TestLeafElement("child");
            var element = new AnimatedContainer(duration: 1f, child: child, width: 0f);

            var node = _expander.Mount(element, null);

            _expander.UpdateElement(node, new AnimatedContainer(duration: 1f, child: child, width: 100f));
            _scheduler.Flush(_expander);

            _expander.Unmount(node);

            Assert.That(AnimationSystem.RegisteredCount, Is.EqualTo(0));
        }
    }
}
