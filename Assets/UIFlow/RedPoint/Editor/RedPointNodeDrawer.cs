using System.Collections.Generic;
using UIFlow.RedPoint;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace UIFlow.Editor
{
    [CustomPropertyDrawer(typeof(RedPointNode))]
    public class RedPointNodeDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var id = property.FindPropertyRelative(nameof(RedPointNode.selfId));
            var parentId = property.FindPropertyRelative(nameof(RedPointNode.parentId));
            var name = property.FindPropertyRelative(nameof(RedPointNode.name));
            var confirm = property.FindPropertyRelative(nameof(RedPointNode.confirm));

            var toggleRect = new Rect(position);
            toggleRect.width = EditorGUIUtility.singleLineHeight;
            toggleRect.height = EditorGUIUtility.singleLineHeight;
            
            confirm.boolValue = EditorGUI.Toggle(toggleRect, confirm.boolValue);

            if (confirm.boolValue)
            {
                toggleRect.x += toggleRect.width;
                toggleRect.width = position.width - EditorGUIUtility.singleLineHeight;
                EditorGUI.LabelField(toggleRect,$"{name.stringValue} : {id.intValue}");
            }
            else
            {
                
                toggleRect.x += toggleRect.width;
                toggleRect.width = position.width - EditorGUIUtility.singleLineHeight;
                name.stringValue = EditorGUI.TextField(toggleRect, name.stringValue);

                var rect = new Rect(position);
                rect.y += EditorGUIUtility.singleLineHeight;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.width /= 2;
                EditorGUIUtility.labelWidth = 40;
                parentId.intValue = EditorGUI.IntField(rect, new GUIContent("父节点"), parentId.intValue);
                
                rect.x += rect.width;
                EditorGUIUtility.labelWidth = 25;
                id.intValue = EditorGUI.IntField(rect, new GUIContent("自身"), id.intValue);
            }

            // EditorGUIUtility.labelWidth = 30f;
            // var rect = new Rect(position);
            // rect.height = EditorGUIUtility.singleLineHeight;
            // rect.width -= 10;
            // rect.width /= 3;
            // parentId.intValue = EditorGUI.IntField(rect, new GUIContent("父节点"),parentId.intValue);
            //
            // rect.x = rect.x + rect.width + 5;
            // id.intValue = EditorGUI.IntField(rect, new GUIContent("ID"),id.intValue);
            //
            // rect.x = rect.x + rect.width + 5;
            // name.stringValue = EditorGUI.TextField(rect,new GUIContent("备注"), name.stringValue);
            //
            // var childRect = new Rect(position);
            // childRect.y += EditorGUIUtility.singleLineHeight;
            // childRect.height = EditorGUIUtility.singleLineHeight;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var confirm = property.FindPropertyRelative(nameof(RedPointNode.confirm));
            return confirm.boolValue ? EditorGUIUtility.singleLineHeight : EditorGUIUtility.singleLineHeight * 2;
        }
    }
}
