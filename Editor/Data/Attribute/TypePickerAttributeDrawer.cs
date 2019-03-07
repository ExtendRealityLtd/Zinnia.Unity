namespace Zinnia.Data.Attribute
{
    using UnityEditor;
    using UnityEngine;
    using System;
    using Zinnia.Utility;

    [CustomPropertyDrawer(typeof(TypePickerAttribute))]
    public class TypePickerAttributeDrawer : PropertyDrawer
    {
        private Type type;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label.tooltip = EditorHelper.GetTooltipAttribute(fieldInfo)?.tooltip ?? string.Empty;

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                SerializedProperty assemblyQualifiedTypeNameProperty = property.FindPropertyRelative("assemblyQualifiedTypeName");
                int? index = property.TryGetIndex();

                label.text = index == null ? label.text : $"Element {index}";
                Rect buttonPosition = EditorGUI.PrefixLabel(position, label);

                if (type?.AssemblyQualifiedName != assemblyQualifiedTypeNameProperty.stringValue)
                {
                    type = Type.GetType(assemblyQualifiedTypeNameProperty.stringValue);
                }

                if (!GUI.Button(buttonPosition, new GUIContent(type?.Name, type?.FullName)))
                {
                    return;
                }

                Rect creatorRect = new Rect
                {
                    min = GUIUtility.GUIToScreenPoint(position.min),
                    max = GUIUtility.GUIToScreenPoint(position.max)
                };

                Type superType = ((TypePickerAttribute)attribute).superType;
                TypePickerWindow.Show(
                    creatorRect,
                    superType,
                    selectedType =>
                    {
                        assemblyQualifiedTypeNameProperty.stringValue = selectedType.AssemblyQualifiedName;
                        property.serializedObject.ApplyModifiedProperties();
                    });
            }
        }
    }
}