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
    }
}
