#nullable enable
using System;
using System.Collections.Generic;

namespace Fram3.UI.Core
{
    /// <summary>
    /// Holds a single observable value and notifies listeners when it changes.
    /// Use this to share reactive state between parts of the UI that are not
    /// directly related in the element tree. Pair with
    /// <see cref="FValueListenableBuilder{T}"/> to rebuild a subtree whenever
    /// the value changes.
    /// </summary>
    /// <typeparam name="T">The type of value held by this notifier.</typeparam>
    public sealed class FValueNotifier<T> : IDisposable
    {
        private T _value;
        private readonly List<Action> _listeners = new List<Action>();
        private bool _disposed;

        /// <summary>
        /// Creates a new notifier with the given initial value.
        /// </summary>
        /// <param name="initialValue">The initial value.</param>
        public FValueNotifier(T initialValue)
        {
            _value = initialValue;
        }

        /// <summary>
        /// The current value. Setting this to a new value notifies all listeners.
        /// Setting it to a value that compares equal to the current value is a no-op.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown when accessed after disposal.</exception>
        public T Value
        {
            get
            {
                ThrowIfDisposed();
                return _value;
            }
            set
            {
                ThrowIfDisposed();
                if (EqualityComparer<T>.Default.Equals(_value, value))
                {
                    return;
                }

                _value = value;
                NotifyListeners();
            }
        }

        /// <summary>
        /// Registers a callback that is invoked whenever the value changes.
        /// The callback is invoked synchronously during the value assignment.
        /// </summary>
        /// <param name="listener">The callback to invoke on change.</param>
        /// <exception cref="ArgumentNullException">Thrown when listener is null.</exception>
        /// <exception cref="ObjectDisposedException">Thrown when called after disposal.</exception>
        public void AddListener(Action listener)
        {
            ThrowIfDisposed();
            if (listener is null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            _listeners.Add(listener);
        }

        /// <summary>
        /// Removes a previously registered listener. If the listener was added
        /// more than once, only the first occurrence is removed.
        /// Does nothing if the listener is not found.
        /// </summary>
        /// <param name="listener">The callback to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when listener is null.</exception>
        /// <exception cref="ObjectDisposedException">Thrown when called after disposal.</exception>
        public void RemoveListener(Action listener)
        {
            ThrowIfDisposed();
            if (listener is null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            _listeners.Remove(listener);
        }

        /// <summary>
        /// Disposes this notifier, clearing all listeners and preventing further use.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _listeners.Clear();
        }

        private void NotifyListeners()
        {
            foreach (var t in _listeners)
            {
                t.Invoke();
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(FValueNotifier<T>));
            }
        }
    }
}