#nullable enable
using System;
using Fram3.UI.GlobalState;
using NUnit.Framework;

namespace Fram3.UI.Tests.GlobalState
{
    [TestFixture]
    internal sealed class CubitTests
    {
        private sealed class CounterCubit : Cubit<int>
        {
            public CounterCubit(int initial = 0) : base(initial)
            {
            }

            public void Increment() => Emit(State + 1);

            // ReSharper disable once UnusedMember.Local
            public void Decrement() => Emit(State - 1);

            public void SetTo(int value) => Emit(value);
        }

        [Test]
        public void InitialState_IsSetCorrectly()
        {
            var cubit = new CounterCubit(10);

            Assert.That(cubit.State, Is.EqualTo(10));
        }

        [Test]
        public void Emit_ChangesState()
        {
            var cubit = new CounterCubit();

            cubit.Increment();

            Assert.That(cubit.State, Is.EqualTo(1));
        }

        [Test]
        public void Emit_NotifiesListeners()
        {
            var cubit = new CounterCubit();
            int? received = null;
            cubit.AddListener(v => received = v);

            cubit.Increment();

            Assert.That(received, Is.EqualTo(1));
        }

        [Test]
        public void Emit_SameValue_DoesNotNotifyListeners()
        {
            var cubit = new CounterCubit(5);
            var callCount = 0;
            cubit.AddListener(_ => callCount++);

            cubit.SetTo(5);

            Assert.That(callCount, Is.EqualTo(0));
        }

        [Test]
        public void AddListener_Null_ThrowsArgumentNullException()
        {
            var cubit = new CounterCubit();

            Assert.Throws<ArgumentNullException>(() => cubit.AddListener(null!));
        }

        [Test]
        public void RemoveListener_Null_ThrowsArgumentNullException()
        {
            var cubit = new CounterCubit();

            Assert.Throws<ArgumentNullException>(() => cubit.RemoveListener(null!));
        }

        [Test]
        public void RemoveListener_StopsNotifications()
        {
            var cubit = new CounterCubit();
            var callCount = 0;

            cubit.AddListener(Listener);
            cubit.Increment();
            cubit.RemoveListener(Listener);
            cubit.Increment();

            Assert.That(callCount, Is.EqualTo(1));
            return;

            void Listener(int _) => callCount++;
        }

        [Test]
        public void Dispose_PreventsStateAccess()
        {
            var cubit = new CounterCubit();
            cubit.Dispose();

            Assert.Throws<ObjectDisposedException>(() => { _ = cubit.State; });
        }

        [Test]
        public void Dispose_PreventsFurtherListenerRegistration()
        {
            var cubit = new CounterCubit();
            cubit.Dispose();

            Assert.Throws<ObjectDisposedException>(() => cubit.AddListener(_ => { }));
        }

        [Test]
        public void Dispose_ClearsListeners()
        {
            var cubit = new CounterCubit();
            var callCount = 0;
            cubit.AddListener(_ => callCount++);
            cubit.Dispose();

            // Listeners are gone -- no throw, no invocation.
            Assert.That(callCount, Is.EqualTo(0));
        }

        [Test]
        public void Dispose_CalledTwice_DoesNotThrow()
        {
            var cubit = new CounterCubit();

            cubit.Dispose();

            Assert.DoesNotThrow(cubit.Dispose);
        }

        [Test]
        public void MultipleListeners_AllReceiveNotification()
        {
            var cubit = new CounterCubit();
            var received = new System.Collections.Generic.List<int>();

            cubit.AddListener(received.Add);
            cubit.AddListener(v => received.Add(v * 10));
            cubit.Increment();

            Assert.That(received, Is.EqualTo(new[] { 1, 10 }));
        }

        [Test]
        public void RecordState_SameValue_DoesNotNotify()
        {
            var cubit = new EquatableCubit(new EquatableState(7));
            var callCount = 0;
            cubit.AddListener(_ => callCount++);

            cubit.SetSameValue();

            Assert.That(callCount, Is.EqualTo(0));
        }

        [Test]
        public void RecordState_DifferentValue_Notifies()
        {
            var cubit = new EquatableCubit(new EquatableState(7));
            var callCount = 0;
            cubit.AddListener(_ => callCount++);

            cubit.SetDifferentValue();

            Assert.That(callCount, Is.EqualTo(1));
        }

        private record EquatableState(int Value);

        private sealed class EquatableCubit : Cubit<EquatableState>
        {
            public EquatableCubit(EquatableState initial) : base(initial)
            {
            }

            public void SetSameValue() => Emit(new EquatableState(State.Value));
            public void SetDifferentValue() => Emit(new EquatableState(State.Value + 1));
        }
    }
}