using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UIFlow.Editor
{
    [CustomPropertyDrawer(typeof(ShowNameAttribute))]
    public class ShowNameAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as ShowNameAttribute;
            EditorGUI.PropertyField(position, property, new GUIContent(attr.displayName));
        }
    }
}
