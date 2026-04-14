#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.State
{
    /// <summary>
    /// An element that rebuilds its subtree whenever the value of an
    /// <see cref="ValueNotifier{T}"/> changes.
    /// Subscribe a notifier and provide a builder delegate that turns the current
    /// value into an element tree. The subtree is rebuilt automatically each time
    /// the notifier emits a new value.
    /// </summary>
    /// <typeparam name="T">The type of value held by the notifier.</typeparam>
    public sealed class ValueListenableBuilder<T> : StatefulElement
    {
        /// <summary>
        /// The notifier whose value changes drive rebuilds of this element's subtree.
        /// </summary>
        public ValueNotifier<T> Notifier { get; }

        /// <summary>
        /// A delegate that maps the current notifier value to a child element.
        /// Called during every build triggered by a value change.
        /// </summary>
        public Func<BuildContext, T, Element> Builder { get; }

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
        public ValueListenableBuilder(
            ValueNotifier<T> notifier,
            Func<BuildContext, T, Element> builder,
            Key? key = null
        ) : base(key)
        {
            Notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc/>
        public override Fram3.UI.Core.State CreateState() => new ValueListenableBuilderState();

        private sealed class ValueListenableBuilderState : Fram3.UI.Core.State<ValueListenableBuilder<T>>
        {
            private Action? _listener;
            private ValueNotifier<T>? _subscribedNotifier;

            public override void InitState()
            {
                _listener = OnValueChanged;
                _subscribedNotifier = Element!.Notifier;
                _subscribedNotifier.AddListener(_listener);
            }

            public override Element Build(BuildContext context)
            {
                return Element!.Builder(context, Element.Notifier.Value);
            }

            public override void DidUpdateElement(StatefulElement oldElement)
            {
                var old = (ValueListenableBuilder<T>)oldElement;
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