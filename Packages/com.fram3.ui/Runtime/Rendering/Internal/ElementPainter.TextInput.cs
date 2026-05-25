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
        private static Label CreateLabel(Text text, Theme theme)
        {
            var label = new Label(text.Content)
            {
                style =
                {
                    whiteSpace = WhiteSpace.Normal
                }
            };

            PaintText(text, label, theme);

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

            ApplyCaretColors(textField, theme);
            textField.RegisterCallback<CustomStyleResolvedEvent>(_ => ApplyCaretColors(textField, theme));

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
                    input.style.borderTopLeftRadius = theme.BorderRadius;
                    input.style.borderTopRightRadius = theme.BorderRadius;
                    input.style.borderBottomLeftRadius = theme.BorderRadius;
                    input.style.borderBottomRightRadius = theme.BorderRadius;
                    input.style.color = ToUnity(theme.PrimaryTextColor);
                }

                var textElement = textField.Q<VisualElement>(className: "unity-text-element");
                if (textElement != null)
                {
                    textElement.style.color = ToUnity(theme.PrimaryTextColor);
#if !FRAM3_PURE_TESTS
                    SetCursorWidth(textElement, 2f);
#endif
                }
            });

            textField.RegisterCallback<FocusInEvent>(_ => ApplyCaretColors(textField, theme));

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

            ApplyCaretColors(uiTextField, theme);
            uiTextField.RegisterCallback<CustomStyleResolvedEvent>(_ => ApplyCaretColors(uiTextField, theme));

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
                    input.style.borderTopLeftRadius = theme.BorderRadius;
                    input.style.borderTopRightRadius = theme.BorderRadius;
                    input.style.borderBottomLeftRadius = theme.BorderRadius;
                    input.style.borderBottomRightRadius = theme.BorderRadius;
                    input.style.color = ToUnity(theme.PrimaryTextColor);
                }

                var textElement = uiTextField.Q<VisualElement>(className: "unity-text-element");
                if (textElement != null)
                {
                    textElement.style.color = ToUnity(theme.PrimaryTextColor);
#if !FRAM3_PURE_TESTS
                    SetCursorWidth(textElement, 2f);
#endif
                }
            });

            uiTextField.RegisterCallback<FocusInEvent>(_ => ApplyCaretColors(uiTextField, theme));

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

        private static void PaintText(Text text, Label label, Theme theme)
        {
            label.text = text.Content;
            if (text.Style == null)
            {
                ApplyTextStyle(TextStyle.Inherit, label, theme);
                return;
            }

            ApplyTextStyle(text.Style, label, theme);
        }

        private static void ApplyTextStyle(TextStyle style, VisualElement native, Theme? theme = null)
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

            var effectiveFont = style.FontAsset ?? theme?.FontFamily;
            if (effectiveFont != null)
            {
                native.style.unityFontDefinition = FontDefinition.FromSDFFont(effectiveFont);
            }
#endif
        }

#pragma warning disable CS0618
        private static void ApplyCaretColors(UITextField textField, Theme theme)
        {
            // No public runtime API exists for --unity-cursor-color; deprecated setter is the only option.
            textField.textSelection.cursorColor = ToUnity(theme.PrimaryColor);
            textField.textSelection.selectionColor = ToUnity(theme.PrimaryColor.WithAlpha(0.3f));
        }
#pragma warning restore CS0618

#if !FRAM3_PURE_TESTS
        private static void SetCursorWidth(VisualElement textElement, float width)
        {
            // cursorWidth is internal on ITextSelection with no USS equivalent; reflection is the only option.
            typeof(TextElement).GetField("m_CursorWidth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(textElement, width);
        }
#endif

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