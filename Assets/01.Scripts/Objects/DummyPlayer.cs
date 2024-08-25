using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
            });
    }

    private void FixedUpdate()
    {
        if (!isMove) { return; }

        Vector3 moveVec = Vector3.right * Time.fixedDeltaTime * 10;

        _characterController.Move(moveVec);
    }
}
