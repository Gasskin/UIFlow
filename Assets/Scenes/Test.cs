using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UIFlow.RedPoint;
using UIFlow.UI;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        UIManager.Instance.Init();
        RedPointManager.Instance.Init();
        await UniTask.DelayFrame(3);
        await UniTask.DelayFrame(3);
        RedPointManager.Instance.RefreshRedPoint(9, 2);
        UIManager.Instance.Open<UILogin>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
