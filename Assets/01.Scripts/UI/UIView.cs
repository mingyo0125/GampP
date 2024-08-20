using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UIView : MonoBehaviour
{
    private UIRootView _uiRootView;
    public UIRootView RootView => _uiRootView;

    protected CanvasGroup _canvasGroup
    {
        get
        {
            bool hasCanvasGroup = gameObject.TryGetComponent(out CanvasGroup canvasGroup);
            if (hasCanvasGroup)
            {
                return canvasGroup;
            }
            return gameObject.AddComponent<CanvasGroup>();
        }
    }
    public CanvasGroup CanvasGroupCompo => _canvasGroup;

    public void SetUIView(UIRootView uIRootView)
    {
        _uiRootView = uIRootView;
    }

    protected virtual void Awake()
    {
    }
}
