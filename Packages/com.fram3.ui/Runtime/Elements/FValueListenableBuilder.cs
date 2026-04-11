#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// An element that rebuilds its subtree whenever the value of an
    /// <see cref="FValueNotifier{T}"/> changes.
    /// Subscribe a notifier and provide a builder delegate that turns the current
    /// value into an element tree. The subtree is rebuilt automatically each time
    /// the notifier emits a new value.
    /// </summary>
    /// <typeparam name="T">The type of value held by the notifier.</typeparam>
    public sealed class FValueListenableBuilder<T> : FStatefulElement
    {
        /// <summary>
        /// The notifier whose value changes drive rebuilds of this element's subtree.
        /// </summary>
        public FValueNotifier<T> Notifier { get; }

        /// <summary>
        /// A delegate that maps the current notifier value to a child element.
        /// Called during every build triggered by a value change.
        /// </summary>
        public Func<FBuildContext, T, FElement> Builder { get; }

        /// <summary>
        /// Creates a new listenable builder.
        /// </summary>
        /// <param name="notifier">The notifier to observe.</param>
        /// <param name="builder">
        /// A delegate that receives the build context and the current value
        /// and returns the element subtree.
        /// </param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="notifier"/> or <paramref name="builder"/> is null.
        /// </exception>
        public FValueListenableBuilder(
            FValueNotifier<T> notifier,
            Func<FBuildContext, T, FElement> builder,
            FKey? key = null
        ) : base(key)
        {
            Notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc/>
        public override FState CreateState() => new FValueListenableBuilderState();

        private sealed class FValueListenableBuilderState : FState<FValueListenableBuilder<T>>
        {
            private Action? _listener;
            private FValueNotifier<T>? _subscribedNotifier;

            public override void InitState()
            {
                _listener = OnValueChanged;
                _subscribedNotifier = Element!.Notifier;
                _subscribedNotifier.AddListener(_listener);
            }

            public override FElement Build(FBuildContext context)
            {
                return Element!.Builder(context, Element.Notifier.Value);
            }

            public override void DidUpdateElement(FStatefulElement oldElement)
            {
                var old = (FValueListenableBuilder<T>)oldElement;
                if (ReferenceEquals(old.Notifier, Element!.Notifier))
                {
                    return;
                }

                _subscribedNotifier!.RemoveListener(_listener!);
                _subscribedNotifier = Element.Notifier;
                _subscribedNotifier.AddListener(_listener!);
            }

            public override void Dispose()
            {
                _subscribedNotifier?.RemoveListener(_listener!);
                _subscribedNotifier = null;
                _listener = null;
            }

            private void OnValueChanged()
            {
                SetState(null);
            }
        }
    }
}