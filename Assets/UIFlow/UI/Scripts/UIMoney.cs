using System.Collections;
using System.Collections.Generic;
using UIFlow.UI;
using UnityEngine;

public partial class UIMoney : UISubBase
{
    protected override void OnShow()
    {
        Glod.text = "黄金";
        Power.text = "体力";
        Diamond.text = "钻石";

        Debug.Log("打开");
    }

    protected override void OnClose()
    {
        Debug.Log("关闭");
    }

    protected override void OnUpdate()
    {
        Debug.Log("Update");
    }
}
