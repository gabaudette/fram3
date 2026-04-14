#nullable enable
namespace Fram3.UI.Navigation
{
    /// <summary>
    /// Provides imperative navigation operations to descendants in the element tree.
    /// Obtain an instance via <c>context.GetInherited&lt;NavigatorScope&gt;().Navigator</c>.
    /// </summary>
    public interface NavigatorHandle
    {
        /// <summary>
        /// Pushes a named route onto the navigation stack.
        /// The route must be registered in the enclosing <see cref="Navigator"/>.
        /// </summary>
        /// <param name="routeName">The name of the registered route to push.</param>
        /// <param name="arguments">Optional arguments forwarded to the route builder. May be null.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when <paramref name="routeName"/> is not registered in the enclosing navigator.
        /// </exception>
        void Push(string routeName, object? arguments = null);

        /// <summary>
        /// Removes the top route from the navigation stack.
        /// Does nothing when the stack contains only the initial route.
        /// </summary>
        void Pop();

        /// <summary>
        /// Returns <c>true</c> when the stack has more than one route and <see cref="Pop"/> would
        /// have an effect.
        /// </summary>
        bool CanPop { get; }
    }
}