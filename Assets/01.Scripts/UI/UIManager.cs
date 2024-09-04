using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoSingleTon<UIManager>
{
    [SerializeField]
    private float uiFadeTime;

    private Canvas _canvas;

    private Stack<UIPopUp> _prevPopUpUIViews = new();

    private Dictionary<string, UIView> _staticUIs = new Dictionary<string, UIView>();

    private Dictionary<string, UIView> _curActiveUIs = new Dictionary<string, UIView>();

    private void Awake()
    {
        _staticUIs = new Dictionary<string, UIView>();
        _curActiveUIs = new Dictionary<string, UIView>();

        List<UIView> uIViews = FindObjectsOfType<UIView>().ToList();
        uIViews.ForEach(view => _staticUIs.Add(view.name, view));

        _canvas = FindAnyObjectByType<Canvas>();
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

    public void ShowUI(string uiName, bool isFade = false)
    {
        if (_curActiveUIs.ContainsKey(uiName))
        {
            Debug.LogError($"{uiName} is already opened");
            return;
        }

        (bool, UIView) ui = GetStaticUI(uiName);
        if (!ui.Item1) { return; }

        if (isFade) { FadeOut(ui.Item2.CanvasGroupCompo, uiFadeTime); }
        else { ui.Item2.gameObject.SetActive(true); }

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

    public void HideAll()
    {
        List<string> activeUIKeys = _curActiveUIs.Keys.ToList();

        activeUIKeys.ForEach(uiName => HideUI(uiName));
    }

    public void HideUI(string uiName, bool isFade = false)
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
            if (isFade) { FadeIn(ui.Item2.CanvasGroupCompo, uiFadeTime); }
            else { ui.Item2.gameObject.SetActive(false); }
        }

        _curActiveUIs.Remove(uiName);
    }

    public Tween FadeOut(CanvasGroup canvasGroup, float fadeTime, Action action = null)
    {
        canvasGroup.DOKill();
        canvasGroup.alpha = 0.0f;
        canvasGroup.gameObject.SetActive(true);
        return canvasGroup.DOFade(1f, fadeTime).OnComplete(() =>
        {
            canvasGroup.interactable = true;
            action?.Invoke();
        });
    }

    public void SceneFadeOut(Action FadeFinAction = null)
    {
        CanvasGroup fadeImage = GameObject.Find("Fade_Image").GetComponent<CanvasGroup>();
        //fadeImage.gameObject.name = "Fading"; // 여러번 될 수도 있으니까 이름을 바꿔버려서 다음에는 못 찾게
        FadeOut(fadeImage, 1.5f, () =>
        {
            FadeFinAction?.Invoke();
        });
    }

    public Tween FadeIn(CanvasGroup canvasGroup, float fadeTime, Action action = null)
    {
        canvasGroup.DOKill();
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = false;
        return canvasGroup.DOFade(0f, fadeTime).OnComplete(() =>
        {
            canvasGroup.gameObject.SetActive(false);
            action?.Invoke();
        });
    }

    public void ShowWarningText(string textContent)
    {
        UIText warningText = GetStaticUI("Warning_Text").Item2 as UIText;
        warningText.SetText(textContent);

        UIView warningpannel = GetStaticUI("Warning_Panel").Item2;

        FadeOut(warningpannel.CanvasGroupCompo, uiFadeTime)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(1f, () =>
                {
                    FadeIn(warningpannel.CanvasGroupCompo, uiFadeTime);
                });
            });
    }

}
