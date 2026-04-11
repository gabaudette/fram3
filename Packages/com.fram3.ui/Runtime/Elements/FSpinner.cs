#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// An indeterminate progress spinner that continuously rotates a fixed arc.
    /// The animation is driven by an internal <c>FAnimationController</c> and
    /// requires <c>FRenderer.Tick</c> to be called each frame.
    /// Maps to an <c>FSpinnerElement</c> native visual that draws the arc using
    /// UIToolkit's <c>MeshGenerationContext</c>.
    /// </summary>
    public sealed class FSpinner : FLeafElement
    {
        /// <summary>
        /// The diameter of the spinner in logical pixels. Defaults to 32.
        /// </summary>
        public float Size { get; }

        /// <summary>
        /// The thickness of the spinning arc in logical pixels. Defaults to 4.
        /// </summary>
        public float StrokeWidth { get; }

        /// <summary>
        /// The color of the arc. Null applies no explicit color (inherits from context).
        /// </summary>
        public FColor? Color { get; }

        /// <summary>
        /// Duration of one full rotation in seconds. Defaults to 1.
        /// </summary>
        public float Speed { get; }

        /// <summary>
        /// Creates an <see cref="FSpinner"/> element.
        /// </summary>
        /// <param name="size">Diameter in logical pixels. Defaults to 32.</param>
        /// <param name="strokeWidth">Arc thickness in logical pixels. Defaults to 4.</param>
        /// <param name="color">Arc color. Null means no explicit color is applied.</param>
        /// <param name="speed">Seconds per full rotation. Defaults to 1.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FSpinner(
            float size = 32f,
            float strokeWidth = 4f,
            FColor? color = null,
            float speed = 1f,
            FKey? key = null
        ) : base(key)
        {
            Size = size;
            StrokeWidth = strokeWidth;
            Color = color;
            Speed = speed;
        }
    }
}