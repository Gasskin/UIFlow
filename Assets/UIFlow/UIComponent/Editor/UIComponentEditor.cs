using System.IO;
using System.Text;
using UIFlow.Config;
using UIFlow.UIComponent;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIComponent))]
public class UIComponentEditor : Editor
{
    private SerializedProperty uiName;
    private SerializedProperty uiData;

    private UIFlowConfig config;
    
    private void OnEnable()
    {
        uiName = serializedObject.FindProperty(nameof(UIComponent.uiName));
        uiData = serializedObject.FindProperty(nameof(UIComponent.uiData));

        if (string.IsNullOrEmpty(uiName.stringValue))
        {
            uiName.stringValue = ((UIComponent) target).transform.name;
            serializedObject.ApplyModifiedProperties();
        }

        config = Resources.Load<UIFlowConfig>("UIFlowConfig");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUIUtility.labelWidth = 40f;
        EditorGUILayout.PropertyField(uiName, new GUIContent("UI名称"));
        EditorGUILayout.PropertyField(uiData);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Gen Code"))
        {
            GenCode();
        }

        if (GUILayout.Button("Gen Prefab"))
        {
            GenPrefab();
        }
        EditorGUILayout.EndHorizontal();
        
        serializedObject.ApplyModifiedProperties();
    }

    private void GenCode()
    {
        var genPath = $"{Application.dataPath}/{config.genPath}/{uiName.stringValue}.cs";
        var sb = new StringBuilder();
        
        sb.AppendLine("// <AUTO-GENERATE>");
        sb.AppendLine("// This File Is Auto Generated By UIComponentEditor");
        sb.AppendLine("// </AUTO-GENERATE>");
        sb.AppendLine();
        for (int i = 0; i < config.nameSpaceUse.Length; i++)
            sb.AppendLine($"using {config.nameSpaceUse[i]};");
        sb.AppendLine();
        sb.AppendLine($"public partial class {uiName.stringValue}");
        sb.AppendLine("{");
        for (int i = 0; i < uiData.arraySize; i++)
        {
            var data = uiData.GetArrayElementAtIndex(i);
            var componentType = data.FindPropertyRelative(nameof(UIComponentData.componentType)).stringValue;
            var fieldName = data.FindPropertyRelative(nameof(UIComponentData.fieldName)).stringValue;
            sb.AppendLine($"    private {componentType} {fieldName};");
        }
        sb.AppendLine();
        sb.AppendLine("    private void BindComponent(GameObject prefab)");
        sb.AppendLine("    {");
        sb.AppendLine("        var uiComponent = prefab.GetComponent<UIComponent>();");
        sb.AppendLine("        if (uiComponent == null)");
        sb.AppendLine("            return;");
        for (int i = 0; i < uiData.arraySize; i++)
        {
            var data = uiData.GetArrayElementAtIndex(i);
            var componentType = data.FindPropertyRelative(nameof(UIComponentData.componentType)).stringValue;
            var fieldName = data.FindPropertyRelative(nameof(UIComponentData.fieldName)).stringValue;
            sb.AppendLine($"        {fieldName} =  uiComponent.GetComponent<{componentType}>({i});");
        }
        sb.AppendLine("    }");
        sb.AppendLine("}");

        File.WriteAllText(genPath, sb.ToString(), Encoding.UTF8);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void GenPrefab()
    {
        var gameObject = (target as UIComponent).gameObject;
        if (!PrefabUtility.IsAnyPrefabInstanceRoot(gameObject))
            return;
        if (!gameObject.name.EndsWith("_Editor"))
        {
            Debug.LogError("仅可对_Editor结尾的Prefab操作");
            return;
        }

        var runtimeName = gameObject.name.Substring(0, gameObject.name.Length - 7);
        var clone = Instantiate(gameObject);
        clone.name = runtimeName;

        PrefabUtility.SaveAsPrefabAsset(clone, $"{Application.dataPath}/{config.prefabPath}/{runtimeName}.prefab");
        DestroyImmediate(clone);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}