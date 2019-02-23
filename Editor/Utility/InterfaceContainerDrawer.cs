namespace Zinnia.Utility
{
    using UnityEngine;
    using UnityEditor;
    using System;

    [CustomPropertyDrawer(typeof(InterfaceContainer), true)]
    public class InterfaceContainerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Type type = fieldInfo.FieldType;
            if (type.IsGenericType)
            {
                type = type.GenericTypeArguments[0];
            }

            if (type.HasElementType)
            {
                type = type.GetElementType();
            }

            while (type != null && type.BaseType != typeof(InterfaceContainer))
            {
                type = type.BaseType;
            }

            if (type?.BaseType != typeof(InterfaceContainer))
            {
                throw new ArgumentException();
            }

            type = type.GenericTypeArguments[0];
            label.tooltip = EditorHelper.GetTooltipAttribute(fieldInfo)?.tooltip ?? string.Empty;

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                EditorGUI.ObjectField(position, property.FindPropertyRelative("field"), type, label);
            }
        }
    }
}