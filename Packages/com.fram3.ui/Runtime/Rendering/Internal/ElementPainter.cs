#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;
using UnityEngine.UIElements;
using UiButton = UnityEngine.UIElements.Button;
using UiTextField = UnityEngine.UIElements.TextField;
using UiScrollView = UnityEngine.UIElements.ScrollView;
using UiProgressBar = UnityEngine.UIElements.ProgressBar;
using UiFloatField = UnityEngine.UIElements.FloatField;
using UiMinMaxSlider = UnityEngine.UIElements.MinMaxSlider;
using UiWrap = UnityEngine.UIElements.Wrap;

namespace Fram3.UI.Rendering.Internal
{
    /// <summary>
    /// Responsible for creating and updating native UIToolkit <see cref="VisualElement"/>
    /// instances to match a given <see cref="Element"/> description.
    /// </summary>
    internal static class ElementPainter
    {
        /// <summary>
        /// Creates the appropriate native <see cref="VisualElement"/> for the given element
        /// and applies all initial style properties to it.
        /// </summary>
        /// <param name="element">The framework element to produce a native element for.</param>
        /// <returns>
        /// A <c>Label</c> for <see cref="Text"/>, a <c>Button</c> for <see cref="Button"/>,
        /// a <c>TextField</c> for <see cref="TextField"/>, a <c>Toggle</c> for <see cref="FrameToggle"/>,
        /// a <c>Slider</c> for <see cref="FrameSlider"/>, a <c>DropdownField</c> for <see cref="Dropdown"/>,
        /// a <c>ProgressBar</c> for <see cref="ProgressBar"/>, a <c>ScrollView</c> for <see cref="ScrollView"/>,
        /// an <c>Image</c> for <see cref="FrameImage"/> or <see cref="Icon"/>,
        /// an <c>SpinnerElement</c> for <see cref="Spinner"/>,
        /// or a plain <c>VisualElement</c> for all layout/container/gesture elements.
        /// </returns>
        internal static VisualElement CreateNative(Element element)
        {
#if !FRAM3_PURE_TESTS
            if (element is Spinner spinner)
            {
                return CreateSpinner(spinner);
            }
#endif
            switch (element)
            {
                case Text text:
                    return CreateLabel(text);
                case Fram3.UI.Elements.Input.Button button:
                    return CreateButton(button);
                case PasswordField passwordField:
                    return CreatePasswordField(passwordField);
                case Fram3.UI.Elements.Input.TextField textField:
                    return CreateTextField(textField);
                case Checkbox checkbox:
                    return CreateCheckbox(checkbox);
                case RadioGroup radioGroup:
                    return CreateRadioGroup(radioGroup);
                case Modal modal:
                    return CreateModal(modal);
                case FrameToggle toggle:
                    return CreateToggle(toggle);
                case IntField intField:
                    return CreateIntField(intField);
                case Fram3.UI.Elements.Input.FloatField floatField:
                    return CreateFloatField(floatField);
                case Fram3.UI.Elements.Input.MinMaxSlider minMaxSlider:
                    return CreateMinMaxSlider(minMaxSlider);
                case IFEnumFieldDescriptor enumField:
                    return CreateEnumField(enumField);
                case FrameSlider slider:
                    return CreateSlider(slider);
                case Dropdown dropdown:
                    return CreateDropdown(dropdown);
                case Fram3.UI.Elements.Content.ProgressBar progressBar:
                    return CreateProgressBar(progressBar);
                case Fram3.UI.Elements.Content.ScrollView scrollView:
                    return CreateScrollView(scrollView);
                case FrameImage image:
                    return CreateImage(image);
                case Icon icon:
                    return CreateIcon(icon);
                default:
                    switch (element)
                    {
                        case IFListViewDescriptor listView:
                            return CreateListView(listView);
                        case IFEnumFieldDescriptor enumFieldDesc:
                            return CreateEnumField(enumFieldDesc);
                    }

                    var native = new VisualElement();
                    ApplyLayout(element, native);
                    RegisterGestureCallbacks(element, native);
                    return native;
            }
        }

        /// <summary>
        /// Applies absolute positioning to a native element so that it behaves as a
        /// layer inside an <see cref="Stack"/> container. Called by the renderer after
        /// attaching the child to a stack parent.
        /// </summary>
        internal static void ApplyAsStackChild(VisualElement native)
        {
            native.style.position = Position.Absolute;
        }

        /// <summary>
        /// Updates an existing native <see cref="VisualElement"/> to reflect the latest
        /// property values from the given element description.
        /// </summary>
        /// <param name="element">The current element description.</param>
        /// <param name="native">The existing native element to update.</param>
        internal static void Paint(Element element, VisualElement native)
        {
#if !FRAM3_PURE_TESTS
            if (element is Spinner spinner && native is SpinnerElement spinnerEl)
            {
                spinnerEl.Apply(spinner);
                return;
            }
#endif
            switch (element)
            {
                case Text text when native is Label label:
                    PaintText(text, label);
                    break;
                case Fram3.UI.Elements.Input.Button button when native is UiButton btn:
                    PaintButton(button, btn);
                    break;
                case PasswordField passwordField when native is UiTextField ptf:
                    PaintPasswordField(passwordField, ptf);
                    break;
                case Fram3.UI.Elements.Input.TextField textField when native is UiTextField tf:
                    PaintTextField(textField, tf);
                    break;
                case Checkbox checkbox when native is Toggle chkTgl:
                    PaintCheckbox(checkbox, chkTgl);
                    break;
                case RadioGroup radioGroup when native is RadioButtonGroup rbg:
                    PaintRadioGroup(radioGroup, rbg);
                    break;
                case Modal modal:
                    PaintModal(modal, native);
                    break;
                case FrameToggle toggle when native is Toggle tgl:
                    PaintToggle(toggle, tgl);
                    break;
                case IntField intField when native is IntegerField intf:
                    PaintIntField(intField, intf);
                    break;
                case Fram3.UI.Elements.Input.FloatField floatField when native is UiFloatField ff:
                    PaintFloatField(floatField, ff);
                    break;
                case Fram3.UI.Elements.Input.MinMaxSlider minMaxSlider when native is UiMinMaxSlider mms:
                    PaintMinMaxSlider(minMaxSlider, mms);
                    break;
                case FrameSlider slider when native is Slider sld:
                    PaintSlider(slider, sld);
                    break;
                case Dropdown dropdown when native is DropdownField dd:
                    PaintDropdown(dropdown, dd);
                    break;
                case Fram3.UI.Elements.Content.ProgressBar progressBar when native is UiProgressBar pb:
                    PaintProgressBar(progressBar, pb);
                    break;
                case Fram3.UI.Elements.Content.ScrollView scrollView when native is UiScrollView sv:
                    PaintScrollView(scrollView, sv);
                    break;
                case FrameImage image when native is Image img:
                    PaintImage(image, img);
                    break;
                case Icon icon when native is Image iconImg:
                    PaintIcon(icon, iconImg);
                    break;
                default:
                    if (element is IFListViewDescriptor listView && native is ListView lv)
                    {
                        PaintListView(listView, lv);
                        break;
                    }

                    if (element is IFEnumFieldDescriptor enumFieldDesc && native is EnumField ef)
                    {
                        PaintEnumField(enumFieldDesc, ef);
                        break;
                    }

                    ApplyLayout(element, native);
                    break;
            }
        }

        private static Label CreateLabel(Text text)
        {
            var label = new Label(text.Content);
            PaintText(text, label);
            return label;
        }

        private static UiButton CreateButton(Fram3.UI.Elements.Input.Button button)
        {
            var btn = new UiButton(button.OnPressed) { text = button.Label };
            return btn;
        }

        private static UiTextField CreatePasswordField(PasswordField passwordField)
        {
            var tf = new UiTextField
            {
                value = passwordField.Value,
                isReadOnly = passwordField.ReadOnly,
                isPasswordField = true
            };

            if (passwordField.Placeholder != null)
            {
                tf.textEdition.placeholder = passwordField.Placeholder;
            }

            if (passwordField.OnChanged == null)
            {
                return tf;
            }

            var callback = passwordField.OnChanged;
            tf.RegisterValueChangedCallback(evt => callback(evt.newValue));

            return tf;
        }

        private static UiTextField CreateTextField(Fram3.UI.Elements.Input.TextField textField)
        {
            var tf = new UiTextField
            {
                value = textField.Value,
                isReadOnly = textField.ReadOnly,
                multiline = textField.Multiline
            };

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

        private static Toggle CreateToggle(FrameToggle toggle)
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

        private static Toggle CreateCheckbox(Checkbox checkbox)
        {
            var tgl = new Toggle { value = checkbox.Value };
            if (checkbox.Label != null)
            {
                tgl.label = checkbox.Label;
            }

            if (checkbox.OnChanged == null)
            {
                return tgl;
            }

            var callback = checkbox.OnChanged;
            tgl.RegisterValueChangedCallback(evt => callback(evt.newValue));

            return tgl;
        }

        private static RadioButtonGroup CreateRadioGroup(RadioGroup radioGroup)
        {
            var rbg = new RadioButtonGroup
            {
                choices = new List<string>(radioGroup.Options),
                value = ResolveRadioIndex(radioGroup)
            };

            if (radioGroup.OnChanged == null)
            {
                return rbg;
            }

            var callback = radioGroup.OnChanged;
            var options = radioGroup.Options;
            rbg.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue >= 0 && evt.newValue < options.Count)
                {
                    callback(options[evt.newValue]);
                }
            });

            return rbg;
        }

        private static VisualElement CreateModal(Modal modal)
        {
            var native = new VisualElement
            {
                style =
                {
                    position = Position.Absolute
                }
            };

            if (!modal.BarrierDismissible || modal.OnDismiss == null)
            {
                return native;
            }

            var callback = modal.OnDismiss;
            native.RegisterCallback<ClickEvent>(_ => callback());

            return native;
        }

        private static void PaintCheckbox(Checkbox checkbox, Toggle tgl)
        {
            tgl.value = checkbox.Value;
            if (checkbox.Label != null)
            {
                tgl.label = checkbox.Label;
            }
        }

        private static void PaintRadioGroup(RadioGroup radioGroup, RadioButtonGroup rbg)
        {
            rbg.choices = new List<string>(radioGroup.Options);
            rbg.value = ResolveRadioIndex(radioGroup);
        }

        private static int ResolveRadioIndex(RadioGroup radioGroup)
        {
            if (radioGroup.SelectedValue == null)
            {
                return -1;
            }

            for (var i = 0; i < radioGroup.Options.Count; i++)
            {
                if (radioGroup.Options[i] == radioGroup.SelectedValue)
                {
                    return i;
                }
            }

            return -1;
        }

        private static void PaintModal(Modal modal, VisualElement native)
        {
            native.style.position = Position.Absolute;
        }

        private static Slider CreateSlider(FrameSlider slider)
        {
            var sld = new Slider(slider.Min, slider.Max) { value = slider.Value };
            sld.style.alignSelf = Align.Stretch;
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

        private static DropdownField CreateDropdown(Dropdown dropdown)
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

        private static void PaintPasswordField(PasswordField passwordField, UiTextField tf)
        {
            tf.value = passwordField.Value;
            tf.isReadOnly = passwordField.ReadOnly;
            if (passwordField.Placeholder != null)
            {
                tf.textEdition.placeholder = passwordField.Placeholder;
            }
        }

        private static void PaintTextField(Fram3.UI.Elements.Input.TextField textField, UiTextField tf)
        {
            tf.value = textField.Value;
            tf.isReadOnly = textField.ReadOnly;
            tf.multiline = textField.Multiline;
            if (textField.Placeholder != null)
            {
                tf.textEdition.placeholder = textField.Placeholder;
            }
        }

        private static void PaintToggle(FrameToggle toggle, Toggle tgl)
        {
            tgl.value = toggle.Value;
            if (toggle.Label != null)
            {
                tgl.label = toggle.Label;
            }
        }

        private static void PaintSlider(FrameSlider slider, Slider sld)
        {
            sld.lowValue = slider.Min;
            sld.highValue = slider.Max;
            sld.value = slider.Value;
            if (slider.Label != null)
            {
                sld.label = slider.Label;
            }
        }

        private static void PaintDropdown(Dropdown dropdown, DropdownField dd)
        {
            dd.choices = new List<string>(dropdown.Options);
            dd.index = dropdown.SelectedIndex;
            if (dropdown.Label != null)
            {
                dd.label = dropdown.Label;
            }
        }

        private static UiScrollView CreateScrollView(Fram3.UI.Elements.Content.ScrollView scrollView)
        {
            var sv = new UiScrollView(MapScrollMode(scrollView.ScrollDirection));
            return sv;
        }

        private static void PaintScrollView(Fram3.UI.Elements.Content.ScrollView scrollView, UiScrollView sv)
        {
            sv.mode = MapScrollMode(scrollView.ScrollDirection);
        }

        private static UiProgressBar CreateProgressBar(Fram3.UI.Elements.Content.ProgressBar progressBar)
        {
            var pb = new UiProgressBar
            {
                value = progressBar.Value,
                lowValue = progressBar.Min,
                highValue = progressBar.Max
            };

            if (progressBar.Title != null)
            {
                pb.title = progressBar.Title;
            }

            return pb;
        }

        private static void PaintProgressBar(Fram3.UI.Elements.Content.ProgressBar progressBar, UiProgressBar pb)
        {
            pb.value = progressBar.Value;
            pb.lowValue = progressBar.Min;
            pb.highValue = progressBar.Max;
            if (progressBar.Title != null)
            {
                pb.title = progressBar.Title;
            }
        }

        private static Image CreateImage(FrameImage image)
        {
            var img = new Image();
            ApplyImageDimensions(image.Width, image.Height, img);
#if !FRAM3_PURE_TESTS
            switch (image.Source)
            {
                case UnityEngine.Sprite sprite:
                    img.sprite = sprite;
                    break;
                case UnityEngine.Texture2D texture:
                    img.image = texture;
                    break;
            }
#endif
            return img;
        }

        private static void PaintImage(FrameImage image, Image img)
        {
            ApplyImageDimensions(image.Width, image.Height, img);
#if !FRAM3_PURE_TESTS
            switch (image.Source)
            {
                case UnityEngine.Sprite sprite:
                    img.sprite = sprite;
                    break;
                case UnityEngine.Texture2D texture:
                    img.image = texture;
                    break;
            }
#endif
        }

        private static Image CreateIcon(Icon icon)
        {
            var img = new Image();
            ApplyImageDimensions(icon.Width, icon.Height, img);
#if !FRAM3_PURE_TESTS
            ApplyIconSource(icon, img);
#endif
            return img;
        }

        private static void PaintIcon(Icon icon, Image img)
        {
            ApplyImageDimensions(icon.Width, icon.Height, img);
#if !FRAM3_PURE_TESTS
            ApplyIconSource(icon, img);
#endif
        }

#if !FRAM3_PURE_TESTS
        private static void ApplyIconSource(Icon icon, Image img)
        {
            if (icon.Source is VectorImage preloaded)
            {
                img.vectorImage = preloaded;
                return;
            }

            if (icon.SvgPath == null)
            {
                return;
            }

            var loaded = UnityEditor.AssetDatabase.LoadAssetAtPath<VectorImage>(icon.SvgPath);
            if (loaded != null)
            {
                img.vectorImage = loaded;
            }
        }
#endif

#if !FRAM3_PURE_TESTS
        private static SpinnerElement CreateSpinner(Spinner spinner)
        {
            return new SpinnerElement(spinner);
        }
#endif

        private static ListView CreateListView(IFListViewDescriptor listView)
        {
            var lv = new ListView
            {
                fixedItemHeight = listView.ItemHeight,
                selectionType = MapSelectionType(listView.SelectionMode),
                makeItem = () => new VisualElement(),
                bindItem = (item, index) =>
                {
                    item.Clear();
                    var childElement = listView.BuildItemAt(index);
                    var childNative = CreateNative(childElement);
                    item.Add(childNative);
                }
            };

            // ReSharper disable once InvertIf
            if (listView.OnSelectionChangedUntyped != null)
            {
                var callback = listView.OnSelectionChangedUntyped;
                lv.selectionChanged += items =>
                {
                    var list = new List<object?>();
                    foreach (var item in items)
                    {
                        list.Add(item);
                    }

                    callback(list);
                };
            }

            return lv;
        }

        private static void PaintListView(IFListViewDescriptor listView, ListView lv)
        {
            lv.fixedItemHeight = listView.ItemHeight;
            lv.selectionType = MapSelectionType(listView.SelectionMode);
        }

        private static SelectionType MapSelectionType(ListSelectionMode mode)
        {
            return mode switch
            {
                ListSelectionMode.Single => SelectionType.Single,
                ListSelectionMode.Multiple => SelectionType.Multiple,
                _ => SelectionType.None
            };
        }

        private static void ApplyImageDimensions(float? width, float? height, VisualElement native)
        {
            if (width.HasValue)
            {
                native.style.width = width.Value;
            }

            if (height.HasValue)
            {
                native.style.height = height.Value;
            }
        }

        private static void RegisterGestureCallbacks(Element element, VisualElement native)
        {
            if (element is not GestureDetector gesture)
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

        private static void PaintText(Text text, Label label)
        {
            label.text = text.Content;
            if (text.Style == null)
            {
                return;
            }

            ApplyTextStyle(text.Style, label);
        }

        private static void PaintButton(Fram3.UI.Elements.Input.Button button, UiButton btn)
        {
            btn.text = button.Label;
        }

        private static void ApplyTextStyle(TextStyle style, VisualElement native)
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

#if !FRAM3_PURE_TESTS
            if (style.LetterSpacing != 0f)
            {
                native.style.letterSpacing = style.LetterSpacing;
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

        private static void ApplyLayout(Element element, VisualElement native)
        {
            switch (element)
            {
                case Column column:
                    ApplyColumnLayout(column, native);
                    break;
                case Row row:
                    ApplyRowLayout(row, native);
                    break;
                case Padding padding:
                    ApplyPaddingLayout(padding, native);
                    break;
                case SizedBox sizedBox:
                    ApplySizedBoxLayout(sizedBox, native);
                    break;
                case Container container:
                    ApplyContainerLayout(container, native);
                    break;
                case Center:
                    ApplyCenterLayout(native);
                    break;
                case Expanded expanded:
                    ApplyExpandedLayout(expanded, native);
                    break;
                case Divider divider:
                    ApplyDividerLayout(divider, native);
                    break;
                case Tooltip tooltip:
                    ApplyTooltipLayout(tooltip, native);
                    break;
                case Fram3.UI.Elements.Layout.Wrap:
                    ApplyWrapLayout(native);
                    break;
                case Opacity opacity:
                    ApplyOpacityLayout(opacity, native);
                    break;
                case Margin margin:
                    ApplyMarginLayout(margin, native);
                    break;
                case Stack:
                    ApplyStackLayout(native);
                    break;
                case GestureDetector:
                    ApplyPassthroughLayout(native);
                    break;
                default:
                    ApplyPassthroughLayout(native);
                    break;
            }
        }

        private static void ApplyPassthroughLayout(VisualElement native)
        {
            native.style.flexGrow = 1f;
            native.style.flexShrink = 1f;
            native.style.alignSelf = Align.Stretch;
            native.style.flexDirection = FlexDirection.Column;
        }

        private static void ApplyStackLayout(VisualElement native)
        {
            native.style.flexGrow = 1f;
            native.style.flexShrink = 1f;
            native.style.alignSelf = Align.Stretch;
        }

        private static void ApplyColumnLayout(Column column, VisualElement native)
        {
            native.style.flexDirection = FlexDirection.Column;
            native.style.justifyContent = MapMainAxis(column.MainAxisAlignment);
            native.style.alignItems = MapCrossAxis(column.CrossAxisAlignment);
        }

        private static void ApplyRowLayout(Row row, VisualElement native)
        {
            native.style.flexDirection = FlexDirection.Row;
            native.style.justifyContent = MapMainAxis(row.MainAxisAlignment);
            native.style.alignItems = MapCrossAxis(row.CrossAxisAlignment);
        }

        private static void ApplyPaddingLayout(Padding padding, VisualElement native)
        {
            var insets = padding.Insets;

            native.style.paddingTop = insets.Top;
            native.style.paddingRight = insets.Right;
            native.style.paddingBottom = insets.Bottom;
            native.style.paddingLeft = insets.Left;
        }

        private static void ApplyMarginLayout(Margin margin, VisualElement native)
        {
            var insets = margin.Insets;

            native.style.marginTop = insets.Top;
            native.style.marginRight = insets.Right;
            native.style.marginBottom = insets.Bottom;
            native.style.marginLeft = insets.Left;
        }

        private static void ApplySizedBoxLayout(SizedBox sizedBox, VisualElement native)
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

        private static void ApplyContainerLayout(Container container, VisualElement native)
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
                if (container.Decoration.BorderRadius.HasValue)
                {
                    native.style.overflow = Overflow.Hidden;
                }
            }
        }

        private static void ApplyCenterLayout(VisualElement native)
        {
            native.style.alignItems = Align.Center;
            native.style.justifyContent = Justify.Center;
        }

        private static void ApplyDecoration(BoxDecoration decoration, VisualElement native)
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

            if (decoration.BorderRadius.HasValue)
            {
                var radius = decoration.BorderRadius.Value;
                native.style.borderTopLeftRadius = radius.TopLeft;
                native.style.borderTopRightRadius = radius.TopRight;
                native.style.borderBottomRightRadius = radius.BottomRight;
                native.style.borderBottomLeftRadius = radius.BottomLeft;
            }

            if (decoration.Shadow == null)
            {
                return;
            }

#if !FRAM3_PURE_TESTS
            var shadow = decoration.Shadow;
            var shadowColor = new UnityEngine.Color(
                shadow.Color.R, shadow.Color.G, shadow.Color.B, shadow.Color.A
            );
            native.style.textShadow = new TextShadow
            {
                color = shadowColor,
                offset = new UnityEngine.Vector2(shadow.OffsetX, shadow.OffsetY),
                blurRadius = shadow.BlurRadius
            };
#endif
        }

        private static IntegerField CreateIntField(IntField intField)
        {
            var intf = new IntegerField { value = intField.Value };
            if (intField.Label != null)
            {
                intf.label = intField.Label;
            }

            if (intField.OnChanged == null)
            {
                return intf;
            }

            var callback = intField.OnChanged;
            intf.RegisterValueChangedCallback(evt => callback(evt.newValue));

            return intf;
        }

        private static void PaintIntField(IntField intField, IntegerField intf)
        {
            intf.value = intField.Value;
            if (intField.Label != null)
            {
                intf.label = intField.Label;
            }
        }

        private static UiFloatField CreateFloatField(Fram3.UI.Elements.Input.FloatField floatField)
        {
            var ff = new UiFloatField { value = floatField.Value };
            if (floatField.Label != null)
            {
                ff.label = floatField.Label;
            }

            if (floatField.OnChanged == null)
            {
                return ff;
            }

            var callback = floatField.OnChanged;
            ff.RegisterValueChangedCallback(evt => callback(evt.newValue));

            return ff;
        }

        private static void PaintFloatField(Fram3.UI.Elements.Input.FloatField floatField, UiFloatField ff)
        {
            ff.value = floatField.Value;
            if (floatField.Label != null)
            {
                ff.label = floatField.Label;
            }
        }

        private static UiMinMaxSlider CreateMinMaxSlider(Fram3.UI.Elements.Input.MinMaxSlider minMaxSlider)
        {
            var mms = new UiMinMaxSlider(
                minMaxSlider.MinValue,
                minMaxSlider.MaxValue,
                minMaxSlider.LowLimit,
                minMaxSlider.HighLimit
            );
            mms.style.alignSelf = Align.Stretch;

            if (minMaxSlider.Label != null)
            {
                mms.label = minMaxSlider.Label;
            }

            if (minMaxSlider.OnChanged == null)
            {
                return mms;
            }

            var callback = minMaxSlider.OnChanged;
            mms.RegisterValueChangedCallback(evt => callback(evt.newValue.x, evt.newValue.y));

            return mms;
        }

        private static void PaintMinMaxSlider(Fram3.UI.Elements.Input.MinMaxSlider minMaxSlider, UiMinMaxSlider mms)
        {
            mms.minValue = minMaxSlider.MinValue;
            mms.maxValue = minMaxSlider.MaxValue;
            mms.lowLimit = minMaxSlider.LowLimit;
            mms.highLimit = minMaxSlider.HighLimit;
            if (minMaxSlider.Label != null)
            {
                mms.label = minMaxSlider.Label;
            }
        }

        private static EnumField CreateEnumField(IFEnumFieldDescriptor enumField)
        {
            var ef = new EnumField(enumField.ValueAsEnum);
            if (enumField.Label != null)
            {
                ef.label = enumField.Label;
            }

            if (!enumField.HasOnChanged)
            {
                return ef;
            }

            var descriptor = enumField;
            ef.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue != null)
                {
                    descriptor.InvokeOnChanged(evt.newValue);
                }
            });

            return ef;
        }

        private static void PaintEnumField(IFEnumFieldDescriptor enumField, EnumField ef)
        {
            ef.value = enumField.ValueAsEnum;
            if (enumField.Label != null)
            {
                ef.label = enumField.Label;
            }
        }

        private static void ApplyExpandedLayout(Expanded expanded, VisualElement native)
        {
            native.style.flexGrow = expanded.Flex;
        }

        private static void ApplyDividerLayout(Divider divider, VisualElement native)
        {
            if (divider.Axis == DividerAxis.Horizontal)
            {
                native.style.height = divider.Thickness;
                if (divider.Indent > 0f)
                {
                    native.style.marginLeft = divider.Indent;
                    native.style.marginRight = divider.Indent;
                }
            }
            else
            {
                native.style.width = divider.Thickness;
                if (divider.Indent > 0f)
                {
                    native.style.marginTop = divider.Indent;
                    native.style.marginBottom = divider.Indent;
                }
            }

            if (!divider.Color.HasValue)
            {
                return;
            }

            var c = divider.Color.Value;
            native.style.backgroundColor = new UnityEngine.Color(c.R, c.G, c.B, c.A);
        }

        private static void ApplyTooltipLayout(Tooltip tooltip, VisualElement native)
        {
            native.tooltip = tooltip.Message;
            native.style.flexDirection = FlexDirection.Column;
            native.style.alignSelf = Align.Stretch;
            native.style.alignItems = Align.Stretch;
        }

        private static void ApplyWrapLayout(VisualElement native)
        {
            native.style.flexDirection = FlexDirection.Row;
            native.style.flexWrap = UiWrap.Wrap;
        }

        private static void ApplyOpacityLayout(Opacity opacity, VisualElement native)
        {
            native.style.opacity = opacity.Value;
        }

        private static ScrollViewMode MapScrollMode(ScrollDirection direction)
        {
            return direction switch
            {
                ScrollDirection.Horizontal => ScrollViewMode.Horizontal,
                ScrollDirection.Both => ScrollViewMode.VerticalAndHorizontal,
                _ => ScrollViewMode.Vertical
            };
        }

        private static Justify MapMainAxis(MainAxisAlignment alignment)
        {
            return alignment switch
            {
                MainAxisAlignment.End => Justify.FlexEnd,
                MainAxisAlignment.Center => Justify.Center,
                MainAxisAlignment.SpaceBetween => Justify.SpaceBetween,
                MainAxisAlignment.SpaceAround => Justify.SpaceAround,
                MainAxisAlignment.SpaceEvenly => Justify.SpaceEvenly,
                _ => Justify.FlexStart
            };
        }

        private static Align MapCrossAxis(CrossAxisAlignment alignment)
        {
            return alignment switch
            {
                CrossAxisAlignment.End => Align.FlexEnd,
                CrossAxisAlignment.Center => Align.Center,
                CrossAxisAlignment.Stretch => Align.Stretch,
                _ => Align.FlexStart
            };
        }
    }
}