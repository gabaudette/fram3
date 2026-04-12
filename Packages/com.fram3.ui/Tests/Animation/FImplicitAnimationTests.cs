#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Animation;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Animation;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Animation
{
    [TestFixture]
    internal sealed class FImplicitAnimationTests
    {
        private FRebuildScheduler _scheduler = null!;
        private FNodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new FRebuildScheduler();
            _expander = new FNodeExpander(_scheduler);
        }

        [TearDown]
        public void TearDown()
        {
            FAnimationSystem.Reset();
        }

        private static IReadOnlyList<IFAnimatedValue> FloatValues(float value) =>
            new IFAnimatedValue[]
            {
                new FAnimatedValue<float>("x", value, FLerp.Float),
            };

        [Test]
        public void Constructor_NullValues_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FImplicitAnimation(null!, 1f, (_, __) => new TestLeafElement("x")));
        }

        [Test]
        public void Constructor_NullBuilder_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FImplicitAnimation(FloatValues(0f), 1f, null!));
        }

        [Test]
        public void Constructor_ZeroDuration_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new FImplicitAnimation(FloatValues(0f), 0f, (_, __) => new TestLeafElement("x")));
        }

        [Test]
        public void Mount_SnapshotAtTOneReflectsTargetValue()
        {
            float? capturedX = null;

            var element = new FImplicitAnimation(
                FloatValues(5f),
                1f,
                (ctx, snapshot) =>
                {
                    capturedX = snapshot.Get<float>("x");
                    return new TestLeafElement("child");
                }
            );

            _expander.Mount(element, null);

            Assert.That(capturedX, Is.EqualTo(5f).Within(0.0001f));
        }

        [Test]
        public void WhenTargetUnchanged_NoAnimationControllerStarted()
        {
            var element = new FImplicitAnimation(
                FloatValues(5f),
                1f,
                (_, __) => new TestLeafElement("child")
            );

            var node = _expander.Mount(element, null);
            node.IsDirty = false;

            _expander.UpdateElement(node, new FImplicitAnimation(
                FloatValues(5f),
                1f,
                (_, __) => new TestLeafElement("child")
            ));

            FAnimationSystem.Tick(0.1f);

            Assert.That(node.IsDirty, Is.False);
        }

        [Test]
        public void WhenTargetChanges_AnimationControllerStarted()
        {
            var element = new FImplicitAnimation(
                FloatValues(0f),
                1f,
                (_, __) => new TestLeafElement("child")
            );

            var node = _expander.Mount(element, null);
            node.IsDirty = false;

            _expander.UpdateElement(node, new FImplicitAnimation(
                FloatValues(1f),
                1f,
                (_, __) => new TestLeafElement("child")
            ));

            FAnimationSystem.Tick(0.1f);

            Assert.That(node.IsDirty, Is.True);
        }

        [Test]
        public void DuringAnimation_SnapshotInterpolatesBetweenPreviousAndTarget()
        {
            float? capturedX = null;

            var element = new FImplicitAnimation(
                FloatValues(0f),
                1f,
                (ctx, snapshot) =>
                {
                    capturedX = snapshot.Get<float>("x");
                    return new TestLeafElement("child");
                }
            );

            var node = _expander.Mount(element, null);

            _expander.UpdateElement(node, new FImplicitAnimation(
                FloatValues(10f),
                1f,
                (ctx, snapshot) =>
                {
                    capturedX = snapshot.Get<float>("x");
                    return new TestLeafElement("child");
                }
            ));

            FAnimationSystem.Tick(0.5f);
            _scheduler.Flush(_expander);

            Assert.That(capturedX, Is.EqualTo(5f).Within(0.0001f));
        }

        [Test]
        public void AfterAnimationCompletes_SnapshotReturnsTargetValue()
        {
            float? capturedX = null;

            var element = new FImplicitAnimation(
                FloatValues(0f),
                0.5f,
                (ctx, snapshot) =>
                {
                    capturedX = snapshot.Get<float>("x");
                    return new TestLeafElement("child");
                }
            );

            var node = _expander.Mount(element, null);

            _expander.UpdateElement(node, new FImplicitAnimation(
                FloatValues(10f),
                0.5f,
                (ctx, snapshot) =>
                {
                    capturedX = snapshot.Get<float>("x");
                    return new TestLeafElement("child");
                }
            ));

            FAnimationSystem.Tick(1f);
            _scheduler.Flush(_expander);

            Assert.That(capturedX, Is.EqualTo(10f).Within(0.0001f));
        }

        [Test]
        public void WhenTargetChangesAgainMidAnimation_TweensFromCurrentSnapshot()
        {
            float? capturedX = null;

            var element = new FImplicitAnimation(
                FloatValues(0f),
                1f,
                (ctx, snapshot) =>
                {
                    capturedX = snapshot.Get<float>("x");
                    return new TestLeafElement("child");
                }
            );

            var node = _expander.Mount(element, null);

            _expander.UpdateElement(node, new FImplicitAnimation(
                FloatValues(10f),
                1f,
                (ctx, snapshot) =>
                {
                    capturedX = snapshot.Get<float>("x");
                    return new TestLeafElement("child");
                }
            ));

            FAnimationSystem.Tick(0.5f);

            _expander.UpdateElement(node, new FImplicitAnimation(
                FloatValues(20f),
                1f,
                (ctx, snapshot) =>
                {
                    capturedX = snapshot.Get<float>("x");
                    return new TestLeafElement("child");
                }
            ));

            FAnimationSystem.Tick(0f);
            _scheduler.Flush(_expander);

            Assert.That(capturedX, Is.EqualTo(5f).Within(0.0001f));
        }

        [Test]
        public void Unmount_ReleasesController()
        {
            var element = new FImplicitAnimation(
                FloatValues(0f),
                1f,
                (_, __) => new TestLeafElement("child")
            );

            var node = _expander.Mount(element, null);

            _expander.UpdateElement(node, new FImplicitAnimation(
                FloatValues(1f),
                1f,
                (_, __) => new TestLeafElement("child")
            ));

            _expander.Unmount(node);

            Assert.That(FAnimationSystem.RegisteredCount, Is.EqualTo(0));
        }

        [Test]
        public void Snapshot_Get_UnknownKey_ThrowsKeyNotFoundException()
        {
            FImplicitAnimationSnapshot? captured = null;

            var element = new FImplicitAnimation(
                FloatValues(1f),
                1f,
                (ctx, snapshot) =>
                {
                    captured = snapshot;
                    return new TestLeafElement("child");
                }
            );

            _expander.Mount(element, null);

            Assert.Throws<KeyNotFoundException>(() => captured!.Get<float>("nonexistent"));
        }
    }
}
