﻿// <AUTO-GENERATE>
// This File Is Auto Generated By UIComponentEditor
// </AUTO-GENERATE>

using UnityEngine.UI;
using UnityEngine;
using UIFlow.UIComponent;

public partial class UIMoney
{
    private Text Glod;
    private Text Power;
    private Text Diamond;

    public override bool BindComponent()
    {
        if (prefabInstance == null) 
        {
            Debug.LogError("UIMoney，绑定组件失败，没有实例资源");
            return false;
        }
        var uiComponent = prefabInstance.GetComponent<UIComponent>();
        if (uiComponent == null)
        {
            Debug.LogError("UIMoney，绑定组件失败，没有实例资源");
            return false;
        }
        Glod =  uiComponent.GetComponent<Text>(0);
        if (Glod == null) return false;
        Power =  uiComponent.GetComponent<Text>(1);
        if (Power == null) return false;
        Diamond =  uiComponent.GetComponent<Text>(2);
        if (Diamond == null) return false;
        return true;
    }
}
