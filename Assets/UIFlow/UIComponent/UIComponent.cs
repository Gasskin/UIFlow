using System.Collections;
using System.Collections.Generic;
using UIFlow.Editor;
using UnityEngine;

namespace UIFlow.UIComponent
{
    public class UIComponent: MonoBehaviour
    {
        [ShowName("UI名称")]
        public string uiName;
        
        public UIComponentData[] uiData;
    }
}