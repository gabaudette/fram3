#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Layout
{
    /// <summary>
    /// Lays out its children in a horizontal sequence.
    /// Maps to a UIToolkit <c>VisualElement</c> with <c>FlexDirection.Row</c>.
    /// </summary>
    public sealed class Row : MultiChildElement
    {
        /// <summary>
        /// Controls how children are distributed along the horizontal axis.
        /// Defaults to <see cref="MainAxisAlignment.Start"/>.
        /// </summary>
        public MainAxisAlignment MainAxisAlignment { get; }

        /// <summary>
        /// Controls how children are aligned along the vertical axis.
        /// Defaults to <see cref="CrossAxisAlignment.Start"/>.
        /// </summary>
        public CrossAxisAlignment CrossAxisAlignment { get; }

        /// <summary>
        /// Creates an <see cref="Row"/> element.
        /// </summary>
        /// <param name="mainAxisAlignment">Alignment along the horizontal axis.</param>
        /// <param name="crossAxisAlignment">Alignment along the vertical axis.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Row(
            MainAxisAlignment mainAxisAlignment = MainAxisAlignment.Start,
            CrossAxisAlignment crossAxisAlignment = CrossAxisAlignment.Start,
            Key? key = null
        ) : base(key)
        {
            MainAxisAlignment = mainAxisAlignment;
            CrossAxisAlignment = crossAxisAlignment;
        }
    }
}