using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIButton : UIView
{
    private Button _button;

    protected virtual void Awake()
    {
        _button = GetComponent<Button>();

        _button.onClick.AddListener(() => ButtonEvent());
    }

    protected abstract void ButtonEvent();
}
