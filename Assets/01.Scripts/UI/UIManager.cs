using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoSingleTon<UIManager>
{
    private Canvas _canvas;

    private Stack<UIPopUp> _prevPopUpUIViews = new();

    private Dictionary<string, UIView> _staticUIs = new Dictionary<string, UIView>();

    private Dictionary<string, UIView> _curActiveUIs = new Dictionary<string, UIView>();

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

    public void ShowUI(string uiName)
    {
        if(_prevPopUpUIViews.TryPeek(out UIPopUp _prevPopUpUIView)) // 이거 Peek로 할지 Pop으로 할지 보류
        {
            _prevPopUpUIView.gameObject.SetActive(false);
        }

        UIView uiView = _staticUIs[uiName];
        uiView.gameObject.SetActive(true);

        _curActiveUIs.Add(uiName, uiView);
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
        UIView uiView = _curActiveUIs[uiName];

        if (uiView is UIPopUp)
        {
            _prevPopUpUIViews.Pop();
            Destroy(uiView.gameObject);
        }
        else
        {
            uiView.RootView.gameObject.SetActive(false); // 일단 이걸로 나중에 트윈 넣자.
        }

        _curActiveUIs.Remove(uiName);
    }
}
