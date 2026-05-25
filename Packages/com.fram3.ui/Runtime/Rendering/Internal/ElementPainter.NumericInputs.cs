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

                var dragContainer = slider.Q<VisualElement>(className: "unity-base-slider__drag-container");
                if (dragContainer != null)
                {
                    dragContainer.style.backgroundImage = StyleKeyword.None;
                    dragContainer.style.backgroundColor = new UnityEngine.Color(0, 0, 0, 0);

                    // Attach a custom child with no USS class names so no !important rules apply.
                    // Inserted at index 0 so it renders behind all Unity-managed children.
                    var trackBg = new VisualElement { pickingMode = PickingMode.Ignore };
                    trackBg.style.position = Position.Absolute;
                    trackBg.style.left = 0;
                    trackBg.style.top = 0;
                    trackBg.style.right = 0;
                    trackBg.style.bottom = 0;
                    var trackBgColor = ToUnity(theme.TrackColor);
                    var trackBgRadius = theme.BorderRadius;
                    trackBg.generateVisualContent += ctx => PaintRoundedFill(ctx, trackBgColor, trackBgRadius);
                    dragContainer.Insert(0, trackBg);
                }

                var tracker = slider.Q<VisualElement>(className: "unity-base-slider__tracker");
                if (tracker != null)
                {
                    // Clear the tracker's own background so the trackBg child shows through.
                    tracker.style.backgroundImage = StyleKeyword.None;
                    tracker.style.backgroundColor = new UnityEngine.Color(0, 0, 0, 0);
                }

                var fill = slider.Q<VisualElement>(className: "unity-base-slider__fill");
                if (fill != null)
                {
                    fill.style.backgroundImage = StyleKeyword.None;
                    fill.style.backgroundColor = new UnityEngine.Color(0, 0, 0, 0);
                    var fillColor = ToUnity(theme.PrimaryColor);
                    var fillRadius = theme.BorderRadius;
                    fill.generateVisualContent += ctx => PaintRoundedFill(ctx, fillColor, fillRadius);
                }

                // Unity 6 splits the dragger into __dragger (interaction) and __dragger-border (visual).
                // Both need styling for the themed thumb to show correctly.
                var dragger = slider.Q<VisualElement>(className: "unity-base-slider__dragger");
                if (dragger != null)
                {
                    dragger.style.backgroundImage = StyleKeyword.None;
                    dragger.style.backgroundColor = ToUnity(theme.PrimaryColor);
                    dragger.style.borderTopColor = ToUnity(theme.PrimaryColor);
                    dragger.style.borderRightColor = ToUnity(theme.PrimaryColor);
                    dragger.style.borderBottomColor = ToUnity(theme.PrimaryColor);
                    dragger.style.borderLeftColor = ToUnity(theme.PrimaryColor);
                    dragger.style.borderTopLeftRadius = theme.SliderDraggerRadius;
                    dragger.style.borderTopRightRadius = theme.SliderDraggerRadius;
                    dragger.style.borderBottomLeftRadius = theme.SliderDraggerRadius;
                    dragger.style.borderBottomRightRadius = theme.SliderDraggerRadius;
                }

                var draggerBorder = slider.Q<VisualElement>(className: "unity-base-slider__dragger-border");
                if (draggerBorder != null)
                {
                    draggerBorder.style.backgroundImage = StyleKeyword.None;
                    draggerBorder.style.backgroundColor = ToUnity(theme.PrimaryColor);
                    draggerBorder.style.borderTopColor = ToUnity(theme.PrimaryColor);
                    draggerBorder.style.borderRightColor = ToUnity(theme.PrimaryColor);
                    draggerBorder.style.borderBottomColor = ToUnity(theme.PrimaryColor);
                    draggerBorder.style.borderLeftColor = ToUnity(theme.PrimaryColor);
                    draggerBorder.style.borderTopLeftRadius = theme.SliderDraggerRadius;
                    draggerBorder.style.borderTopRightRadius = theme.SliderDraggerRadius;
                    draggerBorder.style.borderBottomLeftRadius = theme.SliderDraggerRadius;
                    draggerBorder.style.borderBottomRightRadius = theme.SliderDraggerRadius;
                }
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
                    input.style.borderTopWidth = 1f;
                    input.style.borderRightWidth = 1f;
                    input.style.borderBottomWidth = 1f;
                    input.style.borderLeftWidth = 1f;
                    input.style.borderTopLeftRadius = theme.BorderRadius;
                    input.style.borderTopRightRadius = theme.BorderRadius;
                    input.style.borderBottomLeftRadius = theme.BorderRadius;
                    input.style.borderBottomRightRadius = theme.BorderRadius;
                }

                var textEl = dropdownField.Q<VisualElement>(className: "unity-text-element");
                if (textEl != null)
                {
                    textEl.style.color = ToUnity(theme.PrimaryTextColor);
                }
            });

            // Unity's DropdownField calls schedule.Execute(ShowMenu) on PointerDown — the popup
            // is created asynchronously on the next panel update, not immediately. We poll every
            // 16 ms until the popup appears in the visual tree, then style it once and stop.
            // The poller reference is kept so rapid re-clicks cancel the previous poll.
            IVisualElementScheduledItem? dropdownPoller = null;

            dropdownField.RegisterCallback<PointerDownEvent>(_ =>
            {
                dropdownPoller?.Pause();
                var attempts = 0;
                dropdownPoller = dropdownField.schedule.Execute(() =>
                {
                    var popup = dropdownField.panel?.visualTree.Q<VisualElement>(
                        className: "unity-base-dropdown"
                    );

                    if (popup == null)
                    {
                        if (++attempts >= 10) dropdownPoller?.Pause();
                        return;
                    }

                    dropdownPoller?.Pause();

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
                }).Every(16);
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

            ApplyCaretColors(intf, theme);

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
                    textElement.RegisterCallback<CustomStyleResolvedEvent>(_ => ApplyCaretColors(intf, theme));
                }
            });

            intf.RegisterCallback<FocusInEvent>(_ => ApplyCaretColors(intf, theme));
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

            ApplyCaretColors(uiFloatField, theme);

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
                    textElement.RegisterCallback<CustomStyleResolvedEvent>(_ => ApplyCaretColors(uiFloatField, theme));
                }
            });

            uiFloatField.RegisterCallback<FocusInEvent>(_ => ApplyCaretColors(uiFloatField, theme));
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
                    tracker.style.backgroundImage = StyleKeyword.None;
                    tracker.style.backgroundColor = ToUnity(theme.TrackColor);
                    tracker.style.borderTopLeftRadius = theme.BorderRadius;
                    tracker.style.borderTopRightRadius = theme.BorderRadius;
                    tracker.style.borderBottomLeftRadius = theme.BorderRadius;
                    tracker.style.borderBottomRightRadius = theme.BorderRadius;
                }

                var fill = newMinMaxSlider.Q<VisualElement>(className: "unity-min-max-slider__dragger");
                if (fill != null)
                {
                    fill.style.backgroundImage = StyleKeyword.None;
                    fill.style.backgroundColor = ToUnity(theme.PrimaryColor);
                    fill.style.opacity = 0.4f;
                    fill.style.borderTopLeftRadius = theme.BorderRadius;
                    fill.style.borderTopRightRadius = theme.BorderRadius;
                    fill.style.borderBottomLeftRadius = theme.BorderRadius;
                    fill.style.borderBottomRightRadius = theme.BorderRadius;
                }

                // Unity 6 renamed dragger-low/dragger-high to min-thumb/max-thumb.
                // Deferred one frame in case Unity finishes internal layout after attach.
                newMinMaxSlider.schedule.Execute(() =>
                {
                    var minThumbs = newMinMaxSlider.Query<VisualElement>(
                        className: "unity-min-max-slider__min-thumb"
                    ).ToList();

                    foreach (var thumb in minThumbs)
                    {
                        thumb.style.backgroundImage = StyleKeyword.None;
                        thumb.style.backgroundColor = ToUnity(theme.PrimaryColor);
                        thumb.style.borderTopColor = ToUnity(theme.PrimaryColor);
                        thumb.style.borderRightColor = ToUnity(theme.PrimaryColor);
                        thumb.style.borderBottomColor = ToUnity(theme.PrimaryColor);
                        thumb.style.borderLeftColor = ToUnity(theme.PrimaryColor);
                        thumb.style.borderTopLeftRadius = theme.SliderDraggerRadius;
                        thumb.style.borderTopRightRadius = theme.SliderDraggerRadius;
                        thumb.style.borderBottomLeftRadius = theme.SliderDraggerRadius;
                        thumb.style.borderBottomRightRadius = theme.SliderDraggerRadius;
                    }

                    var maxThumbs = newMinMaxSlider
                        .Query<VisualElement>(className: "unity-min-max-slider__max-thumb")
                        .ToList();

                    foreach (var thumb in maxThumbs)
                    {
                        thumb.style.backgroundImage = StyleKeyword.None;
                        thumb.style.backgroundColor = ToUnity(theme.PrimaryColor);
                        thumb.style.borderTopColor = ToUnity(theme.PrimaryColor);
                        thumb.style.borderRightColor = ToUnity(theme.PrimaryColor);
                        thumb.style.borderBottomColor = ToUnity(theme.PrimaryColor);
                        thumb.style.borderLeftColor = ToUnity(theme.PrimaryColor);
                        thumb.style.borderTopLeftRadius = theme.SliderDraggerRadius;
                        thumb.style.borderTopRightRadius = theme.SliderDraggerRadius;
                        thumb.style.borderBottomLeftRadius = theme.SliderDraggerRadius;
                        thumb.style.borderBottomRightRadius = theme.SliderDraggerRadius;
                    }
                }).ExecuteLater(0);
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

        // Draws a filled rounded rectangle using painter2D, bypassing USS !important overrides.
        private static void PaintRoundedFill(MeshGenerationContext ctx, UnityEngine.Color color, float radius)
        {
            var p = ctx.painter2D;
            var r = ctx.visualElement.contentRect;
            float x = r.x, y = r.y, w = r.width, h = r.height;
            float rad = UnityEngine.Mathf.Min(radius, w / 2f, h / 2f);
            p.fillColor = color;
            p.BeginPath();
            p.MoveTo(new UnityEngine.Vector2(x + rad, y));
            p.LineTo(new UnityEngine.Vector2(x + w - rad, y));
            p.ArcTo(new UnityEngine.Vector2(x + w, y), new UnityEngine.Vector2(x + w, y + rad), rad);
            p.LineTo(new UnityEngine.Vector2(x + w, y + h - rad));
            p.ArcTo(new UnityEngine.Vector2(x + w, y + h), new UnityEngine.Vector2(x + w - rad, y + h), rad);
            p.LineTo(new UnityEngine.Vector2(x + rad, y + h));
            p.ArcTo(new UnityEngine.Vector2(x, y + h), new UnityEngine.Vector2(x, y + h - rad), rad);
            p.LineTo(new UnityEngine.Vector2(x, y + rad));
            p.ArcTo(new UnityEngine.Vector2(x, y), new UnityEngine.Vector2(x + rad, y), rad);
            p.ClosePath();
            p.Fill();
        }

        private static void ApplyCaretColors(IntegerField field, Theme theme)
        {
            field.textSelection.cursorColor = ToUnity(theme.PrimaryColor);
            field.textSelection.selectionColor = ToUnity(theme.PrimaryColor.WithAlpha(0.3f));
        }

        private static void ApplyCaretColors(UIFloatField field, Theme theme)
        {
            field.textSelection.cursorColor = ToUnity(theme.PrimaryColor);
            field.textSelection.selectionColor = ToUnity(theme.PrimaryColor.WithAlpha(0.3f));
        }
    }
}