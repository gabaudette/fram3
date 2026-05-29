#nullable enable
#if !FRAM3_PURE_TESTS
using System;
using UnityEngine;

namespace Fram3.UI.GlobalState
{
    /// <summary>
    /// An <see cref="IPersistenceAdapter{TState}"/> that persists state as JSON
    /// in <c>UnityEngine.PlayerPrefs</c>. Suitable for lightweight state that
    /// fits comfortably in a string (settings, preferences, small session data).
    /// </summary>
    /// <typeparam name="TState">
    /// The type of state to persist. Must be serializable by <c>UnityEngine.JsonUtility</c>.
    /// The state type must be marked with <c>[Serializable]</c> and use public fields
    /// or properties backed by serializable fields.
    /// </typeparam>
    // ReSharper disable once UnusedType.Global
    public sealed class PlayerPrefsAdapter<TState> : IPersistenceAdapter<TState>
    {
        private readonly string _key;

        /// <summary>
        /// Creates an adapter that stores state under the given PlayerPrefs key.
        /// </summary>
        /// <param name="key">The PlayerPrefs key under which the state JSON is stored.</param>
        /// <exception cref="ArgumentNullException">Thrown when key is null or empty.</exception>
        public PlayerPrefsAdapter(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            _key = key;
        }

        /// <inheritdoc/>
        public void Save(TState state)
        {
            var json = JsonUtility.ToJson(state);
            PlayerPrefs.SetString(_key, json);
            PlayerPrefs.Save();
        }

        /// <inheritdoc/>
        public bool TryLoad(out TState state)
        {
            if (!PlayerPrefs.HasKey(_key))
            {
                state = default!;
                return false;
            }

            var json = PlayerPrefs.GetString(_key);
            state = JsonUtility.FromJson<TState>(json)!;
            return true;
        }
    }
}
#endif