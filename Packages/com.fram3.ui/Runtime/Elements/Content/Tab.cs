#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// Describes a single tab in an <see cref="TabView"/>.
    /// Contains the tab's label and the content element displayed when the tab is active.
    /// </summary>
    public sealed class Tab
    {
        /// <summary>The text label displayed in the tab bar.</summary>
        public string Label { get; }

        /// <summary>The element rendered when this tab is selected.</summary>
        public Element Content { get; }

        /// <summary>
        /// Creates an <see cref="Tab"/>.
        /// </summary>
        /// <param name="label">The text label for the tab header. Must not be null.</param>
        /// <param name="content">The element rendered when this tab is active. Must not be null.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="label"/> or <paramref name="content"/> is null.
        /// </exception>
        public Tab(string label, Element content)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }
    }
}