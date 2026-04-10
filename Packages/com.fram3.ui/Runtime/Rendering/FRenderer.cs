#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Rendering.Internal;
using UnityEngine.UIElements;

namespace Fram3.UI.Rendering
{
    /// <summary>
    /// The primary entry point for mounting and driving a Fram3 element tree
    /// into a UIToolkit <see cref="VisualElement"/> container. Create one
    /// <see cref="FRenderer"/> per UIDocument root, call <see cref="Mount"/>
    /// once to build the initial tree, then call <see cref="Tick"/> from
    /// <c>MonoBehaviour.Update</c> on every frame to flush pending rebuilds.
    /// </summary>
    /// <remarks>
    /// <see cref="FRenderer"/> owns the entire lifecycle of the node tree. Call
    /// <see cref="Dispose"/> when the owning <c>MonoBehaviour</c> is destroyed to
    /// unmount the tree and release all framework state.
    /// </remarks>
    public sealed class FRenderer : IDisposable
    {
        private readonly FRebuildScheduler _scheduler;
        private readonly FNodeExpander _expander;
        private readonly NativeAdapter _adapter;
        private FNode? _rootNode;
        private bool _disposed;

        /// <summary>
        /// Initializes a new <see cref="FRenderer"/>.
        /// </summary>
        public FRenderer()
        {
            _adapter = new NativeAdapter();
            _scheduler = new FRebuildScheduler();
            _expander = new FNodeExpander(_scheduler, _adapter);
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
        public void Mount(FElement? root, VisualElement container)
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
                    "FRenderer is already mounted. Call Dispose before mounting again.");
            }

            var rootElement = new FRootElement { Child = root };
            _adapter.SetRootContainer(container);
            _rootNode = _expander.Mount(rootElement, null);
        }

        /// <summary>
        /// Flushes all pending rebuilds scheduled by <see cref="FState.SetState"/>
        /// calls since the last tick. Call this from <c>MonoBehaviour.Update</c>.
        /// </summary>
        public void Tick()
        {
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
        internal VisualElement? GetNativeElement(FNode node)
        {
            return _adapter.GetNativeElement(node);
        }

        private sealed class NativeAdapter : IRenderAdapter
        {
            private readonly Dictionary<FNode, FRenderHandle> _handles = new();
            private VisualElement? _rootContainer;

            internal void SetRootContainer(VisualElement container)
            {
                _rootContainer = container;
            }

            internal VisualElement? GetNativeElement(FNode node)
            {
                return _handles.TryGetValue(node, out var handle) ? handle.NativeElement : null;
            }

            public void OnMounted(FNode node)
            {
                var native = CreateNativeElement(node);
                var handle = new FRenderHandle(node, native);
                _handles[node] = handle;
                AttachToParent(node, native);
            }

            public void OnUnmounting(FNode node)
            {
                if (!_handles.TryGetValue(node, out var handle))
                {
                    return;
                }

                handle.NativeElement.RemoveFromHierarchy();
                _handles.Remove(node);
            }

            public void OnRebuilt(FNode node)
            {
                if (!_handles.TryGetValue(node, out var handle))
                {
                    return;
                }

                FElementPainter.Paint(node.Element, handle.NativeElement);
            }

            private VisualElement CreateNativeElement(FNode node)
            {
                return FElementPainter.CreateNative(node.Element);
            }

            private void AttachToParent(FNode node, VisualElement native)
            {
                if (node.Parent == null)
                {
                    _rootContainer?.Add(native);
                    return;
                }

                if (_handles.TryGetValue(node.Parent, out var parentHandle))
                {
                    parentHandle.NativeElement.Add(native);
                }
            }
        }
    }
}