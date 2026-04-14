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
    /// Verifies that AnimationController drives rebuilds through a real element tree when
    /// ticked via AnimationSystem. Also validates the frozen-callback limitation as a
    /// documented contract.
    /// </summary>
    [TestFixture]
    internal sealed class AnimationIntegrationTests
    {
        private RebuildScheduler _scheduler = null!;
        private NodeExpander _expander = null!;

        [SetUp]
        public void SetUp()
        {
            AnimationSystem.Reset();
            (_scheduler, _expander) = TreeBuilder.MakePipeline();
        }

        [TearDown]
        public void TearDown()
        {
            AnimationSystem.Reset();
        }

        [Test]
        public void AnimationController_RegistersWithSystem_OnConstruction()
        {
            var controller = new AnimationController(1f);
            try
            {
                Assert.That(AnimationSystem.RegisteredCount, Is.EqualTo(1));
            }
            finally
            {
                controller.Dispose();
            }
        }

        [Test]
        public void AnimationController_Dispose_DeregistersFromSystem()
        {
            var controller = new AnimationController(1f);
            controller.Dispose();
            Assert.That(AnimationSystem.RegisteredCount, Is.EqualTo(0));
        }

        [Test]
        public void AnimationController_Forward_TickAdvancesValue()
        {
            var controller = new AnimationController(1f);
            try
            {
                controller.Forward();
                AnimationSystem.Tick(0.5f);
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
            var controller = new AnimationController(1f);
            try
            {
                controller.Forward();
                AnimationSystem.Tick(1f);
                Assert.That(controller.Status, Is.EqualTo(AnimationStatus.Completed));
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
            AnimationSystem.Tick(0.1f);
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

            Assert.That(AnimationSystem.RegisteredCount, Is.EqualTo(1));

            _expander.Unmount(rootNode);

            Assert.That(AnimationSystem.RegisteredCount, Is.EqualTo(0));
        }

        [Test]
        public void AnimationController_Reverse_TickDecreasesValue()
        {
            var controller = new AnimationController(1f);
            try
            {
                controller.Forward();
                AnimationSystem.Tick(1f); // reach end
                controller.Reverse();
                AnimationSystem.Tick(0.3f);
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
            var controller = new AnimationController(1f);
            try
            {
                controller.AddListener(v => values.Add(v));
                controller.Forward();
                AnimationSystem.Tick(0.25f);
                AnimationSystem.Tick(0.25f);
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

        private sealed class AnimationDrivenState : State
        {
            private AnimationController? _controller;
            public int BuildCount { get; private set; }

            public override void InitState()
            {
                _controller = new AnimationController(1f);
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

            public override Element Build(BuildContext context)
            {
                BuildCount++;
                return new TestLeafElement("animated");
            }
        }
    }
}
