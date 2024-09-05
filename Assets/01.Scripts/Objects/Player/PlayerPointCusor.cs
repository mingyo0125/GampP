using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointCusor : MonoBehaviour
{
    private float bounceHeight = 1.0f;
    private float bounceDuration = 0.5f;
    private float scaleDuration = 0.5f;
    private Vector3 _originalScale;

    void Start()
    {
        _originalScale = transform.localScale;

        BounceArrow();
        ScaleArrow();
    }

    void BounceArrow()
    {
        transform.DOMoveY(transform.position.y + bounceHeight, bounceDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    void ScaleArrow()
    {
        transform.DOScale(new Vector3(_originalScale.x * 1.1f, _originalScale.y * 1.1f, _originalScale.z), scaleDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
