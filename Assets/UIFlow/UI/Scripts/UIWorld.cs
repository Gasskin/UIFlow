using System.Collections;
using System.Collections.Generic;
using UIFlow.RedPoint;
using UIFlow.UI;
using UnityEngine;

public partial class UIWorld : UIBase
{
    public override UIType Layer => UIType.Normal;
    public override string PrefabName => "UIWorld";

    public override bool UseUIStack => true;

    protected override void OnLoad()
    {
        AddSubUI<UIMoney>(Money);
    }

    protected override void OnShow()
    {
        RedPointManager.Instance.AddWatcher(1, Icon.transform);
        
        Bag.onClick.AddListener((() =>
        {
            UIManager.Instance.Open<UIBag>();
        }));
    }

    private int num = 1;
    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            RedPointManager.Instance.RefreshRedPoint(1, 1);
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            RedPointManager.Instance.RefreshRedPoint(1, -1);
        }
    }

    protected override void OnClose()
    {
        RedPointManager.Instance.RemoveWatcher(1);
        Bag.onClick.RemoveAllListeners();
    }
}
