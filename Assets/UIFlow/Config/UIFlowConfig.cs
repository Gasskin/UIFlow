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
#endif

        [ShowName("代码生成路径")]
        public string genPath;
    }
}