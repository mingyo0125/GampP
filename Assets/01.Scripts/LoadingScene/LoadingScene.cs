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

    private void Start()
    {
        SceneController.LoadAddressableScene(this, SetReady);
    }

    private void SetReady()
    {
        _loadingText.StopTextAnimation();
        _loadingText.SetText();
        _loadingText.FlipAnimation();
    }
}
