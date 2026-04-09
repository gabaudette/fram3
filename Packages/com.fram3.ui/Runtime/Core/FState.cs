#nullable enable
using System;
using Fram3.UI.Core.Internal;

namespace Fram3.UI.Core
{
    /// <summary>
    /// Holds the mutable state for a FStatefulElement.
    /// State objects are created once and persist across rebuilds of their
    /// associated element. Use SetState to mutate state and trigger a rebuild
    /// of the subtree rooted at the associated element.
    /// </summary>
    public abstract class FState
    {
        private FNode? _node;

        /// <summary>
        /// The element description currently associated with this state.
        /// Updated by the framework when the parent rebuilds and provides
        /// a new element of the same type and key.
        /// </summary>
        protected FStatefulElement? Element => _node?.Element as FStatefulElement;

        /// <summary>
        /// The build context providing access to the element's position
        /// in the tree and ancestor lookup capabilities.
        /// Available after InitState has been called.
        /// </summary>
        public FBuildContext? Context => _node?.Context;

        /// <summary>
        /// Whether this state is currently mounted in the element tree.
        /// </summary>
        public bool Mounted { get; private set; }

        /// <summary>
        /// Called exactly once after the state is created and attached to the tree.
        /// Override this to perform one-time initialization such as subscribing
        /// to external data sources or creating animation controllers.
        /// The Context and Element properties are available when this is called.
        /// </summary>
        public virtual void InitState()
        {
        }

        /// <summary>
        /// Describes the UI subtree for the associated stateful element.
        /// Called by the framework whenever this state is marked dirty.
        /// </summary>
        /// <param name="context">
        /// The build context providing access to the element's position
        /// in the tree and ancestor lookup capabilities.
        /// </param>
        /// <returns>An element describing the UI subtree.</returns>
        public abstract FElement Build(FBuildContext context);

        /// <summary>
        /// Called when the associated element is replaced with a new element
        /// of the same type and key. Override this to respond to configuration
        /// changes from the parent, such as updating internal state when
        /// new parameters are received.
        /// </summary>
        /// <param name="oldElement">The previous element description.</param>
        public virtual void DidUpdateElement(FStatefulElement oldElement)
        {
        }

        /// <summary>
        /// Called when the element is removed from the tree.
        /// Override this to release resources such as unsubscribing from
        /// data sources, disposing animation controllers, or cancelling
        /// pending operations.
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// Schedules a rebuild of the subtree rooted at this state's element.
        /// Call this within the provided action to mutate state fields,
        /// then the framework will re-invoke Build to produce an updated subtree.
        /// </summary>
        /// <param name="action">
        /// An action that mutates the state. Executed synchronously before
        /// the rebuild is scheduled.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if called when the state is not mounted.
        /// </exception>
        public void SetState(Action? action)
        {
            if (!Mounted)
            {
                throw new InvalidOperationException(
                    "SetState called on an unmounted state. " +
                    "This can happen if SetState is called after Dispose."
                );
            }

            action?.Invoke();
            _node?.MarkDirty();
        }

        internal void Mount(FNode node)
        {
            _node = node ?? throw new ArgumentNullException(nameof(node));
            Mounted = true;
        }

        internal void Unmount()
        {
            Mounted = false;
            _node = null;
        }
    }

    /// <summary>
    /// A strongly-typed state class that provides typed access to its associated element.
    /// Inherit from this class to create state for a specific FStatefulElement subclass,
    /// giving convenient access to the element's properties without casting.
    /// </summary>
    /// <typeparam name="T">The type of FStatefulElement this state is associated with.</typeparam>
    public abstract class FState<T> : FState where T : FStatefulElement
    {
        /// <summary>
        /// The strongly-typed element description currently associated with this state.
        /// </summary>
        public new T? Element => base.Element as T;
    }
}
