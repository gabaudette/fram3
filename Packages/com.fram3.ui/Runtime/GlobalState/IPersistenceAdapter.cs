#nullable enable
namespace Fram3.UI.GlobalState
{
    /// <summary>
    /// Abstracts the persistence mechanism used by <see cref="PersistentStore{TState}"/>.
    /// Implement this interface to provide a custom storage back-end.
    /// A default PlayerPrefs implementation is provided by
    /// <see cref="PersistentStore{TState}"/> when running inside Unity.
    /// </summary>
    /// <typeparam name="TState">The type of state value to persist.</typeparam>
    public interface IPersistenceAdapter<TState>
    {
        /// <summary>
        /// Saves <paramref name="state"/> to persistent storage.
        /// Called every time the state changes.
        /// </summary>
        /// <param name="state">The state to persist.</param>
        void Save(TState state);

        /// <summary>
        /// Attempts to load the previously persisted state.
        /// Returns <c>true</c> and sets <paramref name="state"/> when persisted data exists.
        /// Returns <c>false</c> when no data has been saved yet, leaving
        /// <paramref name="state"/> as the default value for <typeparamref name="TState"/>.
        /// </summary>
        /// <param name="state">
        /// When this method returns <c>true</c>, contains the loaded state value.
        /// When this method returns <c>false</c>, contains the default value.
        /// </param>
        /// <returns><c>true</c> if persisted data was found; otherwise <c>false</c>.</returns>
        bool TryLoad(out TState state);
    }
}