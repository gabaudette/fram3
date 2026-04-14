#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.GlobalState;

namespace Fram3.UI.Elements.State
{
    /// <summary>
    /// Rebuilds its subtree whenever the state of the nearest ancestor
    /// <see cref="Provider{T}"/>-provided <typeparamref name="TCubit"/> changes.
    /// </summary>
    /// <typeparam name="TCubit">
    /// The concrete <see cref="Cubit{TState}"/> type to listen to.
    /// Must be provided by a <see cref="Provider{T}"/> ancestor in the tree.
    /// </typeparam>
    /// <typeparam name="TState">The state type of the cubit.</typeparam>
    public sealed class CubitBuilder<TCubit, TState> : StatefulElement
        where TCubit : Cubit<TState>
    {
        /// <summary>
        /// The builder function that receives the build context and current state
        /// and returns an element describing the UI.
        /// </summary>
        public Func<BuildContext, TState, Element> Builder { get; }

        /// <summary>
        /// Creates an <see cref="CubitBuilder{TCubit, TState}"/>.
        /// </summary>
        /// <param name="builder">
        /// A function that receives the current cubit state and returns the UI subtree.
        /// Must not be null.
        /// </param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when builder is null.</exception>
        public CubitBuilder(Func<BuildContext, TState, Element> builder, Key? key = null)
            : base(key)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc/>
        public override Fram3.UI.Core.State CreateState() => new CubitBuilderState();

        private sealed class CubitBuilderState : Fram3.UI.Core.State<CubitBuilder<TCubit, TState>>
        {
            private TCubit? _cubit;
            private Action<TState>? _listener;

            public override void InitState()
            {
                _cubit = Context!.GetInherited<Provider<TCubit>>().Value;
                _listener = _ => SetState(null);
                _cubit.AddListener(_listener);
            }

            public override Element Build(BuildContext context)
            {
                return Element!.Builder(context, _cubit!.State);
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