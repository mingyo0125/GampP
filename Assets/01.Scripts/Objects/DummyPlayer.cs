using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DummyPlayer : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerAnimator _playerAnimator;

    private bool isMove;

    private void Awake()
    {
        _playerAnimator = transform.Find("Visual").GetComponent<PlayerAnimator>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        isMove = false;
    }

    public void SetMove()
    {
        Vector3 playerOriginPos = transform.position;

        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOMoveY(playerOriginPos.y + 3, 0.35f).SetEase(Ease.OutQuad))
            .Join(transform.DORotate(new Vector3(0f, -90f, 0f), 0.7f))
            .Insert(0.35f, transform.DOMoveY(playerOriginPos.y, 0.35f).SetEase(Ease.InQuad))
            .AppendCallback(() =>
            {
                isMove = true;
                _playerAnimator.SetSpeed(10);
                CoroutineUtil.CallWaitForSeconds(1.5f, () =>
                {
                    UIManager.Instance.SceneFadeIn(() =>
                    {
                        SceneManager.Instance.SetNextLoadGameScene("City"); // 바꾸면 이것도 바꾸시고
                        SceneManager.Instance.LoadScene("CityLoadingScene"); // 이거 나중에 맵 선택하는걸로 바꾸셈
                    });
                });
            });
    }

    private void FixedUpdate()
    {
        if (!isMove) { return; }

        Vector3 moveVec = Vector3.right * Time.fixedDeltaTime * 10;

        _characterController.Move(moveVec);
    }
}
