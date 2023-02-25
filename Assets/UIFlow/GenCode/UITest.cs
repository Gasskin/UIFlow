﻿// <AUTO-GENERATE>
// This File Is Auto Generated By UIComponentEditor
// </AUTO-GENERATE>

using UnityEngine.UI;
using UnityEngine;
using UIFlow.UIComponent;

public partial class UITest
{
    private CanvasRenderer Bg;
    private Button Confirm;
    private Button Cancel;

    private void BindComponent(GameObject prefab)
    {
        var uiComponent = prefab.GetComponent<UIComponent>();
        if (uiComponent == null)
            return;
        Bg =  uiComponent.GetComponent<CanvasRenderer>(0);
        Confirm =  uiComponent.GetComponent<Button>(1);
        Cancel =  uiComponent.GetComponent<Button>(2);
    }
}