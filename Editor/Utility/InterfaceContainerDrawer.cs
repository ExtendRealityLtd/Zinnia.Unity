namespace Zinnia.Utility
{
    using System;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Displays an InterfaceContainer property in the Unity inspector.
    /// </summary>
    [CustomPropertyDrawer(typeof(InterfaceContainer), true)]
    public class InterfaceContainerDrawer : PropertyDrawer
    {
        /// <summary>
        /// The PickedWindow to display to show the available <see cref="InterfaceContainer"/> choices.
        /// </summary>
        public class PickerWindow : PickerWindow<Component, PickerWindow> { }

        /// <inheritdoc/>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                label.tooltip = EditorHelper.GetTooltipAttribute(fieldInfo)?.tooltip ?? string.Empty;
                DrawPicker(property.FindPropertyRelative("field"), GetTargetType(), position, label);
            }
        }

        /// <summary>
        /// Gets the Target <see cref="Type"/>  of the current field info.
        /// </summary>
        /// <returns>The type associated with the field info.</returns>
        protected virtual Type GetTargetType()
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
            return type;
        }

        /// <summary>
        /// Draws the Picker window.
        /// </summary>
        /// <param name="property">The property to draw for.</param>
        /// <param name="type">The data type.</param>
        /// <param name="rect">The default drawing location for the control.</param>
        /// <param name="label">The default label for the control.</param>
        protected virtual void DrawPicker(SerializedProperty property, Type type, Rect rect, GUIContent label)
        {
            Event currentEvent = Event.current;
            bool isMouseDragging = currentEvent.type == EventType.DragUpdated || currentEvent.type == EventType.DragPerform;
            bool isMouseHovering = rect.Contains(currentEvent.mousePosition);

            bool drawAsComponent;
            if (isMouseDragging)
            {
                // Only allow dropping dragged objects if they have at least one component of the correct type.
                drawAsComponent = DragAndDrop.objectReferences
                    .Where(draggedObject => !EditorUtility.IsPersistent(draggedObject))
                    .Select(
                        draggedObject =>
                        {
                            switch (draggedObject)
                            {
                                case Component component:
                                    return component.gameObject;
                                case GameObject gameObject:
                                    return gameObject;
                                default:
                                    return null;
                            }
                        })
                    .Where(gameObject => gameObject != null)
                    .Select(gameObject => gameObject.GetComponents(type))
                    .All(components => components.Length > 0);
            }
            else
            {
                // Regular object picks should just lookup all components.
                drawAsComponent = currentEvent.type == EventType.MouseDown && isMouseHovering;
            }

            string controlName = $"{nameof(InterfaceContainerDrawer)}_{nameof(EditorGUI.ObjectField)}_{property.propertyPath}";
            GUI.SetNextControlName(controlName);
            UnityEngine.Object pickedObject = EditorGUI.ObjectField(
                rect,
                label,
                property.objectReferenceValue,
                drawAsComponent ? typeof(Component) : type,
                true);

            if (pickedObject != property.objectReferenceValue && isMouseDragging && isMouseHovering)
            {
                // The object has been changed by dropping a dragged object.
                HandlePickedObject(property, type, rect, pickedObject);
                return;
            }

            if (currentEvent.type == EventType.ExecuteCommand
                && currentEvent.commandName == "ObjectSelectorClosed"
                && GUI.GetNameOfFocusedControl() == controlName
                && GUIUtility.keyboardControl == EditorGUIUtility.GetObjectPickerControlID())
            {
                // The picker window we opened was closed.
                pickedObject = EditorGUIUtility.GetObjectPickerObject();
                HandlePickedObject(property, type, rect, pickedObject);
            }
        }

        /// <summary>
        /// Determines what to do with the picked object from the ObjectPicker.
        /// </summary>
        /// <param name="property">The property to handle.</param>
        /// <param name="type">The data type.</param>
        /// <param name="rect">The default drawing location for the control.</param>
        /// <param name="pickedObject">The picked object.</param>
        protected virtual void HandlePickedObject(SerializedProperty property, Type type, Rect rect, UnityEngine.Object pickedObject)
        {
            if (pickedObject == property.objectReferenceValue)
            {
                return;
            }

            if (pickedObject == null)
            {
                property.objectReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
                return;
            }

            GameObject pickedGameObject = null;
            switch (pickedObject)
            {
                case Component component:
                    pickedGameObject = component.gameObject;
                    break;
                case GameObject gameObject:
                    pickedGameObject = gameObject;
                    break;
            }

            if (pickedGameObject == null)
            {
                property.objectReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
                return;
            }

            Component[] components = pickedGameObject.GetComponents(type);
            switch (components.Length)
            {
                case 0:
                    property.objectReferenceValue = null;
                    property.serializedObject.ApplyModifiedProperties();
                    break;
                case 1:
                    property.objectReferenceValue = components[0];
                    property.serializedObject.ApplyModifiedProperties();
                    break;
                default:
                    PickerWindow.Show(
                        new Rect
                        {
                            min = GUIUtility.GUIToScreenPoint(rect.min + (Vector2.right * EditorGUIUtility.labelWidth)),
                            max = GUIUtility.GUIToScreenPoint(rect.max)
                        },
                        components,
                        selectedComponent =>
                        {
                            pickedObject = selectedComponent;
                            property.objectReferenceValue = selectedComponent;
                            property.serializedObject.ApplyModifiedProperties();
                        },
                        searchedComponent => searchedComponent.ToString(),
                        drawnComponent =>
                        {
                            Type drawnType = drawnComponent.GetType();
                            return new GUIContent(
                                ObjectNames.NicifyVariableName(drawnType.Name),
                                AssetPreview.GetMiniTypeThumbnail(drawnType),
                                drawnType.FullName);
                        });
                    break;
            }
        }
    }
}