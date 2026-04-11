#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Navigation
{
    /// <summary>
    /// Internal stateless element that evaluates a route builder function.
    /// Created by <see cref="FNavigatorState"/> each time a route is pushed or the initial
    /// route is mounted. Not intended for direct use.
    /// </summary>
    internal sealed class FRouteElement : FStatelessElement
    {
        private readonly Func<FBuildContext, FElement> _builder;

        /// <summary>
        /// The optional arguments that were passed to <see cref="FNavigatorHandle.Push"/>.
        /// Available via <c>context.GetInherited&lt;FNavigatorScope&gt;().Navigator</c>
        /// but more commonly passed through application-level state.
        /// </summary>
        internal object? Arguments { get; }

        internal FRouteElement(
            Func<FBuildContext, FElement> builder,
            object? arguments,
            FKey? key = null) : base(key)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            Arguments = arguments;
        }

        public override FElement Build(FBuildContext context)
        {
            return _builder(context);
        }
    }
}
