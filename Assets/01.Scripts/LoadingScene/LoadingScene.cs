using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LoadingScene : NetworkBehaviour
{
    private LoadingText _loadingText;

    private void Awake()
    {
        _loadingText = FindAnyObjectByType<LoadingText>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        Debug.Log("OnNetworkSpawn");
        SceneManager.Instance.LoadAddressableScene(this, SetReady);
    }

    private void SetReady()
    {
        _loadingText.StopTextAnimation();
        _loadingText.SetText();
        _loadingText.FlipAnimation();
    }
}
