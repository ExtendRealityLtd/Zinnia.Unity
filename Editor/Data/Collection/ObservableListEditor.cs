namespace Zinnia.Unity.Editor.Data.Collection
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Events;
    using System;
    using System.Collections;
    using System.Reflection;
    using Malimbe.FodyRunner.UnityIntegration;
    using Zinnia.Data.Collection;
    using Zinnia.Utility;

    /// <summary>
    /// A custom inspector editor for <see cref="ObservableList{TElement,TEvent}"/> and subclasses of it.
    /// </summary>
    [CustomEditor(typeof(ObservableList<,>), true)]
    public class ObservableListEditor : InspectorEditor
    {
        /// <summary>
        /// The name of the field on <see cref="ObservableList{TElement,TEvent}"/> that holds the elements.
        /// </summary>
        protected const string ElementsFieldName = "elements";
        /// <summary>
        /// The name of the property on <see cref="ObservableList{TElement,TEvent}"/> that returns the elements in its getter.
        /// </summary>
        protected const string ElementsPropertyName = "Elements";

        /// <summary>
        /// The previous size of the elements collection in case a size change was done, otherwise <see langword="null"/>.
        /// </summary>
        protected int? sizeBeforeSizeChange;
        /// <summary>
        /// The index of the changed element if a change was done, otherwise <see langword="null"/>.
        /// </summary>
        protected int? changedIndex;

        /// <inheritdoc/>
        protected override void DrawProperty(SerializedProperty property)
        {
            sizeBeforeSizeChange = null;
            changedIndex = null;

            if (property.propertyPath != ElementsFieldName)
            {
                base.DrawProperty(property);
                return;
            }

            if (!EditorGUILayout.PropertyField(property, false))
            {
                // The property is collapsed. Don't draw any children manually.
                return;
            }

            bool drawAsDisabled = Application.isPlaying
                && property.serializedObject.targetObject is Behaviour behaviour
                && !behaviour.isActiveAndEnabled;
            if (drawAsDisabled)
            {
                EditorGUILayout.HelpBox(
                    @"Enable the component to edit this property.
This restriction is in place to ensure any subscribed listener to events on this component will stay in sync with the state of this collection.",
                    MessageType.Info);
            }

            using (new EditorGUI.DisabledGroupScope(drawAsDisabled))
            {
                // Copy the property so InspectorEditor can continue iterating over the other properties once we're done drawing this one's children.
                property = property.Copy();
                if (!property.NextVisible(true))
                {
                    return;
                }

                using (new EditorGUI.IndentLevelScope(property.depth))
                {
                    do
                    {
                        if (property.propertyType == SerializedPropertyType.ArraySize)
                        {
                            using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
                            {
                                int previousSize = property.intValue;
                                base.DrawProperty(property);
                                if (changeCheckScope.changed)
                                {
                                    sizeBeforeSizeChange = previousSize;
                                }
                            }
                        }
                        else
                        {
                            using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
                            {
                                base.DrawProperty(property);
                                if (changeCheckScope.changed)
                                {
                                    changedIndex = property.TryGetIndex();
                                }
                            }
                        }
                    }
                    while (property.NextVisible(false));
                }
            }
        }

        /// <inheritdoc />
        protected override void ApplyModifiedProperty(SerializedProperty property, bool hasChangeHandlers)
        {
            if (property.propertyPath == ElementsFieldName)
            {
                hasChangeHandlers = true;
            }

            base.ApplyModifiedProperty(property, hasChangeHandlers);
        }

        /// <inheritdoc/>
        protected override void BeforeChange(SerializedProperty property)
        {
            base.BeforeChange(property);

            if (sizeBeforeSizeChange != null)
            {
                int previousSize = sizeBeforeSizeChange.Value;
                int newSize = property.arraySize;

                if (newSize >= previousSize)
                {
                    return;
                }

                Action<int> removedRaiser = CreateElementsEventRaiser(property, nameof(GameObjectObservableList.ElementRemoved));
                for (int index = previousSize - 1; index >= newSize; index--)
                {
                    removedRaiser(index);
                }

                if (newSize == 0)
                {
                    CreateElementsEventRaiser(property, nameof(GameObjectObservableList.BecameEmpty))(0);
                }
            }
            else if (changedIndex != null)
            {
                CreateElementsEventRaiser(property, nameof(GameObjectObservableList.ElementRemoved))(changedIndex.Value);
            }
        }

        /// <inheritdoc/>
        protected override void AfterChange(SerializedProperty property)
        {
            base.AfterChange(property);

            if (sizeBeforeSizeChange != null)
            {
                int previousSize = sizeBeforeSizeChange.Value;
                int newSize = property.arraySize;

                if (newSize <= previousSize)
                {
                    return;
                }

                Action<int> addedRaiser = CreateElementsEventRaiser(property, nameof(GameObjectObservableList.ElementAdded));
                if (previousSize == 0)
                {
                    addedRaiser(0);
                    CreateElementsEventRaiser(property, nameof(GameObjectObservableList.BecamePopulated))(0);

                    previousSize++;
                }

                for (int index = previousSize; index < newSize; index++)
                {
                    addedRaiser(index);
                }
            }
            else if (changedIndex != null)
            {
                CreateElementsEventRaiser(property, nameof(GameObjectObservableList.ElementAdded))(changedIndex.Value);
            }
        }

        /// <summary>
        /// Creates a reusable <see cref="Action"/> to raise an event on the target object with the element at the given index as the event data.
        /// </summary>
        /// <param name="property">The property that just changed.</param>
        /// <param name="name">The name of the event to raise.</param>
        /// <returns>A reusable <see cref="Action"/> that when called raises an event on the target object with the element at the given index as the event data.</returns>
        protected virtual Action<int> CreateElementsEventRaiser(SerializedProperty property, string name)
        {
            UnityEngine.Object targetObject = property.serializedObject.targetObject;
            Type type = targetObject.GetType();

            FieldInfo eventFieldInfo = type.GetField(name, BindingFlags.Public | BindingFlags.Instance);
            object unityEvent = eventFieldInfo.GetValue(targetObject);
            MethodInfo eventInvokeMethodInfo = eventFieldInfo.FieldType.GetMethod(nameof(UnityEvent.Invoke), BindingFlags.Public | BindingFlags.Instance);

            PropertyInfo elementsPropertyInfo = type.GetProperty(ElementsPropertyName, BindingFlags.NonPublic | BindingFlags.Instance);
            IList elements = (IList)elementsPropertyInfo.GetMethod.Invoke(targetObject, null);

            return index => eventInvokeMethodInfo.Invoke(
                unityEvent,
                new[]
                {
                    elements[index]
                });
        }
    }
}