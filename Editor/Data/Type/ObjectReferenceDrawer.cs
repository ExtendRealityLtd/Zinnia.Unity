namespace Zinnia.Data.Type
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Displays a custom inspector showing a button for the linked reference.
    /// </summary>
    [CustomPropertyDrawer(typeof(ObjectReference))]
    public class ObjectReferenceDrawer : PropertyDrawer
    {
        private const float BUTTON_SIZE = 22f;
        private const float BUTTON_PADDING = 0f;

        /// <inheritdoc/>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty linkedReference = property.FindPropertyRelative("linkedObject");
            SerializedProperty buttonText = property.FindPropertyRelative("linkText");
            SerializedProperty isActive = property.FindPropertyRelative("isActive");

            EditorGUI.BeginProperty(position, label, property);
            if (isActive.boolValue)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(" ");

                if (GUILayout.Button(buttonText.stringValue))
                {
                    EditorGUIUtility.PingObject(linkedReference.objectReferenceValue);
                }
                if (GUILayout.Button(EditorGUIUtility.IconContent("IN foldout focus on", "|Edit Reference Data"), GUILayout.Width(BUTTON_SIZE)))
                {
                    isActive.boolValue = false;
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
            EditorGUI.EndProperty();
        }

        /// <inheritdoc/>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty isActive = property.FindPropertyRelative("isActive");

            return isActive.boolValue ? BUTTON_PADDING : EditorGUI.GetPropertyHeight(property);
        }
    }
}