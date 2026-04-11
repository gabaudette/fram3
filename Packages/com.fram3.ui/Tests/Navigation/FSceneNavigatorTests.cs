#nullable enable
using System;
using Fram3.UI.Navigation;
using Fram3.UI.Navigation.Internal;
using NUnit.Framework;

namespace Fram3.UI.Tests.Navigation
{
    [TestFixture]
    internal sealed class FSceneNavigatorTests
    {
        private StubSceneAdapter _stub = null!;

        [SetUp]
        public void SetUp()
        {
            _stub = new StubSceneAdapter();
            FSceneNavigator.SetAdapter(_stub);
        }

        [TearDown]
        public void TearDown()
        {
            FSceneNavigator.SetAdapter(null);
        }

        private sealed class StubSceneAdapter : ISceneAdapter
        {
            public string? LastSceneName { get; private set; }
            public FSceneOperation? LastOperation { get; private set; }
            public int CallCount { get; private set; }

            public FSceneOperation LoadAsync(string sceneName)
            {
                LastSceneName = sceneName;
                CallCount++;
                var operation = new FSceneOperation();
                LastOperation = operation;
                return operation;
            }

            public void CompleteLoad()
            {
                LastOperation?.Complete();
            }
        }

        // ---- GoTo argument validation ----

        [Test]
        public void GoTo_NullSceneName_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => FSceneNavigator.GoTo(null!));
        }

        [Test]
        public void GoTo_EmptySceneName_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => FSceneNavigator.GoTo(string.Empty));
        }

        // ---- GoTo delegates to adapter ----

        [Test]
        public void GoTo_PassesSceneNameToAdapter()
        {
            FSceneNavigator.GoTo("MainMenu");

            Assert.That(_stub.LastSceneName, Is.EqualTo("MainMenu"));
        }

        [Test]
        public void GoTo_ReturnsOperation()
        {
            var operation = FSceneNavigator.GoTo("MainMenu");

            Assert.That(operation, Is.Not.Null);
        }

        [Test]
        public void GoTo_CalledTwice_InvokesAdapterTwice()
        {
            FSceneNavigator.GoTo("SceneA");
            FSceneNavigator.GoTo("SceneB");

            Assert.That(_stub.CallCount, Is.EqualTo(2));
        }

        // ---- FSceneOperation progress ----

        [Test]
        public void Operation_InitialProgress_IsZero()
        {
            var operation = FSceneNavigator.GoTo("MainMenu");

            Assert.That(operation.Progress, Is.EqualTo(0f));
        }

        [Test]
        public void Operation_InitialIsCompleted_IsFalse()
        {
            var operation = FSceneNavigator.GoTo("MainMenu");

            Assert.That(operation.IsCompleted, Is.False);
        }

        [Test]
        public void Operation_AfterComplete_IsCompletedIsTrue()
        {
            var operation = FSceneNavigator.GoTo("MainMenu");
            _stub.CompleteLoad();

            Assert.That(operation.IsCompleted, Is.True);
        }

        [Test]
        public void Operation_AfterComplete_ProgressIsOne()
        {
            var operation = FSceneNavigator.GoTo("MainMenu");
            _stub.CompleteLoad();

            Assert.That(operation.Progress, Is.EqualTo(1f));
        }

        [Test]
        public void Operation_ProgressCanBeUpdated()
        {
            var operation = FSceneNavigator.GoTo("MainMenu");
            operation.Progress = 0.5f;

            Assert.That(operation.Progress, Is.EqualTo(0.5f).Within(0.001f));
        }

        // ---- FSceneOperation Completed event ----

        [Test]
        public void Operation_CompletedEvent_RaisedOnComplete()
        {
            var operation = FSceneNavigator.GoTo("MainMenu");
            bool raised = false;
            operation.Completed += () => raised = true;

            _stub.CompleteLoad();

            Assert.That(raised, Is.True);
        }

        [Test]
        public void Operation_CompletedEvent_NotRaisedBeforeComplete()
        {
            var operation = FSceneNavigator.GoTo("MainMenu");
            bool raised = false;
            operation.Completed += () => raised = true;

            Assert.That(raised, Is.False);
        }

        [Test]
        public void Operation_CompleteCalledTwice_EventRaisedOnce()
        {
            var operation = FSceneNavigator.GoTo("MainMenu");
            int count = 0;
            operation.Completed += () => count++;

            _stub.CompleteLoad();
            _stub.CompleteLoad();

            Assert.That(count, Is.EqualTo(1));
        }

        // ---- SetAdapter restores default ----

        [Test]
        public void SetAdapter_Null_RestoresDefaultAdapter()
        {
            FSceneNavigator.SetAdapter(null);

            Assert.DoesNotThrow(() => FSceneNavigator.GoTo("AnyScene"));
        }
    }
}
