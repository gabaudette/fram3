#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.GlobalState;

namespace Fram3.UI.Elements.State
{
    /// <summary>
    /// Selects a derived value from a <typeparamref name="TCubit"/> state and rebuilds
    /// its subtree only when that derived value changes, ignoring state changes that
    /// do not affect the selected value. This makes it more efficient than
    /// <see cref="CubitBuilder{TCubit,TState}"/> when only a small slice of the state
    /// drives a given piece of UI.
    /// </summary>
    /// <typeparam name="TCubit">
    /// The concrete <see cref="Cubit{TState}"/> type to listen to.
    /// Must be provided by a <see cref="Provider{T}"/> ancestor in the tree.
    /// </typeparam>
    /// <typeparam name="TState">The state type of the cubit.</typeparam>
    /// <typeparam name="TValue">
    /// The derived value type returned by the selector function.
    /// Equality is determined via <see cref="EqualityComparer{T}.Default"/>.
    /// </typeparam>
    public sealed class Selector<TCubit, TState, TValue> : StatefulElement
        where TCubit : Cubit<TState>
    {
        /// <summary>
        /// The function that derives a value from the current cubit state.
        /// </summary>
        public Func<TState, TValue> SelectorFunc { get; }

        /// <summary>
        /// The builder function that receives the derived value and returns the UI subtree.
        /// </summary>
        public Func<BuildContext, TValue, Element> Builder { get; }

        /// <summary>
        /// Creates an <see cref="Selector{TCubit, TState, TValue}"/>.
        /// </summary>
        /// <param name="selector">
        /// A pure function that extracts the relevant value from the cubit state.
        /// Must not be null.
        /// </param>
        /// <param name="builder">
        /// A function that receives the derived value and returns the UI subtree.
        /// Must not be null.
        /// </param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when selector or builder is null.
        /// </exception>
        public Selector(
            Func<TState, TValue> selector,
            Func<BuildContext, TValue, Element> builder,
            Key? key = null
        ) : base(key)
        {
            SelectorFunc = selector ?? throw new ArgumentNullException(nameof(selector));
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc/>
        public override Fram3.UI.Core.State CreateState() => new SelectorState();

        private sealed class SelectorState : Fram3.UI.Core.State<Selector<TCubit, TState, TValue>>
        {
            private TCubit? _cubit;
            private TValue _lastValue = default!;
            private Action<TState>? _listener;

            public override void InitState()
            {
                _cubit = Context!.GetInherited<Provider<TCubit>>().Value;
                _lastValue = Element!.SelectorFunc(_cubit.State);
                _listener = OnStateChanged;
                _cubit.AddListener(_listener);
            }

            private void OnStateChanged(TState state)
            {
                var newValue = Element!.SelectorFunc(state);
                if (EqualityComparer<TValue>.Default.Equals(_lastValue, newValue))
                {
                    return;
                }

                _lastValue = newValue;
                SetState(null);
            }

            public override Element Build(BuildContext context)
            {
                var value = Element!.SelectorFunc(_cubit!.State);
                return Element.Builder(context, value);
            }

            public override void DidUpdateElement(StatefulElement oldElement)
            {
            }

            public override void Dispose()
            {
                if (_cubit != null && _listener != null)
                {
                    _cubit.RemoveListener(_listener);
                }
            }
        }
    }
}