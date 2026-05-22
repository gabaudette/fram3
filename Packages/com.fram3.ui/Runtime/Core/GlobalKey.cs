#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core.Internal;

namespace Fram3.UI.Core
{
    /// <summary>
    /// <status>live</status>
    /// A key that provides a stable handle to a mounted node, allowing imperative
    /// access to its <see cref="BuildContext"/> and <see cref="State"/> from anywhere
    /// in the application. Only one element with a given <see cref="GlobalKey"/> may
    /// exist in the tree at a time.
    /// </summary>
    public class GlobalKey : Key
    {
        private static readonly Dictionary<GlobalKey, Node> Registry = new();

        /// <summary>
        /// The <see cref="BuildContext"/> of the currently mounted node associated
        /// with this key, or null when the node is not mounted.
        /// </summary>
        public BuildContext? CurrentContext =>
            Registry.TryGetValue(this, out var node) ? node.Context : null;

        /// <summary>
        /// The <see cref="State"/> of the currently mounted stateful node associated
        /// with this key, or null when the node is not mounted or is stateless.
        /// </summary>
        public State? CurrentState =>
            Registry.TryGetValue(this, out var node) ? node.State : null;

        /// <summary>
        /// Registers <paramref name="node"/> under <paramref name="key"/> in the
        /// global registry. Called by <see cref="NodeExpander"/> after mount.
        /// </summary>
        internal static void Register(GlobalKey key, Node node)
        {
            Registry[key] = node;
        }

        /// <summary>
        /// Removes <paramref name="key"/> from the global registry.
        /// Called by <see cref="NodeExpander"/> before or during unmount.
        /// </summary>
        internal static void Unregister(GlobalKey key)
        {
            Registry.Remove(key);
        }

        public override bool Equals(Key? other) => ReferenceEquals(this, other);

        public override int GetHashCode() => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
    }

    /// <summary>
    /// A typed <see cref="GlobalKey"/> that exposes the node's state as
    /// <typeparamref name="TState"/> without requiring a manual cast.
    /// </summary>
    /// <typeparam name="TState">The concrete <see cref="State"/> type expected at the node.</typeparam>
    public sealed class GlobalKey<TState> : GlobalKey where TState : State
    {
        /// <summary>
        /// The state of the currently mounted node cast to <typeparamref name="TState"/>,
        /// or null when the node is not mounted or the state is of a different type.
        /// </summary>
        public new TState? CurrentState => base.CurrentState as TState;
    }
}