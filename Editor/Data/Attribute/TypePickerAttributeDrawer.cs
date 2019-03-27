namespace Zinnia.Data.Attribute
{
    using UnityEditor;
    using UnityEngine;
    using System;
    using System.Linq;
    using System.Reflection;
    using Zinnia.Utility;

    [CustomPropertyDrawer(typeof(TypePickerAttribute))]
    public class TypePickerAttributeDrawer : PropertyDrawer
    {
        public class PickerWindow : PickerWindow<Type, PickerWindow> { }

        protected Type type;

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

                Type baseType = ((TypePickerAttribute)attribute).baseType;
                PickerWindow.Show(
                    creatorRect,
                    AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(assembly => assembly.GetTypes())
                        .Where(
                            possibleComponentType =>
                            {
                                AddComponentMenu addComponentMenuAttribute = possibleComponentType
                                    .GetCustomAttributes<AddComponentMenu>(true)
                                    .FirstOrDefault();
                                return baseType.IsAssignableFrom(possibleComponentType)
                                    && !possibleComponentType.IsAbstract
                                    && !possibleComponentType.IsNestedPrivate
                                    && (addComponentMenuAttribute == null
                                        || !string.IsNullOrWhiteSpace(addComponentMenuAttribute.componentMenu));
                            })
                        .OrderBy(componentType => componentType.Name),
                    selectedType =>
                    {
                        assemblyQualifiedTypeNameProperty.stringValue = selectedType.AssemblyQualifiedName;
                        property.serializedObject.ApplyModifiedProperties();
                    },
                    searchedType => searchedType.Name,
                    drawnType => new GUIContent(
                        ObjectNames.NicifyVariableName(drawnType.Name),
                        AssetPreview.GetMiniTypeThumbnail(drawnType),
                        drawnType.FullName));
            }
        }
    }
}