namespace Zinnia.Data.Type
{
    using UnityEngine;
    using UnityEditor;
    using Zinnia.Data.Collection.List;

    [CustomPropertyDrawer(typeof(ObservableList), true)]
    public class DefaultObservableListDrawer : PropertyDrawer
    {
        protected const string propertyReference = "elements";
        protected readonly GUIContent propertyReferenceLabel = new GUIContent("Elements");
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