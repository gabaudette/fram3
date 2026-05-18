#nullable enable
using System.Collections.Generic;
using Fram3.UI.Elements.Input;
using Fram3.UI.Styling;
using UnityEngine.UIElements;

namespace Fram3.UI.Rendering.Internal
{
    internal static partial class ElementPainter
    {
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
    }
}
