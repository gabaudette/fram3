#nullable enable
using System;

namespace Fram3.UI.GlobalState
{
    /// <summary>
    /// A reactive state store that automatically persists its state across scene reloads
    /// using a pluggable <see cref="IPersistenceAdapter{TState}"/>.
    /// On first access the previously persisted state is restored; every subsequent
    /// <see cref="FCubit{TState}.Emit"/> call that changes the state persists the new state.
    /// </summary>
    /// <typeparam name="TState">
    /// The type of state held by this store. Must be serialisable by the adapter.
    /// </typeparam>
    /// <remarks>
    /// Inside Unity use the <c>FPersistentStore.PlayerPrefs</c> factory method to obtain
    /// a store backed by <c>UnityEngine.PlayerPrefs</c> and <c>System.Text.Json</c>.
    /// Outside Unity, or in tests, inject a custom <see cref="IPersistenceAdapter{TState}"/>.
    /// </remarks>
    public class FPersistentStore<TState> : FCubit<TState>
    {
        private readonly IPersistenceAdapter<TState> _adapter;

        /// <summary>
        /// Creates a persistent store. If the adapter can load a previously persisted state
        /// that state is used as the initial value; otherwise <paramref name="fallbackState"/>
        /// is used.
        /// </summary>
        /// <param name="fallbackState">
        /// The state to use when no persisted state is available.
        /// </param>
        /// <param name="adapter">
        /// The adapter responsible for saving and loading state. Must not be null.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when adapter is null.</exception>
        public FPersistentStore(TState fallbackState, IPersistenceAdapter<TState> adapter)
            : base(ResolveInitialState(fallbackState, adapter))
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            AddListener(PersistState);
        }

        private void PersistState(TState state)
        {
            _adapter.Save(state);
        }

        private static TState ResolveInitialState(TState fallback, IPersistenceAdapter<TState> adapter)
        {
            if (adapter is null)
            {
                return fallback;
            }

            return adapter.TryLoad(out var loaded) ? loaded : fallback;
        }
    }
}
