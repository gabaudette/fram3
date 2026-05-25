#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Rendering;
using TMPro;
using UnityEngine;

namespace Fram3.UI.Storybook
{
    public sealed class StorybookRunner : AppRoot
    {
        [SerializeField] private TMP_FontAsset? _primaryFont;
        [SerializeField] private TMP_FontAsset? _displayFont;

        protected override Element CreateRoot() => StorybookApp.Create(_primaryFont, _displayFont);
    }
}