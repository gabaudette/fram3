#nullable enable
#if !FRAM3_PURE_TESTS
using System;
using Fram3.UI.Animation;
using Fram3.UI.Elements.Content;
using UnityEngine;
using UnityEngine.UIElements;

namespace Fram3.UI.Rendering.Internal
{
    /// <summary>
    /// A UIToolkit <see cref="VisualElement"/> that renders a continuously rotating
    /// 270-degree arc using <c>Painter2D</c>. Owns an <c>FAnimationController</c>
    /// that loops the rotation angle. Created and configured by
    /// <see cref="FElementPainter"/> for <see cref="FSpinner"/> elements.
    /// </summary>
    internal sealed class FSpinnerElement : VisualElement, IDisposable
    {
        private const float ArcSweepDegrees = 270f;

        private FAnimationController _controller;
        private float _rotationDegrees;
        private Color _arcColor;
        private float _strokeWidth;
        private bool _disposed;

        /// <summary>
        /// Creates an <see cref="FSpinnerElement"/> and starts the spinning animation.
        /// </summary>
        internal FSpinnerElement(FSpinner spinner)
        {
            _arcColor = ResolveColor(spinner.Color);
            _strokeWidth = spinner.StrokeWidth;

            style.width = spinner.Size;
            style.height = spinner.Size;

            generateVisualContent += OnGenerateVisualContent;

            _controller = CreateController(spinner.Speed);
        }

        /// <summary>
        /// Updates the visual properties of the spinner to match a new
        /// <see cref="FSpinner"/> description without recreating the native element.
        /// </summary>
        internal void Apply(FSpinner spinner)
        {
            _arcColor = ResolveColor(spinner.Color);
            _strokeWidth = spinner.StrokeWidth;

            style.width = spinner.Size;
            style.height = spinner.Size;

            if (MathF.Abs(_controller.Duration - spinner.Speed) > 0.0001f)
            {
                _controller.Dispose();
                _controller = CreateController(spinner.Speed);
            }

            MarkDirtyRepaint();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _controller.Dispose();
            _disposed = true;
        }

        private FAnimationController CreateController(float speed)
        {
            var controller = new FAnimationController(duration: speed);
            controller.AddListener(OnTick);
            controller.Forward();
            return controller;
        }

        private void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            var rect = contentRect;
            var cx = rect.width * 0.5f;
            var cy = rect.height * 0.5f;
            var radius = MathF.Min(cx, cy) - _strokeWidth * 0.5f;

            if (radius <= 0f)
            {
                return;
            }

            var painter = mgc.painter2D;
            painter.strokeColor = _arcColor;
            painter.lineWidth = _strokeWidth;
            painter.lineCap = LineCap.Round;

            var startAngle = _rotationDegrees;
            var endAngle = _rotationDegrees + ArcSweepDegrees;

            painter.BeginPath();
            painter.Arc(new Vector2(cx, cy), radius, startAngle, endAngle);
            painter.Stroke();
        }

        private void OnTick(float value)
        {
            _rotationDegrees = value * 360f;

            if (_controller.Status == FAnimationStatus.Completed)
            {
                _controller.Forward();
            }

            MarkDirtyRepaint();
        }

        private static Color ResolveColor(Styling.FColor? color)
        {
            if (!color.HasValue)
            {
                return Color.white;
            }

            var c = color.Value;

            return new Color(c.R, c.G, c.B, c.A);
        }
    }
}
#endif