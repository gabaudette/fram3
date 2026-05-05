#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Rendering;

namespace Fram3.UI.Storybook
{
    public sealed class StorybookRunner : AppRoot
    {
        protected override Element CreateRoot() => StorybookApp.Create();
    }
}