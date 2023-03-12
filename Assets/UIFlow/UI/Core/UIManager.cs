using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
    #region 属性
        private float unLoadInterval = 10f;
        
        private bool isInit = false;

        private GameObject uiRoot;

        private Dictionary<string, Transform> uiLayer;

        private Dictionary<string, UIBase> uiLogics;

        private Dictionary<string, GameObject> uiPrefabAssets;

        private Dictionary<string, UIBase> uiWaitForUnLoad;
        private Dictionary<string, float> uiUnLoadCountDown;
        private List<string> removeHelper;

        private UIBase preOpen;
        private UIBase preClose;
    #endregion

    #region 生命周期

        public void Init()
        {
            if (isInit)
                return;

            uiLayer = new Dictionary<string, Transform>();
            uiLogics = new Dictionary<string, UIBase>();
            uiPrefabAssets = new Dictionary<string, GameObject>();
            uiWaitForUnLoad = new Dictionary<string, UIBase>();
            uiUnLoadCountDown = new Dictionary<string, float>();
            removeHelper = new List<string>();
            
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

            isInit = true;
        }

        private void Update()
        {
            foreach (var uiLogic in uiLogics.Values)
            {
                if (uiLogic.IsOpen)
                {
                    uiLogic.Update();
                }
            }

            removeHelper.Clear();
            var currentTime = Time.realtimeSinceStartup;
            foreach (var countDownPair in uiUnLoadCountDown)
            {
                if (currentTime - countDownPair.Value >= unLoadInterval)
                {
                    removeHelper.Add(countDownPair.Key);
                }
            }

            foreach (var uiName in removeHelper)
            {
                if (uiWaitForUnLoad.TryGetValue(uiName,out var logic))
                {
                    logic.Unload();
                    UnLoadAsset(uiName);
                }

                uiWaitForUnLoad.Remove(uiName);
                uiUnLoadCountDown.Remove(uiName);
            }
        }
    #endregion

    #region 接口方法

        public async void Open<T>(object uiParam = null) where T : UIBase, new()
        {
            var uiName = typeof(T).Name;
            if (uiLogics.ContainsKey(uiName))
            {
                Debug.LogError($"禁止打开两个相同的主UI：{uiName}");
                return;
            }

            var asset = await LoadAsset(uiName);
            if (asset == null)
            {
                Debug.LogError($"加载UI Prefab失败：{uiName}");
                return;
            }

            OpenOrCreateUI<T>(uiName, asset, uiParam);
        }

        public void Close<T>() where T : UIBase, new()
        {
            var uiName = typeof(T).Name;
            if (!uiLogics.TryGetValue(uiName,out var uiLogic))
            {
                Debug.LogError($"关闭了一个没有打开的UI：{uiName}");
                return;
            }
            uiLogic.Close();
            uiLogics.Remove(uiName);
            uiWaitForUnLoad.Add(uiName, uiLogic);
            uiUnLoadCountDown.Add(uiName, Time.realtimeSinceStartup);
        }

        private void OpenOrCreateUI<T>(string uiName,GameObject asset, object uiParam)where T : UIBase, new()
        {
            // 是否在卸载列表里
            if (uiWaitForUnLoad.TryGetValue(uiName,out var uiLogic))
            {
                uiWaitForUnLoad.Remove(uiName);
                uiUnLoadCountDown.Remove(uiName);
                uiLogics.Add(uiName,uiLogic);
                uiLogic.Show(preOpen, uiParam);
            }
            else
            {
                uiLogic = new T();
                if (!uiLayer.TryGetValue(uiLogic.Layer.ToString(),out var layer))
                {
                    Debug.LogError($"找不到UI：{uiName} 的目标层级：{uiLogic.Layer.ToString()}");
                    return;
                }
                var instance = Instantiate(asset, layer);
                uiLogic.Load(instance);
            
                if (!uiLogic.BindComponent())
                {
                    uiLogic.Unload();
                    UnLoadAsset(uiName);
                    Debug.LogError($"{uiName} 绑定组件失败");
                    return;
                }
                uiLogics.Add(uiName, uiLogic);
                uiLogic.Show(preOpen,uiParam);
            }
        }
    #endregion

    #region 工具方法

        private async UniTask<GameObject> LoadAsset(string assetName)
        {
            if (uiPrefabAssets.TryGetValue(assetName, out var asset))
            {
                return asset;
            }

            asset = await Resources.LoadAsync<GameObject>(assetName) as GameObject;
            uiPrefabAssets.Add(assetName, asset);
            return asset;
        }


        private void UnLoadAsset(string assetName)
        {
            if (uiPrefabAssets.ContainsKey(assetName))
            {
                Resources.UnloadUnusedAssets();
                uiPrefabAssets.Remove(assetName);
            }
            else
            {
                Debug.LogError($"想卸载不存在的UI资源：{assetName}");
            }
        }
    #endregion
    }
}