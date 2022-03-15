namespace Zinnia.Data.Type
{
    using UnityEditor;
    using UnityEngine;
    using Zinnia.Data.Collection.List;

    /// <summary>
    /// Displays a custom inspector for an observable list component.
    /// </summary>
    [CustomPropertyDrawer(typeof(ObservableList), true)]
    public class DefaultObservableListDrawer : PropertyDrawer
    {
        /// <summary>
        /// The property reference for the list collection.
        /// </summary>
        protected const string propertyReference = "elements";
        /// <summary>
        /// The label style.
        /// </summary>
        protected readonly GUIContent propertyReferenceLabel = new GUIContent("Elements");
        /// <summary>
        /// Whether to set the serialized list data.
        /// </summary>
        protected bool setSerializedList;
        /// <summary>
        /// The serialized list object.
        /// </summary>
        protected SerializedObject referenceObject;

        /// <inheritdoc/>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.PropertyField(property, label, true);

            Object propertyObject = (Object)fieldInfo.GetValue(property.serializedObject.targetObject);
            if (propertyObject == null)
            {
                return;
            }

            using (new EditorGUI.IndentLevelScope())
            using (new EditorGUI.DisabledScope(Application.isPlaying))
            {
                if (!setSerializedList)
                {
                    referenceObject = new SerializedObject(propertyObject);
                    setSerializedList = true;
                }

                SerializedProperty elements = referenceObject.FindProperty(propertyReference);
                EditorGUILayout.PropertyField(elements, propertyReferenceLabel, true);
                if (referenceObject.ApplyModifiedProperties())
                {
                    setSerializedList = false;
                }
            }
        }
    }
}