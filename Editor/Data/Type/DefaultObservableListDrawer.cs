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
                SerializedObject referenceObject = new SerializedObject(propertyObject);
                SerializedProperty elements = referenceObject.FindProperty(propertyReference);
                EditorGUILayout.PropertyField(elements, propertyReferenceLabel, true);
                referenceObject.ApplyModifiedProperties();
            }
        }
    }
}