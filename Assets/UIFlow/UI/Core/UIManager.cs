using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIFlow.UI
{
    public enum UIType
    {
        Normal,
        Top,
    }

    public class UIManager : Singleton<UIManager>
    {
        private GameObject uiRoot;

        private Dictionary<string, Transform> uiLayer;

        private Dictionary<string, UIBase> uiLogics;

        private Dictionary<string, GameObject> uiPrefabs;
        
        public void Init()
        {
            var uiRootAsset = Resources.Load<GameObject>("UIRoot");
            if (uiRootAsset == null)
            {
                Debug.LogError("找不到 UIRoot");
                return;
            }

            uiRoot = Instantiate(uiRootAsset);
            uiRoot.name = "UIRoot";
            DontDestroyOnLoad(uiRoot);

            uiLayer = new Dictionary<string, Transform>();
            var layerNames = Enum.GetNames(typeof(UIType));
            foreach (var layerName in layerNames)
            {
                var layer = uiRoot.transform.Find(layerName);
                if (layer != null)
                {
                    uiLayer.Add(layerName, layer);
                }
                else
                {
                    Debug.LogError($"找不到层级：{layer}");
                }
            }
        }

        public void Open<T>() where T : UIBase
        {
            
        }

        public void Close<T>() where T : UIBase
        {
            
        }
    }
}
