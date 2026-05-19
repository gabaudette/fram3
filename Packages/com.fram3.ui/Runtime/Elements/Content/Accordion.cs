#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Animation;
using Fram3.UI.Core;
using Fram3.UI.Elements.Animation;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// A single item in an <see cref="Accordion"/>. Contains a header label and a content
    /// element that is shown when the item is expanded.
    /// </summary>
    /// <since>2.0.0-beta.2</since>
    /// <status>live</status>
    public sealed class AccordionItem
    {
        /// <summary>The label shown in the clickable header row.</summary>
        public string Header { get; }

        /// <summary>The element displayed in the body when this item is expanded.</summary>
        public Element Content { get; }

        /// <summary>
        /// Creates an <see cref="AccordionItem"/>.
        /// </summary>
        /// <param name="header">The header label. Must not be null.</param>
        /// <param name="content">The body content element. Must not be null.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="header"/> or <paramref name="content"/> is null.
        /// </exception>
        public AccordionItem(string header, Element content)
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }
    }

    /// <summary>
    /// A vertically stacked list of collapsible sections. Each section has a header row
    /// that the user can tap to expand or collapse the body content beneath it.
    /// The body animates open and closed with a height-reveal slide.
    /// When <see cref="AllowMultiple"/> is false (the default), expanding one item
    /// collapses any previously expanded item.
    /// </summary>
    public sealed class Accordion : StatefulElement
    {
        /// <summary>The ordered list of items to display.</summary>
        public IReadOnlyList<AccordionItem> Items { get; }

        /// <summary>
        /// When true, multiple items may be expanded at the same time.
        /// When false (the default), expanding an item collapses all others.
        /// </summary>
        public bool AllowMultiple { get; }

        /// <summary>
        /// The zero-based index of the item that should be expanded on first mount,
        /// or -1 for all collapsed. Defaults to -1.
        /// </summary>
        public int InitialIndex { get; }

        /// <summary>
        /// Duration in seconds for the open/close slide animation. Defaults to 0.25s.
        /// </summary>
        public float AnimationDuration { get; }

        /// <summary>
        /// The maximum height in logical pixels that an expanded body panel can reach
        /// during the slide animation. Content taller than this value will be clipped
        /// at full expansion. Defaults to 500.
        /// </summary>
        public float MaxBodyHeight { get; }

        /// <summary>
        /// Creates an <see cref="Accordion"/> element.
        /// </summary>
        /// <param name="items">The ordered list of accordion items. Must not be null.</param>
        /// <param name="allowMultiple">
        /// Whether multiple items may be open simultaneously. Defaults to false.
        /// </param>
        /// <param name="initialIndex">
        /// Zero-based index of the item expanded on first mount, or -1 for all collapsed.
        /// Defaults to -1.
        /// </param>
        /// <param name="animationDuration">
        /// Open/close animation duration in seconds. Defaults to 0.25.
        /// </param>
        /// <param name="maxBodyHeight">
        /// Maximum body height in logical pixels during the slide animation. Defaults to 500.
        /// </param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is null.</exception>
        public Accordion(
            IReadOnlyList<AccordionItem> items,
            bool allowMultiple = false,
            int initialIndex = -1,
            float animationDuration = 0.25f,
            float maxBodyHeight = 500f,
            Key? key = null
        ) : base(key)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            AllowMultiple = allowMultiple;
            InitialIndex = initialIndex;
            AnimationDuration = animationDuration;
            MaxBodyHeight = maxBodyHeight;
        }

        /// <inheritdoc/>
        public override Fram3.UI.Core.State CreateState() => new AccordionState();

        private sealed class AccordionState : State<Accordion>
        {
            private bool[] _expanded = Array.Empty<bool>();
            private AnimationController[] _controllers = Array.Empty<AnimationController>();

            public override void InitState()
            {
                var count = Element!.Items.Count;
                _expanded = new bool[count];
                _controllers = new AnimationController[count];

                for (var i = 0; i < count; i++)
                {
                    _controllers[i] = new AnimationController(
                        Element.AnimationDuration,
                        Curves.EaseOut
                    );
                    
                    _controllers[i].AddListener(_ => SetStateIfMounted(null));
                }

                if (Element.InitialIndex < 0 || Element.InitialIndex >= count)
                {
                    return;
                }

                _expanded[Element.InitialIndex] = true;
                _controllers[Element.InitialIndex].Forward();
            }

            public override void DidUpdateElement(StatefulElement oldElement)
            {
                var count = Element!.Items.Count;
                if (count == _expanded.Length)
                {
                    return;
                }

                // Item count changed — resize arrays, preserve existing state.
                var nextExpanded = new bool[count];
                var nextControllers = new AnimationController[count];
                var copyLen = Math.Min(_expanded.Length, count);

                Array.Copy(_expanded, nextExpanded, copyLen);

                for (var i = 0; i < count; i++)
                {
                    if (i < copyLen)
                    {
                        nextControllers[i] = _controllers[i];
                    }
                    else
                    {
                        nextControllers[i] = new AnimationController(
                            Element.AnimationDuration,
                            Curves.EaseOut
                        );
                        nextControllers[i].AddListener(_ => SetStateIfMounted(null));
                    }
                }

                // Dispose controllers that are no longer needed.
                for (var i = copyLen; i < _controllers.Length; i++)
                {
                    _controllers[i].Dispose();
                }

                _expanded = nextExpanded;
                _controllers = nextControllers;
            }

            public override void Dispose()
            {
                foreach (var controller in _controllers)
                {
                    controller.Dispose();
                }

                base.Dispose();
            }

            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);
                var rows = new List<Element>();

                for (var i = 0; i < Element!.Items.Count; i++)
                {
                    // Divider above every item except the first, always present so
                    // the child list structure is stable across expand/collapse.
                    if (i > 0)
                    {
                        rows.Add(new Container(
                            height: 1f,
                            decoration: new BoxDecoration(
                                Color: theme.SecondaryTextColor.WithAlpha(0.15f)
                            )
                        ));
                    }

                    rows.Add(BuildItem(i, theme));
                }

                return new Container(
                    decoration: new BoxDecoration(
                        Color: theme.SurfaceColor,
                        BorderRadius: BorderRadius.All(theme.BorderRadius),
                        Border: new Border(theme.SecondaryTextColor.WithAlpha(0.15f), 1f)
                    )
                )
                {
                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = rows.ToArray()
                    }
                };
            }

            private Element BuildItem(int index, Styling.Theme theme)
            {
                var item = Element!.Items[index];
                var controller = _controllers[index];
                var progress = controller.Value; // 0 = closed, 1 = open
                var isExpanded = _expanded[index];
                var captured = index;

                var headerContent = new Container(
                    padding: EdgeInsets.Symmetric(
                        horizontal: theme.Spacing * 2f,
                        vertical: theme.Spacing * 1.5f
                    )
                )
                {
                    Child = new Row(
                        mainAxisAlignment: MainAxisAlignment.SpaceBetween,
                        crossAxisAlignment: CrossAxisAlignment.Center
                    )
                    {
                        Children = new Element[]
                        {
                            new Text(item.Header, new TextStyle(
                                FontSize: theme.FontSize,
                                Bold: true,
                                Color: theme.PrimaryTextColor
                            )),
                            new Text(
                                isExpanded ? "-" : "+",
                                new TextStyle(
                                    FontSize: theme.FontSizeLarge,
                                    Color: theme.SecondaryTextColor
                                )
                            )
                        }
                    }
                };

                var headerRow = new GestureDetector(
                    headerContent,
                    onTap: () => Toggle(captured)
                );

                // The body slot is always present in the tree so the child list structure
                // is stable. When collapsed, height is 0 and content is clipped.
                var bodyHeight = progress * Element.MaxBodyHeight;

                // A tiny border radius on the clip container forces Overflow.Hidden,
                // which clips the content to the animated height during the tween.
                var bodyClip = new Container(
                    height: bodyHeight,
                    decoration: new BoxDecoration(
                        BorderRadius: BorderRadius.All(0.001f)
                    )
                )
                {
                    Child = new Container(
                        padding: EdgeInsets.Symmetric(
                            horizontal: theme.Spacing * 2f,
                            vertical: theme.Spacing * 1.5f
                        )
                    )
                    {
                        Child = item.Content
                    }
                };

                // The divider between header and body is only visible when the body has height.
                var bodyDivider = new Container(
                    height: progress > 0f ? 1f : 0f,
                    decoration: new BoxDecoration(
                        Color: theme.SecondaryTextColor.WithAlpha(0.15f)
                    )
                );

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        headerRow,
                        bodyDivider,
                        bodyClip
                    }
                };
            }

            private void Toggle(int index)
            {
                if (Element!.AllowMultiple)
                {
                    if (_expanded[index])
                    {
                        _expanded[index] = false;
                        _controllers[index].Reverse();
                    }
                    else
                    {
                        _expanded[index] = true;
                        _controllers[index].Forward();
                    }
                }
                else
                {
                    var wasOpen = _expanded[index];

                    for (var i = 0; i < _expanded.Length; i++)
                    {
                        if (!_expanded[i] || i == index)
                        {
                            continue;
                        }

                        _expanded[i] = false;
                        _controllers[i].Reverse();
                    }

                    if (wasOpen)
                    {
                        _expanded[index] = false;
                        _controllers[index].Reverse();
                    }
                    else
                    {
                        _expanded[index] = true;
                        _controllers[index].Forward();
                    }
                }
            }
        }
    }
}