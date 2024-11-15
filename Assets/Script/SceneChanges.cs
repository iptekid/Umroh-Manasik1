using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanges : MonoBehaviour
{
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadAsyncScene(sceneName));
    }

    private IEnumerator LoadAsyncScene(string sceneName)
    {
        // Begin loading scene in background
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Optional: Prevent scene from activating immediately when ready
        asyncLoad.allowSceneActivation = false;

        // Wait until scene is almost ready
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // Activate the scene
        asyncLoad.allowSceneActivation = true;
    }

    // Optional: Load scene with progress callback
    public void LoadSceneWithProgress(string sceneName, System.Action<float> onProgressUpdate)
    {
        StartCoroutine(LoadAsyncSceneWithProgress(sceneName, onProgressUpdate));
    }

    private IEnumerator LoadAsyncSceneWithProgress(string sceneName, System.Action<float> onProgressUpdate)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            onProgressUpdate?.Invoke(asyncLoad.progress);
            yield return null;
        }

        onProgressUpdate?.Invoke(1f);
        asyncLoad.allowSceneActivation = true;
    }
}