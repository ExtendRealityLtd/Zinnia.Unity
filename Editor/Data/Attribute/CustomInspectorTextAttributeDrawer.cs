namespace Zinnia.Data.Attribute
{
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(CustomInspectorTextAttribute))]
    public class CustomInspectorTextAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, new GUIContent(((CustomInspectorTextAttribute)attribute).customText));
        }
    }
}