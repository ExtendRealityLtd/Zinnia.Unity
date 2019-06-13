namespace Zinnia.Data.Type
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using System.Linq;
    using System.Reflection;
    using System.Collections;

    [CustomPropertyDrawer(typeof(UnityEventBase), true)]
    [CustomPropertyDrawer(typeof(UnityEvent), true)]
    [CustomPropertyDrawer(typeof(UnityEvent<>), true)]
    [CustomPropertyDrawer(typeof(UnityEvent<BaseEventData>), true)]
    public class CollapsibleUnityEventDrawer : UnityEventDrawer
    {
        protected const float headerHeight = 20f;
        protected const float padding = 6f;
        protected const float heightOffset = 2f;
        protected readonly GUIStyle headerBackground = new GUIStyle("RL Header");
        protected bool wasBaseOnGuiCalled = false;

        [InitializeOnLoadMethod]
        public static void ReplaceDefaultDrawer()
        {
            System.Type utilityType = System.Type.GetType("UnityEditor.ScriptAttributeUtility, UnityEditor");
            MethodInfo buildMethod = utilityType.GetMethod(
                "BuildDrawerTypeForTypeDictionary",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static,
                null,
                System.Type.EmptyTypes,
                null);
            FieldInfo dictionaryField = utilityType.GetField(
                "s_DrawerTypeForType",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            FieldInfo drawerField = utilityType
                .GetNestedType("DrawerKeySet", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .GetField("drawer", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Ensure the key set is populated.
            buildMethod.Invoke(null, null);

            IDictionary dictionary = (IDictionary)dictionaryField.GetValue(null);
            System.Type[] types = dictionary.Keys.OfType<System.Type>()
                .Where(
                    type => typeof(UnityEventBase).IsAssignableFrom(type)
                        && typeof(UnityEventDrawer) == (System.Type)drawerField.GetValue(dictionary[type]))
                .ToArray();

            foreach (System.Type type in types)
            {
                object keySet = dictionary[type];
                drawerField.SetValue(keySet, typeof(CollapsibleUnityEventDrawer));
                // DrawerKeySet is a struct, so set it again after modifying it.
                dictionary[type] = keySet;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!wasBaseOnGuiCalled)
            {
                base.OnGUI(position, property, label);
                wasBaseOnGuiCalled = true;
                EditorUtility.SetDirty(property.serializedObject.targetObject);
                return;
            }

            Rect foldoutPosition = new Rect(position.x, position.y + heightOffset, position.width, headerHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutPosition, property.isExpanded, GUIContent.none, true);

            if (property.isExpanded)
            {
                base.OnGUI(position, property, label);
            }
            else
            {
                DrawHeader(position);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            property.isExpanded
                ? base.GetPropertyHeight(property, label)
                : EditorGUIUtility.singleLineHeight + heightOffset * 2f;

        protected virtual void DrawHeader(Rect position)
        {
            if (Event.current.type == EventType.Repaint)
            {
                headerBackground.Draw(position, false, false, false, false);
            }

            Rect headerRect = new Rect(position.x, position.y, position.width, headerHeight);
            headerRect.xMin += padding;
            headerRect.xMax -= padding;
            headerRect.height -= heightOffset;
            headerRect.y += heightOffset * 0.5f;
            DrawEventHeader(headerRect);
        }
    }
}