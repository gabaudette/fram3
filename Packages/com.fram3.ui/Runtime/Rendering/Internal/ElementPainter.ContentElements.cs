#nullable enable
using Fram3.UI.Elements.Content;
using Fram3.UI.Styling;
using UnityEngine.UIElements;
using UIScrollView = UnityEngine.UIElements.ScrollView;
using UIProgressBar = UnityEngine.UIElements.ProgressBar;

namespace Fram3.UI.Rendering.Internal
{
    internal static partial class ElementPainter
    {
        private sealed class ScrollViewThemeHolder
        {
            public Theme Theme;
            public ScrollViewThemeHolder(Theme theme) { Theme = theme; }
        }

        private static UIScrollView CreateScrollView(Elements.Content.ScrollView scrollView, Theme theme)
        {
            var uiScrollView = new UIScrollView(MapScrollMode(scrollView.ScrollDirection));
            var holder = new ScrollViewThemeHolder(theme);
            uiScrollView.userData = holder;

            ApplyScrollerWidth(uiScrollView.verticalScroller, theme.ScrollbarWidth);
            ApplyScrollerWidth(uiScrollView.horizontalScroller, theme.ScrollbarWidth);

#if !FRAM3_PURE_TESTS
            uiScrollView.verticalScroller.RegisterCallback<GeometryChangedEvent>(_ =>
                ApplyScrollerWidth(uiScrollView.verticalScroller, holder.Theme.ScrollbarWidth));
            uiScrollView.horizontalScroller.RegisterCallback<GeometryChangedEvent>(_ =>
                ApplyScrollerWidth(uiScrollView.horizontalScroller, holder.Theme.ScrollbarWidth));
#endif

            uiScrollView.RegisterCallback<AttachToPanelEvent>(_ =>
                uiScrollView.schedule.Execute(() => ApplyScrollbarThemeDecorations(uiScrollView, holder.Theme)).ExecuteLater(1)
            );

            return uiScrollView;
        }

        private static void ApplyScrollerWidth(Scroller scroller, float width)
        {
            scroller.style.width = width;
            scroller.style.minWidth = width;
            scroller.style.maxWidth = width;
            scroller.style.flexBasis = width;
            scroller.style.flexGrow = 0;
            scroller.style.flexShrink = 0;
        }

        private static void ApplyScrollbarTheme(UIScrollView uiScrollView, Theme theme)
        {
            ApplyScrollerWidth(uiScrollView.verticalScroller, theme.ScrollbarWidth);
            ApplyScrollerWidth(uiScrollView.horizontalScroller, theme.ScrollbarWidth);
            ApplyScrollbarThemeDecorations((VisualElement)uiScrollView, theme);
        }

        private static void ApplyScrollbarTheme(VisualElement container, Theme theme)
        {
            foreach (var sv in container.Query<UIScrollView>().ToList())
            {
                ApplyScrollerWidth(sv.verticalScroller, theme.ScrollbarWidth);
                ApplyScrollerWidth(sv.horizontalScroller, theme.ScrollbarWidth);
            }

            ApplyScrollbarThemeDecorations(container, theme);
        }

        private static void ApplyScrollbarThemeDecorations(VisualElement container, Theme theme)
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

        private static void PaintScrollView(Elements.Content.ScrollView scrollView, UIScrollView uiScrollView, Theme theme)
        {
            uiScrollView.mode = MapScrollMode(scrollView.ScrollDirection);
#if !FRAM3_PURE_TESTS
            if (uiScrollView.userData is ScrollViewThemeHolder holder)
                holder.Theme = theme;
            ApplyScrollerWidth(uiScrollView.verticalScroller, theme.ScrollbarWidth);
            ApplyScrollerWidth(uiScrollView.horizontalScroller, theme.ScrollbarWidth);
            if (uiScrollView.panel != null)
                uiScrollView.schedule.Execute(() => ApplyScrollbarThemeDecorations(uiScrollView, theme)).ExecuteLater(1);
#endif
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

        private static Image CreateImage(FrameImage frameImage)
        {
            var image = new Image();
            ApplyImageDimensions(frameImage.Width, frameImage.Height, image);
#if !FRAM3_PURE_TESTS
            switch (frameImage.Source)
            {
                case UnityEngine.Sprite sprite:
                    image.sprite = sprite;
                    break;
                case UnityEngine.Texture2D texture:
                    image.image = texture;
                    break;
            }
#endif
            return image;
        }

        private static void PaintImage(FrameImage frameImage, Image image)
        {
            ApplyImageDimensions(frameImage.Width, frameImage.Height, image);
#if !FRAM3_PURE_TESTS
            switch (frameImage.Source)
            {
                case UnityEngine.Sprite sprite:
                    image.sprite = sprite;
                    break;
                case UnityEngine.Texture2D texture:
                    image.image = texture;
                    break;
            }
#endif
        }

        private static Image CreateIcon(Icon icon)
        {
            var image = new Image();
            ApplyImageDimensions(icon.Width, icon.Height, image);
#if !FRAM3_PURE_TESTS
            ApplyIconSource(icon, image);
#endif
            return image;
        }

        private static void PaintIcon(Icon icon, Image image)
        {
            ApplyImageDimensions(icon.Width, icon.Height, image);
#if !FRAM3_PURE_TESTS
            ApplyIconSource(icon, image);
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

        private static ScrollViewMode MapScrollMode(ScrollDirection direction)
        {
            return direction switch
            {
                ScrollDirection.Horizontal => ScrollViewMode.Horizontal,
                ScrollDirection.Both => ScrollViewMode.VerticalAndHorizontal,
                _ => ScrollViewMode.Vertical
            };
        }

#if !FRAM3_PURE_TESTS
        private static void ApplyIconSource(Icon icon, Image image)
        {
            if (icon.Source is VectorImage preloaded)
            {
                image.vectorImage = preloaded;
                return;
            }

            if (icon.ResourcePath != null)
            {
                var loaded = UnityEngine.Resources.Load<VectorImage>(icon.ResourcePath);
                if (loaded != null)
                {
                    image.vectorImage = loaded;
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
                image.vectorImage = svgLoaded;
            }
#endif
        }

        private static SpinnerElement CreateSpinner(Spinner spinner)
        {
            return new SpinnerElement(spinner);
        }
#endif

#if !FRAM3_PURE_TESTS
        internal static VisualElement CreateContextMenu(ContextMenu menu, Theme theme)
        {
            // Full-screen transparent backdrop that dismisses on click.
            var backdrop = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    top = 0,
                    left = 0,
                    right = 0,
                    bottom = 0
                }
            };

            var onDismiss = menu.OnDismiss;
            backdrop.RegisterCallback<PointerDownEvent>(_ => onDismiss());

            // Menu card positioned at (X, Y).
            var card = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    left = menu.X,
                    top = menu.Y,
                    backgroundColor = ToUnity(theme.SurfaceColor),
                    borderTopLeftRadius = theme.BorderRadius,
                    borderTopRightRadius = theme.BorderRadius,
                    borderBottomLeftRadius = theme.BorderRadius,
                    borderBottomRightRadius = theme.BorderRadius,
                    borderTopWidth = 1f,
                    borderRightWidth = 1f,
                    borderBottomWidth = 1f,
                    borderLeftWidth = 1f,
                    borderTopColor = ToUnity(theme.SecondaryTextColor.WithAlpha(0.2f)),
                    borderRightColor = ToUnity(theme.SecondaryTextColor.WithAlpha(0.2f)),
                    borderBottomColor = ToUnity(theme.SecondaryTextColor.WithAlpha(0.2f)),
                    borderLeftColor = ToUnity(theme.SecondaryTextColor.WithAlpha(0.2f)),
                    paddingTop = theme.Spacing * 0.5f,
                    paddingBottom = theme.Spacing * 0.5f,
                    minWidth = 160f,
                    overflow = Overflow.Hidden
                }
            };

            // Stop backdrop dismissal when clicking inside the card.
            card.RegisterCallback<PointerDownEvent>(evt => evt.StopPropagation());

            foreach (var item in menu.Items)
            {
                card.Add(BuildContextMenuRow(item, theme, onDismiss));
            }

            backdrop.Add(card);

            return backdrop;
        }

        internal static void PaintContextMenu(ContextMenu menu, VisualElement native, Theme theme)
        {
            // Reposition card on rebuild (x/y or items may have changed).
            if (native.childCount == 0)
            {
                return;
            }

            var card = native[0];
            card.style.left = menu.X;
            card.style.top = menu.Y;
            card.style.backgroundColor = ToUnity(theme.SurfaceColor);

            card.Clear();

            var onDismiss = menu.OnDismiss;

            card.RegisterCallback<PointerDownEvent>(evt => evt.StopPropagation());

            foreach (var item in menu.Items)
            {
                card.Add(BuildContextMenuRow(item, theme, onDismiss));
            }
        }

        private static VisualElement BuildContextMenuRow(ContextMenuItem item, Theme theme, System.Action onDismiss)
        {
            var row = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    paddingTop = theme.Spacing * 0.6f,
                    paddingBottom = theme.Spacing * 0.6f,
                    paddingLeft = theme.Spacing,
                    paddingRight = theme.Spacing * 1.5f,
                    opacity = item.Disabled ? 0.38f : 1f
                }
            };

            var label = new Label(item.Label)
            {
                style =
                {
                    fontSize = theme.FontSize,
                    color = ToUnity(theme.PrimaryTextColor),
                    unityFontStyleAndWeight = UnityEngine.FontStyle.Normal,
                    flexGrow = 1f
                }
            };

            row.Add(label);

            if (item.Disabled)
            {
                return row;
            }

            var onTap = item.OnTap;

            row.RegisterCallback<PointerDownEvent>(_ =>
            {
                onTap();
                onDismiss();
            });

            row.RegisterCallback<PointerEnterEvent>(_ =>
            {
                row.style.backgroundColor = ToUnity(theme.PrimaryColor.WithAlpha(0.1f));
            });

            row.RegisterCallback<PointerLeaveEvent>(_ => { row.style.backgroundColor = UnityEngine.Color.clear; });

            return row;
        }

        private sealed class DragState
        {
            public float X;
            public float Y;
            public bool Dragging;
            public float OffsetX;
            public float OffsetY;
            public bool Resizing;
            public float ResizeStartX;
            public float ResizeStartY;
            public float ResizeStartWidth;
            public float ResizeStartHeight;
        }

        internal static VisualElement CreateDraggablePanel(DraggablePanel panel, Theme theme)
        {
            var state = new DragState { X = panel.InitialX, Y = panel.InitialY };

            var root = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    left = state.X,
                    top = state.Y,
                    backgroundColor = ToUnity(theme.SurfaceColor),
                    borderTopLeftRadius = theme.BorderRadius,
                    borderTopRightRadius = theme.BorderRadius,
                    borderBottomLeftRadius = theme.BorderRadius,
                    borderBottomRightRadius = theme.BorderRadius,
                    borderTopWidth = 1f,
                    borderRightWidth = 1f,
                    borderBottomWidth = 1f,
                    borderLeftWidth = 1f,
                    borderTopColor = ToUnity(theme.SecondaryTextColor.WithAlpha(0.2f)),
                    borderRightColor = ToUnity(theme.SecondaryTextColor.WithAlpha(0.2f)),
                    borderBottomColor = ToUnity(theme.SecondaryTextColor.WithAlpha(0.2f)),
                    borderLeftColor = ToUnity(theme.SecondaryTextColor.WithAlpha(0.2f)),
                    minWidth = panel.Resizable ? panel.MinWidth : 120f,
                    overflow = Overflow.Hidden
                }
            };

            if (panel.Width.HasValue)
            {
                root.style.width = panel.Width.Value;
            }

            if (panel.Height.HasValue)
            {
                root.style.height = panel.Height.Value;
            }

            if (panel.Resizable)
            {
                root.style.minWidth = panel.MinWidth;
                root.style.maxWidth = panel.MaxWidth < float.MaxValue ? panel.MaxWidth : StyleKeyword.None;
                root.style.minHeight = panel.MinHeight;
                root.style.maxHeight = panel.MaxHeight < float.MaxValue ? panel.MaxHeight : StyleKeyword.None;
            }

            root.userData = state;
            root.Add(BuildDragHandle(panel, theme, root, state));

            var body = new VisualElement
            {
                style =
                {
                    flexGrow = 1f,
                    paddingTop = theme.Spacing,
                    paddingBottom = theme.Spacing,
                    paddingLeft = theme.Spacing,
                    paddingRight = theme.Spacing
                }
            };
            root.Add(body);

            if (panel.Resizable)
            {
                root.Add(BuildResizeGrip(panel, theme, root, state));
            }

            return root;
        }

        internal static void PaintDraggablePanel(DraggablePanel panel, VisualElement native, Theme theme)
        {
            if (native.userData is not DragState state)
            {
                return;
            }

            native.style.backgroundColor = ToUnity(theme.SurfaceColor);

            if (panel.Width.HasValue)
            {
                native.style.width = panel.Width.Value;
            }

            if (panel.Height.HasValue)
            {
                native.style.height = panel.Height.Value;
            }

            if (panel.Resizable)
            {
                native.style.minWidth = panel.MinWidth;
                native.style.maxWidth = panel.MaxWidth < float.MaxValue ? panel.MaxWidth : StyleKeyword.None;
                native.style.minHeight = panel.MinHeight;
                native.style.maxHeight = panel.MaxHeight < float.MaxValue ? panel.MaxHeight : StyleKeyword.None;
            }

            if (native.childCount >= 1)
            {
                native.RemoveAt(0);
                native.Insert(0, BuildDragHandle(panel, theme, native, state));
            }

            var hasGrip = native.childCount >= 3;

            switch (panel.Resizable)
            {
                case true when !hasGrip:
                    native.Add(BuildResizeGrip(panel, theme, native, state));
                    break;
                case false when hasGrip:
                    native.RemoveAt(2);
                    break;
            }
        }

        private static VisualElement BuildDragHandle(
            DraggablePanel panel,
            Theme theme,
            VisualElement root,
            DragState state
        )
        {
            var handle = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    paddingTop = theme.Spacing * 0.75f,
                    paddingBottom = theme.Spacing * 0.75f,
                    paddingLeft = theme.Spacing,
                    paddingRight = theme.Spacing * 0.5f,
                    backgroundColor = ToUnity(theme.PrimaryColor.WithAlpha(0.08f))
                }
            };

            if (!string.IsNullOrEmpty(panel.Title))
            {
                var title = new Label(panel.Title)
                {
                    style =
                    {
                        flexGrow = 1f,
                        fontSize = theme.FontSize,
                        color = ToUnity(theme.PrimaryTextColor),
                        unityFontStyleAndWeight = UnityEngine.FontStyle.Bold
                    }
                };

                handle.Add(title);
            }
            else
            {
                var spacer = new VisualElement { style = { flexGrow = 1f } };
                handle.Add(spacer);
            }

            if (panel.OnClose != null)
            {
                var onClose = panel.OnClose;
                var closeBtn = new Label("✕")
                {
                    style =
                    {
                        fontSize = theme.FontSize,
                        color = ToUnity(theme.SecondaryTextColor),
                        paddingLeft = theme.Spacing * 0.5f,
                        paddingRight = theme.Spacing * 0.5f
                    }
                };

                closeBtn.RegisterCallback<PointerDownEvent>(evt =>
                    {
                        evt.StopPropagation();
                        onClose();
                    }
                );

                closeBtn.RegisterCallback<PointerEnterEvent>(_ =>
                    {
                        closeBtn.style.color = ToUnity(theme.PrimaryTextColor);
                    }
                );

                closeBtn.RegisterCallback<PointerLeaveEvent>(_ =>
                    {
                        closeBtn.style.color = ToUnity(theme.SecondaryTextColor);
                    }
                );

                handle.Add(closeBtn);
            }

            handle.RegisterCallback<PointerDownEvent>(evt =>
            {
                state.Dragging = true;
                state.OffsetX = evt.position.x - state.X;
                state.OffsetY = evt.position.y - state.Y;
                handle.CapturePointer(evt.pointerId);
                evt.StopPropagation();
            });

            handle.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (!state.Dragging)
                {
                    return;
                }

                state.X = evt.position.x - state.OffsetX;
                state.Y = evt.position.y - state.OffsetY;
                root.style.left = state.X;
                root.style.top = state.Y;
            });

            handle.RegisterCallback<PointerUpEvent>(evt =>
            {
                state.Dragging = false;
                handle.ReleasePointer(evt.pointerId);
            });

            return handle;
        }

        private static VisualElement BuildResizeGrip(
            DraggablePanel panel,
            Theme theme,
            VisualElement root,
            DragState state
        )
        {
            const float gripSize = 16f;
            var grip = new VisualElement
            {
                style =
                {
                    alignSelf = Align.FlexEnd,
                    width = gripSize,
                    height = gripSize,
                    marginRight = 2f,
                    marginBottom = 2f
                }
            };

            // Draw three diagonal lines as a classic resize grip.
            // TODO: Fix this, the grip is like not in the right place and the lines are not perfectly diagonal
            for (var i = 0; i < 3; i++)
            {
                var offset = i * 4f + 2f;
                var line = new VisualElement
                {
                    style =
                    {
                        position = Position.Absolute,
                        width = gripSize - offset,
                        height = 1f,
                        bottom = offset,
                        right = 0,
                        backgroundColor = ToUnity(theme.SecondaryTextColor.WithAlpha(0.4f)),
                        transformOrigin = new StyleTransformOrigin(
                            new TransformOrigin(
                                Length.Percent(100), Length.Percent(0)
                            )
                        ),
                        rotate = new StyleRotate(new Rotate(new Angle(45f, AngleUnit.Degree)))
                    }
                };

                grip.Add(line);
            }

            grip.RegisterCallback<PointerDownEvent>(evt =>
            {
                state.Resizing = true;
                state.ResizeStartX = evt.position.x;
                state.ResizeStartY = evt.position.y;
                state.ResizeStartWidth = root.resolvedStyle.width;
                state.ResizeStartHeight = root.resolvedStyle.height;
                grip.CapturePointer(evt.pointerId);
                evt.StopPropagation();
            });

            grip.RegisterCallback<PointerMoveEvent>(evt =>
            {
                if (!state.Resizing)
                {
                    return;
                }

                var newW = state.ResizeStartWidth + (evt.position.x - state.ResizeStartX);
                var newH = state.ResizeStartHeight + (evt.position.y - state.ResizeStartY);

                newW = System.Math.Clamp(newW, panel.MinWidth, panel.MaxWidth);
                newH = System.Math.Clamp(newH, panel.MinHeight, panel.MaxHeight);
                root.style.width = newW;
                root.style.height = newH;
            });

            grip.RegisterCallback<PointerUpEvent>(evt =>
            {
                state.Resizing = false;
                grip.ReleasePointer(evt.pointerId);
            });

            grip.RegisterCallback<PointerEnterEvent>(_ =>
            {
                foreach (var line in grip.Children())
                {
                    line.style.backgroundColor = ToUnity(theme.PrimaryColor.WithAlpha(0.6f));
                }
            });

            grip.RegisterCallback<PointerLeaveEvent>(_ =>
                {
                    foreach (var line in grip.Children())
                    {
                        line.style.backgroundColor = ToUnity(theme.SecondaryTextColor.WithAlpha(0.4f));
                    }
                }
            );

            return grip;
        }
#endif
    }
}