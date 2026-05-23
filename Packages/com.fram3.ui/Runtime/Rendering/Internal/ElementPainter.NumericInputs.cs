#nullable enable
using System.Collections.Generic;
using Fram3.UI.Elements.Input;
using Fram3.UI.Styling;
using UnityEngine.UIElements;
using UIFloatField = UnityEngine.UIElements.FloatField;
using UIMinMaxSlider = UnityEngine.UIElements.MinMaxSlider;

namespace Fram3.UI.Rendering.Internal
{
    internal static partial class ElementPainter
    {
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
                dragger.style.borderTopLeftRadius = theme.BorderRadius;
                dragger.style.borderTopRightRadius = theme.BorderRadius;
                dragger.style.borderBottomLeftRadius = theme.BorderRadius;
                dragger.style.borderBottomRightRadius = theme.BorderRadius;
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
            });

            // Registered outside AttachToPanelEvent to avoid stacking a new listener on every
            // re-attach. TrickleDown ensures we fire before DropdownField's internal handler,
            // which stops the event before it bubbles.
            dropdownField.RegisterCallback<PointerDownEvent>(_ =>
            {
                // ExecuteLater(16) gives the popup one frame to be created before we style it.
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
                    inner.style.borderTopLeftRadius = theme.BorderRadius;
                    inner.style.borderTopRightRadius = theme.BorderRadius;
                    inner.style.borderBottomLeftRadius = theme.BorderRadius;
                    inner.style.borderBottomRightRadius = theme.BorderRadius;

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
                }).ExecuteLater(16);
            }, TrickleDown.TrickleDown);

            if (dropdown.OnChanged == null)
            {
                return dropdownField;
            }

            var callback = dropdown.OnChanged;
            dropdownField.RegisterValueChangedCallback(_ => callback(dropdownField.index));

            return dropdownField;
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
                    input.style.borderTopLeftRadius = theme.BorderRadius;
                    input.style.borderTopRightRadius = theme.BorderRadius;
                    input.style.borderBottomLeftRadius = theme.BorderRadius;
                    input.style.borderBottomRightRadius = theme.BorderRadius;
                }

                var textElement = intf.Q<VisualElement>(className: "unity-text-element");
                if (textElement != null)
                {
                    textElement.style.color = ToUnity(theme.PrimaryTextColor);
                }
            });

            intf.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (
                    !IsAllowedNumericKey(
                        evt.character,
                        evt.keyCode,
                        evt.ctrlKey || evt.commandKey,
                        allowDecimal: false
                    )
                )
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
                throw new System.ArgumentNullException(nameof(floatField));
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
                    input.style.borderTopLeftRadius = theme.BorderRadius;
                    input.style.borderTopRightRadius = theme.BorderRadius;
                    input.style.borderBottomLeftRadius = theme.BorderRadius;
                    input.style.borderBottomRightRadius = theme.BorderRadius;
                }

                var textElement = uiFloatField.Q<VisualElement>(className: "unity-text-element");
                if (textElement != null)
                {
                    textElement.style.color = ToUnity(theme.PrimaryTextColor);
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
                    fill.style.borderTopLeftRadius = theme.BorderRadius;
                    fill.style.borderTopRightRadius = theme.BorderRadius;
                    fill.style.borderBottomLeftRadius = theme.BorderRadius;
                    fill.style.borderBottomRightRadius = theme.BorderRadius;
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
                    dragger.style.borderTopLeftRadius = theme.BorderRadius;
                    dragger.style.borderTopRightRadius = theme.BorderRadius;
                    dragger.style.borderBottomLeftRadius = theme.BorderRadius;
                    dragger.style.borderBottomRightRadius = theme.BorderRadius;
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
                    dragger.style.borderTopLeftRadius = theme.BorderRadius;
                    dragger.style.borderTopRightRadius = theme.BorderRadius;
                    dragger.style.borderBottomLeftRadius = theme.BorderRadius;
                    dragger.style.borderBottomRightRadius = theme.BorderRadius;
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
    }
}