#nullable enable
using System;
using Fram3.UI.GlobalState;
using NUnit.Framework;

namespace Fram3.UI.Tests.GlobalState
{
    [TestFixture]
    internal sealed class FStoreTests
    {
        private sealed class IncrementAction : FAction { }
        private sealed class DecrementAction : FAction { }
        private sealed class ResetAction : FAction { }

        private static int Reducer(int state, FAction action)
        {
            return action switch
            {
                IncrementAction => state + 1,
                DecrementAction => state - 1,
                ResetAction => 0,
                _ => state
            };
        }

        [Test]
        public void InitialState_IsSetCorrectly()
        {
            var store = new FStore<int>(10, Reducer);

            Assert.That(store.State, Is.EqualTo(10));
        }

        [Test]
        public void Dispatch_UpdatesState()
        {
            var store = new FStore<int>(0, Reducer);

            store.Dispatch(new IncrementAction());

            Assert.That(store.State, Is.EqualTo(1));
        }

        [Test]
        public void Dispatch_NotifiesListeners()
        {
            var store = new FStore<int>(0, Reducer);
            int? received = null;
            store.AddListener(v => received = v);

            store.Dispatch(new IncrementAction());

            Assert.That(received, Is.EqualTo(1));
        }

        [Test]
        public void Dispatch_UnknownAction_KeepsStateUnchanged()
        {
            var store = new FStore<int>(5, Reducer);
            var callCount = 0;
            store.AddListener(_ => callCount++);

            store.Dispatch(new ResetAction());

            Assert.That(store.State, Is.EqualTo(0));
        }

        [Test]
        public void Dispatch_ReducerReturningCurrentState_DoesNotNotifyListeners()
        {
            // A reducer that always returns the same value should not notify.
            var store = new FStore<int>(0, (state, _) => state);
            var callCount = 0;
            store.AddListener(_ => callCount++);

            store.Dispatch(new IncrementAction());

            Assert.That(callCount, Is.EqualTo(0));
        }

        [Test]
        public void Dispatch_Null_ThrowsArgumentNullException()
        {
            var store = new FStore<int>(0, Reducer);

            Assert.Throws<ArgumentNullException>(() => store.Dispatch(null!));
        }

        [Test]
        public void Constructor_NullReducer_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new FStore<int>(0, null!));
        }

        [Test]
        public void Dispatch_MultipleActions_AreAppliedSequentially()
        {
            var store = new FStore<int>(0, Reducer);

            store.Dispatch(new IncrementAction());
            store.Dispatch(new IncrementAction());
            store.Dispatch(new IncrementAction());
            store.Dispatch(new DecrementAction());

            Assert.That(store.State, Is.EqualTo(2));
        }
    }
}
