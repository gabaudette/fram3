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

        private static SpinnerElement CreateSpinner(Spinner spinner)
        {
            return new SpinnerElement(spinner);
        }
#endif
    }
}
