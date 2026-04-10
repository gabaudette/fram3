#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// Lays out its children in a horizontal sequence.
    /// Maps to a UIToolkit <c>VisualElement</c> with <c>FlexDirection.Row</c>.
    /// </summary>
    public sealed class FRow : FMultiChildElement
    {
        /// <summary>
        /// Controls how children are distributed along the horizontal axis.
        /// Defaults to <see cref="FMainAxisAlignment.Start"/>.
        /// </summary>
        public FMainAxisAlignment MainAxisAlignment { get; }

        /// <summary>
        /// Controls how children are aligned along the vertical axis.
        /// Defaults to <see cref="FCrossAxisAlignment.Start"/>.
        /// </summary>
        public FCrossAxisAlignment CrossAxisAlignment { get; }

        /// <summary>
        /// Creates an <see cref="FRow"/> element.
        /// </summary>
        /// <param name="mainAxisAlignment">Alignment along the horizontal axis.</param>
        /// <param name="crossAxisAlignment">Alignment along the vertical axis.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FRow(
            FMainAxisAlignment mainAxisAlignment = FMainAxisAlignment.Start,
            FCrossAxisAlignment crossAxisAlignment = FCrossAxisAlignment.Start,
            FKey? key = null) : base(key)
        {
            MainAxisAlignment = mainAxisAlignment;
            CrossAxisAlignment = crossAxisAlignment;
        }
    }
}