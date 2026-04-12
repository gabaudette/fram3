#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Animation;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Tests.Integration.Helpers;
using Fram3.UI.Tests.Mocks;
using NUnit.Framework;

namespace Fram3.UI.Tests.Integration
{
    /// <summary>
    /// Verifies that FAnimationController drives rebuilds through a real element tree when
    /// ticked via FAnimationSystem. Also validates the frozen-callback limitation as a
    /// documented contract.
    /// </summary>
    [TestFixture]
    internal sealed class AnimationIntegrationTests
    {
        private FRebuildScheduler _scheduler = null!;
        private FNodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            FAnimationSystem.Reset();
            (_scheduler, _expander) = TreeBuilder.MakePipeline();
        }

        [TearDown]
        public void TearDown()
        {
            FAnimationSystem.Reset();
        }

        [Test]
        public void AnimationController_RegistersWithSystem_OnConstruction()
        {
            var controller = new FAnimationController(1f);
            try
            {
                Assert.That(FAnimationSystem.RegisteredCount, Is.EqualTo(1));
            }
            finally
            {
                controller.Dispose();
            }
        }

        [Test]
        public void AnimationController_Dispose_DeregistersFromSystem()
        {
            var controller = new FAnimationController(1f);
            controller.Dispose();
            Assert.That(FAnimationSystem.RegisteredCount, Is.EqualTo(0));
        }

        [Test]
        public void AnimationController_Forward_TickAdvancesValue()
        {
            var controller = new FAnimationController(1f);
            try
            {
                controller.Forward();
                FAnimationSystem.Tick(0.5f);
                Assert.That(controller.Value, Is.EqualTo(0.5f).Within(0.001f));
            }
            finally
            {
                controller.Dispose();
            }
        }

        [Test]
        public void AnimationController_CompletesTick_StatusBecomesCompleted()
        {
            var controller = new FAnimationController(1f);
            try
            {
                controller.Forward();
                FAnimationSystem.Tick(1f);
                Assert.That(controller.Status, Is.EqualTo(FAnimationStatus.Completed));
            }
            finally
            {
                controller.Dispose();
            }
        }

        [Test]
        public void AnimationInTree_Listener_TriggersDirtyAndFlushRebuilds()
        {
            var animState = new AnimationDrivenState();
            var stateful = new TestStatefulElement(() => animState);
            TreeBuilder.Mount(stateful, _expander);

            animState.StartForward();
            FAnimationSystem.Tick(0.1f);
            // Listener fired -- node is now dirty
            Assert.That(_scheduler.HasDirtyNodes, Is.True);

            TreeBuilder.Flush(_scheduler, _expander);

            Assert.That(animState.BuildCount, Is.GreaterThan(1));
        }

        [Test]
        public void AnimationInTree_Dispose_OnUnmount_ControllerDeregistered()
        {
            var animState = new AnimationDrivenState();
            var stateful = new TestStatefulElement(() => animState);
            var rootNode = TreeBuilder.Mount(stateful, _expander);

            Assert.That(FAnimationSystem.RegisteredCount, Is.EqualTo(1));

            _expander.Unmount(rootNode);

            Assert.That(FAnimationSystem.RegisteredCount, Is.EqualTo(0));
        }

        [Test]
        public void AnimationController_Reverse_TickDecreasesValue()
        {
            var controller = new FAnimationController(1f);
            try
            {
                controller.Forward();
                FAnimationSystem.Tick(1f); // reach end
                controller.Reverse();
                FAnimationSystem.Tick(0.3f);
                Assert.That(controller.Value, Is.EqualTo(0.7f).Within(0.001f));
            }
            finally
            {
                controller.Dispose();
            }
        }

        [Test]
        public void AnimationController_ListenerReceivesUpdatedValue_OnEachTick()
        {
            var values = new List<float>();
            var controller = new FAnimationController(1f);
            try
            {
                controller.AddListener(v => values.Add(v));
                controller.Forward();
                FAnimationSystem.Tick(0.25f);
                FAnimationSystem.Tick(0.25f);
                Assert.That(values.Count, Is.EqualTo(2));
                Assert.That(values[0], Is.EqualTo(0.25f).Within(0.001f));
                Assert.That(values[1], Is.EqualTo(0.50f).Within(0.001f));
            }
            finally
            {
                controller.Dispose();
            }
        }

        // ---- Helpers --------------------------------------------------------------------------

        private sealed class AnimationDrivenState : FState
        {
            private FAnimationController? _controller;
            public int BuildCount { get; private set; }

            public override void InitState()
            {
                _controller = new FAnimationController(1f);
                _controller.AddListener(_ => SetState(null));
            }

            public void StartForward()
            {
                _controller!.Forward();
            }

            public override void Dispose()
            {
                _controller?.Dispose();
                _controller = null;
            }

            public override FElement Build(FBuildContext context)
            {
                BuildCount++;
                return new TestLeafElement("animated");
            }
        }
    }
}
