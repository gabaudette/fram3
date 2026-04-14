#nullable enable
using System;
using System.Collections.Generic;

namespace Fram3.UI.GlobalState
{
    /// <summary>
    /// A reactive state machine that owns a single immutable state value
    /// and notifies listeners whenever that state changes.
    /// Subclass <see cref="Cubit{TState}"/> and add methods that call
    /// <see cref="Emit"/> to transition to a new state.
    /// Pair with <see cref="Fram3.UI.Elements.State.CubitBuilder{TCubit,TState}"/>
    /// to rebuild UI subtrees in response to state changes, or expose via
    /// <see cref="Fram3.UI.Elements.State.Provider{T}"/> for tree-wide access.
    /// </summary>
    /// <typeparam name="TState">
    /// The type that represents the state of this cubit.
    /// Value equality is used to suppress rebuilds when the new state is
    /// equivalent to the current one. Use a C# <c>record</c> type or subclass
    /// </typeparam>
    public abstract class Cubit<TState> : IDisposable
    {
        private TState _state;
        private readonly List<Action<TState>> _listeners = new();
        private bool _disposed;

        /// <summary>
        /// Creates a new cubit with the given initial state.
        /// </summary>
        /// <param name="initialState">The initial state value.</param>
        protected Cubit(TState initialState)
        {
            _state = initialState;
        }

        /// <summary>
        /// The current state of this cubit.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown when accessed after disposal.</exception>
        public TState State
        {
            get
            {
                ThrowIfDisposed();
                return _state;
            }
        }

        /// <summary>
        /// Registers a callback that is invoked whenever the state changes.
        /// </summary>
        /// <param name="listener">The callback to invoke. Receives the new state value.</param>
        /// <exception cref="ArgumentNullException">Thrown when listener is null.</exception>
        /// <exception cref="ObjectDisposedException">Thrown when called after disposal.</exception>
        public void AddListener(Action<TState> listener)
        {
            ThrowIfDisposed();
            if (listener is null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            _listeners.Add(listener);
        }

        /// <summary>
        /// Removes a previously registered listener.
        /// Does nothing if the listener is not found.
        /// </summary>
        /// <param name="listener">The callback to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when listener is null.</exception>
        /// <exception cref="ObjectDisposedException">Thrown when called after disposal.</exception>
        public void RemoveListener(Action<TState> listener)
        {
            ThrowIfDisposed();
            if (listener is null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            _listeners.Remove(listener);
        }

        /// <summary>
        /// Disposes this cubit, clearing all listeners and preventing further state transitions.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _listeners.Clear();
            OnDispose();
        }

        /// <summary>
        /// Called when this cubit is disposed. Override to release additional resources.
        /// </summary>
        protected virtual void OnDispose()
        {
        }

        /// <summary>
        /// Transitions to <paramref name="newState"/>, notifying all listeners.
        /// If <paramref name="newState"/> is equal to the current state the call is a no-op
        /// and no listeners are invoked. Equality is determined via
        /// <see cref="EqualityComparer{T}.Default"/>
        /// </summary>
        /// <param name="newState">The state to transition to.</param>
        /// <exception cref="ObjectDisposedException">Thrown when called after disposal.</exception>
        protected void Emit(TState newState)
        {
            ThrowIfDisposed();
            if (EqualityComparer<TState>.Default.Equals(_state, newState))
            {
                return;
            }

            _state = newState;
            NotifyListeners(newState);
        }

        private void NotifyListeners(TState state)
        {
            foreach (var listener in _listeners)
            {
                listener(state);
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}