#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Navigation
{
    /// <summary>
    /// Internal stateless element that evaluates a route builder function.
    /// Created by <see cref="NavigatorState"/> each time a route is pushed or the initial
    /// route is mounted. Not intended for direct use.
    /// </summary>
    internal sealed class RouteElement : StatelessElement
    {
        private readonly Func<BuildContext, Element> _builder;

        /// <summary>
        /// The optional arguments that were passed to <see cref="NavigatorHandle.Push"/>.
        /// Available via <c>context.GetInherited&lt;NavigatorScope&gt;().Navigator</c>
        /// but more commonly passed through application-level state.
        /// </summary>
        internal object? Arguments { get; }

        internal RouteElement(
            Func<BuildContext, Element> builder,
            object? arguments,
            Key? key = null) : base(key)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            Arguments = arguments;
        }

        public override Element Build(BuildContext context)
        {
            return _builder(context);
        }
    }
}