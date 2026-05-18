#nullable enable
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Input;
using Fram3.UI.Styling;
using UnityEngine.UIElements;
using UITextField = UnityEngine.UIElements.TextField;

namespace Fram3.UI.Rendering.Internal
{
    internal static partial class ElementPainter
    {
        private static Label CreateLabel(Text text)
        {
            var label = new Label(text.Content)
            {
                style =
                {
                    whiteSpace = WhiteSpace.Normal
                }
            };
            
            PaintText(text, label);
            
            return label;
        }

        private static UITextField CreatePasswordField(PasswordField passwordField, Theme theme)
        {
            var textField = new UITextField
            {
                value = passwordField.Value,
                isReadOnly = passwordField.ReadOnly,
                isPasswordField = true
            };

            if (passwordField.Placeholder != null)
            {
                textField.textEdition.placeholder = passwordField.Placeholder;
            }

            textField.RegisterCallback<AttachToPanelEvent>(_ =>
            {
                var input = textField.Q<VisualElement>(className: "unity-base-text-field__input");
                if (input != null)
                {
                    input.style.backgroundColor = ToUnity(theme.SurfaceColor);
                    input.style.borderTopColor = ToUnity(theme.InputBorderColor);
                    input.style.borderRightColor = ToUnity(theme.InputBorderColor);
                    input.style.borderBottomColor = ToUnity(theme.InputBorderColor);
                    input.style.borderLeftColor = ToUnity(theme.InputBorderColor);
                    input.style.color = ToUnity(theme.PrimaryTextColor);
                }

                var textElement = textField.Q<VisualElement>(className: "unity-text-element");
                if (textElement != null)
                {
                    textElement.style.color = ToUnity(theme.PrimaryTextColor);
                }
            });

            if (passwordField.OnChanged == null)
            {
                return textField;
            }

            var callback = passwordField.OnChanged;
            textField.RegisterValueChangedCallback(evt => callback(evt.newValue));

            return textField;
        }

        private static UITextField CreateTextField(Elements.Input.TextField textField, Theme theme)
        {
            var uiTextField = new UITextField
            {
                value = textField.Value,
                isReadOnly = textField.ReadOnly,
                multiline = textField.Multiline
            };

            if (textField.Placeholder != null)
            {
                uiTextField.textEdition.placeholder = textField.Placeholder;
            }

            uiTextField.RegisterCallback<AttachToPanelEvent>(_ =>
            {
                var input = uiTextField.Q<VisualElement>(className: "unity-base-text-field__input");
                if (input != null)
                {
                    input.style.backgroundColor = ToUnity(theme.SurfaceColor);
                    input.style.borderTopColor = ToUnity(theme.InputBorderColor);
                    input.style.borderRightColor = ToUnity(theme.InputBorderColor);
                    input.style.borderBottomColor = ToUnity(theme.InputBorderColor);
                    input.style.borderLeftColor = ToUnity(theme.InputBorderColor);
                    input.style.color = ToUnity(theme.PrimaryTextColor);
                }

                var textElement = uiTextField.Q<VisualElement>(className: "unity-text-element");
                if (textElement != null)
                {
                    textElement.style.color = ToUnity(theme.PrimaryTextColor);
                }
            });

            if (textField.OnChanged == null)
            {
                return uiTextField;
            }

            var callback = textField.OnChanged;
            uiTextField.RegisterValueChangedCallback(evt => callback(evt.newValue));

            return uiTextField;
        }

        private static void PaintPasswordField(PasswordField passwordField, UITextField uiTextField)
        {
            uiTextField.value = passwordField.Value;
            uiTextField.isReadOnly = passwordField.ReadOnly;
            if (passwordField.Placeholder != null)
            {
                uiTextField.textEdition.placeholder = passwordField.Placeholder;
            }
        }

        private static void PaintTextField(Elements.Input.TextField textField, UITextField uiTextField)
        {
            uiTextField.value = textField.Value;
            uiTextField.isReadOnly = textField.ReadOnly;
            uiTextField.multiline = textField.Multiline;
            if (textField.Placeholder != null)
            {
                uiTextField.textEdition.placeholder = textField.Placeholder;
            }
        }

        private static void PaintText(Text text, Label label)
        {
            label.text = text.Content;
            if (text.Style == null)
            {
                return;
            }

            ApplyTextStyle(text.Style, label);
        }

        private static void ApplyTextStyle(TextStyle style, VisualElement native)
        {
            if (style.ResetPadding)
            {
                native.style.paddingTop = 0f;
                native.style.paddingBottom = 0f;
                native.style.paddingLeft = 0f;
                native.style.paddingRight = 0f;
                native.style.marginTop = 0f;
                native.style.marginBottom = 0f;
                native.style.marginLeft = 0f;
                native.style.marginRight = 0f;
            }

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

#if !FRAM3_PURE_TESTS
            if (style.LetterSpacing != 0f)
            {
                native.style.letterSpacing = style.LetterSpacing;
            }

            if (style.TextAlign.HasValue)
            {
                native.style.unityTextAlign = style.TextAlign.Value;
            }

            if (style.FontAsset != null)
            {
                native.style.unityFontDefinition = FontDefinition.FromSDFFont(style.FontAsset);
            }
#endif
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
    }
}
