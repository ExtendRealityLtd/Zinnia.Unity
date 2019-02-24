namespace Zinnia.Data.Type
{
    using UnityEngine;
    using UnityEditor;
    using Zinnia.Utility;

    [CustomPropertyDrawer(typeof(Vector3State))]
    public class Vector3StateDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label.tooltip = EditorHelper.GetTooltipAttribute(fieldInfo)?.tooltip ?? string.Empty;

            SerializedProperty xState = property.FindPropertyRelative("xState");
            SerializedProperty yState = property.FindPropertyRelative("yState");
            SerializedProperty zState = property.FindPropertyRelative("zState");

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            float updatePositionX = position.x;
            const float labelWidth = 15f;
            float fieldWidth = (position.width / 3f) - labelWidth;

            using (new EditorGUI.PropertyScope(position, GUIContent.none, xState))
            {
                EditorGUI.LabelField(new Rect(updatePositionX, position.y, labelWidth, position.height), "X");
                updatePositionX += labelWidth;
                xState.boolValue = EditorGUI.Toggle(
                    new Rect(updatePositionX, position.y, fieldWidth, position.height),
                    xState.boolValue);
                updatePositionX += fieldWidth;
            }

            using (new EditorGUI.PropertyScope(position, GUIContent.none, yState))
            {
                EditorGUI.LabelField(new Rect(updatePositionX, position.y, labelWidth, position.height), "Y");
                updatePositionX += labelWidth;
                yState.boolValue = EditorGUI.Toggle(
                    new Rect(updatePositionX, position.y, fieldWidth, position.height),
                    yState.boolValue);
                updatePositionX += fieldWidth;
            }

            using (new EditorGUI.PropertyScope(position, GUIContent.none, zState))
            {
                EditorGUI.LabelField(new Rect(updatePositionX, position.y, labelWidth, position.height), "Z");
                updatePositionX += labelWidth;
                zState.boolValue = EditorGUI.Toggle(
                    new Rect(updatePositionX, position.y, fieldWidth, position.height),
                    zState.boolValue);
                updatePositionX += fieldWidth;
            }
        }
    }
}