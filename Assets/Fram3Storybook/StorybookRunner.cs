#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Rendering;
using UnityEngine;

namespace Fram3.UI.Storybook
{
    public sealed class StorybookRunner : AppRoot
    {
        [SerializeField] private Font? _primaryFont;
        [SerializeField] private Font? _displayFont;

        protected override Element CreateRoot() => StorybookApp.Create(_primaryFont, _displayFont);
    }
}