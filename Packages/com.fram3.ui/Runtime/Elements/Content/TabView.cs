#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// A tabbed view that displays one tab content at a time, with a tab bar at the top.
    /// The tab bar is built from existing framework elements (<see cref="Row"/>,
    /// <see cref="Button"/>, <see cref="Column"/>); no native UIToolkit tab control is used.
    /// </summary>
    public sealed class TabView : StatefulElement
    {
        /// <summary>The list of tabs. Must not be null or empty.</summary>
        public IReadOnlyList<Tab> Tabs { get; }

        /// <summary>
        /// Callback invoked whenever the active tab changes.
        /// Receives the new tab index as its argument.
        /// </summary>
        public Action<int>? OnTabChanged { get; }

        /// <summary>The zero-based index of the tab that is initially selected.</summary>
        public int InitialIndex { get; }

        /// <summary>
        /// Creates an <see cref="TabView"/> element.
        /// </summary>
        /// <param name="tabs">The list of tabs to display. Must not be null or empty.</param>
        /// <param name="onTabChanged">Callback invoked when the active tab changes.</param>
        /// <param name="initialIndex">The index of the initially selected tab. Defaults to 0.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="tabs"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="tabs"/> is empty.</exception>
        public TabView(
            IReadOnlyList<Tab> tabs,
            Action<int>? onTabChanged = null,
            int initialIndex = 0,
            Key? key = null
        ) : base(key)
        {
            if (tabs == null)
            {
                throw new ArgumentNullException(nameof(tabs));
            }

            if (tabs.Count == 0)
            {
                throw new ArgumentException("Tabs must not be empty.", nameof(tabs));
            }

            Tabs = tabs;
            OnTabChanged = onTabChanged;
            InitialIndex = initialIndex;
        }

        /// <inheritdoc/>
        public override Fram3.UI.Core.State CreateState() => new TabViewState();

        private sealed class TabViewState : Fram3.UI.Core.State<TabView>
        {
            private int _selectedIndex;

            public override void InitState()
            {
                _selectedIndex = ClampIndex(Element!.InitialIndex, Element.Tabs.Count);
            }

            public override Element Build(BuildContext context)
            {
                var tabs = Element!.Tabs;
                var tabBarButtons = new Element[tabs.Count];

                for (var i = 0; i < tabs.Count; i++)
                {
                    var capturedIndex = i;
                    tabBarButtons[i] = new Button(
                        label: tabs[i].Label,
                        onPressed: () => SelectTab(capturedIndex)
                    );
                }

                return new Column
                {
                    Children = new Element[]
                    {
                        new Row { Children = tabBarButtons },
                        tabs[_selectedIndex].Content
                    }
                };
            }

            public override void DidUpdateElement(StatefulElement oldElement)
            {
                var newCount = Element!.Tabs.Count;
                _selectedIndex = ClampIndex(_selectedIndex, newCount);
            }

            private void SelectTab(int index)
            {
                if (index == _selectedIndex)
                {
                    return;
                }

                SetState(() => _selectedIndex = index);
                Element?.OnTabChanged?.Invoke(index);
            }

            private static int ClampIndex(int index, int count)
            {
                if (index < 0)
                {
                    return 0;
                }

                return index >= count ? count - 1 : index;
            }
        }
    }
}