#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;
using UnityEngine.UIElements;
using UIWrap = UnityEngine.UIElements.Wrap;
using Column = Fram3.UI.Elements.Layout.Column;
using Row = Fram3.UI.Elements.Layout.Row;

namespace Fram3.UI.Rendering.Internal
{
    internal static partial class ElementPainter
    {
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

        private static void PaintModal(VisualElement native)
        {
            native.style.position = Position.Absolute;
            native.style.top = 0;
            native.style.left = 0;
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
                native.style.flexShrink = 0f;
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

        private static void ApplyExpandedLayout(Expanded expanded, VisualElement native)
        {
            native.style.flexGrow = expanded.Flex;

            if (expanded.Padding.HasValue)
            {
                var insets = expanded.Padding.Value;
                native.style.paddingTop = insets.Top;
                native.style.paddingRight = insets.Right;
                native.style.paddingBottom = insets.Bottom;
                native.style.paddingLeft = insets.Left;
            }
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

        private static void ApplyWrapLayout(VisualElement native)
        {
            native.style.flexDirection = FlexDirection.Row;
            native.style.flexWrap = UIWrap.Wrap;
        }

        private static void ApplyOpacityLayout(Opacity opacity, VisualElement native)
        {
            native.style.opacity = opacity.Value;
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
