namespace VRTK.Core.Utility
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using System.Reflection;

    /// <summary>
    /// The EditorHelper provides a collection of common static methods for use within Editor scripts.
    /// </summary>
    public static class EditorHelper
    {
        /// <summary>
        /// The GetTooltipAttribute method returns the [Tooltip(")] attribute for the given FieldInfo.
        /// </summary>
        /// <param name="fieldInfo">The FieldInfo to get the tooltip attribute from.</param>
        /// <returns>A TooltipAttribute linked to the given FieldInfo.</returns>
        public static TooltipAttribute GetTooltipAttribute(FieldInfo fieldInfo)
        {
            return (TooltipAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(TooltipAttribute));
        }

        /// <summary>
        /// The BuildGUIContent method returns a GUIContent formatted element.
        /// </summary>
        /// <typeparam name="T">The type of the FieldInfo for the given field name.</typeparam>
        /// <param name="fieldName">The name of the field to build the GUIContent for.</param>
        /// <param name="displayOverride">An optional string to override the display text in the GUIContent.</param>
        /// <returns>A GUIContent formatted with the appropriate field information and tooltip attribute.</returns>
        public static GUIContent BuildGUIContent<T>(string fieldName, string displayOverride = null)
        {
            string displayName = (displayOverride != null ? displayOverride : ObjectNames.NicifyVariableName(fieldName));
            FieldInfo fieldInfo = typeof(T).GetField(fieldName);
            TooltipAttribute tooltipAttribute = GetTooltipAttribute(fieldInfo);
            return (tooltipAttribute == null ? new GUIContent(displayName) : new GUIContent(displayName, tooltipAttribute.tooltip));
        }

        /// <summary>
        /// The AddHeader method attempts to add a header label.
        /// </summary>
        /// <typeparam name="T">The type of the FieldInfo for the given field name.</typeparam>
        /// <param name="fieldName">The name of the field to look for the header attribute from.</param>
        /// <param name="displayOverride">An optional string to use for the display text in the header.</param>
        public static void AddHeader<T>(string fieldName, string displayOverride = null)
        {
            string displayName = (displayOverride != null ? displayOverride : ObjectNames.NicifyVariableName(fieldName));
            FieldInfo fieldInfo = typeof(T).GetField(fieldName);
            HeaderAttribute headerAttribute = (HeaderAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(HeaderAttribute));
            AddHeader(headerAttribute == null ? displayName : headerAttribute.header);
        }

        /// <summary>
        /// The AddHeader method attempts to add a header label.
        /// </summary>
        /// <param name="header">The string for the header label.</param>
        /// <param name="spaceBeforeHeader">Dertermines whether a line space should be added before the header.</param>
        public static void AddHeader(string header, bool spaceBeforeHeader = true)
        {
            if (spaceBeforeHeader)
            {
                EditorGUILayout.Space();
            }

            EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
        }

        /// <summary>
        /// The DrawUsingDestructiveStyle method draws an action with a destructive style.
        /// </summary>
        /// <param name="styleToCopy">The GUIStyle to copy the style from.</param>
        /// <param name="drawAction">The Action to draw the style with.</param>
        public static void DrawUsingDestructiveStyle(GUIStyle styleToCopy, Action<GUIStyle> drawAction)
        {
            Color previousBackgroundColor = GUI.backgroundColor;
            GUIStyle destructiveButtonStyle = new GUIStyle(styleToCopy)
            {
                normal =
                {
                    textColor = Color.white
                },
                active =
                {
                    textColor = Color.white
                }
            };

            GUI.backgroundColor = Color.red;
            drawAction(destructiveButtonStyle);
            GUI.backgroundColor = previousBackgroundColor;
        }

        /// <summary>
        /// The DrawScrollableSelectableLabel method attempts to draw a selectable label that is also scrollable.
        /// </summary>
        /// <param name="scrollPosition">The current scroll position of the label element.</param>
        /// <param name="width">The width of the label element.</param>
        /// <param name="text">The text to display within the label element.</param>
        /// <param name="style">The GUIStyle to draw the label with.</param>
        public static void DrawScrollableSelectableLabel(ref Vector2 scrollPosition, ref float width, string text, GUIStyle style)
        {
            using (EditorGUILayout.ScrollViewScope scrollViewScope = new EditorGUILayout.ScrollViewScope(scrollPosition))
            {
                scrollPosition = scrollViewScope.scrollPosition;

                float textHeight = style.CalcHeight(new GUIContent(text), width);
                EditorGUILayout.SelectableLabel(text, style, GUILayout.MinHeight(textHeight));

                if (Event.current.type == EventType.Repaint)
                {
                    width = GUILayoutUtility.GetLastRect().width;
                }
            }
        }
    }
}