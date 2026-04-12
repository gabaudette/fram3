#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Theme
{
    /// <summary>
    /// Makes an <see cref="FTheme"/> available to all descendants. Descendants retrieve
    /// the current theme via <see cref="FThemeConsumer.Of"/>.
    /// When the parent rebuilds with a different <see cref="FTheme"/> instance, all
    /// dependents are automatically scheduled for a rebuild.
    /// </summary>
    public sealed class FThemeProvider : FInheritedElement
    {
        /// <summary>The theme made available to descendants.</summary>
        public FTheme Theme { get; }

        /// <summary>
        /// Creates an <see cref="FThemeProvider"/> that supplies <paramref name="theme"/>
        /// to the subtree rooted at <paramref name="child"/>.
        /// </summary>
        /// <param name="theme">The theme to provide. Must not be null.</param>
        /// <param name="child">The child subtree that can access the theme. Must not be null.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when theme or child is null.</exception>
        public FThemeProvider(FTheme theme, FElement child, FKey? key = null) : base(key)
        {
            Theme = theme ?? throw new ArgumentNullException(nameof(theme));
            Child = child ?? throw new ArgumentNullException(nameof(child));
        }

        /// <inheritdoc/>
        public override bool UpdateShouldNotify(FInheritedElement oldElement)
        {
            if (oldElement is not FThemeProvider old)
            {
                return true;
            }

            return !Equals(Theme, old.Theme);
        }
    }
}