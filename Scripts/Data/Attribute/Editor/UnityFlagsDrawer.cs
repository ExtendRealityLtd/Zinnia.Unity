namespace VRTK.Core.Data.Attribute
{
    using UnityEngine;
    using UnityEditor;
    using VRTK.Core.Utility;

    [CustomPropertyDrawer(typeof(UnityFlagsAttribute))]
    public class UnityFlagsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label.tooltip = EditorHelper.GetTooltipAttribute(fieldInfo)?.tooltip ?? string.Empty;
            property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
        }
    }
}