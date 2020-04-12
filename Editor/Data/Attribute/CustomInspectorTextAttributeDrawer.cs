namespace Zinnia.Data.Attribute
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Displays a custom inspector text attribute in the Unity inspector.
    /// </summary>
    [CustomPropertyDrawer(typeof(CustomInspectorTextAttribute))]
    public class CustomInspectorTextAttributeDrawer : PropertyDrawer
    {
        /// <inheritdoc/>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, new GUIContent(((CustomInspectorTextAttribute)attribute).customText));
        }
    }
}