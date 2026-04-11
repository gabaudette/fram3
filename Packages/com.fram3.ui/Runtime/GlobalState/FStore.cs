#nullable enable
using System;

namespace Fram3.UI.GlobalState
{
    /// <summary>
    /// A Redux-style reactive state container. Holds a single immutable state value
    /// and mutates it exclusively through a pure <see cref="Reducer{TState}"/> function
    /// that maps the current state and a dispatched <see cref="FAction"/> to a new state.
    /// </summary>
    /// <typeparam name="TState">
    /// The type that represents the application state held by this store.
    /// Value equality is used to suppress redundant rebuilds. Use a C# <c>record</c>
    /// type or subclass <see cref="Fram3.UI.Core.FEquatable"/> to express structural equality.
    /// </typeparam>
    /// <remarks>
    /// Expose the store to the element tree by wrapping the root with
    /// <c>FProvider&lt;FStore&lt;TState&gt;&gt;</c>. Consume it via
    /// <c>FConsumer&lt;FStore&lt;TState&gt;&gt;</c> or
    /// <c>FSelector&lt;FStore&lt;TState&gt;, TState, TValue&gt;</c>.
    /// </remarks>
    public sealed class FStore<TState> : FCubit<TState>
    {
        private readonly Func<TState, FAction, TState> _reducer;

        /// <summary>
        /// Creates a store with an initial state and a pure reducer function.
        /// </summary>
        /// <param name="initialState">The initial state of the store.</param>
        /// <param name="reducer">
        /// A pure function that takes the current state and a dispatched action
        /// and returns the next state. Must not be null.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when reducer is null.</exception>
        public FStore(TState initialState, Func<TState, FAction, TState> reducer) : base(initialState)
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
        public void Dispatch(FAction action)
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
