#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// A single node in a <see cref="TreeView"/> hierarchy.
    /// A node with no children is a leaf; a node with children can be expanded or collapsed.
    /// </summary>
    /// <since>2.0.0-beta.2</since>
    /// <status>live</status>
    public sealed class TreeViewNode
    {
        /// <summary>The label displayed for this node.</summary>
        public string Label { get; }

        /// <summary>An optional icon character or short string shown before the label.</summary>
        public string? Icon { get; }

        /// <summary>The child nodes nested beneath this node. Empty means leaf.</summary>
        public IReadOnlyList<TreeViewNode> Children { get; }

        /// <summary>Whether this node starts expanded on first mount. Defaults to false.</summary>
        public bool InitiallyExpanded { get; }

        /// <summary>
        /// Creates a <see cref="TreeViewNode"/>.
        /// </summary>
        /// <param name="label">The display label. Must not be null.</param>
        /// <param name="children">Child nodes. Null is treated as empty.</param>
        /// <param name="icon">Optional icon string shown before the label.</param>
        /// <param name="initiallyExpanded">Whether the node starts expanded.</param>
        public TreeViewNode(
            string label,
            IReadOnlyList<TreeViewNode>? children = null,
            string? icon = null,
            bool initiallyExpanded = false
        )
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            Children = children ?? Array.Empty<TreeViewNode>();
            Icon = icon;
            InitiallyExpanded = initiallyExpanded;
        }

        /// <summary>Whether this node has any children.</summary>
        public bool HasChildren => Children.Count > 0;
    }

    /// <summary>
    /// A hierarchical expandable/collapsible tree. Nodes with children show a chevron
    /// that toggles their subtree. Leaf nodes optionally fire <see cref="OnNodeTap"/>.
    /// </summary>
    public sealed class TreeView : StatefulElement
    {
        /// <summary>The root-level nodes of the tree.</summary>
        public IReadOnlyList<TreeViewNode> Nodes { get; }

        /// <summary>
        /// Called when the user taps a node. Receives the tapped <see cref="TreeViewNode"/>.
        /// Null means no tap callback.
        /// </summary>
        public Action<TreeViewNode>? OnNodeTap { get; }

        /// <summary>
        /// Horizontal indent in logical pixels applied per nesting level. Defaults to 20.
        /// </summary>
        public float Indent { get; }

        /// <summary>
        /// Creates a <see cref="TreeView"/> element.
        /// </summary>
        /// <param name="nodes">The root-level nodes. Must not be null.</param>
        /// <param name="onNodeTap">Optional callback invoked when a node is tapped.</param>
        /// <param name="indent">Horizontal indent per depth level in logical pixels. Defaults to 20.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nodes"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="indent"/> is negative.</exception>
        public TreeView(
            IReadOnlyList<TreeViewNode> nodes,
            Action<TreeViewNode>? onNodeTap = null,
            float indent = 20f,
            Key? key = null
        ) : base(key)
        {
            if (indent < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(indent), "Indent must be non-negative.");
            }

            Nodes = nodes ?? throw new ArgumentNullException(nameof(nodes));
            OnNodeTap = onNodeTap;
            Indent = indent;
        }

        /// <inheritdoc/>
        public override Core.State CreateState() => new TreeViewState();

        private sealed class TreeViewState : State<TreeView>
        {
            private readonly HashSet<string> _expanded = new HashSet<string>();

            public override void InitState()
            {
                CollectInitialExpanded(Element!.Nodes, "");
            }

            private void CollectInitialExpanded(IReadOnlyList<TreeViewNode> nodes, string prefix)
            {
                for (var i = 0; i < nodes.Count; i++)
                {
                    var node = nodes[i];
                    var path = $"{prefix}/{i}:{node.Label}";
                    if (node.HasChildren && node.InitiallyExpanded)
                    {
                        _expanded.Add(path);
                    }

                    if (node.HasChildren)
                    {
                        CollectInitialExpanded(node.Children, path);
                    }
                }
            }

            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);
                var rows = new List<Element>();

                BuildNodes(
                    nodes: Element!.Nodes,
                    prefix: "",
                    depth: 0,
                    rows,
                    theme
                );

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = rows.ToArray()
                };
            }

            private void BuildNodes(
                IReadOnlyList<TreeViewNode> nodes,
                string prefix,
                int depth,
                List<Element> rows,
                Styling.Theme theme
            )
            {
                for (var i = 0; i < nodes.Count; i++)
                {
                    var node = nodes[i];
                    var path = $"{prefix}/{i}:{node.Label}";
                    var isExpanded = _expanded.Contains(path);

                    rows.Add(
                        BuildRow(
                            node,
                            path,
                            depth,
                            isExpanded,
                            theme
                        )
                    );

                    if (node.HasChildren && isExpanded)
                    {
                        BuildNodes(
                            nodes: node.Children,
                            prefix: path,
                            depth + 1,
                            rows,
                            theme
                        );
                    }
                }
            }

            private Element BuildRow(
                TreeViewNode node,
                string path,
                int depth,
                bool isExpanded,
                Styling.Theme theme
            )
            {
                var rowChildren = new List<Element>();

                // Per-level indent.
                if (depth > 0)
                {
                    rowChildren.Add(SizedBox.FromSize(width: Element!.Indent * depth));
                }

                // Chevron — same font size as the label so it reads at the same scale.
                // Leaf nodes get an invisible placeholder to keep label alignment consistent.
                if (node.HasChildren)
                {
                    rowChildren.Add(
                        new Text(
                            isExpanded ? "v" : ">",
                            style: new TextStyle(
                                FontSize: theme.FontSize,
                                Color: theme.PrimaryColor,
                                Bold: true
                            )
                        )
                    );
                }
                else
                {
                    // Invisible placeholder — same character width keeps labels aligned.
                    rowChildren.Add(
                        new Text(
                            "v",
                            style: new TextStyle(
                                FontSize: theme.FontSize,
                                Color: theme.PrimaryColor.WithAlpha(0f)
                            )
                        )
                    );
                }

                // Fixed gap between chevron and label (or icon).
                rowChildren.Add(SizedBox.FromSize(width: theme.Spacing));

                // Optional icon.
                if (node.Icon != null)
                {
                    rowChildren.Add(
                        new Text(
                            node.Icon,
                            style: new TextStyle(
                                FontSize: theme.FontSize,
                                Color: theme.SecondaryTextColor
                            )
                        )
                    );

                    rowChildren.Add(
                        SizedBox.FromSize(width: theme.Spacing * 0.5f)
                    );
                }

                // Label.
                rowChildren.Add(
                    new Text(
                        node.Label,
                        style: new TextStyle(
                            FontSize: theme.FontSize,
                            Color: theme.PrimaryTextColor,
                            Bold: node.HasChildren
                        )
                    )
                );

                var rowContent = new Container(
                    padding: EdgeInsets.Symmetric(
                        vertical: theme.Spacing * 0.75f,
                        horizontal: theme.Spacing
                    )
                )
                {
                    Child = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = rowChildren.ToArray()
                    }
                };

                Action? onTap = null;
                if (node.HasChildren)
                {
                    var capturedPath = path;
                    onTap = () => SetState(() =>
                    {
                        if (!_expanded.Add(capturedPath))
                        {
                            _expanded.Remove(capturedPath);
                        }
                    });
                }
                else if (Element!.OnNodeTap != null)
                {
                    var capturedNode = node;
                    onTap = () => Element.OnNodeTap(capturedNode);
                }

                return new GestureDetector(child: rowContent, onTap: onTap);
            }
        }
    }
}