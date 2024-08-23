using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class LoadingProfile : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    private float loadingAmount;

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
    }

    public void UpadteLoadingUI(float loadingAmount)
    {
        if (!isLoading) { return; }

        if (loadingAmount >= 1f)
        {
            isLoading = false;
            _loadingText.SetText("Ready!!");
            FlipAnimation();
        }
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

    private void Update()
    {
        UpadteLoadingUI(loadingAmount); // 테스트

    }
}
