namespace Zinnia.Data.Attribute
{
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(RestrictedAttribute))]
    public class RestrictedAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            RestrictedAttribute attrib = (RestrictedAttribute)attribute;
            Behaviour behaviour = (Behaviour)property.serializedObject.targetObject;

            bool isPlayingAndActiveAndEnabled = Application.isPlaying && behaviour.isActiveAndEnabled;
            bool isPlayingAndActiveAndDisabled = Application.isPlaying && !behaviour.isActiveAndEnabled;
            bool makeReadOnly = (attrib.restrictions & RestrictedAttribute.Restrictions.ReadOnlyAlways) != 0 ||
                ((attrib.restrictions & RestrictedAttribute.Restrictions.ReadOnlyAtRunTime) != 0 && Application.isPlaying) ||
                ((attrib.restrictions & RestrictedAttribute.Restrictions.ReadOnlyAtRunTimeAndEnabled) != 0 && isPlayingAndActiveAndEnabled) ||
                ((attrib.restrictions & RestrictedAttribute.Restrictions.ReadOnlyAtRunTimeAndDisabled) != 0 && isPlayingAndActiveAndDisabled);

            bool muteProperty = (attrib.restrictions & RestrictedAttribute.Restrictions.Muted) != 0;

            bool originalGUIEnabledState = GUI.enabled;
            if (makeReadOnly)
            {
                GUI.enabled = false;
            }

            Color controlColor = Color.grey;

            Color originalGuiColor = GUI.color;
            FontStyle originalFontStyle = EditorStyles.label.fontStyle;
            Color originalNormalTextColor = EditorStyles.label.normal.textColor;
            Color originalFocusedTextColor = EditorStyles.label.focused.textColor;

            if (muteProperty)
            {
                GUI.color = controlColor;
                EditorStyles.label.normal.textColor = controlColor;
                EditorStyles.label.focused.textColor = controlColor;
                EditorStyles.label.fontStyle = FontStyle.Italic;
            }

            EditorGUI.PropertyField(position, property, label, true);

            if (muteProperty)
            {
                GUI.color = originalGuiColor;
                EditorStyles.label.normal.textColor = originalNormalTextColor;
                EditorStyles.label.focused.textColor = originalFocusedTextColor;
                EditorStyles.label.fontStyle = originalFontStyle;
            }

            if (makeReadOnly)
            {
                GUI.enabled = originalGUIEnabledState;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }
    }
}