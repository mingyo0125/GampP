using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    TextMeshProUGUI _text;

    private Coroutine _textAnimationCoroutine;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();

        _textAnimationCoroutine = _text.TextAnimation(true);
    }

    public void StopTextAnimation()
    {
        StopCoroutine(_textAnimationCoroutine);
        _text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }

    public void SetText(string textInfo)
    {
        _text.SetText(textInfo);
    }
}
