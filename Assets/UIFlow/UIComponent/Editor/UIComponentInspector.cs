using UIFlow.UIComponent;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(UIComponent))]
public class UIComponentInspector : Editor
{
    private SerializedProperty uiName;
    private SerializedProperty uiData;

    private ReorderableList uiDataList;
    
    private void OnEnable()
    {
        uiName = serializedObject.FindProperty(nameof(UIComponent.uiName));
        uiData = serializedObject.FindProperty(nameof(UIComponent.uiData));

        uiDataList = new ReorderableList(serializedObject, uiData, true, true, true, true);
        uiDataList.drawHeaderCallback = rect =>
        {
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(uiName);
        EditorGUILayout.Space();
        uiDataList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}
