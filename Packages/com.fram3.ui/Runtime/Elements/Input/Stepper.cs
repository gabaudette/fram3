#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Input
{
    /// <summary>
    /// A numeric input composed of a decrement button, a value label, and an increment button.
    /// Supports optional min/max clamping, a configurable step, and a disabled state.
    /// </summary>
    public sealed class Stepper : StatefulElement
    {
        /// <summary>The current numeric value displayed by the stepper.</summary>
        public int Value { get; }

        /// <summary>
        /// Callback invoked when the user presses increment or decrement.
        /// Receives the new clamped value. Null means the stepper is disabled.
        /// </summary>
        public Action<int>? OnChanged { get; }

        /// <summary>
        /// The minimum allowed value. Null means no lower bound.
        /// </summary>
        public int? Min { get; }

        /// <summary>
        /// The maximum allowed value. Null means no upper bound.
        /// </summary>
        public int? Max { get; }

        /// <summary>Amount added or subtracted on each press. Defaults to 1.</summary>
        public int Step { get; }

        /// <summary>
        /// Optional label displayed above the stepper control.
        /// Null means no label is shown.
        /// </summary>
        public string? Label { get; }

        /// <summary>
        /// Creates a <see cref="Stepper"/> element.
        /// </summary>
        /// <param name="value">The current integer value.</param>
        /// <param name="onChanged">Callback invoked on increment or decrement. Null means disabled.</param>
        /// <param name="min">Optional lower bound. The value will not go below this.</param>
        /// <param name="max">Optional upper bound. The value will not go above this.</param>
        /// <param name="step">The increment/decrement step size. Defaults to 1.</param>
        /// <param name="label">Optional label shown above the control.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Stepper(
            int value = 0,
            Action<int>? onChanged = null,
            int? min = null,
            int? max = null,
            int step = 1,
            string? label = null,
            Key? key = null
        ) : base(key)
        {
            if (step <= 0) throw new ArgumentOutOfRangeException(nameof(step), "Step must be greater than zero.");
            if (min.HasValue && max.HasValue && min.Value > max.Value)
                throw new ArgumentException("Min must not be greater than Max.");

            Value = value;
            OnChanged = onChanged;
            Min = min;
            Max = max;
            Step = step;
            Label = label;
        }

        /// <inheritdoc/>
        public override Fram3.UI.Core.State CreateState() => new StepperState();

        private sealed class StepperState : State<Stepper>
        {
            private bool _decrementHovered;
            private bool _incrementHovered;

            public override void DidUpdateElement(StatefulElement oldElement)
            {
                var old = (Stepper)oldElement;
                var el = Element!;
                if (old.Value != el.Value ||
                    old.Min != el.Min ||
                    old.Max != el.Max ||
                    old.Step != el.Step ||
                    old.Label != el.Label ||
                    (old.OnChanged == null) != (el.OnChanged == null))
                {
                    SetState(null);
                }
            }

            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);
                var el = Element!;
                var disabled = el.OnChanged == null;

                var canDecrement = !disabled && (el.Min == null || el.Value - el.Step >= el.Min.Value);
                var canIncrement = !disabled && (el.Max == null || el.Value + el.Step <= el.Max.Value);

                void Decrement() => el.OnChanged!(Math.Max(
                    el.Value - el.Step,
                    el.Min ?? int.MinValue
                ));

                void Increment() => el.OnChanged!(Math.Min(
                    el.Value + el.Step,
                    el.Max ?? int.MaxValue
                ));

                var control = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                {
                    Children = new Element[]
                    {
                        BuildStepButton("-", canDecrement, _decrementHovered, Decrement,
                            () => SetState(() => _decrementHovered = true),
                            () => SetState(() => _decrementHovered = false),
                            theme),
                        new Container(
                            width: theme.Spacing * 6f,
                            centerChild: true,
                            decoration: new BoxDecoration(
                                Color: theme.SurfaceColor,
                                Border: new Border(theme.InputBorderColor, 1f)
                            )
                        )
                        {
                            Child = new Text(
                                el.Value.ToString(),
                                new TextStyle(
                                    FontSize: theme.FontSize,
                                    Color: disabled ? theme.DisabledTextColor : theme.PrimaryTextColor,
                                    Bold: true
                                )
                            )
                        },
                        BuildStepButton("+", canIncrement, _incrementHovered, Increment,
                            () => SetState(() => _incrementHovered = true),
                            () => SetState(() => _incrementHovered = false),
                            theme),
                    }
                };

                if (el.Label == null)
                {
                    return control;
                }

                return new Column(crossAxisAlignment: CrossAxisAlignment.Start)
                {
                    Children = new Element[]
                    {
                        new Text(el.Label, new TextStyle(
                            FontSize: theme.FontSizeSmall,
                            Color: disabled ? theme.DisabledTextColor : theme.SecondaryTextColor
                        )),
                        SizedBox.FromSize(height: theme.Spacing * 0.5f),
                        control,
                    }
                };
            }

            private static Element BuildStepButton(
                string symbol,
                bool canAct,
                bool hovered,
                Action onPressed,
                Action onEnter,
                Action onExit,
                Styling.Theme theme)
            {
                FrameColor bgColor;
                FrameColor borderColor;
                FrameColor textColor;

                if (!canAct)
                {
                    bgColor = theme.DisabledTextColor.WithAlpha(0.12f);
                    borderColor = theme.DisabledTextColor.WithAlpha(0.3f);
                    textColor = theme.DisabledTextColor;
                }
                else if (hovered)
                {
                    bgColor = theme.PrimaryColor;
                    borderColor = theme.PrimaryColor;
                    textColor = theme.OnPrimaryColor;
                }
                else
                {
                    bgColor = theme.PrimaryColor.WithAlpha(0.12f);
                    borderColor = theme.PrimaryColor.WithAlpha(0.6f);
                    textColor = theme.PrimaryColor;
                }

                return new GestureDetector(
                    onTap: canAct ? onPressed : null,
                    onPointerEnter: canAct ? onEnter : null,
                    onPointerExit: canAct ? onExit : null,
                    child: new Container(
                        width: theme.Spacing * 4f,
                        height: theme.Spacing * 4f,
                        centerChild: true,
                        decoration: new BoxDecoration(
                            Color: bgColor,
                            Border: new Border(borderColor, 1f),
                            BorderRadius: BorderRadius.All(theme.BorderRadius)
                        )
                    )
                    {
                        Child = new Text(symbol, new TextStyle(
                            FontSize: theme.FontSize,
                            Color: textColor,
                            Bold: true
                        ))
                    }
                );
            }
        }
    }
}
