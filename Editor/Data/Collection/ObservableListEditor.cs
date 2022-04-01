namespace Zinnia.Unity.Editor.Data.Collection
{
#if (UNITY_2020_3_OR_NEWER && !ZINNIA_USE_CUSTOM_LIST_EDITOR) || ZINNIA_IGNORE_CUSTOM_LIST_EDITOR
    /// Ignore this custom list editor in Unity 2020.3 or above as it breaks with Unity 2020.3.24f due to other fixes within Unity.
    /// It can still be forced to use the custom editor if the Scripting Define Symbol of `ZINNIA_USE_CUSTOM_LIST_EDITOR` is added but the list won't be collapsible. 
    /// It can also be completely turned off if the Scripting Define Symbol of `ZINNIA_IGNORE_CUSTOM_LIST_EDITOR` is added.
#else
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using Zinnia.Data.Collection.List;
    using Zinnia.Utility;

    /// <summary>
    /// A custom inspector editor for <see cref="ObservableList{TElement,TEvent}"/> and subclasses of it.
    /// </summary>
    /// <remarks>
    /// This custom editor allows for the list elements to be updated in the editor at runtime and any effects of the changes will be processed just as if the changes were made via code.
    /// </remarks>
    [CustomEditor(typeof(ObservableList<,>), true)]
    public class ObservableListEditor : ZinniaInspector
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
        /// A reused instance to use when remembering removed or added elements.
        /// </summary>
        protected readonly List<object> elements = new List<object>();
        /// <summary>
        /// The previous size of the elements collection in case a size change was done, otherwise <see langword="null"/>.
        /// </summary>
        protected int? sizeBeforeSizeChange;
        /// <summary>
        /// The index of the changed element if a change was done, otherwise <see langword="null"/>.
        /// </summary>
        protected int? changedIndex;
        /// <summary>
        /// The element that was set before it was changed at index <see cref="changedIndex"/> if a change was done, otherwise <see langword="null"/>.
        /// </summary>
        protected object changedElement;

        /// <inheritdoc/>
        protected override void DrawProperty(SerializedProperty property)
        {
            if (property.propertyPath != ElementsFieldName)
            {
                base.DrawProperty(property);
                return;
            }

            if (!EditorGUILayout.PropertyField(property, false))
            {
#if !UNITY_2020_3_OR_NEWER
                // The property is collapsed. Don't draw any children manually.
                return;
#endif
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

        /// <inheritdoc/>
        protected override void ApplyModifiedProperty(SerializedProperty property, bool hasChangeHandlers)
        {
            if (property.propertyPath == ElementsFieldName)
            {
                hasChangeHandlers = true;
            }

            base.ApplyModifiedProperty(property, hasChangeHandlers);
        }

        /// <inheritdoc/>
        protected override void BeforeChange(MethodInfo methodInfo, SerializedProperty property)
        {
            base.BeforeChange(methodInfo, property);

            if (sizeBeforeSizeChange == null && changedIndex == null)
            {
                return;
            }

            IList list = GetElementsList(property, out _, out _);

            if (sizeBeforeSizeChange != null)
            {
                int previousSize = sizeBeforeSizeChange.Value;
                int newSize = property.arraySize;

                if (newSize >= previousSize)
                {
                    return;
                }

                // The elements are about to get removed by Unity. To raise the Removed event for them they're cached here.
                for (int index = newSize; index < previousSize; index++)
                {
                    elements.Add(list[index]);
                }
            }
            else if (changedIndex != null)
            {
                // The element is about to get changed by Unity. To raise the Removed event for it it's cached here.
                changedElement = list[changedIndex.Value];
            }
        }

        /// <inheritdoc />
        protected override void AfterChange(MethodInfo methodInfo, SerializedProperty property)
        {
            base.AfterChange(methodInfo, property);

            if (sizeBeforeSizeChange == null && changedIndex == null)
            {
                return;
            }

            IList list = GetElementsList(property, out UnityEngine.Object targetObject, out Type type);
            object[] parameters = new object[1];

            if (sizeBeforeSizeChange != null)
            {
                int previousSize = sizeBeforeSizeChange.Value;
                int newSize = property.arraySize;

                if (newSize < previousSize)
                {
                    // Unity already removed the elements. Add the previously removed elements from the cache...
                    foreach (object removedElement in elements)
                    {
                        list.Add(removedElement);
                    }

                    // ...and call the Remove API for those elements.
                    MethodInfo apiMethodInfo = type.GetMethod(nameof(GameObjectObservableList.RemoveAt), BindingFlags.Public | BindingFlags.Instance);
                    for (int index = previousSize - 1; index >= newSize; index--)
                    {
                        parameters[0] = index;
                        apiMethodInfo.Invoke(targetObject, parameters);
                    }
                }
                else if (newSize > previousSize)
                {
                    // Unity already added the elements. Cache those elements first...
                    for (int index = previousSize; index < newSize; index++)
                    {
                        elements.Add(list[index]);
                    }

                    // ...now remove them from the backing field...
                    for (int index = newSize - 1; index >= previousSize; index--)
                    {
                        list.RemoveAt(index);
                    }

                    // ...and finally call the Add API for those elements.
                    MethodInfo apiMethodInfo = type.GetMethod(nameof(GameObjectObservableList.Add), BindingFlags.Public | BindingFlags.Instance);
                    foreach (object element in elements)
                    {
                        parameters[0] = element;
                        apiMethodInfo.Invoke(targetObject, parameters);
                    }
                }

                elements.Clear();
                sizeBeforeSizeChange = null;
            }
            else if (changedIndex != null)
            {
                // Unity already changed the element at the index. Get the new element out of it...
                object newElement = list[changedIndex.Value];

                // ...change it back to the previous one...
                list[changedIndex.Value] = changedElement;

                // ...and finally call the SetAt API with the new element.
                MethodInfo apiMethodInfo = type.GetMethod(nameof(GameObjectObservableList.SetAt), BindingFlags.Public | BindingFlags.Instance);
                parameters = new[]
                {
                    newElement,
                    changedIndex.Value
                };
                apiMethodInfo.Invoke(targetObject, parameters);

                changedIndex = null;
                changedElement = null;
            }
        }

        /// <summary>
        /// Returns the <see cref="ObservableList{TElement,TEvent}.Elements"/> list via reflection as well as any objects looked up to do so.
        /// </summary>
        /// <param name="property">The property that is part of the drawn <see cref="ObservableList{TElement,TEvent}"/>.</param>
        /// <param name="targetObject">The instance of <see cref="ObservableList{TElement,TEvent}"/> looked up via <paramref name="property"/>.</param>
        /// <param name="type">The type of <paramref name="targetObject"/>.</param>
        /// <returns>The <see cref="ObservableList{TElement,TEvent}.Elements"/> list.</returns>
        protected virtual IList GetElementsList(SerializedProperty property, out UnityEngine.Object targetObject, out Type type)
        {
            targetObject = property.serializedObject.targetObject;
            type = targetObject.GetType();

            PropertyInfo elementsPropertyInfo = type.GetProperty(ElementsPropertyName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (IList)elementsPropertyInfo.GetMethod.Invoke(targetObject, null);
        }
    }
#endif
}