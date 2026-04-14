#nullable enable
using System;
using Fram3.UI.Navigation;
using Fram3.UI.Navigation.Internal;
using NUnit.Framework;

namespace Fram3.UI.Tests.Navigation
{
    [TestFixture]
    internal sealed class SceneNavigatorTests
    {
        private StubSceneAdapter _stub = null!;

        [SetUp]
        public void SetUp()
        {
            _stub = new StubSceneAdapter();
            SceneNavigator.SetAdapter(_stub);
        }

        [TearDown]
        public void TearDown()
        {
            SceneNavigator.SetAdapter(null);
        }

        private sealed class StubSceneAdapter : ISceneAdapter
        {
            public string? LastSceneName { get; private set; }
            public SceneOperation? LastOperation { get; private set; }
            public int CallCount { get; private set; }

            public SceneOperation LoadAsync(string sceneName)
            {
                LastSceneName = sceneName;
                CallCount++;
                var operation = new SceneOperation();
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
            Assert.Throws<ArgumentNullException>(() => SceneNavigator.GoTo(null!));
        }

        [Test]
        public void GoTo_EmptySceneName_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => SceneNavigator.GoTo(string.Empty));
        }

        // ---- GoTo delegates to adapter ----

        [Test]
        public void GoTo_PassesSceneNameToAdapter()
        {
            SceneNavigator.GoTo("MainMenu");

            Assert.That(_stub.LastSceneName, Is.EqualTo("MainMenu"));
        }

        [Test]
        public void GoTo_ReturnsOperation()
        {
            var operation = SceneNavigator.GoTo("MainMenu");

            Assert.That(operation, Is.Not.Null);
        }

        [Test]
        public void GoTo_CalledTwice_InvokesAdapterTwice()
        {
            SceneNavigator.GoTo("SceneA");
            SceneNavigator.GoTo("SceneB");

            Assert.That(_stub.CallCount, Is.EqualTo(2));
        }

        // ---- SceneOperation progress ----

        [Test]
        public void Operation_InitialProgress_IsZero()
        {
            var operation = SceneNavigator.GoTo("MainMenu");

            Assert.That(operation.Progress, Is.EqualTo(0f));
        }

        [Test]
        public void Operation_InitialIsCompleted_IsFalse()
        {
            var operation = SceneNavigator.GoTo("MainMenu");

            Assert.That(operation.IsCompleted, Is.False);
        }

        [Test]
        public void Operation_AfterComplete_IsCompletedIsTrue()
        {
            var operation = SceneNavigator.GoTo("MainMenu");
            _stub.CompleteLoad();

            Assert.That(operation.IsCompleted, Is.True);
        }

        [Test]
        public void Operation_AfterComplete_ProgressIsOne()
        {
            var operation = SceneNavigator.GoTo("MainMenu");
            _stub.CompleteLoad();

            Assert.That(operation.Progress, Is.EqualTo(1f));
        }

        [Test]
        public void Operation_ProgressCanBeUpdated()
        {
            var operation = SceneNavigator.GoTo("MainMenu");
            operation.Progress = 0.5f;

            Assert.That(operation.Progress, Is.EqualTo(0.5f).Within(0.001f));
        }

        // ---- SceneOperation Completed event ----

        [Test]
        public void Operation_CompletedEvent_RaisedOnComplete()
        {
            var operation = SceneNavigator.GoTo("MainMenu");
            bool raised = false;
            operation.Completed += () => raised = true;

            _stub.CompleteLoad();

            Assert.That(raised, Is.True);
        }

        [Test]
        public void Operation_CompletedEvent_NotRaisedBeforeComplete()
        {
            var operation = SceneNavigator.GoTo("MainMenu");
            bool raised = false;
            operation.Completed += () => raised = true;

            Assert.That(raised, Is.False);
        }

        [Test]
        public void Operation_CompleteCalledTwice_EventRaisedOnce()
        {
            var operation = SceneNavigator.GoTo("MainMenu");
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
            SceneNavigator.SetAdapter(null);

            Assert.DoesNotThrow(() => SceneNavigator.GoTo("AnyScene"));
        }
    }
}
