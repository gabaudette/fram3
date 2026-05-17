#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
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
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is null.</exception>
        public Accordion(
            IReadOnlyList<AccordionItem> items,
            bool allowMultiple = false,
            int initialIndex = -1,
            Key? key = null
        ) : base(key)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            AllowMultiple = allowMultiple;
            InitialIndex = initialIndex;
        }

        /// <inheritdoc/>
        public override Fram3.UI.Core.State CreateState() => new AccordionState();

        private sealed class AccordionState : State<Accordion>
        {
            private bool[] _expanded = Array.Empty<bool>();

            public override void InitState()
            {
                _expanded = new bool[Element!.Items.Count];

                if (Element.InitialIndex >= 0 && Element.InitialIndex < _expanded.Length)
                {
                    _expanded[Element.InitialIndex] = true;
                }
            }

            public override void DidUpdateElement(StatefulElement oldElement)
            {
                if (Element!.Items.Count != _expanded.Length)
                {
                    var next = new bool[Element.Items.Count];
                    var copyLen = Math.Min(_expanded.Length, next.Length);
                    Array.Copy(_expanded, next, copyLen);
                    _expanded = next;
                }
            }

            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);
                var rows = new List<Element>();

                for (var i = 0; i < Element!.Items.Count; i++)
                {
                    if (rows.Count > 0)
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
                    onTap: () => SetState(() => Toggle(captured))
                );

                if (!isExpanded)
                {
                    return headerRow;
                }

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = new Element[]
                    {
                        headerRow,
                        new Container(
                            height: 1f,
                            decoration: new BoxDecoration(
                                Color: theme.SecondaryTextColor.WithAlpha(0.15f)
                            )
                        ),
                        new Container(
                            padding: EdgeInsets.Symmetric(
                                horizontal: theme.Spacing * 2f,
                                vertical: theme.Spacing * 1.5f
                            )
                        )
                        {
                            Child = item.Content
                        }
                    }
                };
            }

            private void Toggle(int index)
            {
                if (Element!.AllowMultiple)
                {
                    _expanded[index] = !_expanded[index];
                }
                else
                {
                    var wasOpen = _expanded[index];
                    for (var i = 0; i < _expanded.Length; i++)
                    {
                        _expanded[i] = false;
                    }

                    _expanded[index] = !wasOpen;
                }
            }
        }
    }
}
