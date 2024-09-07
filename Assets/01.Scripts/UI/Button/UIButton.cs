using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIButton : UIView
{
    private Button _button;

    protected override void Awake()
    {
        base.Awake();

        _button = GetComponent<Button>();

        _button.onClick.AddListener(() => ButtonEvent());
    }

    protected virtual void ButtonEvent()
    {
    }
}
