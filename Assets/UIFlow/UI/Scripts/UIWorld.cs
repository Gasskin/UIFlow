using System.Collections;
using System.Collections.Generic;
using UIFlow.UI;
using UnityEngine;

public partial class UIWorld : UIBase
{
    public override UIType Layer => UIType.Normal;
    protected override string PrefabName => "UIWorld";

    protected override void OnShow()
    {
        Name.text = uiParam as string;
        
        Bag.onClick.AddListener((() =>
        {
            UIManager.Instance.Open<UIBag>();
        }));
    }
}
