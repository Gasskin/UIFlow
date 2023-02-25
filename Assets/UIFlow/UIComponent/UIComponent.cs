using System.Collections;
using System.Collections.Generic;
using UIFlow.Editor;
using UnityEngine;

namespace UIFlow.UIComponent
{
    public class UIComponent : MonoBehaviour
    {
        public string uiName;

        public UIComponentData[] uiData;

        public T GetComponent<T>(int index) where T : Component
        {
            if (index >= uiData.Length) 
            {
                Debug.LogError("UIComponent 获取组件 溢出");
                return null;
            }

            var data = uiData[index];
            return data.target.GetComponent(data.componentType) as T;
        }
    }
}