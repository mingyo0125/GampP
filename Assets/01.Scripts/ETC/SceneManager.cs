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

    // �ε� ���¸� �����ϴ� ����
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
                // Ŭ���̾�Ʈ�� �غ� ���¸� ������ �˸�
                if (!LobbyManager.Instance.ClientInfo.IsServer)
                {
                    NotifyClientReadyServerRpc();
                }

                yield return new WaitForSeconds(2f);

                if (LobbyManager.Instance.ClientInfo.IsServer)
                {
                    // ��� Ŭ���̾�Ʈ�� �غ�Ǿ����� Ȯ��
                    if (clientsReady.Count == NetworkManager.Singleton.ConnectedClientsList.Count - 1)
                    {
                        ActiveAllowSceneActivationClientRpc();
                    }
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void NotifyClientReadyServerRpc(ServerRpcParams rpcParams = default)
    {
        clientsReady.Add(rpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    private void ActiveAllowSceneActivationClientRpc()
    {
        if (LobbyManager.Instance.ClientInfo.IsServer)
        {
            //CoroutineUtil.CallWaitForSeconds(0.8f, () =>
            //{

            //});

            UIManager.Instance.SceneFadeIn(() =>
            {
                _asyncOperation.allowSceneActivation = true;
                _asyncOperation = null;
                LoadScene(NextScene);
            });
        }
        else
        {
            UIManager.Instance.SceneFadeIn(() =>
            {
                _asyncOperation.allowSceneActivation = true;
                _asyncOperation = null;
            });
        }
    }
}
