// <AUTO-GENERATE>
// This File Is Auto Generated By UIComponentEditor
// </AUTO-GENERATE>

using UnityEngine.UI;
using UnityEngine;
using UIFlow.UIComponent;

public partial class UILogin
{
    private Button Confirm;
    private Button Cancel;
    private InputField Input;

    public override bool BindComponent()
    {
        if (prefabInstance == null) 
        {
            Debug.LogError("UILogin，绑定组件失败，没有实例资源");
            return false;
        }
        var uiComponent = prefabInstance.GetComponent<UIComponent>();
        if (uiComponent == null)
        {
            Debug.LogError("UILogin，绑定组件失败，没有实例资源");
            return false;
        }
        Confirm =  uiComponent.GetComponent<Button>(0);
        Cancel =  uiComponent.GetComponent<Button>(1);
        Input =  uiComponent.GetComponent<InputField>(2);
        return true;
    }
}
