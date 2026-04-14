#nullable enable
using System;

namespace Fram3.UI.GlobalState
{
    /// <summary>
    /// A Redux-style reactive state container. Holds a single immutable state value
    /// and mutates it exclusively through a pure reducer function
    /// that maps the current state and a dispatched <see cref="FrameAction"/> to a new state.
    /// </summary>
    /// <typeparam name="TState">
    /// The type that represents the application state held by this store.
    /// Value equality is used to suppress redundant rebuilds.
    /// </typeparam>
    /// <remarks>
    /// Expose the store to the element tree by wrapping the root with
    /// <c>Provider&lt;Store&lt;TState&gt;&gt;</c>. Consume it via
    /// <c>Consumer&lt;Store&lt;TState&gt;&gt;</c> or
    /// <c>Selector&lt;Store&lt;TState&gt;, TState, TValue&gt;</c>.
    /// </remarks>
    public sealed class Store<TState> : Cubit<TState>
    {
        private readonly Func<TState, FrameAction, TState> _reducer;

        /// <summary>
        /// Creates a store with an initial state and a pure reducer function.
        /// </summary>
        /// <param name="initialState">The initial state of the store.</param>
        /// <param name="reducer">
        /// A pure function that takes the current state and a dispatched action
        /// and returns the next state. Must not be null.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when reducer is null.</exception>
        public Store(TState initialState, Func<TState, FrameAction, TState> reducer) : base(initialState)
        {
            _reducer = reducer ?? throw new ArgumentNullException(nameof(reducer));
        }

        /// <summary>
        /// Dispatches <paramref name="action"/> through the reducer, computing the next
        /// state and emitting it if it differs from the current state.
        /// </summary>
        /// <param name="action">The action to dispatch. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when action is null.</exception>
        /// <exception cref="ObjectDisposedException">Thrown when called after disposal.</exception>
        public void Dispatch(FrameAction action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var next = _reducer(State, action);
            Emit(next);
        }
    }
}