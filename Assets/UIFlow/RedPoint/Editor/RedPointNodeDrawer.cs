using System.Collections.Generic;
using UIFlow.RedPoint;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace UIFlow.Editor
{
    [CustomPropertyDrawer(typeof(RedPointNodeConfig))]
    public class RedPointNodeDrawer : PropertyDrawer
    {
        private string[] redPointTypeArr = {"点", "数量"};
        private int relativeSelfId = -1;
        private int relativeParentId = -1;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var selfId = property.FindPropertyRelative(nameof(RedPointNodeConfig.selfId));
            var parentId = property.FindPropertyRelative(nameof(RedPointNodeConfig.parentId));
            var name = property.FindPropertyRelative(nameof(RedPointNodeConfig.name));
            var confirm = property.FindPropertyRelative(nameof(RedPointNodeConfig.confirm));
            var redPointType = property.FindPropertyRelative(nameof(RedPointNodeConfig.redPointType));
            var offsetX = property.FindPropertyRelative(nameof(RedPointNodeConfig.offsetX));
            var offsetY = property.FindPropertyRelative(nameof(RedPointNodeConfig.offsetY));

            var toggleRect = new Rect(position);
            toggleRect.width = EditorGUIUtility.singleLineHeight;
            toggleRect.height = EditorGUIUtility.singleLineHeight;

            confirm.boolValue = EditorGUI.Toggle(toggleRect, confirm.boolValue);

            if (confirm.boolValue)
            {
                toggleRect.x += toggleRect.width;
                toggleRect.width = position.width - EditorGUIUtility.singleLineHeight;
                EditorGUI.LabelField(toggleRect, $"{name.stringValue} : {selfId.intValue}");
            }
            else
            {
                var def = GUI.color;
                if (relativeParentId != 0 && parentId.intValue == relativeParentId || selfId.intValue == relativeParentId)
                {
                    GUI.color = Color.green;
                }
                else if (relativeSelfId != 0 && parentId.intValue == relativeSelfId|| selfId.intValue == relativeSelfId)
                {
                    GUI.color = Color.green;
                }
                
                toggleRect.x += toggleRect.width;
                toggleRect.width = position.width - EditorGUIUtility.singleLineHeight - 100;
                name.stringValue = EditorGUI.TextField(toggleRect, name.stringValue);

                GUI.color = def;
                
                toggleRect.x += toggleRect.width;
                toggleRect.width = 100;
                if (GUI.Button(toggleRect, "RELATIVE"))
                {
                    if (parentId.intValue != 0)
                    {
                        relativeParentId = parentId.intValue;
                        relativeSelfId = 0;
                    }
                    else
                    {
                        relativeSelfId = selfId.intValue;
                        relativeParentId = 0;
                    }
                }

                var rect = new Rect(position);
                rect.y += EditorGUIUtility.singleLineHeight;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.width /= 2;
                EditorGUIUtility.labelWidth = 40;
                parentId.intValue = EditorGUI.IntField(rect, new GUIContent("父节点"), parentId.intValue);

                rect.x += rect.width;
                EditorGUIUtility.labelWidth = 25;
                selfId.intValue = EditorGUI.IntField(rect, new GUIContent("自身"), selfId.intValue);

                rect = new Rect(position);
                rect.y += EditorGUIUtility.singleLineHeight * 2;
                rect.width /= 2;
                rect.height = EditorGUIUtility.singleLineHeight;
                EditorGUIUtility.labelWidth = 50;
                if (redPointType.enumValueIndex < 0)
                    redPointType.enumValueIndex = 0;
                redPointType.enumValueIndex =
                    EditorGUI.Popup(rect, "红点类型", redPointType.enumValueIndex, redPointTypeArr);

                rect.x += rect.width;
                rect.width = 50;
                EditorGUI.LabelField(rect, "坐标偏移");
                rect.x += 50;
                rect.width = (position.width / 2 - 50) / 2;
                offsetX.intValue = EditorGUI.IntField(rect, offsetX.intValue);
                rect.x += rect.width;
                offsetY.intValue = EditorGUI.IntField(rect, offsetY.intValue);

            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var confirm = property.FindPropertyRelative(nameof(RedPointNodeConfig.confirm));
            return confirm.boolValue ? EditorGUIUtility.singleLineHeight : EditorGUIUtility.singleLineHeight * 3;
        }
    }
}