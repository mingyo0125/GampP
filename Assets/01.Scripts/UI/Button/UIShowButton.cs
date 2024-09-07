using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShowButton : UIButton
{
    [SerializeField]
    private string showUIName;

    [SerializeField]
    private bool isPopUp;

    [SerializeField]
    private Transform _popUpTrm;

    protected override void ButtonEvent()
    {
        base.ButtonEvent();

        if (isPopUp)
        {
            UIManager.Instance.ShowUI(showUIName, _popUpTrm.position);
        }
        else
        {
            UIManager.Instance.ShowUI(showUIName);
        }
    }
}
