#nullable enable
namespace Fram3.UI.Styling
{
    /// <summary>
    /// Controls how children are distributed along the main axis of a
    /// <c>FColumn</c> (vertical) or <c>FRow</c> (horizontal).
    /// Maps to UIToolkit <c>Justify</c> values.
    /// </summary>
    public enum FMainAxisAlignment
    {
        /// <summary>Pack children toward the start of the main axis.</summary>
        Start,

        /// <summary>Pack children toward the end of the main axis.</summary>
        End,

        /// <summary>Center children along the main axis.</summary>
        Center,

        /// <summary>Distribute children with equal space between them.</summary>
        SpaceBetween,

        /// <summary>Distribute children with equal space around them.</summary>
        SpaceAround,

        /// <summary>Distribute children with equal space between and on the edges.</summary>
        SpaceEvenly
    }
}