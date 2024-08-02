using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHideButton : UIButton
{
    [SerializeField]
    private string HideUIName;

    protected override void ButtonEvent()
    {
        UIManager.Instance.HideUI(HideUIName);
    }
}
