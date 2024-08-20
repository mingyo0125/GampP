using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIJoinLobbyButton : UIButton
{
    [SerializeField]
    private TextMeshProUGUI _inputText;

    protected async override void ButtonEvent()
    {
        if (_inputText.text.Length != 7)
        {
            UIManager.Instance.ShowWarningText("코드가 올바른 형식이 아닙니다");
            return;
        }

        bool isjoinSucces = await LobbyManager.Instance.JoinLobbyByCode(_inputText.text);

        if(!isjoinSucces) { return; }

        UIManager.Instance.HideUI("PlayUI");
        UIManager.Instance.ShowUI("LobbyUI");
        UIManager.Instance.ShowUI("ExitLobby_Button");
        UIManager.Instance.HideUI("Play_Button");
    }
}
