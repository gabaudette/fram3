#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// An elevated surface container with an optional header, main content area,
    /// and optional footer. Maps to a <c>VisualElement</c> with a <c>SurfaceColor</c>
    /// background, rounded corners, and a subtle ambient shadow.
    /// </summary>
    /// <since>2.0.0-beta.2</since>
    /// <status>live</status>
    public sealed class Card : StatelessElement
    {
        /// <summary>
        /// Optional element rendered above the content area, separated by a divider.
        /// When null, no header or divider is shown.
        /// </summary>
        public Element? Header { get; }

        /// <summary>
        /// The main content of the card body.
        /// </summary>
        public Element Content { get; }

        /// <summary>
        /// Optional element rendered below the content area, separated by a divider.
        /// When null, no footer or divider is shown.
        /// </summary>
        public Element? Footer { get; }

        /// <summary>
        /// When true, the card has no shadow and uses a border instead of elevation.
        /// Useful for flat/outlined card variants.
        /// </summary>
        public bool Outlined { get; }

        /// <summary>
        /// Creates a <see cref="Card"/> element.
        /// </summary>
        /// <param name="content">The main body content. Required.</param>
        /// <param name="header">Optional header element shown above the content.</param>
        /// <param name="footer">Optional footer element shown below the content.</param>
        /// <param name="outlined">When true, renders as a flat outlined card without shadow.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Card(
            Element content,
            Element? header = null,
            Element? footer = null,
            bool outlined = false,
            Key? key = null
        ) : base(key)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Header = header;
            Footer = footer;
            Outlined = outlined;
        }

        public override Element Build(BuildContext context)
        {
            var theme = ThemeConsumer.Of(context);
            var radius = theme.BorderRadius * 2f;
            var pad = theme.Spacing * 2f;

            var decoration = Outlined
                ? new BoxDecoration(
                    Color: theme.SurfaceColor,
                    BorderRadius: BorderRadius.All(radius),
                    Border: new Border(theme.InputBorderColor, 1f)
                )
                : new BoxDecoration(
                    Color: theme.SurfaceColor,
                    BorderRadius: BorderRadius.All(radius),
                    Shadow: Shadow.Ambient(FrameColor.FromRgba255(0, 0, 0, 30), 8f)
                );

            var children = new List<Element>();

            if (Header != null)
            {
                children.Add(new Container(padding: EdgeInsets.Symmetric(vertical: pad, horizontal: pad))
                {
                    Child = Header
                });
                
                children.Add(new Divider(color: theme.InputBorderColor));
            }

            children.Add(new Container(padding: EdgeInsets.All(pad))
            {
                Child = Content
            });

            if (Footer == null)
            {
                return new Container(
                    decoration: decoration
                )
                {
                    Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = children.ToArray()
                    }
                };
            }

            children.Add(new Divider(color: theme.InputBorderColor));
            children.Add(new Container(padding: EdgeInsets.Symmetric(vertical: pad * 0.75f, horizontal: pad))
            {
                Child = Footer
            });

            return new Container(
                decoration: decoration
            )
            {
                Child = new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = children.ToArray()
                }
            };
        }
    }
}