#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Styling;
using StylingTheme = Fram3.UI.Styling.Theme;

namespace Fram3.UI.Elements.Theme
{
    /// <summary>
    /// Provides access to the nearest <see cref="ThemeProvider"/> in the element tree.
    /// When no provider is found, <see cref="StylingTheme.Default"/> is returned.
    /// </summary>
    public static class ThemeConsumer
    {
        /// <summary>
        /// Returns the <see cref="StylingTheme"/> from the nearest <see cref="ThemeProvider"/>
        /// ancestor. Falls back to <see cref="StylingTheme.Default"/> if no provider is in scope.
        /// </summary>
        /// <param name="context">The current build context.</param>
        /// <returns>The nearest theme, or <see cref="StylingTheme.Default"/> if none is found.</returns>
        public static StylingTheme Of(BuildContext context)
        {
            var provider = context.FindInherited<ThemeProvider>();
            return provider?.Theme ?? StylingTheme.Default;
        }
    }
}