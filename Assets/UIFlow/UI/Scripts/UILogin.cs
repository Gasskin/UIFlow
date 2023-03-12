using UIFlow.UI;
using UnityEngine;

public partial class UILogin : UIBase
{
    public override UIType Layer => UIType.Normal;
    protected override string PrefabName => "UILogin";

    protected override void OnShow()
    {
        Confirm.onClick.AddListener((() =>
        {
            UIManager.Instance.Close<UILogin>();
            UIManager.Instance.Open<UIWorld>(Input.text);
        }));        
    }

    protected override void OnClose()
    {
        Debug.Log("Close UILogin");
        Confirm.onClick.RemoveAllListeners();
    }
}
