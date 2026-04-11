#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// Describes a single tab in an <see cref="FTabView"/>.
    /// Contains the tab's label and the content element displayed when the tab is active.
    /// </summary>
    public sealed class FTab
    {
        /// <summary>The text label displayed in the tab bar.</summary>
        public string Label { get; }

        /// <summary>The element rendered when this tab is selected.</summary>
        public FElement Content { get; }

        /// <summary>
        /// Creates an <see cref="FTab"/>.
        /// </summary>
        /// <param name="label">The text label for the tab header. Must not be null.</param>
        /// <param name="content">The element rendered when this tab is active. Must not be null.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="label"/> or <paramref name="content"/> is null.
        /// </exception>
        public FTab(string label, FElement content)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }
    }
}
