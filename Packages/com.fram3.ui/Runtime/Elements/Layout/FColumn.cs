#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Layout
{
    /// <summary>
    /// Lays out its children in a vertical sequence.
    /// Maps to a UIToolkit <c>VisualElement</c> with <c>FlexDirection.Column</c>.
    /// </summary>
    public sealed class FColumn : FMultiChildElement
    {
        /// <summary>
        /// Controls how children are distributed along the vertical axis.
        /// Defaults to <see cref="FMainAxisAlignment.Start"/>.
        /// </summary>
        public FMainAxisAlignment MainAxisAlignment { get; }

        /// <summary>
        /// Controls how children are aligned along the horizontal axis.
        /// Defaults to <see cref="FCrossAxisAlignment.Start"/>.
        /// </summary>
        public FCrossAxisAlignment CrossAxisAlignment { get; }

        /// <summary>
        /// Creates an <see cref="FColumn"/> element.
        /// </summary>
        /// <param name="mainAxisAlignment">Alignment along the vertical axis.</param>
        /// <param name="crossAxisAlignment">Alignment along the horizontal axis.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FColumn(
            FMainAxisAlignment mainAxisAlignment = FMainAxisAlignment.Start,
            FCrossAxisAlignment crossAxisAlignment = FCrossAxisAlignment.Start,
            FKey? key = null
        ) : base(key)
        {
            MainAxisAlignment = mainAxisAlignment;
            CrossAxisAlignment = crossAxisAlignment;
        }
    }
}