#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;

namespace Fram3.UI.Navigation
{
    /// <summary>
    /// Holds the mutable navigation stack for an <see cref="FNavigator"/>.
    /// Exposes push and pop operations that trigger a rebuild of the navigator subtree.
    /// Obtain an instance via <c>context.GetInherited&lt;FNavigatorScope&gt;().Navigator</c>.
    /// </summary>
    public sealed class FNavigatorState : FState<FNavigator>, FNavigatorHandle
    {
        private readonly List<FElement> _stack = new List<FElement>();

        /// <inheritdoc/>
        public override void InitState()
        {
            var initialRoute = Element!.InitialRoute;
            var builder = ResolveRoute(initialRoute, null);
            _stack.Add(BuildRoute(builder, null));
        }

        /// <inheritdoc/>
        public void Push(string routeName, object? arguments = null)
        {
            var builder = ResolveRoute(routeName, arguments);
            SetState(() => _stack.Add(BuildRoute(builder, arguments)));
        }

        /// <inheritdoc/>
        public void Pop()
        {
            if (!CanPop)
            {
                return;
            }

            SetState(() => _stack.RemoveAt(_stack.Count - 1));
        }

        /// <inheritdoc/>
        public bool CanPop => _stack.Count > 1;

        /// <inheritdoc/>
        public override FElement Build(FBuildContext context)
        {
            var currentPage = _stack[_stack.Count - 1];
            return new FNavigatorScope(this, currentPage);
        }

        private Func<FBuildContext, FElement> ResolveRoute(string routeName, object? arguments)
        {
            var routes = Element!.Routes;

            if (!routes.TryGetValue(routeName, out var builder))
            {
                throw new ArgumentException(
                    $"No route named '{routeName}' is registered in this FNavigator.",
                    nameof(routeName)
                );
            }

            return builder;
        }

        private FElement BuildRoute(Func<FBuildContext, FElement> builder, object? arguments)
        {
            return new FRouteElement(builder, arguments);
        }
    }
}