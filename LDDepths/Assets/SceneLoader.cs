using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static Action StartedSceneLoad = delegate {};
    public void OpenScene(string SceneName)
    {
        StartedSceneLoad.Invoke();
        StartCoroutine(LoadAsyncScene(SceneName));
    }
    IEnumerator LoadAsyncScene(string SceneName)
    {
        yield return new WaitForSeconds(.3f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
