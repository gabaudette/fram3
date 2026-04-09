#nullable enable
using System;

namespace Fram3.UI.Core
{
    /// <summary>
    /// Controls how one element replaces another in the element tree.
    /// Elements with matching runtime types and keys are updated in place
    /// during reconciliation rather than being torn down and recreated.
    /// </summary>
    public abstract class FKey : IEquatable<FKey>
    {
        /// <summary>
        /// Determines whether this key is equal to another key.
        /// </summary>
        /// <param name="other">The key to compare against.</param>
        /// <returns>True if the keys are considered equal.</returns>
        public abstract bool Equals(FKey? other);

        /// <summary>
        /// Returns a hash code for this key.
        /// </summary>
        public abstract override int GetHashCode();

        /// <summary>
        /// Determines whether this key is equal to another object.
        /// </summary>
        public override bool Equals(object? obj)
        {
            return obj is FKey other && Equals(other);
        }

        public static bool operator ==(FKey? left, FKey? right)
        {
            if (left is null)
            {
                return right is null;
            }
            return right != null && left.Equals(right);
        }

        public static bool operator !=(FKey? left, FKey? right)
        {
            return !(left == right);
        }
    }

    /// <summary>
    /// A key that uses a value for equality comparison.
    /// Use this to give elements a stable identity across rebuilds,
    /// particularly in lists where items may be reordered.
    /// </summary>
    /// <typeparam name="T">The type of the value used as the key.</typeparam>
    public sealed class FValueKey<T> : FKey where T : IEquatable<T>?
    {
        /// <summary>
        /// The value used to identify this key.
        /// </summary>
        public T? Value { get; }

        /// <summary>
        /// Creates a new value key with the specified identity value.
        /// </summary>
        /// <param name="value">The value to use as the key identity.</param>
        public FValueKey(T value)
        {
            Value = value;
        }

        public override bool Equals(FKey? other)
        {
            if (other is FValueKey<T> otherValueKey)
            {
                return EqualityHelper(otherValueKey);
            }
            
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(typeof(T), Value);
        }

        private bool EqualityHelper(FValueKey<T> other)
        {
            if (Value is null)
            {
                return other.Value is null;
            }
            
            return other.Value != null && Value.Equals(other.Value);
        }
    }

    /// <summary>
    /// A key that uses object reference identity.
    /// Two ObjectKeys are equal only if they wrap the exact same object instance.
    /// </summary>
    public sealed class FObjectKey : FKey
    {
        /// <summary>
        /// The object instance used as the identity for this key.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Creates a new object key using reference identity of the specified object.
        /// </summary>
        /// <param name="value">The object whose reference identity to use.</param>
        public FObjectKey(object? value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override bool Equals(FKey? other)
        {
            if (other is FObjectKey otherObjectKey)
            {
                return ReferenceEquals(Value, otherObjectKey.Value);
            }
            
            return false;
        }

        public override int GetHashCode()
        {
            return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(Value);
        }
    }

    /// <summary>
    /// A key that guarantees uniqueness. No two UniqueKey instances are ever equal,
    /// even if created with the same parameters. Useful when an element must never
    /// be reused during reconciliation.
    /// </summary>
    public sealed class FUniqueKey : FKey
    {
        public override bool Equals(FKey? other)
        {
            return ReferenceEquals(this, other);
        }

        public override int GetHashCode()
        {
            return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
        }
    }
}
