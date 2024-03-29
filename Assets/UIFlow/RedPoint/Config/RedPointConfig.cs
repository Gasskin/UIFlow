using System;
using UIFlow.Editor;
using UnityEditor;
using UnityEngine;

namespace UIFlow.RedPoint
{
    public class RedPointConfig : ScriptableObject
    {
#if UNITY_EDITOR
        [MenuItem("Assets/Create/UIFlow/RedPointConfig")]
        public static void CreateConfig()
        {
            var asset = Resources.Load<RedPointConfig>("RedPointConfig");

            if (asset == null)
            {
                var select = Selection.activeObject;
                var path = select == null
                    ? "Assets/RedPointConfig.asset"
                    : $"{AssetDatabase.GetAssetPath(select)}/RedPointConfig.asset";

                asset = CreateInstance<RedPointConfig>();
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            EditorGUIUtility.PingObject(asset);
        }
#endif

        [ShowName("点类型")]
        public GameObject pointPrefab;
        [ShowName("数字类型")]
        public GameObject numPrefab;
        public RedPointNodeConfig[] nodes;
    }
}
