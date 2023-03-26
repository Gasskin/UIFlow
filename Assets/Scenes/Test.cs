using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UIFlow.UI;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        UIManager.Instance.Init();
        await UniTask.DelayFrame(3);
        UIManager.Instance.Open<UILogin>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
