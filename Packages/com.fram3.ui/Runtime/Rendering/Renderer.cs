#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Animation;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Rendering.Internal;
using Fram3.UI.Styling;
using UnityEngine.UIElements;

namespace Fram3.UI.Rendering
{
    /// <summary>
    /// The primary entry point for mounting and driving a Fram3 element tree
    /// into a UIToolkit <see cref="VisualElement"/> container. Create one
    /// <see cref="Renderer"/> per UIDocument root, call <see cref="Mount"/>
    /// once to build the initial tree, then call <see cref="Tick"/> from
    /// <c>MonoBehaviour.Update</c> on every frame to flush pending rebuilds.
    /// </summary>
    /// <remarks>
    /// <see cref="Renderer"/> owns the entire lifecycle of the node tree. Call
    /// <see cref="Dispose"/> when the owning <c>MonoBehaviour</c> is destroyed to
    /// unmount the tree and release all framework state.
    /// </remarks>
    public sealed class Renderer : IDisposable
    {
        private readonly RebuildScheduler _scheduler;
        private readonly NodeExpander _expander;
        private readonly NativeAdapter _adapter;
        private Node? _rootNode;
        private bool _disposed;

        /// <summary>
        /// Initializes a new <see cref="Renderer"/>.
        /// </summary>
        public Renderer()
        {
            _adapter = new NativeAdapter();
            _scheduler = new RebuildScheduler();
            _expander = new NodeExpander(_scheduler, _adapter);
        }

        /// <summary>
        /// Mounts <paramref name="root"/> into <paramref name="container"/>, building
        /// the initial element tree and producing the corresponding native
        /// <see cref="VisualElement"/> subtree as children of <paramref name="container"/>.
        /// </summary>
        /// <param name="root">The root element of the framework tree.</param>
        /// <param name="container">
        /// The UIToolkit <see cref="VisualElement"/> that will host the rendered output.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="root"/> or <paramref name="container"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the renderer is already mounted.
        /// </exception>
        public void Mount(Element? root, VisualElement container)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            if (_rootNode != null)
            {
                throw new InvalidOperationException(
                    "Renderer is already mounted. Call Dispose before mounting again."
                );
            }

            var rootElement = new RootElement { Child = root };
            _adapter.SetRootContainer(container);
            _rootNode = _expander.Mount(rootElement, null);
        }

        /// <summary>
        /// Advances all active animation controllers by <paramref name="deltaTime"/> seconds,
        /// then flushes all pending rebuilds scheduled by <see cref="State.SetState"/>
        /// calls since the last tick. Call this from <c>MonoBehaviour.Update</c>,
        /// passing <c>UnityEngine.Time.deltaTime</c> as the argument.
        /// </summary>
        /// <param name="deltaTime">
        /// Elapsed time in seconds since the previous frame. Must be non-negative.
        /// </param>
        public void Tick(float deltaTime)
        {
            AnimationSystem.Tick(deltaTime);

            if (_scheduler.HasDirtyNodes)
            {
                _scheduler.Flush(_expander);
            }
        }

        /// <summary>
        /// Unmounts the element tree and releases all framework state.
        /// Safe to call multiple times; subsequent calls are no-ops.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            if (_rootNode != null)
            {
                _expander.Unmount(_rootNode);
                _rootNode = null;
            }

            _disposed = true;
        }

        /// <summary>
        /// Returns the native <see cref="VisualElement"/> associated with the node
        /// for the given element, or <c>null</c> if not found. Intended for testing
        /// and debugging only.
        /// </summary>
        /// <param name="node">The node whose native element to retrieve.</param>
        internal VisualElement? GetNativeElement(Node node)
        {
            return _adapter.GetNativeElement(node);
        }

        private sealed class NativeAdapter : IRenderAdapter
        {
            private readonly Dictionary<Node, RenderHandle> _handles = new();
            private VisualElement? _rootContainer;

            internal void SetRootContainer(VisualElement container)
            {
                _rootContainer = container;
            }

            internal VisualElement? GetNativeElement(Node node)
            {
                return _handles.TryGetValue(node, out var handle) ? handle.NativeElement : null;
            }

            public void OnMounted(Node node)
            {
                var native = CreateNativeElement(node);
                var handle = new RenderHandle(node, native);
                _handles[node] = handle;
                AttachChildrenToNative(node, native);
                AttachToParent(node, native);
            }

            private void AttachChildrenToNative(Node node, VisualElement native)
            {
                foreach (var child in node.Children)
                {
                    if (!_handles.TryGetValue(child, out var childHandle))
                    {
                        continue;
                    }

                    native.Add(childHandle.NativeElement);

                    if (node.Element is Stack)
                    {
                        ElementPainter.ApplyAsStackChild(childHandle.NativeElement);
                    }
                }
            }

            public void OnUnmounting(Node node)
            {
                if (!_handles.TryGetValue(node, out var handle))
                {
                    return;
                }

                handle.NativeElement.RemoveFromHierarchy();
                _handles.Remove(node);
            }

            public void OnRebuilt(Node node)
            {
                if (!_handles.TryGetValue(node, out var handle))
                {
                    return;
                }

                ElementPainter.Paint(node.Element, handle.NativeElement, ResolveTheme(node));
            }

            private VisualElement CreateNativeElement(Node node)
            {
                return ElementPainter.CreateNative(node.Element, ResolveTheme(node));
            }

            private static Theme ResolveTheme(Node node)
            {
                var current = node.Parent;
                while (current != null)
                {
                    if (current.Element is ThemeProvider provider)
                    {
                        return provider.Theme;
                    }

                    current = current.Parent;
                }

                return Theme.Default;
            }

            private void AttachToParent(Node node, VisualElement native)
            {
                if (node.Parent == null)
                {
                    _rootContainer?.Add(native);
                    return;
                }

                if (!_handles.TryGetValue(node.Parent, out var parentHandle))
                {
                    return;
                }

                parentHandle.NativeElement.Add(native);

                if (node.Parent.Element is Stack)
                {
                    ElementPainter.ApplyAsStackChild(native);
                }
            }
        }
    }
}