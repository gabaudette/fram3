#nullable enable
namespace Fram3.UI.Navigation.Internal
{
    /// <summary>
    /// Abstraction over Unity's scene loading API, allowing test code to substitute
    /// a controllable stub in place of the real <c>SceneManager</c>.
    /// </summary>
    internal interface ISceneAdapter
    {
        /// <summary>
        /// Begins loading the scene identified by <paramref name="sceneName"/> using
        /// <c>LoadSceneMode.Single</c> and returns an <see cref="FSceneOperation"/>
        /// that tracks progress and completion.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        FSceneOperation LoadAsync(string sceneName);
    }
}
