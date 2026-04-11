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
        /// a <c>ProgressBar</c> for <see cref="FProgressBar"/>, a <c>ScrollView</c> for <see cref="FScrollView"/>,
        /// an <c>Image</c> for <see cref="FImage"/> or <see cref="FIcon"/>,
        /// an <c>FSpinnerElement</c> for <see cref="FSpinner"/>,
        /// or a plain <c>VisualElement</c> for all layout/container/gesture elements.
        /// </returns>
        internal static VisualElement CreateNative(FElement element)
        {
#if !FRAM3_PURE_TESTS
            if (element is FSpinner spinner)
            {
                return CreateSpinner(spinner);
            }
#endif
            switch (element)
            {
                case FText text:
                    return CreateLabel(text);
                case FButton button:
                    return CreateButton(button);
                case FPasswordField passwordField:
                    return CreatePasswordField(passwordField);
                case FTextField textField:
                    return CreateTextField(textField);
                case FCheckbox checkbox:
                    return CreateCheckbox(checkbox);
                case FRadioGroup radioGroup:
                    return CreateRadioGroup(radioGroup);
                case FModal modal:
                    return CreateModal(modal);
                case FToggle toggle:
                    return CreateToggle(toggle);
                case FIntField intField:
                    return CreateIntField(intField);
                case FFloatField floatField:
                    return CreateFloatField(floatField);
                case FMinMaxSlider minMaxSlider:
                    return CreateMinMaxSlider(minMaxSlider);
                case IFEnumFieldDescriptor enumField:
                    return CreateEnumField(enumField);
                case FSlider slider:
                    return CreateSlider(slider);
                case FDropdown dropdown:
                    return CreateDropdown(dropdown);
                case FProgressBar progressBar:
                    return CreateProgressBar(progressBar);
                case FScrollView scrollView:
                    return CreateScrollView(scrollView);
                case FImage image:
                    return CreateImage(image);
                case FIcon icon:
                    return CreateIcon(icon);
                default:
                    if (element is IFListViewDescriptor listView)
                    {
                        return CreateListView(listView);
                    }

                    if (element is IFEnumFieldDescriptor enumFieldDesc)
                    {
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
        /// layer inside an <see cref="FStack"/> container. Called by the renderer after
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
        internal static void Paint(FElement element, VisualElement native)
        {
#if !FRAM3_PURE_TESTS
            if (element is FSpinner spinner && native is FSpinnerElement spinnerEl)
            {
                spinnerEl.Apply(spinner);
                return;
            }
#endif
            switch (element)
            {
                case FText text when native is Label label:
                    PaintText(text, label);
                    break;
                case FButton button when native is Button btn:
                    PaintButton(button, btn);
                    break;
                case FPasswordField passwordField when native is TextField ptf:
                    PaintPasswordField(passwordField, ptf);
                    break;
                case FTextField textField when native is TextField tf:
                    PaintTextField(textField, tf);
                    break;
                case FCheckbox checkbox when native is Toggle chkTgl:
                    PaintCheckbox(checkbox, chkTgl);
                    break;
                case FRadioGroup radioGroup when native is RadioButtonGroup rbg:
                    PaintRadioGroup(radioGroup, rbg);
                    break;
                case FModal modal:
                    PaintModal(modal, native);
                    break;
                case FToggle toggle when native is Toggle tgl:
                    PaintToggle(toggle, tgl);
                    break;
                case FIntField intField when native is IntegerField intf:
                    PaintIntField(intField, intf);
                    break;
                case FFloatField floatField when native is FloatField ff:
                    PaintFloatField(floatField, ff);
                    break;
                case FMinMaxSlider minMaxSlider when native is MinMaxSlider mms:
                    PaintMinMaxSlider(minMaxSlider, mms);
                    break;
                case FSlider slider when native is Slider sld:
                    PaintSlider(slider, sld);
                    break;
                case FDropdown dropdown when native is DropdownField dd:
                    PaintDropdown(dropdown, dd);
                    break;
                case FProgressBar progressBar when native is ProgressBar pb:
                    PaintProgressBar(progressBar, pb);
                    break;
                case FScrollView scrollView when native is ScrollView sv:
                    PaintScrollView(scrollView, sv);
                    break;
                case FImage image when native is Image img:
                    PaintImage(image, img);
                    break;
                case FIcon icon when native is Image iconImg:
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

        private static TextField CreatePasswordField(FPasswordField passwordField)
        {
            var tf = new TextField
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

        private static TextField CreateTextField(FTextField textField)
        {
            var tf = new TextField
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

        private static Toggle CreateCheckbox(FCheckbox checkbox)
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

        private static RadioButtonGroup CreateRadioGroup(FRadioGroup radioGroup)
        {
            var rbg = new RadioButtonGroup();
            rbg.choices = new List<string>(radioGroup.Options);
            rbg.value = ResolveRadioIndex(radioGroup);

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

        private static VisualElement CreateModal(FModal modal)
        {
            var native = new VisualElement();
            native.style.position = Position.Absolute;

            if (modal.BarrierDismissible && modal.OnDismiss != null)
            {
                var callback = modal.OnDismiss;
                native.RegisterCallback<ClickEvent>(_ => callback());
            }

            return native;
        }

        private static void PaintCheckbox(FCheckbox checkbox, Toggle tgl)
        {
            tgl.value = checkbox.Value;
            if (checkbox.Label != null)
            {
                tgl.label = checkbox.Label;
            }
        }

        private static void PaintRadioGroup(FRadioGroup radioGroup, RadioButtonGroup rbg)
        {
            rbg.choices = new List<string>(radioGroup.Options);
            rbg.value = ResolveRadioIndex(radioGroup);
        }

        private static int ResolveRadioIndex(FRadioGroup radioGroup)
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

        private static void PaintModal(FModal modal, VisualElement native)
        {
            native.style.position = Position.Absolute;
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

        private static void PaintPasswordField(FPasswordField passwordField, TextField tf)
        {
            tf.value = passwordField.Value;
            tf.isReadOnly = passwordField.ReadOnly;
            if (passwordField.Placeholder != null)
            {
                tf.textEdition.placeholder = passwordField.Placeholder;
            }
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

        private static ScrollView CreateScrollView(FScrollView scrollView)
        {
            var sv = new ScrollView(MapScrollMode(scrollView.ScrollDirection));
            return sv;
        }

        private static void PaintScrollView(FScrollView scrollView, ScrollView sv)
        {
            sv.mode = MapScrollMode(scrollView.ScrollDirection);
        }

        private static ProgressBar CreateProgressBar(FProgressBar progressBar)
        {
            var pb = new ProgressBar
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

        private static void PaintProgressBar(FProgressBar progressBar, ProgressBar pb)
        {
            pb.value = progressBar.Value;
            pb.lowValue = progressBar.Min;
            pb.highValue = progressBar.Max;
            if (progressBar.Title != null)
            {
                pb.title = progressBar.Title;
            }
        }

        private static Image CreateImage(FImage image)
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

        private static void PaintImage(FImage image, Image img)
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

        private static Image CreateIcon(FIcon icon)
        {
            var img = new Image();
            ApplyImageDimensions(icon.Width, icon.Height, img);
#if !FRAM3_PURE_TESTS
            ApplyIconSource(icon, img);
#endif
            return img;
        }

        private static void PaintIcon(FIcon icon, Image img)
        {
            ApplyImageDimensions(icon.Width, icon.Height, img);
#if !FRAM3_PURE_TESTS
            ApplyIconSource(icon, img);
#endif
        }

#if !FRAM3_PURE_TESTS
        private static void ApplyIconSource(FIcon icon, Image img)
        {
            if (icon.Source is VectorImage preloaded)
            {
                img.vectorImage = preloaded;
                return;
            }

            if (icon.SvgPath != null)
            {
                var loaded = UnityEditor.AssetDatabase.LoadAssetAtPath<VectorImage>(icon.SvgPath);
                if (loaded != null)
                {
                    img.vectorImage = loaded;
                }
            }
        }
#endif

#if !FRAM3_PURE_TESTS
        private static FSpinnerElement CreateSpinner(FSpinner spinner)
        {
            return new FSpinnerElement(spinner);
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

        private static SelectionType MapSelectionType(FListSelectionMode mode)
        {
            return mode switch
            {
                FListSelectionMode.Single => SelectionType.Single,
                FListSelectionMode.Multiple => SelectionType.Multiple,
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
                case FExpanded expanded:
                    ApplyExpandedLayout(expanded, native);
                    break;
                case FDivider divider:
                    ApplyDividerLayout(divider, native);
                    break;
                case FTooltip tooltip:
                    ApplyTooltipLayout(tooltip, native);
                    break;
                case FWrap:
                    ApplyWrapLayout(native);
                    break;
                case FOpacity opacity:
                    ApplyOpacityLayout(opacity, native);
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

        private static IntegerField CreateIntField(FIntField intField)
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

        private static void PaintIntField(FIntField intField, IntegerField intf)
        {
            intf.value = intField.Value;
            if (intField.Label != null)
            {
                intf.label = intField.Label;
            }
        }

        private static FloatField CreateFloatField(FFloatField floatField)
        {
            var ff = new FloatField { value = floatField.Value };
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

        private static void PaintFloatField(FFloatField floatField, FloatField ff)
        {
            ff.value = floatField.Value;
            if (floatField.Label != null)
            {
                ff.label = floatField.Label;
            }
        }

        private static MinMaxSlider CreateMinMaxSlider(FMinMaxSlider minMaxSlider)
        {
            var mms = new MinMaxSlider(
                minMaxSlider.MinValue,
                minMaxSlider.MaxValue,
                minMaxSlider.LowLimit,
                minMaxSlider.HighLimit
            );

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

        private static void PaintMinMaxSlider(FMinMaxSlider minMaxSlider, MinMaxSlider mms)
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

        private static void ApplyExpandedLayout(FExpanded expanded, VisualElement native)
        {
            native.style.flexGrow = expanded.Flex;
        }

        private static void ApplyDividerLayout(FDivider divider, VisualElement native)
        {
            if (divider.Axis == FDividerAxis.Horizontal)
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

        private static void ApplyTooltipLayout(FTooltip tooltip, VisualElement native)
        {
            native.tooltip = tooltip.Message;
        }

        private static void ApplyWrapLayout(VisualElement native)
        {
            native.style.flexDirection = FlexDirection.Row;
            native.style.flexWrap = Wrap.Wrap;
        }

        private static void ApplyOpacityLayout(FOpacity opacity, VisualElement native)
        {
            native.style.opacity = opacity.Value;
        }

        private static ScrollViewMode MapScrollMode(FScrollDirection direction)
        {
            return direction switch
            {
                FScrollDirection.Horizontal => ScrollViewMode.Horizontal,
                FScrollDirection.Both => ScrollViewMode.VerticalAndHorizontal,
                _ => ScrollViewMode.Vertical
            };
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