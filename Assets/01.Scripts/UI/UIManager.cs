using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoSingleTon<UIManager>
{
    private Canvas _canvas;

    private Stack<UIPopUp> _prevPopUpUIViews = new();

    private Dictionary<string, UIView> _staticUIs = new Dictionary<string, UIView>();

    private Dictionary<string, UIView> _curActiveUIs = new Dictionary<string, UIView>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ShowUI("LobbyUI");
        }
    }

    private void Awake()
    {
        List<UIView> uIViews = FindObjectsOfType<UIView>().ToList();
        uIViews.ForEach(view => _staticUIs.Add(view.name, view));

        _canvas = FindObjectOfType<Canvas>();
    }

    public UIPopUp GetPopUpUIPrefab(string uiName)
    {
        UIPopUp loadpopupUi = Resources.Load<UIPopUp>(uiName);
        UIPopUp popupUi = Instantiate(loadpopupUi);
        popupUi.gameObject.name = uiName;
        return popupUi;
    }

    public void UpdateText(string textName, string textContent)
    {
        (bool, UIView) ui = GetStaticUI(textName);

        if (!ui.Item1) { return; }

        if (ui.Item2 is UIText)
        {
            UIText text = ui.Item2 as UIText;
            text.SetText(textContent);
        }
        else
        {
            Debug.LogError($"{textName} is Not TMP");
        }
    }

    public (bool, UIView) GetStaticUI(string uiName)
    {
        if (_staticUIs[uiName])
        {
            return (true, _staticUIs[uiName]);
        }
        else
        {
            Debug.LogError($"UI {uiName} is Not StaticUI");
            return (false, null);
        }
    }

    public void ShowUI(string uiName)
    {
        if (_curActiveUIs.ContainsKey(uiName))
        {
            Debug.LogError($"{uiName} is already opened");
            return;
        }

        (bool, UIView) ui = GetStaticUI(uiName);

        if (!ui.Item1) { return; }

        ui.Item2.gameObject.SetActive(true);
        _curActiveUIs.Add(uiName, ui.Item2);

        if (_prevPopUpUIViews.TryPeek(out UIPopUp _prevPopUpUIView)) // 이거 Peek로 할지 Pop으로 할지 보류
        {
            _prevPopUpUIView.gameObject.SetActive(false);
        }
    }

    public void ShowUI(string uiName, Vector2 popupPos) // 팝업
    {
        if (_prevPopUpUIViews.TryPop(out UIPopUp _prevPopUpUIView))
        {
            _prevPopUpUIView.gameObject.SetActive(false);
        }

        UIPopUp popupUI = GetPopUpUIPrefab(uiName);
        popupUI.transform.SetParent(_canvas.transform);

        popupUI.SetPosition(popupPos);

        _prevPopUpUIViews.Push(popupUI);

        _curActiveUIs.Add(uiName, popupUI);
    }

    public void HideUI(string uiName)
    {
        (bool, UIView) ui = GetStaticUI(uiName);

        if (!ui.Item1) { return; }

        if (ui.Item2 is UIPopUp)
        {
            _prevPopUpUIViews.Pop();
            Destroy(ui.Item2.gameObject);
        }
        else
        {
            ui.Item2.gameObject.SetActive(false); // 일단 이걸로 나중에 트윈 넣자.
        }

        _curActiveUIs.Remove(uiName);
    }
}
