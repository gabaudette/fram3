#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Styling;
using StylingTheme = Fram3.UI.Styling.Theme;

namespace Fram3.UI.Elements.Theme
{
    /// <summary>
    /// Makes an <see cref="StylingTheme"/> available to all descendants. Descendants retrieve
    /// the current theme via <see cref="ThemeConsumer.Of"/>.
    /// When the parent rebuilds with a different <see cref="StylingTheme"/> instance, all
    /// dependents are automatically scheduled for a rebuild.
    /// </summary>
    public sealed class ThemeProvider : InheritedElement
    {
        /// <summary>The theme made available to descendants.</summary>
        public StylingTheme Theme { get; }

        /// <summary>
        /// Creates an <see cref="ThemeProvider"/> that supplies <paramref name="theme"/>
        /// to the subtree rooted at <paramref name="child"/>.
        /// </summary>
        /// <param name="theme">The theme to provide. Must not be null.</param>
        /// <param name="child">The child subtree that can access the theme. Must not be null.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when theme or child is null.</exception>
        public ThemeProvider(StylingTheme theme, Element child, Key? key = null) : base(key)
        {
            Theme = theme ?? throw new ArgumentNullException(nameof(theme));
            Child = child ?? throw new ArgumentNullException(nameof(child));
        }

        /// <inheritdoc/>
        public override bool UpdateShouldNotify(InheritedElement oldElement)
        {
            if (oldElement is not ThemeProvider old)
            {
                return true;
            }

            return !Equals(Theme, old.Theme);
        }
    }
}