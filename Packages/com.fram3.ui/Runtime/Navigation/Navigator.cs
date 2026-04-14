#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;

namespace Fram3.UI.Navigation
{
    /// <summary>
    /// A stack-based in-scene navigator. Maintains a stack of named routes and renders the
    /// topmost route as its content. Descendants can push and pop routes by retrieving the
    /// <see cref="NavigatorHandle"/> via
    /// <c>context.GetInherited&lt;NavigatorScope&gt;().Navigator</c>.
    /// </summary>
    /// <remarks>
    /// Routes are declared as a dictionary mapping route names to builder functions.
    /// The builder receives the <see cref="BuildContext"/> at the time the route is built
    /// and an optional arguments object passed at push time.
    /// The navigator renders only one route at a time -- the top of the stack.
    /// </remarks>
    public sealed class Navigator : StatefulElement
    {
        /// <summary>
        /// The registered routes. Each key is a route name; each value is a builder that
        /// returns the element tree for that route.
        /// </summary>
        public IReadOnlyDictionary<string, Func<BuildContext, Element>> Routes { get; }

        /// <summary>
        /// The name of the route displayed when the navigator is first mounted.
        /// Must be a key present in <see cref="Routes"/>.
        /// </summary>
        public string InitialRoute { get; }

        /// <summary>
        /// Creates an <see cref="Navigator"/> with the given routes and initial route.
        /// </summary>
        /// <param name="routes">
        /// A dictionary mapping route names to builder functions. Must not be null or empty.
        /// </param>
        /// <param name="initialRoute">
        /// The name of the route to display first. Must be present in <paramref name="routes"/>.
        /// </param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="routes"/> or <paramref name="initialRoute"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="routes"/> is empty or
        /// <paramref name="initialRoute"/> is not a registered route name.
        /// </exception>
        public Navigator(
            IReadOnlyDictionary<string, Func<BuildContext, Element>> routes,
            string initialRoute,
            Key? key = null
        ) : base(key)
        {
            if (routes == null)
            {
                throw new ArgumentNullException(nameof(routes));
            }

            if (routes.Count == 0)
            {
                throw new ArgumentException("Routes must not be empty.", nameof(routes));
            }

            if (initialRoute == null)
            {
                throw new ArgumentNullException(nameof(initialRoute));
            }

            if (!routes.ContainsKey(initialRoute))
            {
                throw new ArgumentException(
                    $"Initial route '{initialRoute}' is not registered in routes.",
                    nameof(initialRoute)
                );
            }

            Routes = routes;
            InitialRoute = initialRoute;
        }

        /// <inheritdoc/>
        public override State CreateState()
        {
            return new NavigatorState();
        }
    }
}