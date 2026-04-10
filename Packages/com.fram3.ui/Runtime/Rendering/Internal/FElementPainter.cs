#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements;
using Fram3.UI.Styling;
using UnityEngine.UIElements;

namespace Fram3.UI.Rendering.Internal
{
    /// <summary>
    /// Responsible for creating and updating native UIToolkit <see cref="VisualElement"/>
    /// instances to match a given <see cref="FElement"/> description.
    /// </summary>
    internal static class FElementPainter
    {
        /// <summary>
        /// Creates the appropriate native <see cref="VisualElement"/> for the given element
        /// and applies all initial style properties to it.
        /// </summary>
        /// <param name="element">The framework element to produce a native element for.</param>
        /// <returns>
        /// A <c>Label</c> for <see cref="FText"/>, a <c>Button</c> for <see cref="FButton"/>,
        /// or a plain <c>VisualElement</c> for all layout/container elements.
        /// </returns>
        internal static VisualElement CreateNative(FElement element)
        {
            switch (element)
            {
                case FText text:
                    return CreateLabel(text);
                case FButton button:
                    return CreateButton(button);
                default:
                    var native = new VisualElement();
                    ApplyLayout(element, native);
                    return native;
            }
        }

        /// <summary>
        /// Updates an existing native <see cref="VisualElement"/> to reflect the latest
        /// property values from the given element description.
        /// </summary>
        /// <param name="element">The current element description.</param>
        /// <param name="native">The existing native element to update.</param>
        internal static void Paint(FElement element, VisualElement native)
        {
            switch (element)
            {
                case FText text when native is Label label:
                    PaintText(text, label);
                    break;
                case FButton button when native is Button btn:
                    PaintButton(button, btn);
                    break;
                default:
                    ApplyLayout(element, native);
                    break;
            }
        }

        private static Label CreateLabel(FText text)
        {
            var label = new Label(text.Text);
            PaintText(text, label);
            return label;
        }

        private static Button CreateButton(FButton button)
        {
            var btn = new Button(button.OnPressed) { text = button.Label };
            return btn;
        }

        private static void PaintText(FText text, Label label)
        {
            label.text = text.Text;
            if (text.Style == null)
            {
                return;
            }

            ApplyTextStyle(text.Style, label);
        }

        private static void PaintButton(FButton button, Button btn)
        {
            btn.text = button.Label;
        }

        private static void ApplyTextStyle(FTextStyle style, VisualElement native)
        {
            if (style.FontSize.HasValue)
            {
                native.style.fontSize = style.FontSize.Value;
            }

            if (style.Color.HasValue)
            {
                var c = style.Color.Value;
                native.style.color = new UnityEngine.Color(c.R, c.G, c.B, c.A);
            }

            var fontStyle = ResolveFontStyle(style.Bold, style.Italic);
            native.style.unityFontStyleAndWeight = fontStyle;
        }

        private static UnityEngine.FontStyle ResolveFontStyle(bool bold, bool italic)
        {
            if (bold && italic)
            {
                return UnityEngine.FontStyle.BoldAndItalic;
            }

            if (bold)
            {
                return UnityEngine.FontStyle.Bold;
            }

            if (italic)
            {
                return UnityEngine.FontStyle.Italic;
            }

            return UnityEngine.FontStyle.Normal;
        }

        private static void ApplyLayout(FElement element, VisualElement native)
        {
            switch (element)
            {
                case FColumn column:
                    ApplyColumnLayout(column, native);
                    break;
                case FRow row:
                    ApplyRowLayout(row, native);
                    break;
                case FPadding padding:
                    ApplyPaddingLayout(padding, native);
                    break;
                case FSizedBox sizedBox:
                    ApplySizedBoxLayout(sizedBox, native);
                    break;
                case FContainer container:
                    ApplyContainerLayout(container, native);
                    break;
                case FCenter _:
                    ApplyCenterLayout(native);
                    break;
            }
        }

        private static void ApplyColumnLayout(FColumn column, VisualElement native)
        {
            native.style.flexDirection = FlexDirection.Column;
            native.style.justifyContent = MapMainAxis(column.MainAxisAlignment);
            native.style.alignItems = MapCrossAxis(column.CrossAxisAlignment);
        }

        private static void ApplyRowLayout(FRow row, VisualElement native)
        {
            native.style.flexDirection = FlexDirection.Row;
            native.style.justifyContent = MapMainAxis(row.MainAxisAlignment);
            native.style.alignItems = MapCrossAxis(row.CrossAxisAlignment);
        }

        private static void ApplyPaddingLayout(FPadding padding, VisualElement native)
        {
            var insets = padding.Padding;
            native.style.paddingTop = insets.Top;
            native.style.paddingRight = insets.Right;
            native.style.paddingBottom = insets.Bottom;
            native.style.paddingLeft = insets.Left;
        }

        private static void ApplySizedBoxLayout(FSizedBox sizedBox, VisualElement native)
        {
            if (sizedBox.IsExpand)
            {
                native.style.flexGrow = 1f;
                return;
            }

            if (sizedBox.Width.HasValue)
            {
                native.style.width = sizedBox.Width.Value;
            }

            if (sizedBox.Height.HasValue)
            {
                native.style.height = sizedBox.Height.Value;
            }
        }

        private static void ApplyContainerLayout(FContainer container, VisualElement native)
        {
            if (container.Width.HasValue)
            {
                native.style.width = container.Width.Value;
            }

            if (container.Height.HasValue)
            {
                native.style.height = container.Height.Value;
            }

            if (container.Padding.HasValue)
            {
                var insets = container.Padding.Value;
                native.style.paddingTop = insets.Top;
                native.style.paddingRight = insets.Right;
                native.style.paddingBottom = insets.Bottom;
                native.style.paddingLeft = insets.Left;
            }

            if (container.Decoration != null)
            {
                ApplyDecoration(container.Decoration, native);
            }
        }

        private static void ApplyCenterLayout(VisualElement native)
        {
            native.style.alignItems = Align.Center;
            native.style.justifyContent = Justify.Center;
        }

        private static void ApplyDecoration(FBoxDecoration decoration, VisualElement native)
        {
            if (decoration.Color.HasValue)
            {
                var c = decoration.Color.Value;
                native.style.backgroundColor = new UnityEngine.Color(c.R, c.G, c.B, c.A);
            }

            if (decoration.Border != null)
            {
                var border = decoration.Border;
                var borderColor = new UnityEngine.Color(
                    border.Color.R, border.Color.G, border.Color.B, border.Color.A);

                native.style.borderTopWidth = border.Width;
                native.style.borderRightWidth = border.Width;
                native.style.borderBottomWidth = border.Width;
                native.style.borderLeftWidth = border.Width;

                native.style.borderTopColor = borderColor;
                native.style.borderRightColor = borderColor;
                native.style.borderBottomColor = borderColor;
                native.style.borderLeftColor = borderColor;
            }

            if (decoration.BorderRadius.HasValue)
            {
                var radius = decoration.BorderRadius.Value;
                native.style.borderTopLeftRadius = radius.TopLeft;
                native.style.borderTopRightRadius = radius.TopRight;
                native.style.borderBottomRightRadius = radius.BottomRight;
                native.style.borderBottomLeftRadius = radius.BottomLeft;
            }
        }

        private static Justify MapMainAxis(FMainAxisAlignment alignment)
        {
            switch (alignment)
            {
                case FMainAxisAlignment.End:
                    return Justify.FlexEnd;
                case FMainAxisAlignment.Center:
                    return Justify.Center;
                case FMainAxisAlignment.SpaceBetween:
                    return Justify.SpaceBetween;
                case FMainAxisAlignment.SpaceAround:
                    return Justify.SpaceAround;
                case FMainAxisAlignment.SpaceEvenly:
                    return Justify.SpaceEvenly;
                default:
                    return Justify.FlexStart;
            }
        }

        private static Align MapCrossAxis(FCrossAxisAlignment alignment)
        {
            switch (alignment)
            {
                case FCrossAxisAlignment.End:
                    return Align.FlexEnd;
                case FCrossAxisAlignment.Center:
                    return Align.Center;
                case FCrossAxisAlignment.Stretch:
                    return Align.Stretch;
                default:
                    return Align.FlexStart;
            }
        }
    }
}
