namespace Zinnia.Rule
{
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using Zinnia.Utility;

    [CustomPropertyDrawer(typeof(RuleContainer), true)]
    public class RuleContainerDrawer : PropertyDrawer
    {
        protected static readonly Dictionary<int, bool> ShowChildrenByObjectId = new Dictionary<int, bool>();

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

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                label.tooltip = EditorHelper.GetTooltipAttribute(fieldInfo)?.tooltip ?? string.Empty;
                position.height = EditorGUIUtility.singleLineHeight;

                SerializedObject serializedObject = property.serializedObject;
                SerializedProperty fieldProperty = property.FindPropertyRelative("field");

                bool showChildren;
                int hashCode = property.propertyPath.GetHashCode();
                ShowChildrenByObjectId.TryGetValue(hashCode, out showChildren);

                bool hasReference = fieldProperty.objectReferenceValue != null;
                bool isCircularReference = fieldProperty.objectReferenceValue == serializedObject.targetObject;

                Rect foldoutRect = position;
                if (hasReference && !isCircularReference)
                {
                    foldoutRect.width = EditorGUI.IndentedRect(new Rect(Vector2.zero, Vector2.zero)).x;
                    ShowChildrenByObjectId[hashCode] = EditorGUI.Foldout(foldoutRect, showChildren, GUIContent.none, true);
                }

                GUIContent buttonContent = new GUIContent(
                    hasReference ? "-" : "+",
                    $"{(hasReference ? "Remove" : "Add a new")} {type.Name} {(hasReference ? "from" : "to")} this game object.");
                float addButtonWidth;
                float removeButtonWidth;
                float buttonMaxWidth;
                GUI.skin.button.CalcMinMaxWidth(new GUIContent("+"), out addButtonWidth, out buttonMaxWidth);
                GUI.skin.button.CalcMinMaxWidth(new GUIContent("-"), out removeButtonWidth, out buttonMaxWidth);
                float buttonWidth = Mathf.Max(addButtonWidth, removeButtonWidth);

                using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
                {
                    Rect pickerRect = position;
                    pickerRect.width -= buttonWidth + 2f * EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.ObjectField(pickerRect, fieldProperty, type, label);
                    if (changeCheckScope.changed)
                    {
                        serializedObject.ApplyModifiedProperties();
                    }
                }

                Rect buttonRect = position;
                buttonRect.x = buttonRect.width - 2f * EditorGUIUtility.standardVerticalSpacing;
                buttonRect.width = buttonWidth;
                if (GUI.Button(buttonRect, buttonContent))
                {
                    if (hasReference)
                    {
                        UnityEngine.Object reference = fieldProperty.objectReferenceValue;
                        fieldProperty.objectReferenceValue = null;
                        serializedObject.ApplyModifiedProperties();
                        Undo.DestroyObjectImmediate(reference);

                        /*
                         * Because we remove a component on the same game object the inspector is drawing currently
                         * Unity will encounter the removed component and throw an exception trying to draw the
                         * inspector for it. This instruction will basically tell Unity to skip the current GUI
                         * loop iteration, preventing any errors.
                         */
                        GUIUtility.ExitGUI();

                        return;
                    }

                    Rect creatorRect = new Rect
                    {
                        min = GUIUtility.GUIToScreenPoint(position.min + Vector2.right * EditorGUIUtility.labelWidth),
                        max = GUIUtility.GUIToScreenPoint(buttonRect.max)
                    };
                    TypePickerWindow.Show(
                        creatorRect,
                        type,
                        selectedType =>
                        {
                            fieldProperty.objectReferenceValue = Undo.AddComponent(Selection.activeGameObject, selectedType);
                            InternalEditorUtility.SetIsInspectorExpanded(fieldProperty.objectReferenceValue, false);
                            serializedObject.ApplyModifiedProperties();
                            ShowChildrenByObjectId[hashCode] = true;
                        });
                }

                if (!showChildren || !hasReference || isCircularReference)
                {
                    return;
                }

                /*
                 * Keep repainting this PropertyDrawer because a child is visible. This ensures the property fields
                 * of the child are updated when an undo operation is performed.
                 */
                EditorUtility.SetDirty(serializedObject.targetObject);

                position.y += position.height;
                using (new EditorGUI.IndentLevelScope())
                {
                    SerializedObject fieldSerializedObject = new SerializedObject(fieldProperty.objectReferenceValue);
                    SerializedProperty iteratedProperty = fieldSerializedObject.GetIterator();
                    iteratedProperty.NextVisible(true);

                    while (iteratedProperty.NextVisible(false))
                    {
                        using (new EditorGUI.PropertyScope(position, label, iteratedProperty))
                        using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
                        {
                            float propertyHeight = EditorGUI.GetPropertyHeight(iteratedProperty);
                            bool expandedChildren = EditorGUI.PropertyField(
                                new Rect(position.x, position.y, position.width, propertyHeight),
                                iteratedProperty,
                                true);
                            if (changeCheckScope.changed)
                            {
                                iteratedProperty.serializedObject.ApplyModifiedProperties();
                            }

                            if (!expandedChildren)
                            {
                                break;
                            }

                            position.y += propertyHeight;
                        }
                    }
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float result = EditorGUIUtility.singleLineHeight;

            SerializedProperty fieldProperty = property.FindPropertyRelative("field");
            bool isCircularReference = fieldProperty.objectReferenceValue == property.serializedObject.targetObject;
            bool showChildren;
            if (ShowChildrenByObjectId.TryGetValue(property.propertyPath.GetHashCode(), out showChildren)
                && !showChildren
                || fieldProperty.objectReferenceValue == null
                || isCircularReference)
            {
                return result;
            }

            using (SerializedObject serializedObject = new SerializedObject(fieldProperty.objectReferenceValue))
            {
                SerializedProperty iteratedProperty = serializedObject.GetIterator();
                iteratedProperty.NextVisible(true);
                while (iteratedProperty.NextVisible(false))
                {
                    result += EditorGUI.GetPropertyHeight(iteratedProperty, true);
                }
            }

            return result;
        }
    }
}