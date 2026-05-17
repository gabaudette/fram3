#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;
using UnityEngine.UIElements;
using UITextField = UnityEngine.UIElements.TextField;
using UIScrollView = UnityEngine.UIElements.ScrollView;
using UIProgressBar = UnityEngine.UIElements.ProgressBar;
using UIFloatField = UnityEngine.UIElements.FloatField;
using UIMinMaxSlider = UnityEngine.UIElements.MinMaxSlider;
using UIWrap = UnityEngine.UIElements.Wrap;
using Column = Fram3.UI.Elements.Layout.Column;
using Row = Fram3.UI.Elements.Layout.Row;

namespace Fram3.UI.Rendering.Internal
{
    /// <summary>
    /// Responsible for creating and updating native UIToolkit <see cref="VisualElement"/>
    /// instances to match a given <see cref="Element"/> description.
    /// </summary>
    internal static class ElementPainter
    {
        private static UnityEngine.Color ToUnity(FrameColor c) => new(c.R, c.G, c.B, c.A);

        /// <summary>
        /// Creates the appropriate native <see cref="VisualElement"/> for the given element
        /// and applies all initial style properties to it.
        /// </summary>
        /// <param name="element">The framework element to produce a native element for.</param>
        /// <param name="theme"></param>
        /// <returns>
        /// A <c>Label</c> for <see cref="Text"/>, a <c>Button</c> for <see cref="Elements.Input.Button"/>,
        /// a <c>TextField</c> for <see cref="Elements.Input.TextField"/>, a <c>Toggle</c> for <see cref="FrameToggle"/>,
        /// a <c>Slider</c> for <see cref="FrameSlider"/>, a <c>DropdownField</c> for <see cref="Dropdown"/>,
        /// a <c>ProgressBar</c> for <see cref="Elements.Content.ProgressBar"/>, a <c>ScrollView</c> for <see cref="Elements.Content.ScrollView"/>,
        /// an <c>Image</c> for <see cref="FrameImage"/> or <see cref="Icon"/>,
        /// an <c>SpinnerElement</c> for <see cref="Spinner"/>,
        /// or a plain <c>VisualElement</c> for all layout/container/gesture elements.
        /// </returns>
        internal static VisualElement CreateNative(Element element, Theme theme)
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
                case PasswordField passwordField:
                    return CreatePasswordField(passwordField, theme);
                case Elements.Input.TextField textField:
                    return CreateTextField(textField, theme);
                case Checkbox checkbox:
                    return CreateCheckbox(checkbox, theme);
                case RadioGroup radioGroup:
                    return CreateRadioGroup(radioGroup, theme);
                case Modal modal:
                    return CreateModal(modal);
                case FrameToggle toggle:
                    return CreateToggle(toggle, theme);
                case IntField intField:
                    return CreateIntField(intField, theme);
                case Elements.Input.FloatField floatField:
                    return CreateFloatField(floatField, theme);
                case Elements.Input.MinMaxSlider minMaxSlider:
                    return CreateMinMaxSlider(minMaxSlider, theme);
                case IEnumFieldDescriptor enumField:
                    return CreateEnumField(enumField);
                case FrameSlider slider:
                    return CreateSlider(slider, theme);
                case Dropdown dropdown:
                    return CreateDropdown(dropdown, theme);
                case Elements.Content.ProgressBar progressBar:
                    return CreateProgressBar(progressBar, theme);
                case Elements.Content.ScrollView scrollView:
                    return CreateScrollView(scrollView, theme);
                case FrameImage image:
                    return CreateImage(image);
                case Icon icon:
                    return CreateIcon(icon);
                case Badge badge:
                    return CreateBadge(badge, theme);
                default:
                    switch (element)
                    {
                        case IGridElement gridElement:
                            return CreateGrid(gridElement, theme);
                        case IListViewDescriptor listView:
                            return CreateListView(listView, theme);
                        case IEnumFieldDescriptor enumFieldDesc:
                            return CreateEnumField(enumFieldDesc);
                    }

                    var native = new VisualElement();
                    ApplyLayout(element, native, theme);
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
        /// <param name="theme">The active theme used when applying colors and style values.</param>
        internal static void Paint(Element element, VisualElement native, Theme theme)
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
                case PasswordField passwordField when native is UITextField ptf:
                    PaintPasswordField(passwordField, ptf);
                    break;
                case Elements.Input.TextField textField when native is UITextField tf:
                    PaintTextField(textField, tf);
                    break;
                case Checkbox checkbox when native is Toggle chkTgl:
                    PaintCheckbox(checkbox, chkTgl);
                    break;
                case RadioGroup radioGroup when native is RadioButtonGroup rbg:
                    PaintRadioGroup(radioGroup, rbg);
                    break;
                case Modal:
                    PaintModal(native);
                    break;
                case FrameToggle toggle when native is Toggle tgl:
                    PaintToggle(toggle, tgl);
                    break;
                case IntField intField when native is IntegerField intf:
                    PaintIntField(intField, intf);
                    break;
                case Elements.Input.FloatField floatField when native is UIFloatField ff:
                    PaintFloatField(floatField, ff);
                    break;
                case Elements.Input.MinMaxSlider minMaxSlider when native is UIMinMaxSlider mms:
                    PaintMinMaxSlider(minMaxSlider, mms);
                    break;
                case FrameSlider slider when native is Slider sld:
                    PaintSlider(slider, sld);
                    break;
                case Dropdown dropdown when native is DropdownField dd:
                    PaintDropdown(dropdown, dd);
                    break;
                case Elements.Content.ProgressBar progressBar when native is UIProgressBar pb:
                    PaintProgressBar(progressBar, pb);
                    break;
                case Elements.Content.ScrollView scrollView when native is UIScrollView sv:
                    PaintScrollView(scrollView, sv);
                    break;
                case FrameImage image when native is Image img:
                    PaintImage(image, img);
                    break;
                case Icon icon when native is Image iconImg:
                    PaintIcon(icon, iconImg);
                    break;
                case Badge badge when native.userData is BadgePipHolder pipHolder:
                    PaintBadge(badge, pipHolder, theme);
                    break;
                default:
                    if (element is IGridElement gridElementPaint)
                    {
                        RebuildNativeGrid(gridElementPaint, native, theme);
                        break;
                    }

                    if (element is IListViewDescriptor listView && native is ListView lv)
                    {
                        PaintListView(listView, lv);
                        break;
                    }

                    if (element is IEnumFieldDescriptor enumFieldDesc && native is EnumField ef)
                    {
                        PaintEnumField(enumFieldDesc, ef);
                        break;
                    }

                    if (element is GestureDetector updatedGesture && native.userData is GestureCallbackHolder holder)
                    {
                        holder.OnTap = updatedGesture.OnTap;
                        holder.OnDoubleTap = updatedGesture.OnDoubleTap;
                        holder.OnLongPress = updatedGesture.OnLongPress;
                        holder.OnPointerEnter = updatedGesture.OnPointerEnter;
                        holder.OnPointerExit = updatedGesture.OnPointerExit;
                    }

                    ApplyLayout(element, native, theme);
                    break;
            }
        }

        /// <summary>
        /// Recursively builds a native <see cref="VisualElement"/> subtree for the given
        /// element and all of its descendants, then attaches the root of that subtree to
        /// <paramref name="parent"/>. Used by <see cref="CreateListView"/> to populate
        /// each list item without a node tree.
        /// </summary>
        /// <param name="element">The element to create a native subtree for.</param>
        /// <param name="parent">The native element to attach the new subtree to.</param>
        /// <param name="theme">The active theme used for styling.</param>
        private static void BuildNativeTree(Element element, VisualElement parent, Theme theme)
        {
            var native = CreateNative(element, theme);
            Paint(element, native, theme);

            var children = element.GetChildren();
            var isStack = element is Stack;

            foreach (var child in children)
            {
                BuildNativeTree(child, native, theme);
            }

            if (isStack)
            {
                foreach (var childNative in native.Children())
                {
                    ApplyAsStackChild(childNative);
                }
            }

            parent.Add(native);
        }

        private static Label CreateLabel(Text text)
        {
            var label = new Label(text.Content);
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

        private static Toggle CreateToggle(FrameToggle frameToggle, Theme theme)
        {
            var toggle = new Toggle { value = frameToggle.Value };
            if (frameToggle.Label != null)
            {
                toggle.label = frameToggle.Label;
            }

            toggle.RegisterCallback<AttachToPanelEvent>(_ =>
            {
                var label = toggle.Q<VisualElement>(className: "unity-base-field__label");
                if (label != null)
                {
                    label.style.color = ToUnity(theme.PrimaryTextColor);
                }

                var checkmark = toggle.Q<VisualElement>(className: "unity-toggle__checkmark");
                if (checkmark == null)
                {
                    return;
                }

                checkmark.style.backgroundImage = default;
                checkmark.style.backgroundColor = ToUnity(theme.SurfaceColor);
                checkmark.style.borderTopColor = ToUnity(theme.PrimaryColor);
                checkmark.style.borderRightColor = ToUnity(theme.PrimaryColor);
                checkmark.style.borderBottomColor = ToUnity(theme.PrimaryColor);
                checkmark.style.borderLeftColor = ToUnity(theme.PrimaryColor);

                var tick = new VisualElement
                {
                    style =
                    {
                        position = Position.Absolute,
                        left = 0f,
                        top = 0f,
                        right = 0f,
                        bottom = 0f,
                        visibility = toggle.value ? Visibility.Visible : Visibility.Hidden
                    }
                };

                checkmark.Add(tick);

                var strokeColor = ToUnity(theme.PrimaryColor);
                tick.generateVisualContent += ctx =>
                {
                    var painter2D = ctx.painter2D;
                    var width = tick.contentRect.width;
                    var height = tick.contentRect.height;

                    painter2D.strokeColor = strokeColor;
                    painter2D.lineWidth = 2f;
                    painter2D.lineCap = LineCap.Round;
                    painter2D.BeginPath();
                    painter2D.MoveTo(new UnityEngine.Vector2(width * 0.15f, height * 0.5f));
                    painter2D.LineTo(new UnityEngine.Vector2(width * 0.4f, height * 0.75f));
                    painter2D.LineTo(new UnityEngine.Vector2(width * 0.85f, height * 0.25f));
                    painter2D.Stroke();
                };

                toggle.RegisterValueChangedCallback(evt =>
                    tick.style.visibility = evt.newValue ? Visibility.Visible : Visibility.Hidden);
            });

            if (frameToggle.OnChanged == null)
            {
                return toggle;
            }

            var callback = frameToggle.OnChanged;
            toggle.RegisterValueChangedCallback(evt => callback(evt.newValue));

            return toggle;
        }

        private static Toggle CreateCheckbox(Checkbox checkbox, Theme theme)
        {
            var toggle = new Toggle { value = checkbox.Value };
            if (checkbox.Label != null)
            {
                toggle.label = checkbox.Label;
            }

            toggle.RegisterCallback<AttachToPanelEvent>(_ =>
            {
                var label = toggle.Q<VisualElement>(className: "unity-base-field__label");
                if (label != null)
                {
                    label.style.color = ToUnity(theme.PrimaryTextColor);
                }

                var checkmark = toggle.Q<VisualElement>(className: "unity-toggle__checkmark");
                if (checkmark == null)
                {
                    return;
                }

                checkmark.style.backgroundImage = default;
                checkmark.style.backgroundColor = ToUnity(theme.SurfaceColor);
                checkmark.style.borderTopColor = ToUnity(theme.PrimaryColor);
                checkmark.style.borderRightColor = ToUnity(theme.PrimaryColor);
                checkmark.style.borderBottomColor = ToUnity(theme.PrimaryColor);
                checkmark.style.borderLeftColor = ToUnity(theme.PrimaryColor);

                var tick = new VisualElement
                {
                    style =
                    {
                        position = Position.Absolute,
                        left = 0f,
                        top = 0f,
                        right = 0f,
                        bottom = 0f,
                        visibility = toggle.value ? Visibility.Visible : Visibility.Hidden
                    }
                };

                checkmark.Add(tick);

                var strokeColor = ToUnity(theme.PrimaryColor);
                tick.generateVisualContent += ctx =>
                {
                    var painter2D = ctx.painter2D;
                    var contentRectWidth = tick.contentRect.width;
                    var contentRectHeight = tick.contentRect.height;

                    painter2D.strokeColor = strokeColor;
                    painter2D.lineWidth = 2f;
                    painter2D.lineCap = LineCap.Round;
                    painter2D.BeginPath();
                    painter2D.MoveTo(new UnityEngine.Vector2(contentRectWidth * 0.15f, contentRectHeight * 0.5f));
                    painter2D.LineTo(new UnityEngine.Vector2(contentRectWidth * 0.4f, contentRectHeight * 0.75f));
                    painter2D.LineTo(new UnityEngine.Vector2(contentRectWidth * 0.85f, contentRectHeight * 0.25f));
                    painter2D.Stroke();
                };

                toggle.RegisterValueChangedCallback(evt =>
                    tick.style.visibility = evt.newValue
                        ? Visibility.Visible
                        : Visibility.Hidden
                );
            });

            if (checkbox.OnChanged == null)
            {
                return toggle;
            }

            var callback = checkbox.OnChanged;
            toggle.RegisterValueChangedCallback(evt => callback(evt.newValue));

            return toggle;
        }

        private static RadioButtonGroup CreateRadioGroup(RadioGroup radioGroup, Theme theme)
        {
            var radioButtonGroup = new RadioButtonGroup
            {
                choices = new List<string>(radioGroup.Options),
                value = ResolveRadioIndex(radioGroup)
            };

            var radioButtonCheckmarks = radioButtonGroup.Query<VisualElement>(
                className: "unity-radio-button__checkmark"
            ).ToList();

            radioButtonGroup.RegisterCallback<AttachToPanelEvent>(_ =>
            {
                foreach (var checkmark in radioButtonCheckmarks)
                {
                    checkmark.style.backgroundColor = ToUnity(theme.PrimaryColor);
                }

                var radioButtonCheckmarkBackgrounds = radioButtonGroup.Query<VisualElement>(
                    className: "unity-radio-button__checkmark-background"
                ).ToList();

                foreach (var checkmarkBackground in radioButtonCheckmarkBackgrounds)
                {
                    checkmarkBackground.style.borderTopColor = ToUnity(theme.PrimaryColor);
                    checkmarkBackground.style.borderRightColor = ToUnity(theme.PrimaryColor);
                    checkmarkBackground.style.borderBottomColor = ToUnity(theme.PrimaryColor);
                    checkmarkBackground.style.borderLeftColor = ToUnity(theme.PrimaryColor);
                    checkmarkBackground.style.backgroundColor = ToUnity(theme.SurfaceColor);
                }

                var labels = radioButtonGroup.Query<VisualElement>(
                    className: "unity-base-field__label"
                ).ToList();

                foreach (var label in labels)
                {
                    label.style.color = ToUnity(theme.PrimaryTextColor);
                }

                var itemLabels = radioButtonGroup.Query<VisualElement>(
                    className: "unity-radio-button__label"
                ).ToList();

                foreach (var itemLabel in itemLabels)
                {
                    itemLabel.style.color = ToUnity(theme.PrimaryTextColor);
                    itemLabel.style.marginLeft = 6f;
                }

                var buttonInputs = radioButtonGroup.Query<VisualElement>(
                    className: "unity-radio-button__input"
                ).ToList();

                foreach (var input in buttonInputs)
                {
                    input.style.marginRight = 0f;
                }
            });

            if (radioGroup.OnChanged == null)
            {
                return radioButtonGroup;
            }

            var callback = radioGroup.OnChanged;
            var options = radioGroup.Options;
            radioButtonGroup.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue >= 0 && evt.newValue < options.Count)
                {
                    callback(options[evt.newValue]);
                }
            });

            return radioButtonGroup;
        }

        private static VisualElement CreateModal(Modal modal)
        {
            var native = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    top = 0,
                    left = 0
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

        private static void PaintCheckbox(Checkbox checkbox, Toggle toggle)
        {
            toggle.value = checkbox.Value;
            if (checkbox.Label != null)
            {
                toggle.label = checkbox.Label;
            }
        }

        private static void PaintRadioGroup(RadioGroup radioGroup, RadioButtonGroup radioButtonGroup)
        {
            radioButtonGroup.choices = new List<string>(radioGroup.Options);
            radioButtonGroup.value = ResolveRadioIndex(radioGroup);
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

        private static void PaintModal(VisualElement native)
        {
            native.style.position = Position.Absolute;
            native.style.top = 0;
            native.style.left = 0;
        }

        private static Slider CreateSlider(FrameSlider frameSlider, Theme theme)
        {
            var slider = new Slider(frameSlider.Min, frameSlider.Max)
            {
                value = frameSlider.Value,
                style =
                {
                    alignSelf = Align.Stretch
                }
            };

            if (frameSlider.Label != null)
            {
                slider.label = frameSlider.Label;
            }

            slider.RegisterCallback<AttachToPanelEvent>(_ =>
            {
                var label = slider.Q<VisualElement>(className: "unity-base-field__label");
                if (label != null)
                {
                    label.style.color = ToUnity(theme.PrimaryTextColor);
                }

                var tracker = slider.Q<VisualElement>(className: "unity-base-slider__tracker");
                if (tracker != null)
                {
                    tracker.style.backgroundColor = ToUnity(theme.TrackColor);
                }

                var fill = slider.Q<VisualElement>(className: "unity-base-slider__fill");
                if (fill != null)
                {
                    fill.style.backgroundColor = ToUnity(theme.PrimaryColor);
                }

                var dragger = slider.Q<VisualElement>(className: "unity-base-slider__dragger");
                if (dragger == null)
                {
                    return;
                }

                dragger.style.backgroundColor = ToUnity(theme.PrimaryColor);
                dragger.style.borderTopColor = ToUnity(theme.PrimaryColor);
                dragger.style.borderRightColor = ToUnity(theme.PrimaryColor);
                dragger.style.borderBottomColor = ToUnity(theme.PrimaryColor);
                dragger.style.borderLeftColor = ToUnity(theme.PrimaryColor);
            });

            if (frameSlider.OnChanged == null)
            {
                return slider;
            }

            var callback = frameSlider.OnChanged;
            slider.RegisterValueChangedCallback(evt => callback(evt.newValue));

            return slider;
        }

        private static DropdownField CreateDropdown(Dropdown dropdown, Theme theme)
        {
            var choices = new List<string>(dropdown.Options);
            var dropdownField = new DropdownField(choices, dropdown.SelectedIndex);
            if (dropdown.Label != null)
            {
                dropdownField.label = dropdown.Label;
            }

            dropdownField.RegisterCallback<AttachToPanelEvent>(_ =>
            {
                var label = dropdownField.Q<VisualElement>(className: "unity-base-field__label");
                if (label != null)
                {
                    label.style.color = ToUnity(theme.PrimaryTextColor);
                }

                var input = dropdownField.Q<VisualElement>(className: "unity-base-popup-field__input");
                if (input != null)
                {
                    input.style.backgroundColor = ToUnity(theme.SurfaceColor);
                    input.style.borderTopColor = ToUnity(theme.InputBorderColor);
                    input.style.borderRightColor = ToUnity(theme.InputBorderColor);
                    input.style.borderBottomColor = ToUnity(theme.InputBorderColor);
                    input.style.borderLeftColor = ToUnity(theme.InputBorderColor);
                }

                var textEl = dropdownField.Q<VisualElement>(className: "unity-text-element");
                if (textEl != null)
                {
                    textEl.style.color = ToUnity(theme.PrimaryTextColor);
                }

                dropdownField.RegisterCallback<PointerDownEvent>(_ =>
                {
                    dropdownField.schedule.Execute(() =>
                    {
                        var popup = dropdownField.panel?.visualTree.Q<VisualElement>(
                            className: "unity-base-dropdown"
                        );

                        if (popup == null)
                        {
                            return;
                        }

                        popup.style.backgroundColor = new UnityEngine.Color(0f, 0f, 0f, 0f);
                        popup.style.borderTopWidth = 0f;
                        popup.style.borderRightWidth = 0f;
                        popup.style.borderBottomWidth = 0f;
                        popup.style.borderLeftWidth = 0f;

                        var containerOuter = popup.Q<VisualElement>(
                            className: "unity-base-dropdown__container-outer"
                        );

                        var containerInner = popup.Q<VisualElement>(
                            className: "unity-base-dropdown__container-inner"
                        );

                        var inner = containerOuter ?? containerInner ?? popup;

                        inner.style.backgroundColor = ToUnity(theme.SurfaceColor);
                        inner.style.borderTopColor = ToUnity(theme.InputBorderColor);
                        inner.style.borderRightColor = ToUnity(theme.InputBorderColor);
                        inner.style.borderBottomColor = ToUnity(theme.InputBorderColor);
                        inner.style.borderLeftColor = ToUnity(theme.InputBorderColor);
                        inner.style.borderTopWidth = 1f;
                        inner.style.borderRightWidth = 1f;
                        inner.style.borderBottomWidth = 1f;
                        inner.style.borderLeftWidth = 1f;
                        inner.style.borderTopLeftRadius = 4f;
                        inner.style.borderTopRightRadius = 4f;
                        inner.style.borderBottomLeftRadius = 4f;
                        inner.style.borderBottomRightRadius = 4f;

                        if (containerOuter != null && containerInner != null)
                        {
                            containerInner.style.backgroundColor = ToUnity(theme.SurfaceColor);
                            containerInner.style.borderTopWidth = 0f;
                            containerInner.style.borderRightWidth = 0f;
                            containerInner.style.borderBottomWidth = 0f;
                            containerInner.style.borderLeftWidth = 0f;
                        }

                        var dropdownItems = popup.Query<VisualElement>(
                            className: "unity-base-dropdown__item"
                        ).ToList();

                        foreach (var dropdownItem in dropdownItems)
                        {
                            dropdownItem.style.color = ToUnity(theme.PrimaryTextColor);
                            dropdownItem.style.backgroundColor = ToUnity(theme.SurfaceColor);

                            var checkmark = dropdownItem.Q<VisualElement>(className: "unity-base-dropdown__checkmark");
                            if (checkmark != null)
                            {
                                checkmark.style.visibility = Visibility.Hidden;
                            }

                            var hoverColor = ToUnity(theme.PrimaryColor.WithAlpha(0.15f));
                            var surfaceColor = ToUnity(theme.SurfaceColor);

                            dropdownItem.RegisterCallback<PointerEnterEvent>(_ =>
                                dropdownItem.style.backgroundColor = hoverColor
                            );

                            dropdownItem.RegisterCallback<PointerLeaveEvent>(_ =>
                                dropdownItem.style.backgroundColor = surfaceColor
                            );
                        }
                    }).ExecuteLater(1);
                });
            });

            if (dropdown.OnChanged == null)
            {
                return dropdownField;
            }

            var callback = dropdown.OnChanged;
            dropdownField.RegisterValueChangedCallback(_ => callback(dropdownField.index));

            return dropdownField;
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

        private static void PaintToggle(FrameToggle frameToggle, Toggle toggle)
        {
            toggle.value = frameToggle.Value;
            if (frameToggle.Label != null)
            {
                toggle.label = frameToggle.Label;
            }

            var checkmark = toggle.Q<VisualElement>(className: "unity-toggle__checkmark");
            if (checkmark == null)
            {
                return;
            }

            checkmark.style.backgroundImage = default;
            if (checkmark.childCount > 0)
            {
                checkmark[0].style.visibility = toggle.value ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private static void PaintSlider(FrameSlider frameSlider, Slider slider)
        {
            slider.lowValue = frameSlider.Min;
            slider.highValue = frameSlider.Max;
            slider.value = frameSlider.Value;
            if (frameSlider.Label != null)
            {
                slider.label = frameSlider.Label;
            }
        }

        private static void PaintDropdown(Dropdown dropdown, DropdownField dropdownField)
        {
            dropdownField.choices = new List<string>(dropdown.Options);
            dropdownField.index = dropdown.SelectedIndex;
            if (dropdown.Label != null)
            {
                dropdownField.label = dropdown.Label;
            }
        }

        private static UIScrollView CreateScrollView(Elements.Content.ScrollView scrollView, Theme theme)
        {
            var uiScrollView = new UIScrollView(MapScrollMode(scrollView.ScrollDirection));

            uiScrollView.RegisterCallback<AttachToPanelEvent>(_ =>
                uiScrollView.schedule.Execute(() => ApplyScrollbarTheme(uiScrollView, theme)).ExecuteLater(1)
            );

            return uiScrollView;
        }

        private static void ApplyScrollbarTheme(VisualElement container, Theme theme)
        {
            var scrollContainers = container.Query<VisualElement>(
                className: "unity-scroll-view__content-and-vertical-scroll-container"
            ).ToList();

            foreach (var scrollContainer in scrollContainers)
            {
                scrollContainer.style.borderTopColor = ToUnity(theme.InputBorderColor);
                scrollContainer.style.borderRightColor = ToUnity(theme.InputBorderColor);
                scrollContainer.style.borderBottomColor = ToUnity(theme.InputBorderColor);
                scrollContainer.style.borderLeftColor = ToUnity(theme.InputBorderColor);
            }

            var lowButtons = container.Query<VisualElement>(className: "unity-scroller__low-button").ToList();

            foreach (var lowButton in lowButtons)
            {
                lowButton.style.display = DisplayStyle.None;
            }

            var highButtons = container.Query<VisualElement>(className: "unity-scroller__high-button")
                .ToList();

            foreach (var highButton in highButtons)
            {
                highButton.style.display = DisplayStyle.None;
            }

            foreach (var scroller in container.Query<VisualElement>(className: "unity-scroller").ToList())
            {
                scroller.style.backgroundColor = new UnityEngine.Color(0f, 0f, 0f, 0f);
                scroller.style.borderTopColor = ToUnity(theme.InputBorderColor);
                scroller.style.borderRightColor = ToUnity(theme.InputBorderColor);
                scroller.style.borderBottomColor = ToUnity(theme.InputBorderColor);
                scroller.style.borderLeftColor = ToUnity(theme.InputBorderColor);
                scroller.style.borderTopWidth = 1f;
                scroller.style.borderRightWidth = 1f;
                scroller.style.borderBottomWidth = 1f;
                scroller.style.borderLeftWidth = 1f;
                scroller.style.paddingTop = 0f;
                scroller.style.paddingBottom = 0f;
                scroller.style.overflow = Overflow.Visible;
            }

            foreach (var scrollerSlider in container.Query<VisualElement>(className: "unity-scroller__slider").ToList())
            {
                scrollerSlider.style.overflow = Overflow.Visible;
                scrollerSlider.style.marginTop = 0f;
                scrollerSlider.style.marginBottom = 0f;
                scrollerSlider.style.paddingTop = 0f;
                scrollerSlider.style.paddingBottom = 0f;
                scrollerSlider.style.borderTopColor = ToUnity(theme.InputBorderColor);
                scrollerSlider.style.borderRightColor = ToUnity(theme.InputBorderColor);
                scrollerSlider.style.borderBottomColor = ToUnity(theme.InputBorderColor);
                scrollerSlider.style.borderLeftColor = ToUnity(theme.InputBorderColor);
            }

            foreach (var baseSlider in container.Query<VisualElement>(className: "unity-base-slider").ToList())
            {
                baseSlider.style.overflow = Overflow.Visible;
                baseSlider.style.marginTop = 0f;
                baseSlider.style.marginBottom = 0f;
                baseSlider.style.paddingTop = 0f;
                baseSlider.style.paddingBottom = 0f;
            }

            foreach (var sliderInput in container.Query<VisualElement>(className: "unity-slider__input").ToList())
            {
                sliderInput.style.overflow = Overflow.Visible;
                sliderInput.style.marginTop = 0f;
                sliderInput.style.marginBottom = 0f;
                sliderInput.style.paddingTop = 0f;
                sliderInput.style.paddingBottom = 0f;
                sliderInput.style.backgroundColor = ToUnity(theme.TrackColor);
                sliderInput.style.borderTopWidth = 0f;
                sliderInput.style.borderRightWidth = 0f;
                sliderInput.style.borderBottomWidth = 0f;
                sliderInput.style.borderLeftWidth = 0f;
            }

            var dragContainers = container.Query<VisualElement>(
                className: "unity-base-slider__drag-container"
            ).ToList();

            foreach (var dragContainer in dragContainers)
            {
                dragContainer.style.overflow = Overflow.Visible;
                dragContainer.style.marginTop = 0f;
                dragContainer.style.marginBottom = 0f;
                dragContainer.style.paddingTop = 0f;
                dragContainer.style.paddingBottom = 0f;
            }

            foreach (var tracker in container.Query<VisualElement>(className: "unity-base-slider__tracker").ToList())
            {
                tracker.style.backgroundColor = new UnityEngine.Color(0f, 0f, 0f, 0f);
            }

            foreach (var dragger in container.Query<VisualElement>(className: "unity-base-slider__dragger").ToList())
            {
                dragger.style.backgroundColor = ToUnity(theme.PrimaryColor);
                dragger.style.borderTopLeftRadius = 4f;
                dragger.style.borderTopRightRadius = 4f;
                dragger.style.borderBottomLeftRadius = 4f;
                dragger.style.borderBottomRightRadius = 4f;
                dragger.style.borderTopColor = ToUnity(theme.PrimaryColor);
                dragger.style.borderRightColor = ToUnity(theme.PrimaryColor);
                dragger.style.borderBottomColor = ToUnity(theme.PrimaryColor);
                dragger.style.borderLeftColor = ToUnity(theme.PrimaryColor);
                dragger.style.borderTopWidth = 0f;
                dragger.style.borderRightWidth = 0f;
                dragger.style.borderBottomWidth = 0f;
                dragger.style.borderLeftWidth = 0f;
            }
        }

        private static void PaintScrollView(Elements.Content.ScrollView scrollView, UIScrollView uiScrollView)
        {
            uiScrollView.mode = MapScrollMode(scrollView.ScrollDirection);
        }

        private static UIProgressBar CreateProgressBar(Elements.Content.ProgressBar progressBar, Theme theme)
        {
            var uiProgressBar = new UIProgressBar
            {
                value = progressBar.Value,
                lowValue = progressBar.Min,
                highValue = progressBar.Max
            };

            if (progressBar.Title != null)
            {
                uiProgressBar.title = progressBar.Title;
            }

            uiProgressBar.RegisterCallback<AttachToPanelEvent>(_ =>
            {
                var progressBarBackground = uiProgressBar.Q<VisualElement>(
                    className: "unity-progress-bar__background"
                );

                if (progressBarBackground != null)
                {
                    progressBarBackground.style.backgroundColor = ToUnity(theme.TrackColor);
                }

                var progress = uiProgressBar.Q<VisualElement>(className: "unity-progress-bar__progress");
                if (progress != null)
                {
                    progress.style.backgroundColor = ToUnity(theme.PrimaryColor);
                }

                var title = uiProgressBar.Q<VisualElement>(className: "unity-progress-bar__title");
                if (title != null)
                {
                    title.style.color = ToUnity(theme.PrimaryTextColor);
                }
            });

            return uiProgressBar;
        }

        private static void PaintProgressBar(Elements.Content.ProgressBar progressBar, UIProgressBar uiProgressBar)
        {
            uiProgressBar.value = progressBar.Value;
            uiProgressBar.lowValue = progressBar.Min;
            uiProgressBar.highValue = progressBar.Max;
            if (progressBar.Title != null)
            {
                uiProgressBar.title = progressBar.Title;
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

        private sealed class BadgePipHolder
        {
            public VisualElement Pip { get; }
            public Label? PipLabel { get; }

            public BadgePipHolder(VisualElement pip, Label? pipLabel)
            {
                Pip = pip;
                PipLabel = pipLabel;
            }
        }

        private static VisualElement CreateBadge(Badge badge, Theme theme)
        {
            var wrapper = new VisualElement
            {
                style =
                {
                    position = Position.Relative,
                    overflow = Overflow.Visible
                }
            };

            var pipHolder = BuildBadgePip(badge, theme);
            wrapper.userData = pipHolder;
            wrapper.RegisterCallback<AttachToPanelEvent>(_ => wrapper.Add(pipHolder.Pip));
            return wrapper;
        }

        private static void PaintBadge(Badge badge, BadgePipHolder holder, Theme theme)
        {
            var pipColor = badge.Color ?? theme.ErrorColor;
            var pip = holder.Pip;

            pip.style.backgroundColor = ToUnity(pipColor);

            if (holder.PipLabel != null)
            {
                var label = badge.Count > 99 ? "99+" : badge.Count?.ToString() ?? string.Empty;
                holder.PipLabel.text = label;
            }

#if !FRAM3_PURE_TESTS
            pip.BringToFront();
#endif
        }

        private static BadgePipHolder BuildBadgePip(Badge badge, Theme theme)
        {
            var pipColor = badge.Color ?? theme.ErrorColor;
            var pipDiameter = theme.Spacing * 2f;
            var pipRadius = pipDiameter * 0.5f;

            if (badge.Count == null)
            {
                var dot = new VisualElement
                {
                    style =
                    {
                        position = Position.Absolute,
                        top = -pipRadius * 0.5f,
                        right = -pipRadius * 0.5f,
                        width = pipRadius,
                        height = pipRadius,
                        borderTopLeftRadius = pipRadius,
                        borderTopRightRadius = pipRadius,
                        borderBottomLeftRadius = pipRadius,
                        borderBottomRightRadius = pipRadius,
                        backgroundColor = ToUnity(pipColor)
                    }
                };

                return new BadgePipHolder(dot, null);
            }

            var countText = badge.Count > 99 ? "99+" : badge.Count.ToString()!;
            var pip = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    top = -pipRadius,
                    right = -pipRadius,
                    height = pipDiameter,
                    paddingLeft = theme.Spacing * 0.5f,
                    paddingRight = theme.Spacing * 0.5f,
                    borderTopLeftRadius = theme.BorderRadius,
                    borderTopRightRadius = theme.BorderRadius,
                    borderBottomLeftRadius = theme.BorderRadius,
                    borderBottomRightRadius = theme.BorderRadius,
                    backgroundColor = ToUnity(pipColor),
                    alignItems = Align.Center,
                    justifyContent = Justify.Center
                }
            };

            var label = new Label(countText)
            {
                style =
                {
                    color = ToUnity(theme.OnPrimaryColor),
                    fontSize = theme.FontSizeSmall,
                    unityFontStyleAndWeight = UnityEngine.FontStyle.Bold,
                    paddingTop = 0f,
                    paddingBottom = 0f,
                    paddingLeft = 0f,
                    paddingRight = 0f,
                    marginTop = 0f,
                    marginBottom = 0f,
                    marginLeft = 0f,
                    marginRight = 0f
                }
            };

            pip.Add(label);
            return new BadgePipHolder(pip, label);
        }

#if !FRAM3_PURE_TESTS
        private static void ApplyIconSource(Icon icon, Image img)
        {
            if (icon.Source is VectorImage preloaded)
            {
                img.vectorImage = preloaded;
                return;
            }

            if (icon.ResourcePath != null)
            {
                var loaded = UnityEngine.Resources.Load<VectorImage>(icon.ResourcePath);
                if (loaded != null)
                {
                    img.vectorImage = loaded;
                }

                return;
            }

            if (icon.SvgPath == null)
            {
                return;
            }

#if UNITY_EDITOR
            var svgLoaded = UnityEditor.AssetDatabase.LoadAssetAtPath<VectorImage>(icon.SvgPath);
            if (svgLoaded != null)
            {
                img.vectorImage = svgLoaded;
            }
#endif
        }
#endif

#if !FRAM3_PURE_TESTS
        private static SpinnerElement CreateSpinner(Spinner spinner)
        {
            return new SpinnerElement(spinner);
        }
#endif

        private static VisualElement CreateGrid(IGridElement grid, Theme theme)
        {
            var container = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Column,
                    alignSelf = Align.Stretch,
                    flexGrow = 1f,
                    flexShrink = 1f
                }
            };

            BuildNativeGridRows(grid, container, theme);

            return container;
        }

        private static void RebuildNativeGrid(IGridElement grid, VisualElement container, Theme theme)
        {
            container.Clear();
            BuildNativeGridRows(grid, container, theme);
        }

        private static void BuildNativeGridRows(IGridElement grid, VisualElement container, Theme theme)
        {
            var columnCount = grid.ColumnCount;
            var itemCount = grid.ItemCount;
            var rowSpacing = grid.RowSpacing;
            var columnSpacing = grid.ColumnSpacing;
            var rowIndex = 0;

            for (var i = 0; i < itemCount; i += columnCount)
            {
                if (rowIndex > 0 && rowSpacing > 0f)
                {
                    var spacer = new VisualElement
                    {
                        style =
                        {
                            height = rowSpacing
                        }
                    };

                    container.Add(spacer);
                }

                var row = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        alignSelf = Align.Stretch,
                        alignItems = Align.Stretch
                    }
                };

                for (var j = 0; j < columnCount; j++)
                {
                    if (j > 0 && columnSpacing > 0f)
                    {
                        var spacer = new VisualElement
                        {
                            style =
                            {
                                width = columnSpacing
                            }
                        };
                        row.Add(spacer);
                    }

                    var cell = new VisualElement
                    {
                        style =
                        {
                            flexGrow = 1f,
                            flexShrink = 1f,
                            flexBasis = new StyleLength(new Length(0, LengthUnit.Percent)),
                            alignSelf = Align.Stretch
                        }
                    };

                    var index = i + j;
                    if (index < itemCount)
                    {
                        BuildNativeTree(grid.BuildItemAt(index), cell, theme);
                    }

                    row.Add(cell);
                }

                container.Add(row);
                rowIndex++;
            }
        }

        private sealed class ListViewDescriptorHolder
        {
            public IListViewDescriptor? Descriptor;
            public int IndexListCount = -1;
        }

        private static ListView CreateListView(IListViewDescriptor listViewDescriptor, Theme theme)
        {
            var holder = new ListViewDescriptorHolder { Descriptor = listViewDescriptor };

            var listView = new ListView
            {
                fixedItemHeight = listViewDescriptor.ItemHeight,
                selectionType = MapSelectionType(listViewDescriptor.SelectionMode),
                makeItem = () =>
                {
                    var container = new VisualElement
                    {
                        style =
                        {
                            flexGrow = 1f,
                            alignSelf = Align.Stretch
                        }
                    };

                    return container;
                },
                bindItem = (item, index) =>
                {
                    item.style.backgroundColor = new UnityEngine.Color(0f, 0f, 0f, 0f);
                    if (item.userData == null)
                    {
                        item.userData = new object();
                        item.RegisterCallback<PointerEnterEvent>(_ =>
                            item.style.backgroundColor = ToUnity(theme.TrackColor)
                        );

                        item.RegisterCallback<PointerLeaveEvent>(_ =>
                            item.style.backgroundColor = new UnityEngine.Color(0f, 0f, 0f, 0f)
                        );
                    }

                    item.Clear();

                    var childElement = holder.Descriptor!.BuildItemAt(index);

                    BuildNativeTree(childElement, item, theme);
                },
                userData = holder
            };
#if !FRAM3_PURE_TESTS
            listView.itemsSource = BuildIndexList(listViewDescriptor.ItemCount);
#endif

            listView.style.flexGrow = 1f;
            listView.style.flexShrink = 1f;

            listView.RegisterCallback<AttachToPanelEvent>(_ =>
                listView.schedule.Execute(() => ApplyScrollbarTheme(listView, theme)).ExecuteLater(1));

            if (listViewDescriptor.OnSelectionChangedUntyped == null)
            {
                return listView;
            }

            {
                var callback = listViewDescriptor.OnSelectionChangedUntyped;
                listView.selectionChanged += items =>
                {
                    var list = new List<object?>();
                    foreach (var item in items)
                    {
                        list.Add(item);
                    }

                    callback(list);
                };
            }

            return listView;
        }

        private static void PaintListView(IListViewDescriptor listViewDescriptor, ListView listView)
        {
            listView.fixedItemHeight = listViewDescriptor.ItemHeight;
            listView.selectionType = MapSelectionType(listViewDescriptor.SelectionMode);
            if (listView.userData is ListViewDescriptorHolder holder)
            {
                holder.Descriptor = listViewDescriptor;
#if !FRAM3_PURE_TESTS
                if (holder.IndexListCount == listViewDescriptor.ItemCount)
                {
                    return;
                }

                holder.IndexListCount = listViewDescriptor.ItemCount;
                listView.itemsSource = BuildIndexList(listViewDescriptor.ItemCount);
#endif
            }
#if !FRAM3_PURE_TESTS
            else
            {
                listView.itemsSource = BuildIndexList(listViewDescriptor.ItemCount);
            }
#endif
        }

        private static List<int> BuildIndexList(int count)
        {
            var list = new List<int>(count);
            for (var i = 0; i < count; i++)
            {
                list.Add(i);
            }

            return list;
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

        private const long LongPressThresholdMs = 500;

        private sealed class GestureCallbackHolder
        {
            public Action? OnTap;
            public Action? OnDoubleTap;
            public Action? OnLongPress;
            public Action? OnPointerEnter;
            public Action? OnPointerExit;
            public IVisualElementScheduledItem? LongPressSchedule;
        }

        private static void RegisterGestureCallbacks(Element element, VisualElement native)
        {
            if (element is not GestureDetector gesture)
            {
                return;
            }

            var holder = new GestureCallbackHolder
            {
                OnTap = gesture.OnTap,
                OnDoubleTap = gesture.OnDoubleTap,
                OnLongPress = gesture.OnLongPress,
                OnPointerEnter = gesture.OnPointerEnter,
                OnPointerExit = gesture.OnPointerExit
            };
            native.userData = holder;

            native.RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.clickCount == 2)
                {
                    holder.LongPressSchedule?.Pause();
                    holder.LongPressSchedule = null;
                    holder.OnDoubleTap?.Invoke();
                }
                else
                {
                    holder.OnTap?.Invoke();
                    if (holder.OnLongPress != null)
                    {
                        holder.LongPressSchedule = native.schedule
                            .Execute(() =>
                            {
                                holder.LongPressSchedule = null;
                                holder.OnLongPress?.Invoke();
                            })
                            .StartingIn(LongPressThresholdMs);
                    }
                }
            });

            native.RegisterCallback<PointerUpEvent>(_ =>
            {
                holder.LongPressSchedule?.Pause();
                holder.LongPressSchedule = null;
            });

            native.RegisterCallback<PointerEnterEvent>(_ => holder.OnPointerEnter?.Invoke());

            native.RegisterCallback<PointerLeaveEvent>(_ =>
            {
                holder.LongPressSchedule?.Pause();
                holder.LongPressSchedule = null;
                holder.OnPointerExit?.Invoke();
            });
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

        private static void ApplyLayout(Element element, VisualElement native, Theme theme)
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
                case Avatar avatar:
                    ApplyAvatarLayout(avatar, native, theme);
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
                    ApplyTooltipLayout(tooltip, native, theme);
                    break;
                case Elements.Layout.Wrap:
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
                case ThemeProvider themeProvider:
                    ApplyThemeProviderLayout(themeProvider, native);
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

        private static void ApplyThemeProviderLayout(ThemeProvider provider, VisualElement native)
        {
            var themePrimaryTextColor = provider.Theme.PrimaryTextColor;
            native.style.color = new UnityEngine.Color(
                themePrimaryTextColor.R,
                themePrimaryTextColor.G,
                themePrimaryTextColor.B,
                themePrimaryTextColor.A
            );

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

            if (container.CenterChild)
            {
                native.style.alignItems = Align.Center;
                native.style.justifyContent = Justify.Center;
#if !FRAM3_PURE_TESTS && !FRAM3_DOC_BUILD
                native.contentContainer.style.alignItems = Align.Center;
                native.contentContainer.style.justifyContent = Justify.Center;
                native.contentContainer.style.flexGrow = 1f;
#endif
            }

            if (container.Decoration == null)
            {
                return;
            }

            ApplyDecoration(container.Decoration, native);
            if (container.Decoration.BorderRadius.HasValue)
            {
                native.style.overflow = Overflow.Hidden;
            }
        }

        private static void ApplyAvatarLayout(Avatar avatar, VisualElement native, Theme theme)
        {
            var diameter = avatar.Size switch
            {
                AvatarSize.Small => theme.Spacing * 4f,
                AvatarSize.Large => theme.Spacing * 7f,
                _ => theme.Spacing * 5f
            };

            native.style.width = diameter;
            native.style.height = diameter;
            native.style.flexShrink = 0f;
            native.style.alignSelf = Align.Center;
        }

        private static void ApplyCenterLayout(VisualElement native)
        {
            native.style.alignItems = Align.Center;
            native.style.justifyContent = Justify.Center;
            native.style.flexGrow = 1f;
            native.style.alignSelf = Align.Stretch;
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
#if !FRAM3_PURE_TESTS
                native.style.textShadow = StyleKeyword.None;
#endif
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

        private static IntegerField CreateIntField(IntField intField, Theme theme)
        {
            var intf = new IntegerField { value = intField.Value };
            if (intField.Label != null)
            {
                intf.label = intField.Label;
            }

            intf.RegisterCallback<AttachToPanelEvent>(_ =>
            {
                var label = intf.Q<VisualElement>(className: "unity-base-field__label");
                if (label != null)
                {
                    label.style.color = ToUnity(theme.PrimaryTextColor);
                }

                var input = intf.Q<VisualElement>(className: "unity-base-text-field__input");
                if (input != null)
                {
                    input.style.backgroundColor = ToUnity(theme.SurfaceColor);
                    input.style.borderTopColor = ToUnity(theme.InputBorderColor);
                    input.style.borderRightColor = ToUnity(theme.InputBorderColor);
                    input.style.borderBottomColor = ToUnity(theme.InputBorderColor);
                    input.style.borderLeftColor = ToUnity(theme.InputBorderColor);
                }

                var textEl = intf.Q<VisualElement>(className: "unity-text-element");
                if (textEl != null)
                {
                    textEl.style.color = ToUnity(theme.PrimaryTextColor);
                }
            });

            intf.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (!IsAllowedNumericKey(evt.character, evt.keyCode, evt.ctrlKey || evt.commandKey,
                        allowDecimal: false))
                {
                    evt.StopImmediatePropagation();
                }
            }, TrickleDown.TrickleDown);

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

        private static UIFloatField CreateFloatField(Elements.Input.FloatField floatField, Theme theme)
        {
            if (floatField == null)
            {
                throw new ArgumentNullException(nameof(floatField));
            }

            var uiFloatField = new UIFloatField { value = floatField.Value };
            if (floatField.Label != null)
            {
                uiFloatField.label = floatField.Label;
            }

            uiFloatField.RegisterCallback<AttachToPanelEvent>(_ =>
            {
                var label = uiFloatField.Q<VisualElement>(className: "unity-base-field__label");
                if (label != null)
                {
                    label.style.color = ToUnity(theme.PrimaryTextColor);
                }

                var input = uiFloatField.Q<VisualElement>(className: "unity-base-text-field__input");
                if (input != null)
                {
                    input.style.backgroundColor = ToUnity(theme.SurfaceColor);
                    input.style.borderTopColor = ToUnity(theme.InputBorderColor);
                    input.style.borderRightColor = ToUnity(theme.InputBorderColor);
                    input.style.borderBottomColor = ToUnity(theme.InputBorderColor);
                    input.style.borderLeftColor = ToUnity(theme.InputBorderColor);
                }

                var textEl = uiFloatField.Q<VisualElement>(className: "unity-text-element");
                if (textEl != null)
                {
                    textEl.style.color = ToUnity(theme.PrimaryTextColor);
                }
            });

            uiFloatField.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (!IsAllowedNumericKey(evt.character, evt.keyCode, evt.ctrlKey || evt.commandKey, allowDecimal: true))
                {
                    evt.StopImmediatePropagation();
                }
            }, TrickleDown.TrickleDown);

            if (floatField.OnChanged == null)
            {
                return uiFloatField;
            }

            var callback = floatField.OnChanged;
            uiFloatField.RegisterValueChangedCallback(evt => callback(evt.newValue));

            return uiFloatField;
        }

        private static void PaintFloatField(Elements.Input.FloatField floatField, UIFloatField uiFloatField)
        {
            uiFloatField.value = floatField.Value;
            if (floatField.Label != null)
            {
                uiFloatField.label = floatField.Label;
            }
        }

        private static UIMinMaxSlider CreateMinMaxSlider(Elements.Input.MinMaxSlider minMaxSlider, Theme theme)
        {
            var newMinMaxSlider = new UIMinMaxSlider(
                minMaxSlider.MinValue,
                minMaxSlider.MaxValue,
                minMaxSlider.LowLimit,
                minMaxSlider.HighLimit
            )
            {
                style =
                {
                    alignSelf = Align.Stretch
                }
            };

            if (minMaxSlider.Label != null)
            {
                newMinMaxSlider.label = minMaxSlider.Label;
            }

            newMinMaxSlider.RegisterCallback<AttachToPanelEvent>(_ =>
            {
                var label = newMinMaxSlider.Q<VisualElement>(className: "unity-base-field__label");
                if (label != null)
                {
                    label.style.color = ToUnity(theme.PrimaryTextColor);
                }

                var tracker = newMinMaxSlider.Q<VisualElement>(className: "unity-min-max-slider__tracker");
                if (tracker != null)
                {
                    tracker.style.backgroundColor = ToUnity(theme.TrackColor);
                }

                var fill = newMinMaxSlider.Q<VisualElement>(className: "unity-min-max-slider__dragger");
                if (fill != null)
                {
                    fill.style.backgroundColor = ToUnity(theme.PrimaryColor);
                    fill.style.opacity = 0.4f;
                    fill.style.borderTopLeftRadius = 3f;
                    fill.style.borderTopRightRadius = 3f;
                    fill.style.borderBottomLeftRadius = 3f;
                    fill.style.borderBottomRightRadius = 3f;
                }

                var lowDraggers = newMinMaxSlider.Query<VisualElement>(
                    className: "unity-min-max-slider__dragger-low"
                ).ToList();

                foreach (var dragger in lowDraggers)
                {
                    dragger.style.backgroundColor = ToUnity(theme.PrimaryColor);
                    dragger.style.borderTopColor = ToUnity(theme.PrimaryColor);
                    dragger.style.borderRightColor = ToUnity(theme.PrimaryColor);
                    dragger.style.borderBottomColor = ToUnity(theme.PrimaryColor);
                    dragger.style.borderLeftColor = ToUnity(theme.PrimaryColor);
                    dragger.style.borderTopLeftRadius = 4f;
                    dragger.style.borderTopRightRadius = 4f;
                    dragger.style.borderBottomLeftRadius = 4f;
                    dragger.style.borderBottomRightRadius = 4f;
                }

                var highDraggers = newMinMaxSlider
                    .Query<VisualElement>(className: "unity-min-max-slider__dragger-high")
                    .ToList();

                foreach (var dragger in highDraggers)
                {
                    dragger.style.backgroundColor = ToUnity(theme.PrimaryColor);
                    dragger.style.borderTopColor = ToUnity(theme.PrimaryColor);
                    dragger.style.borderRightColor = ToUnity(theme.PrimaryColor);
                    dragger.style.borderBottomColor = ToUnity(theme.PrimaryColor);
                    dragger.style.borderLeftColor = ToUnity(theme.PrimaryColor);
                    dragger.style.borderTopLeftRadius = 4f;
                    dragger.style.borderTopRightRadius = 4f;
                    dragger.style.borderBottomLeftRadius = 4f;
                    dragger.style.borderBottomRightRadius = 4f;
                }
            });

            if (minMaxSlider.OnChanged == null)
            {
                return newMinMaxSlider;
            }

            var callback = minMaxSlider.OnChanged;
            newMinMaxSlider.RegisterValueChangedCallback(evt => callback(evt.newValue.x, evt.newValue.y));

            return newMinMaxSlider;
        }

        private static void PaintMinMaxSlider(Elements.Input.MinMaxSlider minMaxSlider, UIMinMaxSlider uiMinMaxSlider)
        {
            uiMinMaxSlider.minValue = minMaxSlider.MinValue;
            uiMinMaxSlider.maxValue = minMaxSlider.MaxValue;
            uiMinMaxSlider.lowLimit = minMaxSlider.LowLimit;
            uiMinMaxSlider.highLimit = minMaxSlider.HighLimit;
            if (minMaxSlider.Label != null)
            {
                uiMinMaxSlider.label = minMaxSlider.Label;
            }
        }

        private static EnumField CreateEnumField(IEnumFieldDescriptor enumField)
        {
            var createEnumField = new EnumField(enumField.ValueAsEnum);
            if (enumField.Label != null)
            {
                createEnumField.label = enumField.Label;
            }

            if (!enumField.HasOnChanged)
            {
                return createEnumField;
            }

            var descriptor = enumField;
            createEnumField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue != null)
                {
                    descriptor.InvokeOnChanged(evt.newValue);
                }
            });

            return createEnumField;
        }

        private static void PaintEnumField(IEnumFieldDescriptor enumFieldDescriptor, EnumField enumField)
        {
            enumField.value = enumFieldDescriptor.ValueAsEnum;
            if (enumFieldDescriptor.Label != null)
            {
                enumField.label = enumFieldDescriptor.Label;
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

            var color = divider.Color.Value;
            native.style.backgroundColor = new UnityEngine.Color(color.R, color.G, color.B, color.A);
        }

        private static void ApplyTooltipLayout(Tooltip tooltip, VisualElement native, Theme theme)
        {
            native.tooltip = tooltip.Message;
            native.style.flexDirection = FlexDirection.Column;
            native.style.alignSelf = Align.Stretch;
            native.style.alignItems = Align.Stretch;

            var message = tooltip.Message;
            Label? tip = null;

            native.RegisterCallback<AttachToPanelEvent>(_ =>
            {
                native.RegisterCallback<PointerEnterEvent>(evt =>
                {
                    if (native.panel == null || tip != null)
                    {
                        return;
                    }

                    tip = new Label(message)
                    {
                        pickingMode = PickingMode.Ignore,
                        style =
                        {
                            position = Position.Absolute,
                            backgroundColor = ToUnity(theme.SurfaceColor.WithAlpha(0.96f)),
                            color = ToUnity(theme.PrimaryTextColor),
                            paddingTop = 5f,
                            paddingBottom = 5f,
                            paddingLeft = 10f,
                            paddingRight = 10f,
                            borderTopLeftRadius = 4f,
                            borderTopRightRadius = 4f,
                            borderBottomLeftRadius = 4f,
                            borderBottomRightRadius = 4f,
                            fontSize = 12f,
                            maxWidth = 240f,
                            whiteSpace = WhiteSpace.Normal,
                            borderTopColor = ToUnity(theme.InputBorderColor),
                            borderRightColor = ToUnity(theme.InputBorderColor),
                            borderBottomColor = ToUnity(theme.InputBorderColor),
                            borderLeftColor = ToUnity(theme.InputBorderColor),
                            borderTopWidth = 1f,
                            borderRightWidth = 1f,
                            borderBottomWidth = 1f,
                            borderLeftWidth = 1f,
                            left = evt.position.x + 14f,
                            top = evt.position.y + 18f
                        }
                    };

                    native.panel.visualTree.Add(tip);
                });

                native.RegisterCallback<PointerLeaveEvent>(_ =>
                {
                    tip?.RemoveFromHierarchy();
                    tip = null;
                });

                native.RegisterCallback<PointerMoveEvent>(evt =>
                {
                    if (tip == null)
                    {
                        return;
                    }

                    tip.style.left = evt.position.x + 14f;
                    tip.style.top = evt.position.y + 18f;
                });
            });
        }

        private static bool IsAllowedNumericKey(char character, UnityEngine.KeyCode keyCode, bool ctrlOrCmd,
            bool allowDecimal)
        {
            if (ctrlOrCmd)
            {
                return true;
            }

            if (char.IsDigit(character) || character == '-')
            {
                return true;
            }

            if (allowDecimal && (character == '.' || character == ','))
            {
                return true;
            }

            return keyCode switch
            {
                UnityEngine.KeyCode.Backspace => true,
                UnityEngine.KeyCode.Delete => true,
                UnityEngine.KeyCode.LeftArrow => true,
                UnityEngine.KeyCode.RightArrow => true,
                UnityEngine.KeyCode.Home => true,
                UnityEngine.KeyCode.End => true,
                UnityEngine.KeyCode.Return => true,
                UnityEngine.KeyCode.KeypadEnter => true,
                UnityEngine.KeyCode.Tab => true,
                _ => false
            };
        }

        private static void ApplyWrapLayout(VisualElement native)
        {
            native.style.flexDirection = FlexDirection.Row;
            native.style.flexWrap = UIWrap.Wrap;
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