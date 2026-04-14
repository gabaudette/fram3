#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Rendering;

namespace Fram3.UI.Storybook
{
    /// <summary>
    /// Mounts the Fram3 Storybook element tree into the UIDocument on this GameObject.
    /// Attach this component to the same GameObject as a UIDocument that has a PanelSettings
    /// asset assigned.
    /// </summary>
    public sealed class StorybookRunner : AppRoot
    {
        protected override Element CreateRoot() => StorybookApp.Create();
    }
}