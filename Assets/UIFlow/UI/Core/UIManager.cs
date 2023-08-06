using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UIFlow.UI.Config;
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
        private bool isInit = false;

        private GameObject uiRoot;

        private Dictionary<UIType, Transform> uiLayer;

        private Dictionary<string, UIBase> uiLogics;

        private Dictionary<string, GameObject> uiPrefabAssets;

        private Dictionary<string, UIBase> uiWaitForUnLoad;
        private Dictionary<string, float> uiUnLoadCountDown;
        private List<string> removeHelper;

        private UIConfig config;
    #endregion

    #region 生命周期
        public async void Init()
        {
            if (isInit)
                return;
            config = await Resources.LoadAsync<UIConfig>("UIConfig") as UIConfig;
            if (config == null)
            {
                Debug.LogError("UIFlowConfig为Null");
                return;
            }

            var uiRootAsset = await Resources.LoadAsync<GameObject>("UIRoot") as GameObject;
            if (uiRootAsset == null)
            {
                Debug.LogError("找不到 UIRoot");
                return;
            }

            uiRoot = Instantiate(uiRootAsset);
            uiRoot.name = "UIRoot";
            DontDestroyOnLoad(uiRoot);

            uiLayer = new Dictionary<UIType, Transform>();
            uiLogics = new Dictionary<string, UIBase>();
            uiPrefabAssets = new Dictionary<string, GameObject>();
            uiWaitForUnLoad = new Dictionary<string, UIBase>();
            uiUnLoadCountDown = new Dictionary<string, float>();
            removeHelper = new List<string>();

            foreach (var type in Enum.GetValues(typeof(UIType)))
            {
                var layer = uiRoot.transform.Find(type.ToString());
                if (layer != null)
                {
                    uiLayer.Add((UIType)type, layer);
                }
                else
                {
                    Debug.LogError($"找不到层级：{type}");
                }
            }
            isInit = true;
        }

        private void Update()
        {
            if (!isInit)
                return;

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
                if (currentTime - countDownPair.Value >= config.unLoadTime)
                {
                    removeHelper.Add(countDownPair.Key);
                }
            }

            foreach (var uiName in removeHelper)
            {
                if (uiWaitForUnLoad.TryGetValue(uiName, out var logic))
                {
                    logic.Unload();
                    UnLoadAsset(logic.PrefabName);
                }

                uiWaitForUnLoad.Remove(uiName);
                uiUnLoadCountDown.Remove(uiName);
            }
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            if (!isInit || !config.enableDebug)
            {
                return;
            }
            
            // if (GUI.Button(new Rect(0, 0, 100, 40), "DEBUG"))
            // {
            //     isDebug = !isDebug;
            // }
            //
            // if (isDebug)
            // {
            //     var style = new GUIStyle("TextArea");
            //     style.richText = true;
            //     
            //     var rect = new Rect(0, 40, 200, 400);
            //     var str = "<size=20><color=#FF0000>Stack:</Color></size>\n";
            //     foreach (var stack in uiStack)
            //     {
            //         str = str + $"<size=15>{stack.PrefabName}</size>" + "\n";
            //     }
            //     
            //     str = str + "<size=20><color=#00FF18>Asset:</Color></size>\n";
            //     foreach (var asset in uiPrefabAssets.Keys)
            //     {
            //         str = str + $"<size=15>{asset}</size>" + "\n";
            //     }
            //     
            //     var curTime = Time.realtimeSinceStartup;
            //     str = str + "<size=20><color=#FFE000>WaitForUnload:</Color></size>\n";
            //     foreach (var ui in uiWaitForUnLoad.Values)
            //     {
            //         var count = curTime - uiUnLoadCountDown[ui.PrefabName];
            //         str = str + $"<size=15>{ui.PrefabName}</size>" + $"_{config.unLoadTime - count}\n";
            //     }
            //
            //     GUI.TextArea(rect, str, style);
            // }
        }
#endif
    #endregion

    #region 接口方法
        public void Open<T>() where T : UIBase, new()
        {
            if (!isInit)
                return;

            var uiName = typeof(T).Name;
            if (uiLogics.ContainsKey(uiName))
            {
                Debug.LogError($"UI：{uiName} 已经打开");
                return;
            }

            // 是否在卸载列表里
            if (uiWaitForUnLoad.TryGetValue(uiName, out var uiLogic))
            {
                InternalOpen(uiName, uiLogic);
                return;
            }

            CreateUI<T>(uiName);
        }


        public void Close<T>() where T : UIBase, new()
        {
            if (!isInit)
                return;

            var uiName = typeof(T).Name;
            if (!uiLogics.TryGetValue(uiName, out var uiLogic))
            {
                Debug.LogError($"关闭了一个没有打开的UI：{uiName}");
                return;
            }

            InternalClose(uiName, uiLogic);
        }
    #endregion

    #region 工具方法
        private void InternalOpen(string uiName, UIBase uiLogic)
        {
            uiWaitForUnLoad.Remove(uiName);
            uiUnLoadCountDown.Remove(uiName);
            uiLogics.Add(uiName, uiLogic);
            uiLogic.Show();
        }

        private void InternalClose(string uiName, UIBase uiLogic)
        {
            uiLogic.Close();
            uiLogics.Remove(uiName);
            uiWaitForUnLoad.Add(uiLogic.PrefabName, uiLogic);
            uiUnLoadCountDown.Add(uiLogic.PrefabName, Time.realtimeSinceStartup);
        }

        private async void CreateUI<T>(string uiName) where T : UIBase, new()
        {
            var uiLogic = new T();
            if (!uiLayer.TryGetValue(uiLogic.Layer, out var layer))
            {
                Debug.LogError($"找不到UI：{uiName} 的目标层级：{uiLogic.Layer}");
                return;
            }
            
            var asset = await LoadAsset(uiLogic.PrefabName);
            if (asset == null)
            {
                return;
            }

            var instance = Instantiate(asset, layer);
            if (!uiLogic.BindComponent(instance))
            {
                Debug.LogError($"{uiName} BindComponent Error!");
                DestroyImmediate(instance);
                UnLoadAsset(uiLogic.PrefabName);
                return;
            }
            uiLogic.Load();
            InternalOpen(uiName, uiLogic);
        }


        private async UniTask<GameObject> LoadAsset(string assetName)
        {
            if (uiPrefabAssets.TryGetValue(assetName, out var asset))
            {
                return asset;
            }

            asset = await Resources.LoadAsync<GameObject>(assetName) as GameObject;
            if (asset == null)
            {
                Debug.LogError($"加载资源失败：{assetName}");
                return null;
            }

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