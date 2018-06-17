namespace VRTK.Core.Data.Attribute
{
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(UnityFlagsAttribute))]
    public class UnityFlagsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
        }
    }
}