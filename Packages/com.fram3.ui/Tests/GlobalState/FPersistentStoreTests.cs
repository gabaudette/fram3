#nullable enable
using System.Collections.Generic;
using Fram3.UI.GlobalState;
using NUnit.Framework;

namespace Fram3.UI.Tests.GlobalState
{
    [TestFixture]
    internal sealed class FPersistentStoreTests
    {
        // A simple in-memory adapter for testing.
        private sealed class InMemoryAdapter<T> : IPersistenceAdapter<T>
        {
            private readonly Dictionary<string, T> _store = new Dictionary<string, T>();
            private readonly string _key;

            public int SaveCount { get; private set; }

            public InMemoryAdapter(string key)
            {
                _key = key;
            }

            public void Save(T state)
            {
                SaveCount++;
                _store[_key] = state;
            }

            public bool TryLoad(out T state)
            {
                if (_store.TryGetValue(_key, out var value))
                {
                    state = value;
                    return true;
                }

                state = default!;
                return false;
            }
        }

        [Test]
        public void Constructor_NoPersistedData_UsesFallbackState()
        {
            var adapter = new InMemoryAdapter<int>("test");
            var store = new FPersistentStore<int>(42, adapter);

            Assert.That(store.State, Is.EqualTo(42));
        }

        [Test]
        public void Constructor_WithPersistedData_UsesLoadedState()
        {
            var adapter = new InMemoryAdapter<int>("test");
            adapter.Save(99);

            var store = new FPersistentStore<int>(0, adapter);

            Assert.That(store.State, Is.EqualTo(99));
        }

        [Test]
        public void StateChange_PersistsAutomatically()
        {
            var adapter = new InMemoryAdapter<int>("test");
            var store = new FPersistentStore<int>(0, adapter);

            // FCubit.Emit is protected; we subclass to expose it.
            var counter = new PersistentCounter(0, adapter);
            counter.Increment();

            Assert.That(adapter.SaveCount, Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void StateChange_SavedValueCanBeReloaded()
        {
            var adapter = new InMemoryAdapter<int>("counter");
            var first = new PersistentCounter(0, adapter);
            first.Increment();
            first.Increment();

            // Simulate a reload by constructing a new store using the same adapter.
            var second = new PersistentCounter(0, adapter);

            Assert.That(second.State, Is.EqualTo(2));
        }

        [Test]
        public void SameValue_DoesNotTriggerExtraSave()
        {
            var adapter = new InMemoryAdapter<int>("test");
            var counter = new PersistentCounter(5, adapter);
            var savesBefore = adapter.SaveCount;

            counter.SetTo(5);

            Assert.That(adapter.SaveCount, Is.EqualTo(savesBefore));
        }

        private sealed class PersistentCounter : FPersistentStore<int>
        {
            public PersistentCounter(int initial, IPersistenceAdapter<int> adapter)
                : base(initial, adapter) { }

            public void Increment() => Emit(State + 1);
            public void SetTo(int v) => Emit(v);
        }
    }
}
