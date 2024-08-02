using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIRootView : UIView
{
    [SerializeField]
    private bool startActive = false;

    private Queue<UIView> _uiQueue = new Queue<UIView>();
    public Queue<UIView> UiQueue => _uiQueue;

    private void Awake()
    {
        List<UIView> uIViews = GetComponentsInChildren<UIView>().ToList();

        uIViews.ForEach(uiView =>
        {
            _uiQueue.Enqueue(uiView);
            uiView.SetQueue(this);
        });
    }

    private void Start()
    {
        UIManager.Instance.ShowUI(name);
        if (!startActive) { UIManager.Instance.HideUI(name); }
    }
}
