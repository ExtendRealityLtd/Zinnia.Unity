namespace Zinnia.Data.Attribute
{
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(InternalSettingAttribute))]
    public class InternalSettingAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Color controlColor = Color.grey;

            Color currentGUIColor = GUI.color;
            FontStyle currentFontStyle = EditorStyles.label.fontStyle;
            Color currentNormalTextColor = EditorStyles.label.normal.textColor;
            Color currentFocusedTextColor = EditorStyles.label.focused.textColor;

            GUI.color = controlColor;
            EditorStyles.label.normal.textColor = controlColor;
            EditorStyles.label.focused.textColor = controlColor;
            EditorStyles.label.fontStyle = FontStyle.Italic;

            EditorGUI.PropertyField(position, property, label, true);

            GUI.color = currentGUIColor;
            EditorStyles.label.normal.textColor = currentNormalTextColor;
            EditorStyles.label.focused.textColor = currentFocusedTextColor;
            EditorStyles.label.fontStyle = currentFontStyle;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }
    }
}