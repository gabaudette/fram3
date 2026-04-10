#nullable enable
namespace Fram3.UI.Styling
{
    /// <summary>
    /// Controls how children are aligned along the cross axis of a
    /// <c>FColumn</c> (horizontal) or <c>FRow</c> (vertical).
    /// Maps to UIToolkit <c>Align</c> values.
    /// </summary>
    public enum FCrossAxisAlignment
    {
        /// <summary>Align children toward the start of the cross axis.</summary>
        Start,

        /// <summary>Align children toward the end of the cross axis.</summary>
        End,

        /// <summary>Center children along the cross axis.</summary>
        Center,

        /// <summary>Stretch children to fill the cross axis.</summary>
        Stretch
    }
}