namespace Zinnia.Data.Type
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using Zinnia.Utility;
    using Zinnia.Data.Collection.List;

    [CustomPropertyDrawer(typeof(ObservableList), true)]
    public class DefaultObservableListDrawer : PropertyDrawer
    {
        protected const string collectionLabel = "Elements";
        protected const string collectionElementLabel = "Element {0}";
        protected const string collectionEmptyLabel = "List is Empty";
        protected const float footerSpacing = 2f;
        protected const float objectPickerPadding = 4f;
        protected const int indentResetLevel = -2;
        protected readonly GUIStyle buttonStyle = new GUIStyle("RL Footer");
        protected readonly GUIContent objectPickerIcon = EditorGUIUtility.TrIconContent("In ObjectField");
        protected readonly GUIContent buttonIconAdd = EditorGUIUtility.TrIconContent("Toolbar Plus", "Add element to list");
        protected readonly GUIContent buttonIconRemove = EditorGUIUtility.TrIconContent("Toolbar Minus", "Remove last element from list");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label.tooltip = EditorHelper.GetTooltipAttribute(fieldInfo)?.tooltip ?? string.Empty;
            EditorGUILayout.PropertyField(property, label, true);
            object propertyObject = fieldInfo.GetValue(property.serializedObject.targetObject);
            if (propertyObject == null)
            {
                return;
            }

            buttonStyle.overflow = new RectOffset(2, 0, 2, 0);
            buttonStyle.fixedHeight = buttonIconAdd.image.height * 1.01f;
            buttonStyle.fixedWidth = objectPickerIcon.image.width * 1.5f;

            using (new EditorGUI.IndentLevelScope())
            {
                property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, collectionLabel, true);
                if (!property.isExpanded)
                {
                    return;
                }

                using (new EditorGUI.IndentLevelScope())
                {
                    dynamic list = propertyObject;
                    bool listIsEmpty = list.NonSubscribableElements.Count == 0;

                    if (listIsEmpty)
                    {
                        using (new EditorGUI.IndentLevelScope(indentResetLevel))
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.PrefixLabel(" ");
                            EditorGUILayout.HelpBox(collectionEmptyLabel, MessageType.None);
                            GUILayout.Space(objectPickerIcon.image.width - objectPickerPadding + 1f);
                        }
                    }
                    else
                    {
                        for (int index = 0; index < list.NonSubscribableElements.Count; index++)
                        {
                            dynamic currentElement = list.NonSubscribableElements[index];
                            Type elementType = currentElement == null ? GetElementType() : currentElement.GetType();
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.PrefixLabel(string.Format(collectionElementLabel, index));
                                using (new EditorGUI.IndentLevelScope(indentResetLevel))
                                using (EditorGUI.ChangeCheckScope check = new EditorGUI.ChangeCheckScope())
                                {
                                    dynamic elementValue = EditorGUILayout.ObjectField(currentElement, elementType, true);
                                    if (check.changed)
                                    {
                                        list.SetAt(elementValue, index);
                                    }
                                }
                            }
                        }
                    }

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button(buttonIconAdd, buttonStyle))
                        {
                            list.Add(null);
                        }

                        using (new EditorGUI.DisabledScope(listIsEmpty))
                        {
                            if (GUILayout.Button(buttonIconRemove, buttonStyle))
                            {
                                list.RemoveAt(list.NonSubscribableElements.Count - 1);
                            }
                        }

                        GUILayout.Space(objectPickerIcon.image.width - objectPickerPadding);
                    }

                    GUILayout.Space(footerSpacing);
                }
            }
        }

        protected virtual Type GetElementType()
        {
            Type type = fieldInfo.FieldType;
            while (type.BaseType != null)
            {
                type = type.BaseType;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ObservableList<,>))
                {
                    return type.GenericTypeArguments[0];
                }
            }

            throw new InvalidOperationException("The element type was not found.");
        }
    }
}