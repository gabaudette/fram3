#nullable enable
#if !FRAM3_PURE_TESTS
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fram3.UI.Navigation.Internal
{
    internal sealed class UnitySceneAdapter : ISceneAdapter
    {
        public FSceneOperation LoadAsync(string sceneName)
        {
            var operation = new FSceneOperation();
            var driver = new GameObject("Fram3.SceneLoadDriver").AddComponent<SceneLoadDriver>();
            driver.Run(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single)!, operation);
            return operation;
        }

        private sealed class SceneLoadDriver : MonoBehaviour
        {
            internal void Run(AsyncOperation asyncOperation, FSceneOperation operation)
            {
                StartCoroutine(Track(asyncOperation, operation));
            }

            private IEnumerator Track(AsyncOperation asyncOperation, FSceneOperation operation)
            {
                while (!asyncOperation.isDone)
                {
                    operation.Progress = asyncOperation.progress;
                    yield return null;
                }

                Destroy(gameObject);
                operation.Complete();
            }
        }
    }
}
#endif