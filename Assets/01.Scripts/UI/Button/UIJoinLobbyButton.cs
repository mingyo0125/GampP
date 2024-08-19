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

            return;
        }

        bool isjoinSucces = await LobbyManager.Instance.JoinLobbyByCode(_inputText.text);

        if (!isjoinSucces) { return; } // 여기서 로비 실패 띄워야됨
        UIManager.Instance.HideUI("PlayUI");
        UIManager.Instance.ShowUI("LobbyUI");
        UIManager.Instance.ShowUI("ExitLobby_Button");
        UIManager.Instance.HideUI("Play_Button");
    }
}
