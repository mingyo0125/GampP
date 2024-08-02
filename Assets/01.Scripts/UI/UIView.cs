using UnityEngine;

public class UIView : MonoBehaviour
{
    private UIRootView _uiRootView;
    public UIRootView RootView => _uiRootView;

    public void SetQueue(UIRootView uIRootView)
    {
        _uiRootView = uIRootView;
    }
}
