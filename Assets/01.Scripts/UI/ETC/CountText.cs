using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountText : MonoBehaviour
{
    private string[] textInfos = new string[4] { "3", "2", "1", "Go!" };

    TextMeshProUGUI _countTextl;

    [SerializeField]
    private float tweenTime, textActiveTime;

    private void Awake()
    {
        _countTextl = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        StartCoroutine(StartCount());
    }

    private IEnumerator StartCount()
    {
        Sequence sequence = DOTween.Sequence();

        for(int i = 0; i < textInfos.Length; i++)
        {
            string info = textInfos[i];

            _countTextl.SetText(info);
            _countTextl.transform.DOScale(Vector3.one, tweenTime).SetEase(Ease.OutBack);
            _countTextl.transform.rotation = Quaternion.identity;

            yield return new WaitForSeconds(textActiveTime);

            if (i == textInfos.Length - 1) { break; }

            sequence
                .Append(transform.DORotate(new Vector3(0.0f, 0.0f, -720.0f),
                                           tweenTime,
                                           RotateMode.FastBeyond360).SetEase(Ease.OutExpo))
                .Join(transform.DOScale(0.0f, tweenTime).SetEase(Ease.OutExpo))
                .OnComplete(() => sequence.Kill());

            yield return new WaitForSeconds(1 - textActiveTime);
        }

        _countTextl.transform.rotation = Quaternion.identity;

        sequence
            .Append(_countTextl.transform.DOScale(Vector3.zero, 1 - textActiveTime));
    }
}
