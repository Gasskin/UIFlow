using System.Collections;
using System.Collections.Generic;
using UIFlow.UI;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.Init();
        UIManager.Instance.Open<UILogin>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
