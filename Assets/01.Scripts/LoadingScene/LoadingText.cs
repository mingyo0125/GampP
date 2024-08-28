using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private Coroutine _textAnimationCoroutine;
    private float originYPos;

    private void Start()
    {
        originYPos = transform.position.y;
    }

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();

        _textAnimationCoroutine = _text.TextAnimation(true);
    }

    public void SetText()
    {
        _text.SetText("Start!!");
    }

    public void FlipAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(transform.DOLocalMoveY(originYPos + 100, 0.5f))
            .Append(transform.DOLocalMoveY(originYPos, 0.5f))
            .Insert(0f, transform.DORotate(new Vector3(0, 720f, 0), 1.25f, RotateMode.FastBeyond360))
            .AppendCallback(() => sequence.Kill());
    }

    public void StopTextAnimation()
    {
        _text.StopCoroutine(_textAnimationCoroutine);
        _text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
}
