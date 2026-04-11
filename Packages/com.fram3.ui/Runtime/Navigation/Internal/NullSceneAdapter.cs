#nullable enable
#if FRAM3_PURE_TESTS
namespace Fram3.UI.Navigation.Internal
{
    internal sealed class NullSceneAdapter : ISceneAdapter
    {
        public FSceneOperation LoadAsync(string sceneName)
        {
            var operation = new FSceneOperation();
            operation.Complete();
            return operation;
        }
    }
}
#endif