using System.ComponentModel;
using UIFlow.Editor;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UIFlow.Config
{
    public class UIFlowConfig : ScriptableObject
    {
#if UNITY_EDITOR
        [MenuItem("Assets/Create/UIFlow/Config")]
        public static void CreateConfig()
        {
            var asset = Resources.Load<UIFlowConfig>("UIFlowConfig");

            if (asset == null)
            {
                asset = CreateInstance<UIFlowConfig>();
                AssetDatabase.CreateAsset(asset, "Assets/UIFlowConfig.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            EditorGUIUtility.PingObject(asset);
        }

        [ShowName("开启DEBUG(仅编辑器)")]
        public bool enableDebug;
#endif

        [ShowName("卸载时间")] [Range(0f, 60f)] public float unLoadTime;

        [ShowName("代码生成路径")] public string genPath;

        [ShowName("Prefab保存路径")] public string prefabPath;

        [ShowName("")] public string[] nameSpaceUse;
    }
}