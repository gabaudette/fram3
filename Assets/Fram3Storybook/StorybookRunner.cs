#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Rendering;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Fram3.UI.Storybook
{
    public sealed class StorybookRunner : AppRoot
    {
        [SerializeField] private FontAsset? _primaryFont;
        [SerializeField] private FontAsset? _displayFont;

        protected override Element CreateRoot() => StorybookApp.Create(_primaryFont, _displayFont);
    }
}