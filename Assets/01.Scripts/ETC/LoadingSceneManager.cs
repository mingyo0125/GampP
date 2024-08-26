using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    static string nextScene;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene(nextScene);
    }

    public void SetProgress() // ���⼭ �ε� �ƴٴ°� �޾Ƽ� ���� �������
    {

    }

    //IEnumerator LoadSceneProgress()
    //{
    //    AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
    //    op.allowSceneActivation = false;

    //    float timer = 0f;
    //    while (!op.isDone)
    //    {
    //        yield return null;

    //        if (op.progress < 0.9f)
    //        {
    //            _progressBar.value = op.progress;
    //        }
    //        else
    //        {
    //            timer += Time.unscaledDeltaTime;
    //            _progressBar.value = Mathf.Lerp(0.9f, 1f, timer);
    //            if (_progressBar.value >= 1f)
    //            {
    //                op.allowSceneActivation = true;
    //                yield break;
    //            }
    //        }
    //    }
    //}
}
