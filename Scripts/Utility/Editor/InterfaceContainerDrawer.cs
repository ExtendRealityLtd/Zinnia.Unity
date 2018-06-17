namespace VRTK.Core.Utility
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Linq;

    [CustomPropertyDrawer(typeof(InterfaceContainer), true)]
    public class InterfaceContainerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // TODO: Cache the generic type lookup per `property`.
            Type type = fieldInfo.FieldType;
            if (type.HasElementType)
            {
                type = type.GetElementType();
            }

            while (type != null && type.BaseType != typeof(InterfaceContainer))
            {
                type = type.BaseType;
            }

            if (type?.BaseType != typeof(InterfaceContainer))
            {
                // TODO: Support List<> and others somehow. See the above -> Unity is calling this PropertyDrawer even if the actual type we're supporting with it is just part of something... Probably just needs to use something else than `fieldInfo` or something...
                throw new ArgumentException();
            }

            label.tooltip = EditorHelper.GetTooltipAttribute(fieldInfo)?.tooltip ?? string.Empty;

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                EditorGUI.ObjectField(position, property.FindPropertyRelative("field"), type.GenericTypeArguments.Single(), label);
            }
        }
    }
}