using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIText : UIView
{
    private TextMeshProUGUI _text;

    protected override void Awake()
    {
        base.Awake();

        _text = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string text)
    {
        _text.SetText(text);
    }
}
