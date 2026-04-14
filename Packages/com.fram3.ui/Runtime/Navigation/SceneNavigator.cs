#nullable enable
using System;
using Fram3.UI.Navigation.Internal;

namespace Fram3.UI.Navigation
{
    /// <summary>
    /// Provides static methods for navigating between Unity scenes.
    /// Use <see cref="GoTo"/> to replace the current scene asynchronously.
    /// </summary>
    /// <remarks>
    /// <see cref="SceneNavigator"/> is a global, stateless API. It does not require a
    /// build context or a navigator element in the element tree. Call it from anywhere --
    /// button callbacks, cubit methods, or game logic.
    /// </remarks>
    public static class SceneNavigator
    {
        private static ISceneAdapter _adapter = CreateDefaultAdapter();

        /// <summary>
        /// Begins loading the scene identified by <paramref name="sceneName"/> using
        /// <c>LoadSceneMode.Single</c>, replacing the current scene. Returns an
        /// <see cref="SceneOperation"/> that tracks progress and fires a
        /// <see cref="SceneOperation.Completed"/> event when the scene is active.
        /// </summary>
        /// <param name="sceneName">
        /// The name of the scene to load. Must match a scene name registered in
        /// Unity's Build Settings.
        /// </param>
        /// <returns>
        /// An <see cref="SceneOperation"/> whose <see cref="SceneOperation.Progress"/>
        /// property advances from 0 to 1 during the load.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="sceneName"/> is null or empty.
        /// </exception>
        public static SceneOperation GoTo(string sceneName)
        {
            return string.IsNullOrEmpty(sceneName)
                ? throw new ArgumentNullException(nameof(sceneName))
                : _adapter.LoadAsync(sceneName);
        }

        /// <summary>
        /// Replaces the scene adapter used by <see cref="GoTo"/>. Intended for testing only.
        /// Pass <c>null</c> to restore the default Unity adapter.
        /// </summary>
        /// <param name="adapter">The adapter to use, or <c>null</c> to restore the default.</param>
        internal static void SetAdapter(ISceneAdapter? adapter)
        {
            _adapter = adapter ?? CreateDefaultAdapter();
        }

        private static ISceneAdapter CreateDefaultAdapter()
        {
#if FRAM3_PURE_TESTS
            return new NullSceneAdapter();
#else
            return new UnitySceneAdapter();
#endif
        }
    }
}