#nullable enable
#if !FRAM3_PURE_TESTS
using UnityEngine;
using UnityEngine.UIElements;

namespace Fram3.UI.Rendering
{
    /// <summary>
    /// Base <c>MonoBehaviour</c> for mounting a Fram3 element tree into a
    /// <see cref="UIDocument"/>. Attach a subclass to the same GameObject as a
    /// <c>UIDocument</c> that has a <c>PanelSettings</c> asset assigned, then
    /// override <see cref="CreateRoot"/> to return the root element of your UI.
    /// </summary>
    /// <remarks>
    /// Lifecycle management -- mounting on <c>Start</c>, ticking on <c>Update</c>,
    /// and disposing on <c>OnDestroy</c> -- is handled automatically. Override the
    /// respective Unity messages if you need additional behaviour; call
    /// <c>base.Start()</c>, <c>base.Update()</c>, and <c>base.OnDestroy()</c> to
    /// preserve the renderer lifecycle.
    /// </remarks>
    [RequireComponent(typeof(UIDocument))]
    public abstract class FMonoBehaviour : MonoBehaviour
    {
        private FRenderer? _renderer;

        /// <summary>
        /// Returns the root <see cref="Core.FElement"/> to mount into the
        /// <see cref="UIDocument"/>. Called once during <c>Start</c>.
        /// </summary>
        protected abstract Core.FElement CreateRoot();

        /// <summary>
        /// Mounts the element tree returned by <see cref="CreateRoot"/> into the
        /// <see cref="UIDocument"/> on this GameObject.
        /// </summary>
        protected virtual void Start()
        {
            var document = GetComponent<UIDocument>();
            var root = document.rootVisualElement;
            root.style.width = Length.Percent(100f);
            root.style.height = Length.Percent(100f);
            root.style.flexDirection = FlexDirection.Column;
            _renderer = new FRenderer();
            _renderer.Mount(CreateRoot(), root);
        }

        /// <summary>
        /// Ticks the renderer to flush pending rebuilds and advance animations.
        /// </summary>
        protected virtual void Update()
        {
            _renderer?.Tick(Time.deltaTime);
        }

        /// <summary>
        /// Disposes the renderer and unmounts the element tree.
        /// </summary>
        protected virtual void OnDestroy()
        {
            _renderer?.Dispose();
            _renderer = null;
        }
    }
}
#endif
