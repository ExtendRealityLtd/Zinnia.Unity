namespace Zinnia.Data.Attribute
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Displays an inspector property drawer with restricted styles.
    /// </summary>
    [CustomPropertyDrawer(typeof(RestrictedAttribute))]
    public class RestrictedAttributeDrawer : PropertyDrawer
    {
        /// <summary>
        /// The original GUI enabled state.
        /// </summary>
        protected static bool originalGUIEnabledState = GUI.enabled;
        /// <summary>
        /// The original GUI color.
        /// </summary>
        protected static Color originalGuiColor = GUI.color;
        /// <summary>
        /// The original font style.
        /// </summary>
        protected static FontStyle originalFontStyle = EditorStyles.label.fontStyle;
        /// <summary>
        /// The original normal text color.
        /// </summary>
        protected static Color originalNormalTextColor = EditorStyles.label.normal.textColor;
        /// <summary>
        /// The original focused text color.
        /// </summary>
        protected static Color originalFocusedTextColor = EditorStyles.label.focused.textColor;
        /// <summary>
        /// The color to use for muted text.
        /// </summary>
        protected static Color mutedColor = new Color(0.75f, 0.75f, 0.75f);
        /// <summary>
        /// The font style to use for muted text.
        /// </summary>
        protected static FontStyle mutedStyle = FontStyle.Italic;

        /// <inheritdoc/>
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

            originalGUIEnabledState = GUI.enabled;

            if (makeReadOnly)
            {
                GUI.enabled = false;
            }

            if (muteProperty)
            {
                GUI.color = mutedColor;
                EditorStyles.label.normal.textColor = mutedColor;
                EditorStyles.label.focused.textColor = mutedColor;
                EditorStyles.label.fontStyle = mutedStyle;
            }

            EditorGUI.PropertyField(position, property, label, true);

            GUI.color = originalGuiColor;
            EditorStyles.label.normal.textColor = originalNormalTextColor;
            EditorStyles.label.focused.textColor = originalFocusedTextColor;
            EditorStyles.label.fontStyle = originalFontStyle;
            GUI.enabled = originalGUIEnabledState;

            EditorGUI.EndProperty();
        }

        /// <inheritdoc/>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }
    }
}