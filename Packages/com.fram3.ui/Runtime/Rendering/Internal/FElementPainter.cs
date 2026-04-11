#nullable enable
using System.Collections.Generic;
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
        /// a <c>TextField</c> for <see cref="FTextField"/>, a <c>Toggle</c> for <see cref="FToggle"/>,
        /// a <c>Slider</c> for <see cref="FSlider"/>, a <c>DropdownField</c> for <see cref="FDropdown"/>,
        /// or a plain <c>VisualElement</c> for all layout/container/gesture elements.
        /// </returns>
        internal static VisualElement CreateNative(FElement element)
        {
            switch (element)
            {
                case FText text:
                    return CreateLabel(text);
                case FButton button:
                    return CreateButton(button);
                case FTextField textField:
                    return CreateTextField(textField);
                case FToggle toggle:
                    return CreateToggle(toggle);
                case FSlider slider:
                    return CreateSlider(slider);
                case FDropdown dropdown:
                    return CreateDropdown(dropdown);
                default:
                    var native = new VisualElement();
                    ApplyLayout(element, native);
                    RegisterGestureCallbacks(element, native);
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
                case FTextField textField when native is TextField tf:
                    PaintTextField(textField, tf);
                    break;
                case FToggle toggle when native is Toggle tgl:
                    PaintToggle(toggle, tgl);
                    break;
                case FSlider slider when native is Slider sld:
                    PaintSlider(slider, sld);
                    break;
                case FDropdown dropdown when native is DropdownField dd:
                    PaintDropdown(dropdown, dd);
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

        private static TextField CreateTextField(FTextField textField)
        {
            var tf = new TextField
                { value = textField.Value, isReadOnly = textField.ReadOnly, multiline = textField.Multiline };
            if (textField.Placeholder != null)
            {
                tf.textEdition.placeholder = textField.Placeholder;
            }

            if (textField.OnChanged == null)
            {
                return tf;
            }

            var callback = textField.OnChanged;
            tf.RegisterValueChangedCallback(evt => callback(evt.newValue));

            return tf;
        }

        private static Toggle CreateToggle(FToggle toggle)
        {
            var tgl = new Toggle { value = toggle.Value };
            if (toggle.Label != null)
            {
                tgl.label = toggle.Label;
            }

            if (toggle.OnChanged == null)
            {
                return tgl;
            }

            var callback = toggle.OnChanged;
            tgl.RegisterValueChangedCallback(evt => callback(evt.newValue));

            return tgl;
        }

        private static Slider CreateSlider(FSlider slider)
        {
            var sld = new Slider(slider.Min, slider.Max) { value = slider.Value };
            if (slider.Label != null)
            {
                sld.label = slider.Label;
            }

            if (slider.OnChanged == null)
            {
                return sld;
            }

            var callback = slider.OnChanged;
            sld.RegisterValueChangedCallback(evt => callback(evt.newValue));

            return sld;
        }

        private static DropdownField CreateDropdown(FDropdown dropdown)
        {
            var choices = new List<string>(dropdown.Options);
            var dd = new DropdownField(choices, dropdown.SelectedIndex);
            if (dropdown.Label != null)
            {
                dd.label = dropdown.Label;
            }

            if (dropdown.OnChanged == null)
            {
                return dd;
            }

            var callback = dropdown.OnChanged;
            dd.RegisterValueChangedCallback(_ => callback(dd.index));

            return dd;
        }

        private static void PaintTextField(FTextField textField, TextField tf)
        {
            tf.value = textField.Value;
            tf.isReadOnly = textField.ReadOnly;
            tf.multiline = textField.Multiline;
            if (textField.Placeholder != null)
            {
                tf.textEdition.placeholder = textField.Placeholder;
            }
        }

        private static void PaintToggle(FToggle toggle, Toggle tgl)
        {
            tgl.value = toggle.Value;
            if (toggle.Label != null)
            {
                tgl.label = toggle.Label;
            }
        }

        private static void PaintSlider(FSlider slider, Slider sld)
        {
            sld.lowValue = slider.Min;
            sld.highValue = slider.Max;
            sld.value = slider.Value;
            if (slider.Label != null)
            {
                sld.label = slider.Label;
            }
        }

        private static void PaintDropdown(FDropdown dropdown, DropdownField dd)
        {
            dd.choices = new List<string>(dropdown.Options);
            dd.index = dropdown.SelectedIndex;
            if (dropdown.Label != null)
            {
                dd.label = dropdown.Label;
            }
        }

        private static void RegisterGestureCallbacks(FElement element, VisualElement native)
        {
            if (element is not FGestureDetector gesture)
            {
                return;
            }

            if (gesture.OnTap != null)
            {
                var callback = gesture.OnTap;
                native.RegisterCallback<ClickEvent>(_ => callback());
            }

            if (gesture.OnPointerEnter != null)
            {
                var callback = gesture.OnPointerEnter;
                native.RegisterCallback<PointerEnterEvent>(_ => callback());
            }

            // ReSharper disable once InvertIf
            if (gesture.OnPointerExit != null)
            {
                var callback = gesture.OnPointerExit;
                native.RegisterCallback<PointerLeaveEvent>(_ => callback());
            }
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
                var color = style.Color.Value;
                native.style.color = new UnityEngine.Color(color.R, color.G, color.B, color.A);
            }

            var fontStyle = ResolveFontStyle(style.Bold, style.Italic);
            native.style.unityFontStyleAndWeight = fontStyle;
        }

        private static UnityEngine.FontStyle ResolveFontStyle(bool bold, bool italic)
        {
            return bold switch
            {
                true when italic => UnityEngine.FontStyle.BoldAndItalic,
                true => UnityEngine.FontStyle.Bold,
                _ => italic ? UnityEngine.FontStyle.Italic : UnityEngine.FontStyle.Normal
            };
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
                case FCenter:
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
                var decorationColor = decoration.Color.Value;
                native.style.backgroundColor = new UnityEngine.Color(
                    decorationColor.R, decorationColor.G,
                    decorationColor.B, decorationColor.A
                );
            }

            if (decoration.Border != null)
            {
                var border = decoration.Border;
                var borderColor = new UnityEngine.Color(
                    border.Color.R, border.Color.G, border.Color.B, border.Color.A
                );

                native.style.borderTopWidth = border.Width;
                native.style.borderRightWidth = border.Width;
                native.style.borderBottomWidth = border.Width;
                native.style.borderLeftWidth = border.Width;

                native.style.borderTopColor = borderColor;
                native.style.borderRightColor = borderColor;
                native.style.borderBottomColor = borderColor;
                native.style.borderLeftColor = borderColor;
            }

            if (!decoration.BorderRadius.HasValue)
            {
                return;
            }

            var radius = decoration.BorderRadius.Value;
            native.style.borderTopLeftRadius = radius.TopLeft;
            native.style.borderTopRightRadius = radius.TopRight;
            native.style.borderBottomRightRadius = radius.BottomRight;
            native.style.borderBottomLeftRadius = radius.BottomLeft;
        }

        private static Justify MapMainAxis(FMainAxisAlignment alignment)
        {
            return alignment switch
            {
                FMainAxisAlignment.End => Justify.FlexEnd,
                FMainAxisAlignment.Center => Justify.Center,
                FMainAxisAlignment.SpaceBetween => Justify.SpaceBetween,
                FMainAxisAlignment.SpaceAround => Justify.SpaceAround,
                FMainAxisAlignment.SpaceEvenly => Justify.SpaceEvenly,
                _ => Justify.FlexStart
            };
        }

        private static Align MapCrossAxis(FCrossAxisAlignment alignment)
        {
            return alignment switch
            {
                FCrossAxisAlignment.End => Align.FlexEnd,
                FCrossAxisAlignment.Center => Align.Center,
                FCrossAxisAlignment.Stretch => Align.Stretch,
                _ => Align.FlexStart
            };
        }
    }
}