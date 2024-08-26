using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingProfile : MonoBehaviour
{
    private LoadingText _loadingText;
    private Image _profileImage;  // 나중에 캐릭터 바꾸는거 넣으면 쓰셈

    private bool isLoading;

    private float originYPos;

    private void Awake()
    {
        _loadingText = transform.Find("Profile_LoadingText").GetComponent<LoadingText>();
        _profileImage = transform.Find("Profile_Image").GetComponent<Image>();
    }

    private void Start()
    {
        isLoading = true;
        originYPos = transform.position.y;

        SceneController.LoadAddressableScene(this, SetReady);
    }

    private void SetReady()
    {
        Debug.Log("SetReady");
        _loadingText.SetText("Ready!!");
        FlipAnimation();
    }

    private void FlipAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(transform.DOLocalMoveY(originYPos + 100, 0.5f))
            .Append(transform.DOLocalMoveY(originYPos, 0.5f))
            .Insert(0f, transform.DORotate(new Vector3(0, 720f, 0), 1.25f, RotateMode.FastBeyond360))
            .AppendCallback(() => sequence.Kill());
    }
}
