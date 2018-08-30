namespace VRTK.Core.Data.Attribute
{
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(InternalSettingAttribute))]
    public class InternalSettingDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Color currentGUIColor = GUI.color;
            GUI.color = Color.grey;
            EditorStyles.label.richText = true;
            EditorGUI.PropertyField(position, property, new GUIContent("<color=grey><i>" + label.text + "</i></color>"), true);
            GUI.color = currentGUIColor;
        }
    }
}