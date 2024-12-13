using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : Singleton<SceneLoader>
{
    [Header("Loading Screen UI Elements")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBar; // Optional: Show loading progress

    #region Scene Transition

    // Public method to load a scene by name
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    // Public method to reload the current scene
    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadSceneAsync(currentSceneName));
    }

    #endregion

    #region Asynchronous Scene Loading

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Show the loading screen UI (if set)
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            if (loadingBar != null)
            {
                loadingBar.value = progress;
            }

            if (asyncOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }


    #endregion
}
