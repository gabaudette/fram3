#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Theme
{
    /// <summary>
    /// Provides access to the nearest <see cref="FThemeProvider"/> in the element tree.
    /// When no provider is found, <see cref="FTheme.Default"/> is returned.
    /// </summary>
    public static class FThemeConsumer
    {
        /// <summary>
        /// Returns the <see cref="FTheme"/> from the nearest <see cref="FThemeProvider"/>
        /// ancestor. Falls back to <see cref="FTheme.Default"/> if no provider is in scope.
        /// </summary>
        /// <param name="context">The current build context.</param>
        /// <returns>The nearest theme, or <see cref="FTheme.Default"/> if none is found.</returns>
        public static FTheme Of(FBuildContext context)
        {
            var provider = context.FindInherited<FThemeProvider>();
            return provider?.Theme ?? FTheme.Default;
        }
    }
}