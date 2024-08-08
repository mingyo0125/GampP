using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIJoinLobbyButton : UIButton
{
    [SerializeField]
    private TextMeshProUGUI _inputText;

    protected override void ButtonEvent()
    {
        LobbyManager.Instance.JoinLobbyByCode(_inputText.text);
        UIManager.Instance.HideUI("PlayUI");
    }
}
