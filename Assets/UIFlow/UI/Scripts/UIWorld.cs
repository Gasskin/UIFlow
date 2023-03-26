using System.Collections;
using System.Collections.Generic;
using UIFlow.UI;
using UnityEngine;

public partial class UIWorld : UIBase
{
    public override UIType Layer => UIType.Normal;
    public override string PrefabName => "Test/UIWorld";

    public override bool UseUIStack => true;

    protected override void OnLoad()
    {
        AddSubUI<UIMoney>(Money);
    }

    protected override void OnShow()
    {
        Bag.onClick.AddListener((() =>
        {
            UIManager.Instance.Open<UIBag>();
        }));
    }

    protected override void OnClose()
    {
        Bag.onClick.RemoveAllListeners();
    }
}
