#nullable enable
using Fram3.UI.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

namespace Fram3.UI.Storybook
{
    /// <summary>
    /// Mounts the Fram3 Storybook element tree into the UIDocument on this GameObject.
    /// Attach this component to the same GameObject as a UIDocument that has a PanelSettings
    /// asset assigned. The UIDocument provides the root VisualElement container.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public sealed class StorybookRunner : MonoBehaviour
    {
        private FRenderer? _renderer;

        private void Start()
        {
            var document = GetComponent<UIDocument>();
            var root = document.rootVisualElement;
            root.style.width = Length.Percent(100f);
            root.style.height = Length.Percent(100f);
            root.style.flexDirection = FlexDirection.Column;
            _renderer = new FRenderer();
            _renderer.Mount(StorybookApp.Create(), root);
            StretchPassthroughWrappers(root);
        }

        private void Update()
        {
            _renderer?.Tick(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _renderer?.Dispose();
            _renderer = null;
        }

        /// <summary>
        /// The framework creates a plain unstyled <see cref="VisualElement"/> for every
        /// passthrough node in the tree (FRootElement, FThemeProvider, FStatefulElement
        /// wrappers, etc.). These nodes have no size and no flexGrow, so they collapse to
        /// zero and prevent child FExpanded nodes from filling the screen.
        ///
        /// This method walks from the container down through each first-only-child that is
        /// a plain <see cref="VisualElement"/> with no explicit size or flex set, and forces
        /// each such node to fill its parent. It stops as soon as it encounters a node that
        /// the painter has already styled (e.g. an FExpanded, FRow, FColumn, or any leaf).
        /// </summary>
        private static void StretchPassthroughWrappers(VisualElement container)
        {
            var current = container;
            while (current.childCount == 1)
            {
                var child = current[0];
                if (!IsPassthroughWrapper(child))
                {
                    break;
                }

                child.style.flexGrow = 1f;
                child.style.flexShrink = 1f;
                child.style.alignSelf = Align.Stretch;
                child.style.flexDirection = FlexDirection.Column;
                current = child;
            }
        }

        private static bool IsPassthroughWrapper(VisualElement element)
        {
            var type = element.GetType();
            return type == typeof(VisualElement)
                   && element.style.width == StyleKeyword.Null
                   && element.style.height == StyleKeyword.Null
                   && element.style.flexGrow == StyleKeyword.Null;
        }
    }
}