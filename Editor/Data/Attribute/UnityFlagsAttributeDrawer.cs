namespace Zinnia.Data.Attribute
{
    using UnityEditor;
    using UnityEngine;
    using Zinnia.Utility;

    /// <summary>
    /// Displays an inspector property for multiple selectable enum.
    /// </summary>
    [CustomPropertyDrawer(typeof(UnityFlagsAttribute))]
    public class UnityFlagsAttributeDrawer : PropertyDrawer
    {
        /// <inheritdoc/>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label.tooltip = EditorHelper.GetTooltipAttribute(fieldInfo)?.tooltip ?? string.Empty;

            using (new EditorGUI.PropertyScope(position, GUIContent.none, property))
            {
                property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
            }
        }
    }
}