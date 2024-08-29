using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager : MonoSingleTon<SceneManager>
{
    private string nextScene;
    public string NextScene => nextScene;

    // 로딩 상태를 추적하는 변수
    private HashSet<ulong> clientsReady = new HashSet<ulong>();

    AsyncOperation _asyncOperation;

    public void SetNextLoadGameScene(string sceneName)
    {
        nextScene = sceneName;
    }

    public void LoadScene(string sceneName)
    {
        NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void LoadAddressableScene(MonoBehaviour gameObject, Action loadingFinAction = null)
    {
        gameObject.StartCoroutine(LoadAddressableSceneCorou(loadingFinAction));
    }

    private IEnumerator LoadAddressableSceneCorou(Action loadingFinAction = null)
    {
        _asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextScene);
        _asyncOperation.allowSceneActivation = false;
        bool isLoading = true;
        while (!_asyncOperation.isDone)
        {
            yield return new WaitForEndOfFrame();

            if (_asyncOperation.progress >= 0.9f && isLoading)
            {
                yield return new WaitForSeconds(2f);
                loadingFinAction?.Invoke();
                isLoading = false;

                Debug.Log(LobbyManager.Instance.ClientInfo.IsServer);
                // 클라이언트가 준비 상태를 서버에 알림
                if (!LobbyManager.Instance.ClientInfo.IsServer)
                {
                    NotifyClientReadyServerRpc();
                }

                yield return new WaitForSeconds(2f);

                Debug.Log($"{clientsReady.Count}: {NetworkManager.Singleton.ConnectedClientsList.Count}");

                if (LobbyManager.Instance.ClientInfo.IsServer)
                {
                    Debug.Log(NetworkManager.Singleton.ConnectedClientsList.Count);
                    // 모든 클라이언트가 준비되었는지 확인
                    if (clientsReady.Count == NetworkManager.Singleton.ConnectedClientsList.Count - 1)
                    {
                        UIManager.Instance.SceneFadeIn(() =>
                        {
                            ActiveAllowSceneActivationClientRpc();
                            LoadScene(NextScene);
                        });
                    }
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void NotifyClientReadyServerRpc(ServerRpcParams rpcParams = default)
    {
        Debug.Log("?!");
        clientsReady.Add(rpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    private void ActiveAllowSceneActivationClientRpc()
    {
        _asyncOperation.allowSceneActivation = true;
        _asyncOperation = null;
    }
}
