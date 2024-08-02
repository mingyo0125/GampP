using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    private UIRootView _uiRootView;
    public UIRootView RootView => _uiRootView;

    public void SetQueue(UIRootView uIRootView)
    {
        _uiRootView = uIRootView;
    }
}
