using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopUp : UIView
{
    public void SetPosition(Vector3 position)
    {
        if (transform is RectTransform rectTransform)
        {
            rectTransform.position = position;
        }
        else
        {
            transform.localPosition = position;
        }
    }

    public void ResetPosition()
    {
        if (transform is RectTransform rectTransform)
        {
            rectTransform.offsetMin = Vector3.zero;
            rectTransform.offsetMax = Vector3.zero;
        }
        else
        {
            transform.position = Vector3.zero;
        }
    }
}
