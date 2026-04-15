#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Input
{
    /// <summary>
    /// Non-generic view of an <see cref="EnumField{T}"/> used by the painter to create
    /// and update the native <c>EnumField</c> without knowing the concrete enum type.
    /// </summary>
    internal interface IEnumFieldDescriptor
    {
        Enum ValueAsEnum { get; }
        string? Label { get; }
        void InvokeOnChanged(Enum newValue);
        bool HasOnChanged { get; }
    }

    /// <summary>
    /// A dropdown field bound to a C# enum type, displaying all enum values as options.
    /// Maps to UIToolkit's <c>EnumField</c>.
    /// </summary>
    /// <typeparam name="T">The enum type. Must be a value type and a <see cref="System.Enum"/>.</typeparam>
    public sealed class EnumField<T> : LeafElement, IEnumFieldDescriptor where T : struct, Enum
    {
        /// <summary>The currently selected enum value.</summary>
        public T Value { get; }

        /// <summary>
        /// An optional text label displayed alongside the field.
        /// Null means no label is shown.
        /// </summary>
        public string? Label { get; }

        /// <summary>
        /// Callback invoked whenever the selected value changes.
        /// Receives the new enum value as its argument.
        /// </summary>
        public Action<T>? OnChanged { get; }

        /// <summary>
        /// Creates an <see cref="EnumField{T}"/> element.
        /// </summary>
        /// <param name="value">The currently selected enum value.</param>
        /// <param name="onChanged">Callback invoked on every selection change.</param>
        /// <param name="label">Optional label text shown beside the field.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public EnumField(
            T value = default,
            Action<T>? onChanged = null,
            string? label = null,
            Key? key = null
        ) : base(key)
        {
            Value = value;
            OnChanged = onChanged;
            Label = label;
        }

        Enum IEnumFieldDescriptor.ValueAsEnum => Value;
        bool IEnumFieldDescriptor.HasOnChanged => OnChanged != null;

        void IEnumFieldDescriptor.InvokeOnChanged(Enum newValue)
        {
            if (newValue is T typed)
            {
                OnChanged?.Invoke(typed);
            }
        }
    }
}