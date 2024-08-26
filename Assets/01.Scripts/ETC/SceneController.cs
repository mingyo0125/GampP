using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController
{
    private static string nextScene;
    public static string NextScene => nextScene;

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void SetNextLoadGameScene(string sceneName)
    {
        nextScene = sceneName;
    }

    public static void LoadAddressableScene(MonoBehaviour gameObject, Action loadingFinAction = null)
    {
        gameObject.StartCoroutine(LoadAddressableSceneCorou(loadingFinAction));
    }

    private static IEnumerator LoadAddressableSceneCorou(Action loadingFinAction = null)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(NextScene);
        op.allowSceneActivation = false;
        bool isLoading = true;
        while (!op.isDone)
        {
            yield return new WaitForEndOfFrame();
            Debug.Log(op.progress);
            // 로딩이 거의 완료되었으면 SetReady 호출
            if (op.progress >= 0.9f && isLoading)
            {
                loadingFinAction?.Invoke();
                isLoading = false;

                yield return new WaitForSeconds(2f);
                // 이거는 모든 유저 다 되면 되도록 바꾸고
                UIManager.Instance.SceneFadeIn(() => op.allowSceneActivation = true);
            }
        }
    }
}
